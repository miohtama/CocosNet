// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using System;
using CocosNet.Support;
using CocosNet.Base;
using System.Collections.Generic;
using OpenTK.Graphics.ES11;
using System.Collections;

namespace CocosNet {

	// TODO:
	// this could use a (much) better design.
	// In Cocos2D, users of the TextureAtlas have to be
	// aware of their "capacity" at all times and maintain this value.
	// This is what determines how many indices get made. I would prefer to avoid that 
	// here. So far the way I negated that is quite a hack, and
	// has some unexpected pitfalls in it. Instead, a better
	// approach is to have one entity aware of both the quad
	// and indices arrays (perhaps TextureAtlas itself), and 
	// makes sure both stay in sync.
	//
	// the current way the atlas works, indices are only created
	// at construction time. So you have to never need more
	// indices than after you first creat it. So, for example, with
	// the FPS display (which is a label atlas), it has to be initialized
	// with a string that is long enough to handle all FPS cases, otherwise
	// any cases that are longer will require more indices than are there,
	// and you get nasty graphic glitching.
	public class TextureAtlas {
		#region AtlasList
		/// <summary>
		/// This class offers the interface of a standard List.
		/// But lets you directly access the backing array.
		/// It is used by the TextureAtlas to efficiently send the quads
		/// and indices over to OpenGL without having to directly muck with an array.
		/// </summary>
		private class AtlasList<T> : IList<T> {
			private T[] _data;
			private int _index;

			private void DoubleArray() {
				T[] newData = new T[_data.Length * 2];
				Array.Copy(_data, newData, _data.Length);
				
				_data = newData;
			}
			
			public AtlasList(int capacity) {
				_data = new T[capacity];
				_index = -1;
			}

			public int Count {
				get { return _index + 1; }
			}

			public bool IsReadOnly {
				get { return false; }
			}

			public T this[int index] {
				get { return _data[index]; }
				set { 
					while(index >= _data.Length) {
						DoubleArray();
					}
					_data[index] = value;
				}
			}

			public T[] Data {
				get { return _data; }
			}
			
			public void Add(T item) {
				++_index;
				if (_index >= _data.Length) {
					DoubleArray();
				}
				_data[_index] = item;
			}
			
			#region Not implemented members of IList<T>
			System.Collections.IEnumerator IEnumerable.GetEnumerator() {
				throw new System.NotImplementedException();
			}

			public IEnumerator<T> GetEnumerator() {
				throw new System.NotImplementedException();
			}

			public bool Contains(T item) {
				throw new System.NotImplementedException();
			}

			public void CopyTo(T[] array, int arrayIndex) {
				throw new System.NotImplementedException();
			}

			public bool Remove(T item) {
				throw new System.NotImplementedException();
			}

			public int IndexOf(T item) {
				throw new System.NotImplementedException();
			}

			public void Insert(int index, T item) {
				throw new System.NotImplementedException();
			}

			public void RemoveAt(int index) {
				throw new System.NotImplementedException();
			}

			public void Clear() {
				throw new System.NotImplementedException();
			}
			#endregion
			
		}
		#endregion AtlasList

		private int _capacity;
		private AtlasList<GLPointQuad3F> _quads;
		private AtlasList<ushort> _indices;

		private void InitIndices() {
			for (int i = 0; i < _capacity; ++i) {
				#if USE_TRIANGLE_STRIP
				_indices.Add(Convert.ToUInt16(i * 4 + 0));
				_indices.Add(Convert.ToUInt16(i * 4 + 0));
				_indices.Add(Convert.ToUInt16(i * 4 + 2));
				_indices.Add(Convert.ToUInt16(i * 4 + 1));
				_indices.Add(Convert.ToUInt16(i * 4 + 3));
				_indices.Add(Convert.ToUInt16(i * 4 + 3));
				#else
				_indices.Add(Convert.ToUInt16(i * 4 + 0));
				_indices.Add(Convert.ToUInt16(i * 4 + 1));
				_indices.Add(Convert.ToUInt16(i * 4 + 2));
				_indices.Add(Convert.ToUInt16(i * 4 + 3));
				_indices.Add(Convert.ToUInt16(i * 4 + 2));
				_indices.Add(Convert.ToUInt16(i * 4 + 1));
				#endif
			}
		}

		public int TotalQuads {
			get { return _quads.Count; }
		}

		public int Capacity {
			get { return _capacity; }
		}

		public IList<GLPointQuad3F> Quads {
			get { return _quads; }
		}


		public Texture2D Texture { get; set; }

		public TextureAtlas(string file, int capacity) : this(TextureMgr.Instance.AddImage(file), capacity) {
		}

		public TextureAtlas(Texture2D texture, int capacity) {
			if (texture == null) {
				throw new ArgumentNullException("texture");
			}
			
			_capacity = capacity;
			
			Texture = texture;
			
			_quads = new AtlasList<GLPointQuad3F>(_capacity);
			_indices = new AtlasList<ushort>(_capacity * 6);
			
			InitIndices();
		}

		public void DrawQuads() {
			DrawQuads(_quads.Count);
		}
		
		public void DrawQuads(int number) {
			GL.BindTexture(All.Texture2D, Texture.Name);
			
			GLPointQuad3F[] quads = _quads.Data;
			
			unsafe {
				fixed (GLPointQuad3F* quadP = quads) {
					// TODO: should come up with a non hardcoded method
					// for these offsets.
					const int PointSize = 24;
					const int ColorOffset = 12;
					const int TexCoordOffset = 16;
					
					GL.VertexPointer(3, All.Float, PointSize, quads);
					GL.ColorPointer(4, All.UnsignedByte, PointSize, (IntPtr)((int)quadP + ColorOffset));
					GL.TexCoordPointer(2, All.Float, PointSize, (IntPtr)((int)quadP + TexCoordOffset));
					
#if USE_TRIANGLE_STRIP
					GL.DrawElements(All.TriangleStrip, number * 6, All.UnsignedShort, _indices.Data);
#else
					GL.DrawElements(All.Triangles, number * 6, All.UnsignedShort, _indices.Data);
#endif
				}	
			}
		}
	}
}

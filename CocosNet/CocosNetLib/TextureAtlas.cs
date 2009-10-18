
using System;
using CocosNet.Support;
using CocosNet.Base;
using System.Collections.Generic;
using OpenTK.Graphics.ES11;
using System.Collections;

namespace CocosNet {


	public class TextureAtlas {
		#region AtlasList
		/// <summary>
		/// This class offers the inteface of a standard List.
		/// But lets you directly access the backing array.
		/// It is used by the TextureAtlas to efficiently send the quads
		/// and indices over to OpenGL without having to directly much with an array.
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

			System.Collections.IEnumerator IEnumerable.GetEnumerator() {
				throw new System.NotImplementedException();
			}

			public IEnumerator<T> GetEnumerator() {
				throw new System.NotImplementedException();
			}

			public void Add(T item) {
				++_index;
				if (_index >= _data.Length) {
					DoubleArray();
				}
				_data[_index] = item;
				_index++;
			}

			public void Clear() {
				_data = new T[0];
				_index = -1;
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

			public int Count {
				get { return _index + 1; }
			}

			public bool IsReadOnly {
				get { return false; }
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

		public TextureAtlas(Texture2D tex, int capacity) {
			_capacity = capacity;
			
			Texture = tex;
			
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
					const int VertexOffset = 0;
					const int PointSize = 24;
					
					//GL.VertexPointer(3, All.Float, PointSize, (IntPtr)(quadP + VertexOffset));
					GL.VertexPointer(3, All.Float, PointSize, quads);
					
					const int ColorOffset = 12;
					
					GL.ColorPointer(4, All.UnsignedByte, PointSize, (IntPtr)(quadP + ColorOffset));
					
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

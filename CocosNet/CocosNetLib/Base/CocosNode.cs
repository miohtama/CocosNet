// CocosNet, Cocos2D in C#
// Copyright 2009 Matthew Greer
// See LICENSE file for license, and README and AUTHORS for more info

using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.CoreGraphics;
using CocosNet.Actions;
using Action = CocosNet.Actions.Action;
using CocosNet.Support;

namespace CocosNet.Base {
	public class CocosNode {
		public const int CocosNodeTagInvalid = -1;

		private int _zorder;
		private Camera _camera;
		private PointF _transformAnchor;
		private float _rotation;
		private bool _isTransformDirty;
		private float _scaleX;
		private float _scaleY;
		private PointF _position;
		private PointF _anchorPoint;
		private bool _isRelativeAnchorPoint;
		private SizeF _contentSize;
		private CGAffineTransform _transform;

		private List<CocosNode> _children;

		public IList<CocosNode> Children {
			get { return _children; }
		}

		public bool IsRunning { get; private set; }

		public int ZOrder {
			get { return _zorder; }
		}
		public float VertexZ { get; set; }
		public Camera Camera {
			get {
				if (_camera == null) {
					_camera = new Camera();
				}
				return _camera;
			}
		}
		public GridBase Grid { get; set; }
		public bool Visible { get; set; }
		public CocosNode Parent { get; set; }
		public int Tag { get; set; }
		public object UserData { get; set; }

		public virtual float Rotation {
			get { return _rotation; }
			set {
				if (_rotation != value) {
					_rotation = value;
					_isTransformDirty = true;
				}
			}
		}

		public float ScaleX {
			get { return _scaleX; }
			set {
				_scaleX = value;
				_isTransformDirty = true;
			}
		}

		public float ScaleY {
			get { return _scaleY; }
			set {
				_scaleY = value;
				_isTransformDirty = true;
			}
		}

		public float Scale {
			get {
				if (ScaleX == ScaleY) {
					return ScaleX;
				} else {
					throw new InvalidOperationException("ScaleX and ScaleY do not match");
				}
			}
			set {
				ScaleX = ScaleY = value;
				_isTransformDirty = true;
			}
		}

		public PointF TransformAnchor {
			get { return _transformAnchor; }
			set {
				_transformAnchor = value;
				_isTransformDirty = true;
			}
		}

		public PointF AnchorPoint {
			get { return _anchorPoint; }
			set {
				if (_anchorPoint != value) {
					_anchorPoint = value;
					TransformAnchor = new PointF(ContentSize.Width * _anchorPoint.X, ContentSize.Height * _anchorPoint.Y);
				}
			}
		}

		public virtual SizeF ContentSize {
			get { return _contentSize; }
			set {
				if (_contentSize != value) {
					_contentSize = value;
					TransformAnchor = new PointF(_contentSize.Width * AnchorPoint.X, _contentSize.Height * AnchorPoint.Y);
				}
			}
		}

		public bool IsRelativeAnchorPoint {
			get { return _isRelativeAnchorPoint; }
			set {
				_isRelativeAnchorPoint = value;
				_isTransformDirty = true;
			}
		}

		public CocosNode() {
			IsRunning = false;
			_rotation = 0;
			ScaleX = ScaleY = 1;
			TransformAnchor = PointF.Empty;
			AnchorPoint = PointF.Empty;
			ContentSize = SizeF.Empty;
			IsRelativeAnchorPoint = true;
			_isTransformDirty = true;
			Grid = null;
			Visible = true;
			Tag = CocosNodeTagInvalid;
			_zorder = 0;
			_camera = null;
			_children = new List<CocosNode>(4);
			UserData = null;
			_position = PointF.Empty;
		}

		public void CleanUp() {
			StopAllActions();
			
			foreach (CocosNode child in _children) {
				child.CleanUp();
			}
		}

		public CocosNode AddChild(CocosNode child) {
			return AddChild(child, child.ZOrder, child.Tag);
		}

		public CocosNode AddChild(CocosNode child, int zOrder) {
			return AddChild(child, zOrder, child.Tag);
		}

		public virtual CocosNode AddChild(CocosNode child, int z, int tag) {
			if (child == null) {
				throw new ArgumentNullException("child");
			}
			if (child.Parent != null) {
				throw new ArgumentException("Child already has a parent", "child");
			}
			
			child.Tag = tag;
			
			InsertChild(child, z);
			child.Parent = this;
			
			if (IsRunning) {
				child.OnEnter();
			}
			
			return this;
		}

		/// <summary>
		/// Changing an object's position is done so often
		/// it's probably cheaper to set it existing PointF
		/// object than to constantly create new ones
		/// </summary>
		public void SetPosition(float x, float y) {
			_position.X = x;
			_position.Y = y;
			
			_isTransformDirty = true;
		}

		public void SetPosition(PointF point) {
			_position = point;
			_isTransformDirty = true;
		}

		public void MoveBy(float dx, float dy) {
			_position.X = _position.X + dx;
			_position.Y = _position.Y + dy;
			
			_isTransformDirty = true;
		}

		public PointF Position {
			get { return _position; }
		}

		private void InsertChild(CocosNode child, int z) {
			int i = 0;
			bool added = false;
			
			foreach (CocosNode node in _children) {
				if (node.ZOrder > z) {
					added = true;
					_children.Insert(i, child);
					break;
				}
				++i;
			}
			
			if (!added) {
				_children.Add(child);
			}
			
			child._zorder = z;
		}

//		private void ReorderChild(CocosNode child, int z) {
//			if (child == null) {
//				throw new ArgumentNullException("child");
//			}
//			
//			_children.Remove(child);
//			InsertChild(child, z);
//		}

		public virtual void RemoveChild(CocosNode child, bool cleanup) {
			if (child != null) {
				if (Children.Contains(child)) {
					DetachChild(child, cleanup);
				}
			}
		}

		public void RemoveChildByTag(int tag, bool cleanup) {
			RemoveChild(GetChildByTag(tag), cleanup);
		}

		public void RemoveAllChildren(bool cleanup) {
			_children.Each(delegate(CocosNode child) {
				if (IsRunning) {
					child.OnExit();
				}
				
				if (cleanup) {
					child.CleanUp();
				}
				
				child.Parent = null;
			});
			
			_children.Clear();
		}

		private void DetachChild(CocosNode child, bool cleanup) {
			if (IsRunning) {
				child.OnExit();
			}
			
			if (cleanup) {
				child.CleanUp();
			}
			
			child.Parent = null;
			
			_children.Remove(child);
		}

		public CocosNode GetChildByTag(int tag) {
			return _children.Where(child => child.Tag == tag).FirstOrDefault();
		}

		public virtual void Draw() {
		}

		public virtual void Visit() {
			if (!Visible) {
				return;
			}
			
			GL.PushMatrix();
			
			if (Grid != null && Grid.Active) {
				Grid.BeforeDraw();
				TransformAncestors();
			}
			
			Transform();
			
			foreach (CocosNode child in _children) {
				if (child.ZOrder < 0) {
					child.Visit();
				} else {
					break;
				}
			}
			
			Draw();
			
			foreach (CocosNode child in _children) {
				if (child.ZOrder >= 0) {
					child.Visit();
				}
			}
			
			if (Grid != null && Grid.Active) {
				Grid.AfterDraw(Camera);
			}
			
			GL.PopMatrix();
		}

		private void TransformAncestors() {
			if (Parent != null) {
				Parent.TransformAncestors();
				Parent.Transform();
			}
		}

		public void Transform() {
			if (!(Grid != null && Grid.Active)) {
				Camera.Locate();
			}
			
			if (IsRelativeAnchorPoint && (TransformAnchor.X != 0 || TransformAnchor.Y != 0)) {
				GL.Translate(-TransformAnchor.X, -TransformAnchor.Y, VertexZ);
			}
			
			if (TransformAnchor.X != 0 || TransformAnchor.Y != 0) {
				GL.Translate(Position.X + TransformAnchor.X, Position.Y + TransformAnchor.Y, VertexZ);
			} else if (Position.X != 0 || Position.Y != 0) {
				GL.Translate(Position.X, Position.Y, VertexZ);
			}
			
			if (Rotation != 0) {
				GL.Rotate(-Rotation, 0, 0, 1);
			}
			
			if (ScaleX != 1 || ScaleY != 1) {
				GL.Scale(ScaleX, ScaleY, 1);
			}
			
			if (TransformAnchor.X != 0 || TransformAnchor.Y != 0) {
				GL.Translate(-TransformAnchor.X, -TransformAnchor.Y, VertexZ);
			}
		}

		public virtual void OnEnter() {
			foreach (CocosNode child in _children) {
				child.OnEnter();
			}
			
			//ActivateTimers();
			
			IsRunning = true;
		}

		public void OnEnterTransitionDidFinish() {
			foreach (CocosNode child in _children) {
				child.OnEnterTransitionDidFinish();
			}
		}

		public virtual void OnExit() {
			//DeactivateTimers();
			
			IsRunning = false;
			
			foreach (CocosNode child in _children) {
				child.OnExit();
			}
		}

		public virtual Action RunAction(Action action) {
			if (action == null) {
				throw new ArgumentNullException("action");
			}
			
			ActionManager.Instance.AddAction(action, this);
			
			return action;
		}

		public void StopAllActions() {
			ActionManager.Instance.RemoveAllActionsForTarget(this);
		}

		public void StopAction(Action action) {
			ActionManager.Instance.RemoveAction(action);
		}

		#region Transform
		private CGAffineTransform NodeToParentTransform() {
			if (_isTransformDirty) {
				_transform = CGAffineTransform.MakeIdentity();
				
				if (!IsRelativeAnchorPoint) {
					_transform.Translate(TransformAnchor.X, TransformAnchor.Y);
				}
				
				_transform.Translate((int)Position.X, (int)Position.Y);
				_transform.Rotate(Rotation.ToRadians());
				_transform.Scale(ScaleX, ScaleY);
				_transform.Translate(-TransformAnchor.X, -TransformAnchor.Y);
				
				_isTransformDirty = false;
			}
			
			return _transform;
		}

		private CGAffineTransform NodeToWorldTransform() {
			CGAffineTransform t = NodeToParentTransform();
			
			for (CocosNode parent = Parent; parent != null; parent = parent.Parent) {
				t = MTSub.CGAffineTransform.Concat(t, parent.NodeToParentTransform());
			}
			
			return t;
		}

		private CGAffineTransform WorldToNodeTransform() {
			CGAffineTransform t = NodeToWorldTransform();
			
			return MTSub.CGAffineTransform.Invert(t);
		}

		public PointF ConvertToNodeSpace(PointF worldPoint) {
			return WorldToNodeTransform().TransformPoint(worldPoint);
		}

		public PointF ConvertToWorldSpace(PointF nodePoint) {
			return NodeToWorldTransform().TransformPoint(nodePoint);
		}
		
		#endregion
		
		#region Timers
		
		
		
		
		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CocosNet.Base;

namespace CocosNet.Tiles {
	public class ParallaxNode : CocosNode {
		class NodeBox {
			public PointF Ratio { get; set; }
			public PointF Offset { get; set; }
			public CocosNode Child { get; set; }

			public NodeBox(PointF ratio, PointF offset, CocosNode child) {
				if (child == null) {
					throw new ArgumentNullException("child");
				}
				
				Ratio = ratio;
				Offset = offset;
				Child = child;
			}
		}

		private List<NodeBox> _parallaxNodes;
		private PointF _lastPosition;

		private PointF GetAbsolutePosition() {
			PointF pos = Position;
			
			CocosNode node = this;
			
			while (node.Parent != null) {
				node = node.Parent;
				pos.X += node.Position.X;
				pos.Y += node.Position.Y;
			}
			
			return pos;
		}

		public ParallaxNode() {
			_parallaxNodes = new List<NodeBox>(5);
			_lastPosition = new PointF(-100, -100);
		}

		public override CocosNode AddChild(CocosNode child, int z, int tag) {
			throw new Exception("Use AddChild(CocosNode node, int z, PointF parallaxRatio, PointF positionOffset) instead");
		}

		public CocosNode AddChild(CocosNode child, int z, PointF parallaxRatio, PointF positionOffset) {
			if (child == null) {
				throw new ArgumentNullException("child");
			}
			
			NodeBox box = new NodeBox(parallaxRatio, positionOffset, child);
			_parallaxNodes.Add(box);
			
			float x = Position.X * parallaxRatio.X + positionOffset.X;
			float y = Position.Y * parallaxRatio.Y + positionOffset.Y;
			child.SetPosition(x, y);
			
			return base.AddChild(child, z, child.Tag);
		}

		public override void Visit() {
			PointF pos = GetAbsolutePosition();

			if (pos.X != _lastPosition.X || pos.Y != _lastPosition.Y) {
				foreach (NodeBox box in _parallaxNodes) {
					float x = -pos.X + pos.X * box.Ratio.X + box.Offset.X;
					float y = -pos.Y + pos.Y * box.Ratio.Y + box.Offset.Y;
					box.Child.SetPosition(x, y);
				}
				
				_lastPosition.X = pos.X;
				_lastPosition.Y = pos.Y;
			}
			
			base.Visit();
		}
		
	}
}

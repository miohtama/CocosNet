using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosNet.Base;

namespace CocosNet.Tiles {
    public class TileMapAtlas : AtlasNode {
		
		protected override void UpdateAtlasValues ()
		{
			throw new System.NotImplementedException ();
		}

		
		public TileMapAtlas(string tileFile, string mapFile, int tileWidth, int tileHeight)
		 : base(tileFile, tileWidth, tileHeight, 0) {
			throw new NotImplementedException("TileMapAtlas, implement this!");
		}
    }
}

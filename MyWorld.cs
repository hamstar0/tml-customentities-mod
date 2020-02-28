using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.XnaHelpers;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace CustomEntities {
	class CustomEntitiesWorld : ModWorld {
		public override void PostDrawTiles() {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			Player player = Main.LocalPlayer;
			if( player == null ) { return; }
			RasterizerState rasterizer = Main.gameMenu ||
					(double)player.gravDir == 1.0 ?
					RasterizerState.CullCounterClockwise : RasterizerState.CullClockwise;

			bool _;
			XnaHelpers.DrawBatch(
				(sb) => {
					var mymod = (CustomEntitiesMod)this.mod;
					mymod.CustomEntMngr?.DrawPostTilesAll( sb );
				},
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				Main.DefaultSamplerState,
				DepthStencilState.None,
				rasterizer,
				(Effect)null,
				Main.GameViewMatrix.TransformationMatrix,
				out _
			);
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}
	}
}

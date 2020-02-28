﻿using CustomEntities.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace CustomEntities {
	partial class CustomEntitiesMod : Mod {
		private void DrawMiniMapForAll( SpriteBatch sb ) {
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<DrawsOnMapEntityComponent>();

			foreach( var ent in ents ) {
				var mapComp = ent.GetComponentByType<DrawsOnMapEntityComponent>();

				if( Main.mapStyle == 1 ) {
					mapComp.DrawMiniMap( sb, ent );
				} else {
					mapComp.DrawOverlayMap( sb, ent );
				}
			}
		}


		private void DrawFullMapForAll( SpriteBatch sb ) {
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<DrawsOnMapEntityComponent>();

			foreach( var ent in ents ) {
				var mapComp = ent.GetComponentByType<DrawsOnMapEntityComponent>();

				mapComp.DrawFullscreenMap( sb, ent );
			}
		}
	}
}

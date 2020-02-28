using CustomEntities.Components;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace CustomEntities {
	class CustomEntitiesNPC : GlobalNPC {
		public override bool PreAI( NPC npc ) {
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_A", 1 );
			var mymod = (CustomEntitiesMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusNpcEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				var hitRadComp = ent.GetComponentByType<HitRadiusNpcEntityComponent>();
				float radius = hitRadComp.GetRadius( ent );

				if( Vector2.Distance( ent.Core.Center, npc.Center ) <= radius ) {
					int dmg = npc.damage;

					if( hitRadComp.PreHurt( ent, npc, ref dmg ) ) {
						hitRadComp.PostHurt( ent, npc, dmg );
					}
				}
			}
			
//DataStore.Add( DebugHelpers.GetCurrentContext()+"_"+npc.whoAmI+":"+npc.type+"_B", 1 );
			return true;
		}
	}
}

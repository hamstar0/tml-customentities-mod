﻿using Terraria;


namespace CustomEntities.Components {
	public abstract class HitRadiusPlayerEntityComponent : CustomEntityComponent {
		public abstract float GetRadius( CustomEntity ent );


		////////////////

		public virtual bool PreHurt( CustomEntity ent, Player player, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, Player player, int damage );
	}
}

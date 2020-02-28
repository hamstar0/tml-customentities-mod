﻿using Terraria;


namespace CustomEntities.Components {
	public abstract class HitRadiusProjectileEntityComponent : CustomEntityComponent {
		public abstract float GetRadius( CustomEntity ent );


		////////////////

		public virtual bool PreHurt( CustomEntity ent, Projectile projectile, ref int damage ) {
			return true;
		}
		public abstract void PostHurt( CustomEntity ent, Projectile projectile, int damage );
	}
}

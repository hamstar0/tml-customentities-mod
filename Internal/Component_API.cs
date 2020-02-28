﻿using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace CustomEntities {
	public abstract partial class CustomEntityComponent : PacketProtocolData {
		public class StaticInitializer {
			internal StaticInitializer() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}

			protected virtual void StaticInitialize() { }
		}



		////////////////

		protected virtual void OnEntityInitialize( CustomEntity ent ) { }

		protected virtual void OnAddToWorld( CustomEntity ent ) { }

		public virtual void UpdateSingle( CustomEntity ent ) { }
		public virtual void UpdateClient( CustomEntity ent ) { }
		public virtual void UpdateServer( CustomEntity ent ) { }
	}
}

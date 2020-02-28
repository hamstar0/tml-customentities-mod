﻿using HamstarHelpers.Components.Network;
using Newtonsoft.Json;
using Terraria.Utilities;


namespace CustomEntities.Components {
	public class PeriodicSyncEntityComponent : CustomEntityComponent {
		public const int RandomSyncDurationMin = 60 * 3;	// 3 minutes
		public const int RandomSyncDurationMax = 60 * 15;	// 15 minutes


		////////////////

		public static int GetRandomSyncDuration() {
			int range = PeriodicSyncEntityComponent.RandomSyncDurationMax - PeriodicSyncEntityComponent.RandomSyncDurationMin;
			return PeriodicSyncEntityComponent.MyRand.Next( range ) + PeriodicSyncEntityComponent.RandomSyncDurationMin;
		}



		////////////////

		private static UnifiedRandom MyRand = new UnifiedRandom();


		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		private int NextSync;



		////////////////

		private PeriodicSyncEntityComponent() {
			this.NextSync = PeriodicSyncEntityComponent.GetRandomSyncDuration();
		}

		protected PeriodicSyncEntityComponent( object _=null ) { }

		////

		protected override void OnClone() { }


		////////////////

		public override void UpdateClient( CustomEntity ent ) {
			if( ent.SyncFromClient ) {
				this.UpdateMe( ent );
			}
		}

		public override void UpdateServer( CustomEntity ent ) {
			if( ent.SyncFromServer ) {
				this.UpdateMe( ent );
			}
		}

		////

		protected virtual bool UpdateMe( CustomEntity ent ) {
			if( this.NextSync-- <= 0 ) {
				this.NextSync = PeriodicSyncEntityComponent.GetRandomSyncDuration();

				ent.SyncToAll();
				return true;
			}
			return false;
		}
	}
}

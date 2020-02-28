﻿using CustomEntities.NetProtocols;
using HamstarHelpers.Components.CustomEntity.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.NetProtocols;
using System;
using System.IO;
using Terraria;


namespace CustomEntities {
	public abstract partial class CustomEntity : PacketProtocolData {
		public void SyncToAll() {
			if( !this.IsInitialized ) {
				//throw new HamstarException( "!ModHelpers.CustomEntity.SyncToAll ("+this.GetType().Name+") - Not initialized." );
				throw new HamstarException( "Not initialized." );
			}
			if( !SaveableEntityComponent.HaveAllEntitiesLoaded ) {
				//LogHelpers.Log( "!ModHelpers.CustomEntity.SyncToAll ("+this.GetType().Name+") - Entities not yet loaded." );
				LogHelpers.Alert( "Entities not yet loaded." );
				return;
			}

			if( CustomEntitiesMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Alert( "Syncing..." );
			}

			if( Main.netMode == 0 ) {
				//throw new HamstarException( "!ModHelpers.CustomEntity.SyncToAll (" + this.GetType().Name + ") - Multiplayer only." );
				throw new HamstarException( "Multiplayer only." );
			}

			if( Main.netMode == 2 ) {
				CustomEntityProtocol.SendToClients( this );
			} else if( Main.netMode == 1 ) {
				CustomEntityProtocol.SyncToAll( this );
			}
		}


		////////////////

		protected override void WriteStream( BinaryWriter writer ) {
			throw new HamstarException( "WriteStream not implemented." );
		}
		protected override void ReadStream( BinaryReader reader ) {
			throw new HamstarException( "ReadStream not implemented." );
		}
	}
}

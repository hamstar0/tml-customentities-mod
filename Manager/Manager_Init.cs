﻿using CustomEntities.NetProtocols;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Services.DataDumper;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Services.Timers;
using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using ReflectionHelpers = HamstarHelpers.Helpers.DotNetHelpers.Reflection.ReflectionHelpers;


namespace CustomEntities {
	public partial class CustomEntityManager {
		internal CustomEntityManager() {
			this.InitializeUpdate();
			this.InitializeEntityTypes();
			this.InitializeComponentTypes();
			this.InitializeDrawingLayer();
			
			// Reset any data from previous games
			Promises.AddPostWorldUnloadEachPromise( () => {
				lock( CustomEntityManager.MyLock ) {
					this.WorldEntitiesByIndexes.Clear();
					this.WorldEntitiesByComponentType.Clear();
				}
			} );

			// Bind 'data dump' hotkey
			DataDumper.SetDumpSource( "CustomEntityList", () => {
				lock( CustomEntityManager.MyLock ) {
					return string.Join( "\n  ", this.WorldEntitiesByIndexes.OrderBy( kv => kv.Key )
									.SafeSelect( kv => kv.Key + ": " + kv.Value?.ToString() ?? "null" ) );
				}
			} );

			// Refresh entity owners on player connect and sync entities to player
			if( Main.netMode == 2 ) {
				Promises.AddValidatedPromise<PlayerLogicPromiseArguments>( PlayerLogic.ServerConnectValidator, ( args ) => {
					if( this.WorldEntitiesByIndexes.Count == 0 ) { return true; }

					foreach( var ent in this.WorldEntitiesByIndexes.Values ) {
						ent.RefreshOwnerWho();
					}
					
					PacketProtocolSendToClient.QuickSend<CustomEntityAllProtocol>( args.Who, -1 );

					return true;
				} );
			}
		}

		////

		~CustomEntityManager() {
			if( !Main.dedServ ) {
				Main.OnPostDraw -= CustomEntityManager._DrawPostDrawAll;
			}
			Main.OnTick -= CustomEntityManager._Update;
		}


		////////////////

		private void InitializeUpdate() {
			this.OnTickGet = Timers.MainOnTickGet();
			Main.OnTick += CustomEntityManager._Update;
		}


		private void InitializeEntityTypes() {
			var entityTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof( CustomEntity ) );

			foreach( Type entityType in entityTypes.OrderBy( e => e.Name ) ) {
				this.CacheTypeIdInfo( entityType );
			}
		}


		private void InitializeComponentTypes() {
			var componentTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof( CustomEntityComponent ) );

			foreach( var componentType in componentTypes ) {
				Type[] nestedTypes = componentType.GetNestedTypes( BindingFlags.Public | BindingFlags.NonPublic );
				if( nestedTypes == null ) { continue; }

				foreach( var nestedType in nestedTypes ) {
					if( nestedType.IsSubclassOf( typeof( CustomEntityComponent.StaticInitializer ) ) ) {
						var staticInit = (CustomEntityComponent.StaticInitializer)Activator.CreateInstance( nestedType );
						staticInit.StaticInitializationWrapper();
					}
				}
			}
		}


		private void InitializeDrawingLayer() {
			if( !Main.dedServ ) {
				Overlays.Scene["CustomEntity"] = new CustomEntityOverlay();
				Overlays.Scene.Activate( "CustomEntity" );

				Main.OnPostDraw += CustomEntityManager._DrawPostDrawAll;
			}
		}
	}
}

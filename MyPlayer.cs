using CustomEntities.Components;
using CustomEntities.NetProtocols;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace CustomEntities {
	class PlayerPromiseArguments : PromiseArguments {
		public int Who;
	}



	class CustomEntitiesPlayer : ModPlayer {
		internal readonly static object MyValidatorKey;
		internal readonly static PromiseValidator LoadValidator;
		internal readonly static PromiseValidator SaveValidator;


		////////////////

		static CustomEntitiesPlayer() {
			CustomEntitiesPlayer.MyValidatorKey = new object();
			CustomEntitiesPlayer.LoadValidator = new PromiseValidator( CustomEntitiesPlayer.MyValidatorKey );
			CustomEntitiesPlayer.SaveValidator = new PromiseValidator( CustomEntitiesPlayer.MyValidatorKey );
		}



		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void Initialize() {
		}

		public override void clientClone( ModPlayer clientClone ) {
			var clone = (CustomEntitiesPlayer)clientClone;
		}


		////////////////

		public override void SyncPlayer( int toWho, int fromWho, bool newPlayer ) {
			if( Main.netMode == 1 ) {
				if( toWho == -1 && fromWho == -1 && newPlayer ) {
					//this.Logic.OnCurrentClientConnect( this.player );
				}
			}
			if( Main.netMode == 2 ) {
				if( toWho == -1 && fromWho == this.player.whoAmI ) {
					//this.Logic.OnServerConnect( Main.player[fromWho] );
				}
			}
		}

		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			if( Main.netMode == 0 ) {
				this.OnConnectSingle();
			} else if( Main.netMode == 1 ) {
				this.OnConnectClient();
			}
		}


		////////////////

		private void OnConnectSingle() {
		}

		private void OnConnectClient() {
			PacketProtocolRequestToServer.QuickRequest<ModSettingsProtocol>( -1 );
		}

		private void OnConnectServer() {
		}


		////////////////

		public override void Load( TagCompound tags ) {
			try {
				var args = new PlayerPromiseArguments { Who = this.player.whoAmI };

				Promises.TriggerValidatedPromise( CustomEntitiesPlayer.LoadValidator, CustomEntitiesPlayer.MyValidatorKey, args );
			} catch( Exception e ) {
				if( !(e is HamstarException) ) {
					throw new HamstarException( e.ToString() );
				}
			}
		}

		public override TagCompound Save() {
			var tags = new TagCompound();
			try {
				var args = new PlayerPromiseArguments { Who = this.player.whoAmI };
			
				Promises.TriggerValidatedPromise( CustomEntitiesPlayer.SaveValidator, CustomEntitiesPlayer.MyValidatorKey, args );
			} catch( Exception e ) {
				if( !(e is HamstarException) ) {
					throw new HamstarException( e.ToString() );
				}
			}

			return tags;
		}


		////////////////

		public override void PreUpdate() {
			var mymod = (CustomEntitiesMod)this.mod;
			ISet<CustomEntity> ents = CustomEntityManager.GetEntitiesByComponent<HitRadiusPlayerEntityComponent>();

			foreach( CustomEntity ent in ents ) {
				var hitRadComp = ent.GetComponentByType<HitRadiusPlayerEntityComponent>();
				float radius = hitRadComp.GetRadius( ent );

				if( Vector2.Distance( ent.Core.Center, this.player.Center ) <= radius ) {
					int dmg = 0;
					if( hitRadComp.PreHurt( ent, this.player, ref dmg ) ) {
						hitRadComp.PostHurt( ent, this.player, dmg );
					}
				}
			}
		}
	}
}

﻿using CustomEntities.Components;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Services.Promises;
using System;
using System.Collections.Generic;
using Terraria;


namespace CustomEntities {
	public partial class CustomEntityManager {
		public static void AddToWorld( CustomEntity ent ) {
			CustomEntityManager mngr = CustomEntitiesMod.Instance.CustomEntMngr;
			int who = mngr.WorldEntitiesByIndexes.Count + 1;

			if( !ent.SyncFromClient && !ent.SyncFromServer ) {
				who = -who;
			}

			CustomEntityManager.AddToWorld( who, ent );
		}


		public static CustomEntity AddToWorld( int who, CustomEntity ent, bool skipSync = false ) {
			//if( ent == null ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Null ent not allowed." ); }
			//if( !ent.IsInitialized ) { throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - Initialized ents only." ); }
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }
			if( !ent.IsInitialized ) { throw new HamstarException( "Initialized ents only." ); }

			CustomEntityManager mngr = CustomEntitiesMod.Instance.CustomEntMngr;
			CustomEntity realEnt = ent;

			if( mngr.WorldEntitiesByIndexes.ContainsKey(who) ) {
				//throw new HamstarException( "!ModHelpers.CustomEntityManager.AddToWorld - "
				//	+ "Attempting to add "+ent.ToString()+" to slot "+who+" occupied by "+mngr.WorldEntitiesByIndexes[who].ToString() );
				throw new HamstarException( "Attempting to add "+ent.ToString()+" to slot "+who+" occupied by "
						+ mngr.WorldEntitiesByIndexes[who].ToString() );
			}

			if( ent is SerializableCustomEntity ) {
				realEnt = ( (SerializableCustomEntity)ent ).Convert();
			}

			Type compType;
			Type baseType = typeof( CustomEntityComponent );

			// Map entity to each of its components
			foreach( CustomEntityComponent component in realEnt.InternalComponents ) {
				compType = component.GetType();
				lock( CustomEntityManager.MyLock ) {
					do {
						if( !mngr.WorldEntitiesByComponentType.ContainsKey( compType ) ) {
							mngr.WorldEntitiesByComponentType[compType] = new HashSet<int>();
						}

						mngr.WorldEntitiesByComponentType[compType].Add( who );

						compType = compType.BaseType;
					} while( compType != baseType );
				}
			}

			realEnt.Core.whoAmI = who;
			mngr.WorldEntitiesByIndexes[ who ] = realEnt;

			realEnt.InternalOnAddToWorld();

			// Sync also
			if( !skipSync ) {
				if( Main.netMode == 1 ) {
					if( ent.SyncFromClient ) {
						Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
							ent.SyncToAll();
							return false;
						} );
					}
				} else if( Main.netMode == 2 ) {
					if( ent.SyncFromServer ) {
						Promises.AddValidatedPromise( SaveableEntityComponent.LoadAllValidator, () => {
							ent.SyncToAll();
							return false;
						} );
					}
				}
			}

			if( CustomEntitiesMod.Instance.Config.DebugModeCustomEntityInfo ) {
				LogHelpers.Alert( "Set " + realEnt.ToString() );
			}

			return realEnt;
		}


		////////////////

		public static void RemoveEntity( CustomEntity ent ) {
			if( ent == null ) { throw new HamstarException( "Null ent not allowed." ); }

			CustomEntityManager.RemoveEntityByWho( ent.Core.whoAmI );
		}

		public static void RemoveEntityByWho( int who ) {
			CustomEntityManager mngr = CustomEntitiesMod.Instance.CustomEntMngr;

			if( !mngr.WorldEntitiesByIndexes.ContainsKey( who ) ) { return; }

			Type compType;
			Type baseType = typeof( CustomEntityComponent );

			lock( CustomEntityManager.MyLock ) {
				IList<CustomEntityComponent> entComponents = mngr.WorldEntitiesByIndexes[who].InternalComponents;

				foreach( CustomEntityComponent component in entComponents ) {
					compType = component.GetType();
					do {
						if( mngr.WorldEntitiesByComponentType.ContainsKey( compType ) ) {
							mngr.WorldEntitiesByComponentType[compType].Remove( who );
						}

						compType = compType.BaseType;
					} while( compType != baseType );
				}

				mngr.WorldEntitiesByIndexes.Remove( who );
			}
		}


		public static void ClearAllEntities() {
			CustomEntityManager mngr = CustomEntitiesMod.Instance.CustomEntMngr;

			lock( CustomEntityManager.MyLock ) {
				mngr.WorldEntitiesByIndexes.Clear();
				mngr.WorldEntitiesByComponentType.Clear();
			}
		}
	}
}

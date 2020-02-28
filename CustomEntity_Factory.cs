﻿using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.PlayerHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;


namespace CustomEntities {
	public abstract partial class CustomEntity : PacketProtocolData {
		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components ) {
			var ent = (CustomEntity)Activator.CreateInstance( mytype, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = "";

			ent.OnClone();

			return ent;
		}

		internal static CustomEntity CreateRaw( Type mytype, CustomEntityCore core, IList<CustomEntityComponent> components, string ownerUid="" ) {
			var ent = (CustomEntity)Activator.CreateInstance( mytype, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { }, null );
			ent.Core = core;
			ent.Components = components;
			ent.OwnerPlayerWho = -1;
			ent.OwnerPlayerUID = ownerUid;
			
			Player plr = PlayerIdentityHelpers.GetPlayerByProperId( ownerUid );
			if( plr != null ) {
				ent.OwnerPlayerWho = plr.whoAmI;
			}

			ent.OnClone();

			return ent;
		}
	}
}

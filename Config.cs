using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Components.Config;
using System;
using Terraria;


namespace CustomEntities {
	public class CustomEntitiesConfigData : ConfigurationDataBase {
		public static string ConfigFileName => "Custom Entities Config.json";


		////////////////

		public string VersionSinceUpdate = new Version(0,0,0,0).ToString();
		
		public bool DebugModeResetCustomEntities = false;
		public bool DebugModeCustomEntityInfo = false;



		////////////////

		internal void SetDefaults() {
		}

		internal bool UpdateToLatestVersion() {
			var mymod = CustomEntitiesMod.Instance;
			var newConfig = new CustomEntitiesConfigData();
			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();
			
			if( versSince >= mymod.Version ) {
				return false;
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}


		////////////////

		internal void LoadFromNetwork( CustomEntitiesConfigData config ) {
			var mymod = CustomEntitiesMod.Instance;
			mymod.ConfigJson.SetData( config );
		}
	}
}

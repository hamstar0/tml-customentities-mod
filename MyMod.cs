using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;
using Terraria.ModLoader;


namespace CustomEntities {
	partial class CustomEntitiesMod : Mod {
		public static CustomEntitiesMod Instance { get; private set; }



		////////////////

		internal CustomEntityManager CustomEntMngr;

		////////////////

		public JsonConfig<CustomEntitiesConfigData> ConfigJson { get; private set; }
		public CustomEntitiesConfigData Config => this.ConfigJson.Data;



		////////////////

		public CustomEntitiesMod() {
			CustomEntitiesMod.Instance = this;

			this.LoadConfig();

			this.CustomEntMngr = new CustomEntityManager();
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.Config.SetDefaults();
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "Custom Entities config " + this.Version.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Custom Entities updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			this.CustomEntMngr = null;

			CustomEntitiesMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( CustomEntitiesAPI ), args );
		}
	}
}

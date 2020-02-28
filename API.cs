using Terraria;


namespace CustomEntities {
	public static partial class CustomEntitiesAPI {
		public static CustomEntitiesConfigData GetModSettings() {
			return CustomEntitiesMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			CustomEntitiesMod.Instance.ConfigJson.SaveFile();
		}
	}
}

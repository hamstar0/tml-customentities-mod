using HamstarHelpers.Components.Network;


namespace CustomEntities.NetProtocols {
	class ModSettingsProtocol : PacketProtocolRequestToServer {
		public CustomEntitiesConfigData Data;



		////////////////

		private ModSettingsProtocol() { }

		////////////////

		protected override void InitializeServerSendData( int fromWho ) {
			this.Data = (CustomEntitiesConfigData)CustomEntitiesMod.Instance.Config.Clone();
		}

		////////////////

		protected override void ReceiveReply() {
			CustomEntitiesMod.Instance.Config.LoadFromNetwork( this.Data );
		}
	}
}

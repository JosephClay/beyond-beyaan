namespace Beyond_Beyaan.Data_Modules
{
	public class PlanetType
	{
		#region Graphics
		public GorgonLibrary.Graphics.Sprite Sprite { get; set; }
		public GorgonLibrary.Graphics.Sprite LargeSprite { get; set; }
		public GorgonLibrary.Graphics.Sprite GroundView { get; set; }
		#endregion

		#region Planet Properties
		public string Name { get; set; }
		public string InternalName { get; set; }
		public string Description { get; set; }
		public string PlanetTerraformsTo { get; set; } //which planet it terraforms, null if none
		public string PlanetDegradesTo { get; set; } //which planet it degrades to due to amount of pollution, null if none
		public int PollutionThreshold { get; set; } //How much pollution it can have before degrading
		public int ProductionCapacityRequiredForTerraforming { get; set; } //How much production capacity required to terraform this
		public int CostForTerraforming { get; set; }
		public string[] DefaultRegions { get; set; }
		public bool Inhabitable { get; set; }
		#endregion
	}
}

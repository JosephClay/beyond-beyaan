namespace Beyond_Beyaan.Data_Modules
{
	public class SectorObject
	{
		public SectorObjectType Type { get; private set; }
		public string Name { get; set; }

		public Empire ClaimedBy { get; set; }
		public StarSystem ConnectsTo { get; set; } //If this type is a gateway and connects directly

		public SectorObject(SectorObjectType type)
		{
			Type = type;
		}
	}
}

namespace Beyond_Beyaan.Data_Modules
{
	public enum SECTORTYPE { PLANET, DERELICT, EMPTY }

	public class Sector
	{
		public SECTORTYPE SectorType { get; private set; }

		public Planet Planet { get; private set; }

		public Empire Owner { get; set; }

		public string Name { get; set; }

		public Sector(SECTORTYPE type, Planet planet, string name, Empire owner)
		{
			SectorType = type;
			Planet = planet;
			Name = name;
			Owner = owner;
		}
	}
}

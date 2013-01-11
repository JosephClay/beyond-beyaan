using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	enum SECTORTYPE { PLANET, DERELICT, EMPTY }

	class Sector
	{
		public SECTORTYPE SectorType { get; private set; }

		public Planet Planet { get; private set; }

		public Sector(SECTORTYPE type, Planet planet)
		{
			SectorType = type;
			Planet = planet;
		}
	}
}

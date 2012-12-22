using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beyond_Beyaan.Data_Modules
{
	public enum LimitTo { PLANET, PLANET_DEVELOPMENT, SYSTEM, SYSTEM_DEVELOPMENT, EMPIRE, EMPIRE_DEVELOPMENT }

	public class Resource
	{
		public string Name { get; set; }
		public string Abbreviation { get; set; }
		public Icon Icon { get; set; }
		public bool Storable { get; set; }
		public Resource ConvertsTo { get; set; }
		public float ConversionRatio { get; set; }
		public LimitTo LimitTo { get; set; }
	}
}

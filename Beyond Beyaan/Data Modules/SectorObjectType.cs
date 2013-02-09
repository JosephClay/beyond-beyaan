using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Modules
{
	class SectorObjectType
	{
		public string Type { get; private set; }
		public string Name { get; private set; }
		public string Code { get; private set; }
		public bool IsInhabitable { get; private set; }
		public string Population { get; private set; }
		public bool LocalProjects { get; private set; }
		public string Sliders { get; private set; }
		public string Improvements { get; private set; }
		public string MiniImprovementType { get; private set; } //When a MiniImprovement is defined in the data file, this tells the game which type of miniImprovement it uses, if any
		public string Description { get; private set; }
		public bool IsGateway { get; private set; }
		public bool ConnectsToAnother { get; private set; }

		private XElement _element;

		public bool Load(XElement element, string type, out string reason)
		{
			SetDefaults();
			_element = element;
			Type = type;
			try
			{
				foreach (XAttribute attribute in element.Attributes())
				{
					switch (attribute.Name.ToString().ToLower())
					{
						case "name": Name = attribute.Value;
							break;
						case "code": Code = attribute.Value;
							break;
						case "inhabitable": IsInhabitable = bool.Parse(attribute.Value);
							break;
						case "localprojects": LocalProjects = bool.Parse(attribute.Value);
							break;
						case "sliders": Sliders = attribute.Value;
							break;
						case "improvements": Improvements = attribute.Value;
							break;
						case "miniimprovements": MiniImprovementType = attribute.Value;
							break;
						case "description": Description = attribute.Value;
							break;
						case "population": Population = attribute.Value;
							break;
						case "isgateway": IsGateway = bool.Parse(attribute.Value);
							break;
						case "connectstoanother": ConnectsToAnother = bool.Parse(attribute.Value);
							break;
					}
				}
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
			reason = null;
			return true;
		}

		private void SetDefaults()
		{
			Name = "Unnamed";
			Code = "Uncoded";
			IsInhabitable = false;
			Population = string.Empty;
			LocalProjects = false;
			Improvements = string.Empty;
			MiniImprovementType = string.Empty;
			Description = "No description";
			IsGateway = false;
			ConnectsToAnother = false;
		}
	}
}

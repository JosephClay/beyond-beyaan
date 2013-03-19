using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Beyond_Beyaan.Data_Modules
{
	public enum ConnectionAlgorithm { RANDOM, MINIMUM, CLOSEST, FARTHEST }
	public class SectorObjectType
	{
		public string Type { get; private set; }
		public string Name { get; private set; }
		public string GenerateName { get; private set; }
		public string Code { get; private set; }
		public bool IsInhabitable { get; private set; }
		public string Population { get; private set; }
		public bool LocalProjects { get; private set; }
		public string Sliders { get; private set; }
		public string Improvements { get; private set; }
		public string ImprovementType { get; private set; }
		public string MiniImprovementType { get; private set; } //When a MiniImprovement is defined in the data file, this tells the game which type of miniImprovement it uses, if any
		public string Description { get; private set; }
		public bool IsGateway { get; private set; }
		public System.Drawing.Color GatewayColor { get; private set; } //Color of the line
		public bool ConnectsToAnother { get; private set; }
		public ConnectionAlgorithm ConnectionAlgorithm { get; private set; }

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
						case "generatename": GenerateName = attribute.Value;
							break;
						case "inhabitable": IsInhabitable = bool.Parse(attribute.Value);
							break;
						case "localprojects": LocalProjects = bool.Parse(attribute.Value);
							break;
						case "sliders": Sliders = attribute.Value;
							break;
						case "improvements": Improvements = attribute.Value;
							break;
						case "improvementtype": ImprovementType = attribute.Value;
							break;
						case "miniimprovementtype": MiniImprovementType = attribute.Value;
							break;
						case "description": Description = attribute.Value;
							break;
						case "population": Population = attribute.Value;
							break;
						case "isgateway": IsGateway = bool.Parse(attribute.Value);
							break;
						case "gatewaycolor":
							{
								string[] rgb = attribute.Value.Split(new[] { ',' });
								GatewayColor = System.Drawing.Color.FromArgb(255, int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
							} break;
						case "connectstoanother": ConnectsToAnother = bool.Parse(attribute.Value);
							break;
						case "connectionalgorithm":
							{
								switch (attribute.Value.ToLower())
								{
									case "minimum": ConnectionAlgorithm = Data_Modules.ConnectionAlgorithm.MINIMUM;
										break;
									case "closest": ConnectionAlgorithm = Data_Modules.ConnectionAlgorithm.CLOSEST;
										break;
									case "farthest": ConnectionAlgorithm = Data_Modules.ConnectionAlgorithm.FARTHEST;
										break;
									case "random": ConnectionAlgorithm = Data_Modules.ConnectionAlgorithm.RANDOM;
										break;
								}
							} break;
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
			GenerateName = string.Empty;
			IsInhabitable = false;
			Population = string.Empty;
			LocalProjects = false;
			Improvements = string.Empty;
			MiniImprovementType = string.Empty;
			Description = "No description";
			IsGateway = false;
			ConnectsToAnother = false;
			ConnectionAlgorithm = Data_Modules.ConnectionAlgorithm.RANDOM;
		}
	}
}

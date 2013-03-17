using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class SectorTypeManager
	{
		private Dictionary<string, SectorObjectType> sectorTypes;

		public bool LoadSectorTypes(string filePath, GameMain gameMain, out string reason)
		{
			sectorTypes = new Dictionary<string, SectorObjectType>();

			gameMain.Log(string.Empty);
			gameMain.Log("Loading SectorTypes from " + filePath);

			try
			{
				XDocument doc = XDocument.Load(filePath);
				gameMain.Log("Getting \"SectorTypes\" Element");
				XElement root = doc.Element("SectorTypes");

				foreach (XElement sectorType in root.Elements())
				{
					foreach (XElement element in sectorType.Elements())
					{
						SectorObjectType type = new SectorObjectType();
						if (!type.Load(element, sectorType.Name.ToString(), out reason))
						{
							return false;
						}
						sectorTypes.Add(type.Code, type);
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

		public SectorObjectType GetType(string code)
		{
			if (sectorTypes.ContainsKey(code))
			{
				return sectorTypes[code];
			}
			return null;
		}

		public List<SectorObjectType> GetGatewayTypes()
		{
			List<SectorObjectType> gateways = new List<SectorObjectType>();
			foreach (var type in sectorTypes)
			{
				if (type.Value.IsGateway)
				{
					gateways.Add(type.Value);
				}
			}
			return gateways;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Beyond_Beyaan.Data_Modules;

namespace Beyond_Beyaan.Data_Managers
{
	public class UITypeManager
	{
		private Dictionary<string, BaseUIType> _uiTypes;

		public bool LoadUITypes(DirectoryInfo baseDirectory, SpriteManager spriteManager, out string reason)
		{
			_uiTypes = new Dictionary<string, BaseUIType>();

			try
			{
				XDocument doc = XDocument.Load(Path.Combine(baseDirectory.FullName, "UITypes.xml"));
				XElement root = doc.Element("UITypes");

				foreach (XElement element in root.Elements())
				{
					BaseUIType type = new BaseUIType();
					if (!type.LoadType(element, spriteManager, out reason))
					{
						return false;
					}
					_uiTypes.Add(type.Name, type);
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

		public UIType GetUI(string type, Random r)
		{
			if (_uiTypes.ContainsKey(type))
			{
				return new UIType(_uiTypes[type], r);
			}
			return null;
		}
	}
}

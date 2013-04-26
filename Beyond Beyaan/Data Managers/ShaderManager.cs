using System;
using System.Collections.Generic;
using System.IO;
using GorgonLibrary.Graphics;

namespace Beyond_Beyaan.Data_Managers
{
	public class ShaderManager
	{
		private Dictionary<string, FXShader> _shaders;

		public ShaderManager()
		{
			_shaders = new Dictionary<string, FXShader>();
		}

		public bool LoadShaders(DirectoryInfo directory, out string reason)
		{
			try
			{
				DirectoryInfo shaderDir = new DirectoryInfo(Path.Combine(directory.FullName, "Shaders"));

				foreach (var file in shaderDir.GetFiles())
				{
					FXShader newShader = FXShader.FromFile(file.FullName, ShaderCompileOptions.OptimizationLevel3);
					_shaders.Add(file.Name.Substring(0, file.Name.IndexOf(file.Extension)), newShader);
				}

				reason = null;
				return true;
			}
			catch (Exception e)
			{
				reason = e.Message;
				return false;
			}
		}

		public FXShader GetShader(string name)
		{
			if (_shaders.ContainsKey(name))
			{
				return _shaders[name];
			}
			return null;
		}
	}
}

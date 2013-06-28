using System;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	public class SystemView : WindowInterface
	{
		private BBSingleLineTextBox _name;
		private BBStretchableImage _systemBackground;
		private BBStretchableImage _productionBackground;
		private BBStretchableImage _shipBackground;

		private BBLabel _terrainLabel;
		private BBLabel _popLabel;


		private StarSystem currentSystem;

		#region Constructor
		public bool Initialize(GameMain gameMain, Random r, out string reason)
		{
			this.gameMain = gameMain;
			if (!base.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 300, 300, 600, gameMain, true, r, out reason))
			{
				return false;
			}

			_name = new BBSingleLineTextBox();
			if (!_name.Initialize(string.Empty, xPos + 20, yPos + 20, 260, 35, false, r, out reason))
			{
				return false;
			}
			_systemBackground = new BBStretchableImage();
			_productionBackground = new BBStretchableImage();
			_shipBackground = new BBStretchableImage();
			_popLabel = new BBLabel();
			_terrainLabel = new BBLabel();

			if (!_systemBackground.Initialize(xPos + 20, yPos + 60, 260, 120, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_productionBackground.Initialize(xPos + 20, yPos + 180, 260, 200, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_shipBackground.Initialize(xPos + 20, yPos + 380, 260, 200, StretchableImageType.ThinBorder, r, out reason))
			{
				return false;
			}
			if (!_terrainLabel.Initialize(xPos + 75, yPos + 75, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}
			if (!_popLabel.Initialize(xPos + 75, yPos + 100, string.Empty, System.Drawing.Color.White, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}
		#endregion

		public void LoadSystem()
		{
			currentSystem = gameMain.EmpireManager.CurrentEmpire.SelectedSystem;
			if (currentSystem.IsThisSystemExploredByEmpire(gameMain.EmpireManager.CurrentEmpire))
			{
				_name.SetString(currentSystem.Name);
				bool isOwned = currentSystem.Planets[0].Owner != null;
				_name.SetColor(isOwned ? currentSystem.Planets[0].Owner.EmpireColor : System.Drawing.Color.White);
				_popLabel.SetText(isOwned ? string.Format("{0}/{1} M", currentSystem.Planets[0].TotalPopulation, currentSystem.Planets[0].PopulationMax) : string.Format("{0} M", currentSystem.Planets[0].PopulationMax));
				_terrainLabel.SetText(Utility.PlanetTypeToString(currentSystem.Planets[0].PlanetType));
			}
			else
			{
				_name.SetString("Unexplored");
				_name.SetColor(System.Drawing.Color.White);
				_popLabel.SetText(string.Empty);
				_terrainLabel.SetText(string.Empty);
			}
			if (currentSystem.Planets[0].Owner == gameMain.EmpireManager.CurrentEmpire)
			{
				_name.SetReadOnly(false);
			}
			else
			{
				_name.SetReadOnly(true);
			}
		}

		public override void Draw()
		{
			base.Draw();
			_name.Draw();
			_systemBackground.Draw();
			_productionBackground.Draw();
			_shipBackground.Draw();
			if (currentSystem.IsThisSystemExploredByEmpire(gameMain.EmpireManager.CurrentEmpire))
			{
				currentSystem.Planets[0].SmallSprite.Draw(xPos + 30, yPos + 75);
				_popLabel.Draw();
				_terrainLabel.Draw();
			}
		}

		public override void MoveWindow()
		{
			base.MoveWindow();
			_name.MoveTo(xPos + 20, yPos + 20);
		}

		public override bool MouseDown(int x, int y)
		{
			bool result = _name.MouseDown(x, y);
			return result || base.MouseDown(x, y);
		}

		public override bool MouseUp(int x, int y)
		{
			bool result = _name.MouseUp(x, y);
			return result || base.MouseUp(x, y);
		}

		public override bool MouseHover(int x, int y, float frameDeltaTime)
		{
			_name.Update(frameDeltaTime);
			return base.MouseHover(x, y, frameDeltaTime);
		}

		public override bool KeyDown(GorgonLibrary.InputDevices.KeyboardInputEventArgs e)
		{
			if (_name.KeyDown(e))
			{
				currentSystem.Name = _name.Text;
				return true;
			}
			return false;
		}
	}
}

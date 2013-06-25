using System;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	public class SystemView : WindowInterface
	{
		BBSingleLineTextBox _name;
		BBStretchableImage _systemBackground;
		BBStretchableImage _productionBackground;
		BBStretchableImage _shipBackground;

		private StarSystem currentSystem;

		#region Constructor
		public bool Initialize(GameMain gameMain, SpriteManager spriteManager, Random r, out string reason)
		{
			this.gameMain = gameMain;
			if (!base.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 300, 300, 600, gameMain, true, spriteManager, r, out reason))
			{
				return false;
			}

			_name = new BBSingleLineTextBox();
			if (!_name.Initialize(string.Empty, xPos + 20, yPos + 20, 260, 35, false, spriteManager, r, out reason))
			{
				return false;
			}
			_systemBackground = new BBStretchableImage();
			_productionBackground = new BBStretchableImage();
			_shipBackground = new BBStretchableImage();

			if (!_systemBackground.Initialize(xPos + 20, yPos + 60, 260, 120, StretchableImageType.ThinBorder, spriteManager, r, out reason))
			{
				return false;
			}
			if (!_productionBackground.Initialize(xPos + 20, yPos + 180, 260, 200, StretchableImageType.ThinBorder, spriteManager, r, out reason))
			{
				return false;
			}
			if (!_shipBackground.Initialize(xPos + 20, yPos + 380, 260, 200, StretchableImageType.ThinBorder, spriteManager, r, out reason))
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
			}
			else
			{
				_name.SetString("Unexplored");
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
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyond_Beyaan.Data_Managers;

namespace Beyond_Beyaan.Screens
{
	public class SystemView : WindowInterface
	{
		BBSingleLineTextBox _name;
		BBStretchableImage _systemBackground;
		BBStretchableImage _productionBackground;
		BBStretchableImage _shipBackground;

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
			if (gameMain.EmpireManager.CurrentEmpire.SelectedSystem.IsThisSystemExploredByEmpire(gameMain.EmpireManager.CurrentEmpire))
			{
				_name.SetString(gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Name);
			}
			else
			{
				_name.SetString("Unexplored");
			}
			if (gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Planets[0].Owner == gameMain.EmpireManager.CurrentEmpire)
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

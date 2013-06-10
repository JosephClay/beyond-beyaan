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

		#region Constructor
		public bool Initialize(GameMain gameMain, SpriteManager spriteManager, Random r, out string reason)
		{
			this.gameMain = gameMain;
			if (!base.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 300, 300, 600, gameMain, true, spriteManager, r, out reason))
			{
				return false;
			}

			_name = new BBSingleLineTextBox();
			if (!_name.Initialize(string.Empty, xPos + 15, yPos + 15, 250, 35, true, spriteManager, r, out reason))
			{
				return false;
			}

			reason = null;
			return true;
		}
		#endregion

		public void LoadSystem()
		{
			_name.SetString(gameMain.EmpireManager.CurrentEmpire.SelectedSystem.Name);
		}

		public override void Draw()
		{
			base.Draw();
			_name.Draw();
		}

		public override void MoveWindow()
		{
			base.MoveWindow();
			_name.MoveTo(xPos + 15, yPos + 15);
		}
	}
}

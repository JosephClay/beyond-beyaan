namespace Beyond_Beyaan.Screens
{
	public class FleetView : WindowInterface
	{
		#region Constructor
		public bool Initialize(GameMain gameMain, out string reason)
		{
			if (!base.Initialize(gameMain.ScreenWidth - 300, gameMain.ScreenHeight / 2 - 240, 300, 480, StretchableImageType.ThinBorderBG, gameMain, true, gameMain.Random, out reason))
			{
				return false;
			}
			return true;
		}
		#endregion
	}
}

﻿namespace Beyond_Beyaan.Data_Modules
{
	public class SitRepItem
	{
		public ScreenEnum ScreenEventIsIn { get; private set; }
		public StarSystem SystemEventOccuredAt { get; private set; }
		public Planet PlanetEventOccuredAt { get; private set; }
		public Point PointEventOccuredAt { get; private set; }
		public string EventMessage { get; private set; }

		public SitRepItem(ScreenEnum screen, StarSystem system, Planet planet, Point point, string message)
		{
			ScreenEventIsIn = screen;
			SystemEventOccuredAt = system;
			PlanetEventOccuredAt = planet;
			PointEventOccuredAt = point;
			EventMessage = message;
		}
	}
}
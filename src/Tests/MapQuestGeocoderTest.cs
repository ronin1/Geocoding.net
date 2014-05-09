using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Geocoding.MapQuest;

namespace Geocoding.Tests
{
	public class MapQuestGeocoderTest : GeocoderTest
	{
		protected override IGeocoder CreateGeocoder()
		{
			string k = ConfigurationManager.AppSettings["mapQuestKey"];
			return new MapQuestGeocoder(k);
		}
	}
}

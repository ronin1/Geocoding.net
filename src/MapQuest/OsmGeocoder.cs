using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	/// <summary>
	/// Native OSM geocode logic
	/// </summary>
	public class OsmGeocoder
	{
		public OsmGeocoder()
		{
		}

		public OsmResponse Geocode(OsmRequest f)
		{
			if (f == null)
				throw new ArgumentNullException("f");
			
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmProvidedLocation
	{
		[JsonProperty("location")]
		public string Location { get; set; }
	}
}

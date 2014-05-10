using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	/// <summary>
	/// Result obj returned in a collection of OSM response under the property: results
	/// </summary>
	public class OsmResult
	{
		[JsonProperty("locations")]
		//[JsonArray(AllowNullItems=true,ItemIsReference=true)]
		public IList<OsmLocation> Locations { get; set; }

		[JsonProperty("providedLocation")]
		public OsmLocation ProvidedLocation { get; set; }
	}
}

using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmResponse
	{
		//[JsonArray(AllowNullItems=true)]
		[JsonProperty("results")]
		public IList<OsmResult> Results { get; set; }

		[JsonProperty("options")]
		public OsmOptions Options { get; set; }

		[JsonProperty("info")]
		public OsmInfo Info { get; set; }
	}
}

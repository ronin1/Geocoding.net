using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmInfo
	{
		/// <summary>
		/// Extended copyright info
		/// </summary>
		//[JsonDictionary]
		[JsonProperty("copyright")]
		public IDictionary<string, string> Copyright { get; set; }

		/// <summary>
		/// Maps to HTTP response code generally
		/// </summary>
		[JsonProperty("statuscode")]
		public OsmResponseStatus Status { get; set; }

		/// <summary>
		/// Error or status messages if applicable
		/// </summary>
		//[JsonArray(AllowNullItems=true)]
		[JsonProperty("messages")]
		public IList<string> Messages { get; set; }
	}
}

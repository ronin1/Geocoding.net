using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmReverseGeocodeRequest : OsmRequest
	{
		public OsmReverseGeocodeRequest(string key, double latitude, double longitude)
			: this(key, new Location(latitude, longitude))
		{

		}

		public OsmReverseGeocodeRequest(string key, Location loc) 
			: base(key)
		{
			Location = loc;
		}

		[JsonIgnore]
		Location _loc;
		/// <summary>
		/// Latitude and longitude for the request
		/// </summary>
		[JsonProperty("location")]
		public virtual Location Location 
		{
			get { return _loc; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("Location");

				_loc = value;
			}
		}

		[JsonIgnore]
		public override string RequestAction
		{
			get { return "reverse"; }
		}
	}
}

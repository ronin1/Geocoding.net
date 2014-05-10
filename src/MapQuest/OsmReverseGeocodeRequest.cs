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

		//public OsmReverseGeocodeRequest(string key, string address)
		//	: this(key, new OsmLocationRequest(address))
		//{
		//}

		public OsmReverseGeocodeRequest(string key, Location loc) 
			: this(key, new OsmLocationRequest(loc))
		{
		}

		public OsmReverseGeocodeRequest(string key, OsmLocationRequest loc)
			: base(key)
		{
			Location = loc;
		}

		[JsonIgnore]
		OsmLocationRequest loc;
		/// <summary>
		/// Latitude and longitude for the request
		/// </summary>
		[JsonProperty("location")]
		public virtual OsmLocationRequest Location 
		{
			get { return loc; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("Location");

				loc = value;
			}
		}

		[JsonIgnore]
		public override string RequestAction
		{
			get { return "reverse"; }
		}
	}
}

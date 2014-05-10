using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmGeocodeRequest : OsmReverseGeocodeRequest
	{
		public OsmGeocodeRequest(string key, string address)
			: this(key, new OsmLocationRequest(address))
		{
		}

		public OsmGeocodeRequest(string key, OsmLocationRequest loc) 
			: base(key, loc)
		{
		}

		public override string RequestAction
		{
			get { return "address"; }
		}
	}
}

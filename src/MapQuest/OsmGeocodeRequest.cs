using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmGeocodeRequest : OsmRequest
	{
		public OsmGeocodeRequest(string key, string location)
			: base(key)
		{
			Locations = new[] { new OsmLocationRequest(location) };
		}

		public OsmGeocodeRequest(string key, ICollection<string> locations) 
			: base(key) 
		{
			if (locations.IsNullOrEmpty())
				throw new ArgumentException("locations can not be null or empty");

			Locations = (from l in locations select new OsmLocationRequest(l)).ToArray();
		}

		[JsonIgnore]
		readonly List<OsmLocationRequest> _locations = new List<OsmLocationRequest>();
		/// <summary>
		/// Required collection of concatenated address string
		/// Note input will be hashed for uniqueness.
		/// Order is not guaranteed.
		/// </summary>
		[JsonProperty("locations")]
		public ICollection<OsmLocationRequest> Locations
		{
			get { return _locations; }
			set
			{
				if (value.IsNullOrEmpty())
					throw new ArgumentNullException("Locations can not be null or empty!");

				_locations.Clear();
				(from v in value
				 where v != null
				 select v).ForEach(v => _locations.Add(v));

				if (_locations.Count == 0)
					throw new InvalidOperationException("At least one valid Location is required");
			}
		}

		public override string RequestAction
		{
			get { return "batch"; }
		}
	}
}

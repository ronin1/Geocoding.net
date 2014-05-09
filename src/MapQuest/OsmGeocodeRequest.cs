using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmGeocodeRequest : OsmRequest
	{
		public OsmGeocodeRequest(string key, string location)
			: base(key)
		{
			Locations = new[] { location };
		}

		public OsmGeocodeRequest(string key, ICollection<string> locations) 
			: base(key) 
		{
			Locations = locations;
		}

		[JsonIgnore]
		readonly HashSet<string> _locations = new HashSet<string>();
		/// <summary>
		/// Required collection of concatenated address string
		/// Note input will be hashed for uniqueness.
		/// Order is not guaranteed.
		/// </summary>
		[JsonProperty("location")]
		public ICollection<string> Locations
		{
			get { return _locations; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("Locations");

				_locations.Clear();
				(from v in value
				 where !string.IsNullOrWhiteSpace(v)
				 select v).ForEach(v => _locations.Add(v));

				if (_locations.Count == 0)
					throw new InvalidOperationException("Locations can not be blank or empty");
			}
		}
	}
}

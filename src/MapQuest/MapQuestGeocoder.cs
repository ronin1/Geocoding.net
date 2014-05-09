using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Geocoding.MapQuest
{
	/// <remarks>
	/// <see cref="http://open.mapquestapi.com/geocoding/"/>
	/// <seealso cref="http://developer.mapquest.com/"/>
	/// </remarks>
	public class MapQuestGeocoder : IGeocoder
	{
		readonly OsmGeocoder _osmlogic;
		readonly string _key;

		public MapQuestGeocoder(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
				throw new ArgumentException("key can not be null or blank");

			_key = key;
			_osmlogic = new OsmGeocoder();
		}

		public IEnumerable<Address> Geocode(string address)
		{
			if (string.IsNullOrWhiteSpace(address))
				throw new ArgumentException("address can not be null or empty!");

			var f = new OsmGeocodeRequest(_key, address) { };
			OsmResponse res = _osmlogic.Geocode(f);
			if (res != null && !res.Results.IsNullOrEmpty())
			{
				return from r in res.Results
					   where r != null && !r.Locations.IsNullOrEmpty()
					   from l in r.Locations
					   where l != null
					   let q = (int)l.Quality
					   let c = string.IsNullOrWhiteSpace(l.Confidence) ? "ZZZZZZ" : l.Confidence
					   orderby q ascending
					   orderby c ascending
					   select l;
			}
			else
				return new Address[0];
		}

		public IEnumerable<Address> Geocode(string street, string city, string state, string postalCode, string country)
		{
			var sb = new StringBuilder ();
			if (!string.IsNullOrWhiteSpace (street))
				sb.AppendFormat ("{0}, ", street);
			if (!string.IsNullOrWhiteSpace (city))
				sb.AppendFormat ("{0}, ", city);
			if (!string.IsNullOrWhiteSpace (state))
				sb.AppendFormat ("{0} ", state);
			if (!string.IsNullOrWhiteSpace (postalCode))
				sb.AppendFormat ("{0} ", postalCode);
			if (!string.IsNullOrWhiteSpace (country))
				sb.AppendFormat ("{0} ", country);

			if (sb.Length > 1)
				sb.Length--;

			string s = sb.ToString ().Trim ();
			if (string.IsNullOrWhiteSpace (s))
				throw new ArgumentException ("Concatenated input values can not be null or blank");

			if (s.Last () == ',')
				s = s.Remove (s.Length - 1);

			return Geocode (s);
		}

		public IEnumerable<Address> ReverseGeocode(Location location)
		{
			if (location == null)
				throw new ArgumentNullException ("location");

			throw new NotImplementedException();
		}

		public IEnumerable<Address> ReverseGeocode(double latitude, double longitude)
		{
			return ReverseGeocode(new Location(latitude, longitude));
		}
	}
}
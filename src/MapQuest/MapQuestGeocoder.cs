using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.XPath;

namespace Geocoding.MapQuest
{
	/// <remarks>
	/// <see cref="http://open.mapquestapi.com/geocoding/"/>
	/// </remarks>
	public class MapQuestGeocoder : IGeocoder
	{
		public IEnumerable<Address> Geocode(string address)
		{
			throw new NotImplementedException();
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
				throw new ArgumentException ("Concatinated input values can not be null or blank");
			if (s.Last () == ',')
				s = s.Remove (s.Length - 1);

			return Geocode (s);
		}

		public IEnumerable<Address> ReverseGeocode(Location location)
		{
			if (location == null)
				throw new ArgumentNullException ("location");

			return ReverseGeocode (location.Latitude, location.Longitude);
		}

		public IEnumerable<Address> ReverseGeocode(double latitude, double longitude)
		{
			throw new NotImplementedException();
		}
	}
}
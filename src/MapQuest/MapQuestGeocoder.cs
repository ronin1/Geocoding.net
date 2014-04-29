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
			throw new NotImplementedException();
		}

		public IEnumerable<Address> ReverseGeocode(Location location)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Address> ReverseGeocode(double latitude, double longitude)
		{
			throw new NotImplementedException();
		}
	}
}
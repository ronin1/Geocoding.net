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
	/// http://code.google.com/apis/maps/documentation/geocoding/
	/// </remarks>
	public class MapQuestGeocoder : IGeocoder, IAsyncGeocoder
	{
		readonly string apiKey;

		const string keyMessage = "Only one of BusinessKey or ApiKey should be set on the MapQuestGeocoder.";

		public MapQuestGeocoder(string apiKey)
		{
			if (string.IsNullOrWhiteSpace(apiKey))
				throw new ArgumentException("apiKey can not be null or blank");

			this.apiKey = apiKey;
		}

		public WebProxy Proxy { get; set; }
		public string Language { get; set; }
		public string RegionBias { get; set; }
		public Bounds BoundsBias { get; set; }

		public string ServiceUrl
		{
			get
			{
				var builder = new StringBuilder();
				builder.Append("https://maps.googleapis.com/maps/api/geocode/xml?{0}={1}&sensor=false");

				if (!string.IsNullOrEmpty(Language))
				{
					builder.Append("&language=");
					builder.Append(HttpUtility.UrlEncode(Language));
				}
				if (!string.IsNullOrEmpty(RegionBias))
				{
					builder.Append("&region=");
					builder.Append(HttpUtility.UrlEncode(RegionBias));
				}
				if (!string.IsNullOrEmpty(apiKey))
				{
					builder.Append("&key=");
					builder.Append(HttpUtility.UrlEncode(apiKey));
				}

				if (BoundsBias != null)
				{
					builder.Append("&bounds=");
					builder.Append(BoundsBias.SouthWest.Latitude.ToString(CultureInfo.InvariantCulture));
					builder.Append(",");
					builder.Append(BoundsBias.SouthWest.Longitude.ToString(CultureInfo.InvariantCulture));
					builder.Append("|");
					builder.Append(BoundsBias.NorthEast.Latitude.ToString(CultureInfo.InvariantCulture));
					builder.Append(",");
					builder.Append(BoundsBias.NorthEast.Longitude.ToString(CultureInfo.InvariantCulture));
				}
				return builder.ToString();
			}
		}

		public IEnumerable<MapQuestAddress> Geocode(string address)
		{
			if (string.IsNullOrEmpty(address))
				throw new ArgumentNullException("address");

			HttpWebRequest request = BuildWebRequest("address", HttpUtility.UrlEncode(address));
			return ProcessRequest(request);
		}

		public IEnumerable<MapQuestAddress> ReverseGeocode(Location location)
		{
			if (location == null)
				throw new ArgumentNullException("location");

			return ReverseGeocode(location.Latitude, location.Longitude);
		}

		public IEnumerable<MapQuestAddress> ReverseGeocode(double latitude, double longitude)
		{
			HttpWebRequest request = BuildWebRequest("latlng", BuildGeolocation(latitude, longitude));
			return ProcessRequest(request);
		}

		public Task<IEnumerable<MapQuestAddress>> GeocodeAsync(string address)
		{
			if (string.IsNullOrEmpty(address))
				throw new ArgumentNullException("address");

			HttpWebRequest request = BuildWebRequest("address", HttpUtility.UrlEncode(address));
			return ProcessRequestAsync(request);
		}

		public Task<IEnumerable<MapQuestAddress>> GeocodeAsync(string address, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(address))
				throw new ArgumentNullException("address");

			HttpWebRequest request = BuildWebRequest("address", HttpUtility.UrlEncode(address));
			return ProcessRequestAsync(request, cancellationToken);
		}

		public Task<IEnumerable<MapQuestAddress>> ReverseGeocodeAsync(double latitude, double longitude)
		{
			HttpWebRequest request = BuildWebRequest("latlng", BuildGeolocation(latitude, longitude));
			return ProcessRequestAsync(request);
		}

		public Task<IEnumerable<MapQuestAddress>> ReverseGeocodeAsync(double latitude, double longitude, CancellationToken cancellationToken)
		{
			HttpWebRequest request = BuildWebRequest("latlng", BuildGeolocation(latitude, longitude));
			return ProcessRequestAsync(request, cancellationToken);
		}

		private string BuildAddress(string street, string city, string state, string postalCode, string country)
		{
			return string.Format("{0} {1}, {2} {3}, {4}", street, city, state, postalCode, country);
		}

		private string BuildGeolocation(double latitude, double longitude)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1}", latitude, longitude);
		}

		private IEnumerable<MapQuestAddress> ProcessRequest(HttpWebRequest request)
		{
			try
			{
				using (WebResponse response = request.GetResponse())
				{
					return ProcessWebResponse(response);
				}
			}
			catch (MapQuestGeocodingException)
			{
				//let these pass through
				throw;
			}
			catch (Exception ex)
			{
				//wrap in google exception
				throw new MapQuestGeocodingException(ex);
			}
		}

		private Task<IEnumerable<MapQuestAddress>> ProcessRequestAsync(HttpWebRequest request, CancellationToken? cancellationToken = null)
		{
			if (cancellationToken != null)
			{
				cancellationToken.Value.ThrowIfCancellationRequested();
				cancellationToken.Value.Register(() => request.Abort());
			}

			var requestState = new RequestState(request, cancellationToken);
			return Task.Factory.FromAsync(
				(callback, asyncState) => SendRequestAsync((RequestState)asyncState, callback),
				result => ProcessResponseAsync((RequestState)result.AsyncState, result),
				requestState
			);
		}

		private IAsyncResult SendRequestAsync(RequestState requestState, AsyncCallback callback)
		{
			try
			{
				return requestState.request.BeginGetResponse(callback, requestState);
			}
			catch (Exception ex)
			{
				throw new MapQuestGeocodingException(ex);
			}
		}

		private IEnumerable<MapQuestAddress> ProcessResponseAsync(RequestState requestState, IAsyncResult result)
		{
			if (requestState.cancellationToken != null)
				requestState.cancellationToken.Value.ThrowIfCancellationRequested();

			try
			{
				using (var response = (HttpWebResponse)requestState.request.EndGetResponse(result))
				{
					return ProcessWebResponse(response);
				}
			}
			catch (MapQuestGeocodingException)
			{
				//let these pass through
				throw;
			}
			catch (Exception ex)
			{
				//wrap in google exception
				throw new MapQuestGeocodingException(ex);
			}
		}

		IEnumerable<Address> IGeocoder.Geocode(string address)
		{
			return Geocode(address).Cast<Address>();
		}

		IEnumerable<Address> IGeocoder.Geocode(string street, string city, string state, string postalCode, string country)
		{
			return Geocode(BuildAddress(street, city, state, postalCode, country)).Cast<Address>();
		}

		IEnumerable<Address> IGeocoder.ReverseGeocode(Location location)
		{
			return ReverseGeocode(location).Cast<Address>();
		}

		IEnumerable<Address> IGeocoder.ReverseGeocode(double latitude, double longitude)
		{
			return ReverseGeocode(latitude, longitude).Cast<Address>();
		}

		Task<IEnumerable<Address>> IAsyncGeocoder.GeocodeAsync(string address)
		{
			return GeocodeAsync(address)
				.ContinueWith(task => task.Result.Cast<Address>());
		}

		Task<IEnumerable<Address>> IAsyncGeocoder.GeocodeAsync(string address, CancellationToken cancellationToken)
		{
			return GeocodeAsync(address, cancellationToken)
				.ContinueWith(task => task.Result.Cast<Address>(), cancellationToken);
		}

		Task<IEnumerable<Address>> IAsyncGeocoder.GeocodeAsync(string street, string city, string state, string postalCode, string country)
		{
			return GeocodeAsync(BuildAddress(street, city, state, postalCode, country))
				.ContinueWith(task => task.Result.Cast<Address>());
		}

		Task<IEnumerable<Address>> IAsyncGeocoder.GeocodeAsync(string street, string city, string state, string postalCode, string country, CancellationToken cancellationToken)
		{
			return GeocodeAsync(BuildAddress(street, city, state, postalCode, country), cancellationToken)
				.ContinueWith(task => task.Result.Cast<Address>(), cancellationToken);
		}

		Task<IEnumerable<Address>> IAsyncGeocoder.ReverseGeocodeAsync(double latitude, double longitude)
		{
			return ReverseGeocodeAsync(latitude, longitude)
				.ContinueWith(task => task.Result.Cast<Address>());
		}

		Task<IEnumerable<Address>> IAsyncGeocoder.ReverseGeocodeAsync(double latitude, double longitude, CancellationToken cancellationToken)
		{
			return ReverseGeocodeAsync(latitude, longitude, cancellationToken)
				.ContinueWith(task => task.Result.Cast<Address>(), cancellationToken);
		}

		private HttpWebRequest BuildWebRequest(string type, string value)
		{
			string url = string.Format(ServiceUrl, type, value);

			var req = WebRequest.Create(url) as HttpWebRequest;
			req.Proxy = Proxy;
			req.Method = "GET";
			return req;
		}

		private IEnumerable<MapQuestAddress> ProcessWebResponse(WebResponse response)
		{
			XPathDocument xmlDoc = LoadXmlResponse(response);
			XPathNavigator nav = xmlDoc.CreateNavigator();

			MapQuestStatus status = EvaluateStatus((string)nav.Evaluate("string(/GeocodeResponse/status)"));

			if (status != MapQuestStatus.Ok && status != MapQuestStatus.ZeroResults)
				throw new MapQuestGeocodingException(status);

			if (status == MapQuestStatus.Ok)
				return ParseAddresses(nav.Select("/GeocodeResponse/result")).ToArray();

			return new MapQuestAddress[0];
		}

		private XPathDocument LoadXmlResponse(WebResponse response)
		{
			using (Stream stream = response.GetResponseStream())
			{
				XPathDocument doc = new XPathDocument(stream);
				return doc;
			}
		}

		private IEnumerable<MapQuestAddress> ParseAddresses(XPathNodeIterator nodes)
		{
			while (nodes.MoveNext())
			{
				XPathNavigator nav = nodes.Current;

				MapQuestAddressType type = EvaluateType((string)nav.Evaluate("string(type)"));
				string formattedAddress = (string)nav.Evaluate("string(formatted_address)");

				var components = ParseComponents(nav.Select("address_component")).ToArray();

				double latitude = (double)nav.Evaluate("number(geometry/location/lat)");
				double longitude = (double)nav.Evaluate("number(geometry/location/lng)");
				Location coordinates = new Location(latitude, longitude);

				bool isPartialMatch;
				bool.TryParse((string)nav.Evaluate("string(partial_match)"), out isPartialMatch);

				yield return new MapQuestAddress(type, formattedAddress, components, coordinates, isPartialMatch);
			}
		}

		private IEnumerable<MapQuestAddressComponent> ParseComponents(XPathNodeIterator nodes)
		{
			while (nodes.MoveNext())
			{
				XPathNavigator nav = nodes.Current;

				string longName = (string)nav.Evaluate("string(long_name)");
				string shortName = (string)nav.Evaluate("string(short_name)");
				var types = ParseComponentTypes(nav.Select("type")).ToArray();

				if (types.Any()) //don't return an address component with no type
					yield return new MapQuestAddressComponent(types, longName, shortName);
			}
		}

		private IEnumerable<MapQuestAddressType> ParseComponentTypes(XPathNodeIterator nodes)
		{
			while (nodes.MoveNext())
				yield return EvaluateType(nodes.Current.InnerXml);
		}

		/// <remarks>
		/// http://code.google.com/apis/maps/documentation/geocoding/#StatusCodes
		/// </remarks>
		private MapQuestStatus EvaluateStatus(string status)
		{
			switch (status)
			{
				case "OK": return MapQuestStatus.Ok;
				case "ZERO_RESULTS": return MapQuestStatus.ZeroResults;
				case "OVER_QUERY_LIMIT": return MapQuestStatus.OverQueryLimit;
				case "REQUEST_DENIED": return MapQuestStatus.RequestDenied;
				case "INVALID_REQUEST": return MapQuestStatus.InvalidRequest;
				default: return MapQuestStatus.Error;
			}
		}

		/// <remarks>
		/// http://code.google.com/apis/maps/documentation/geocoding/#Types
		/// </remarks>
		private MapQuestAddressType EvaluateType(string type)
		{
			switch (type)
			{
				case "street_address": return MapQuestAddressType.StreetAddress;
				case "route": return MapQuestAddressType.Route;
				case "intersection": return MapQuestAddressType.Intersection;
				case "political": return MapQuestAddressType.Political;
				case "country": return MapQuestAddressType.Country;
				case "administrative_area_level_1": return MapQuestAddressType.AdministrativeAreaLevel1;
				case "administrative_area_level_2": return MapQuestAddressType.AdministrativeAreaLevel2;
				case "administrative_area_level_3": return MapQuestAddressType.AdministrativeAreaLevel3;
				case "colloquial_area": return MapQuestAddressType.ColloquialArea;
				case "locality": return MapQuestAddressType.Locality;
				case "sublocality": return MapQuestAddressType.SubLocality;
				case "neighborhood": return MapQuestAddressType.Neighborhood;
				case "premise": return MapQuestAddressType.Premise;
				case "subpremise": return MapQuestAddressType.Subpremise;
				case "postal_code": return MapQuestAddressType.PostalCode;
				case "natural_feature": return MapQuestAddressType.NaturalFeature;
				case "airport": return MapQuestAddressType.Airport;
				case "park": return MapQuestAddressType.Park;
				case "point_of_interest": return MapQuestAddressType.PointOfInterest;
				case "post_box": return MapQuestAddressType.PostBox;
				case "street_number": return MapQuestAddressType.StreetNumber;
				case "floor": return MapQuestAddressType.Floor;
				case "room": return MapQuestAddressType.Room;

				default: return MapQuestAddressType.Unknown;
			}
		}

		protected class RequestState
		{
			public readonly HttpWebRequest request;
			public readonly CancellationToken? cancellationToken;

			public RequestState(HttpWebRequest request, CancellationToken? cancellationToken)
			{
				if (request == null) throw new ArgumentNullException("request");

				this.request = request;
				this.cancellationToken = cancellationToken;
			}
		}
	}
}
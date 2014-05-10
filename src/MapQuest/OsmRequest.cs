using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	/// <summary>
	/// Geo-code request object
	/// <see cref="http://open.mapquestapi.com/geocoding/"/>
	/// </summary>
	public abstract class OsmRequest
	{
		protected OsmRequest(string key) //output only, no need for default ctor
		{
			Key = key;
		}

		[JsonIgnore]
		string _key;
		/// <summary>
		/// A REQUIRED unique key to authorize use of the Routing Service.
		/// <see cref="http://developer.mapquest.com/"/>
		/// </summary>
		[JsonIgnore]
		public virtual string Key
		{
			get { return _key; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("An application key is required for MapQuest");

				_key = value;
			}
		}

		/// <summary>
		/// Defaults to json
		/// </summary>
		[JsonIgnore]
		public virtual OsmFormat InputFormat { get; private set; }

		/// <summary>
		/// Defaults to json
		/// </summary>
		[JsonIgnore]
		public virtual OsmFormat OutputFormat { get; private set; }

		///// <summary>
		///// A delimiter is used only when outFormat=csv. The delimiter is the single character used to separate the fields of a character delimited file. 
		///// The delimiter defaults to a comma(,). 
		///// The valid choices are ,|:;
		///// </summary>
		//[JsonProperty("delimiter")]
		//public virtual char Delimiter { get; private set; }

		[JsonIgnore]
		int _maxResults = -1;
		/// <summary>
		/// The number of results to limit the response to in the case of an ambiguous address.
		/// Defaults: -1 (indicates no limit) 
		/// </summary>
		[JsonProperty("maxResults")]
		public virtual int MaxResults
		{
			get { return _maxResults; }
			set { _maxResults = value > 0 ? value : -1; }
		}

		/// <summary>
		/// This parameter tells the service whether it should return a URL to a static map thumbnail image for a location being geocoded.
		/// </summary>
		[JsonProperty("thumbMaps")]
		public virtual bool ThumbMap { get; set; }

		/// <summary>
		/// This option tells the service whether it should fail when given a latitude/longitude pair in an address or batch geocode call, or if it should ignore that and try and geo-code what it can.
		/// </summary>
		[JsonProperty("ignoreLatLngInput")]
		public virtual bool IgnoreLatLngInput { get; set; }

		/// <summary>
		/// Optional name of JSONP callback method.
		/// </summary>
		[JsonIgnore]
		public virtual string JsonpCallBack { get; set; }

		/// <summary>
		/// We are using v1 of MapQuest OSM API
		/// </summary>
		protected static string BASE_PATH = @"http://open.mapquestapi.com/geocoding/v1/";

		/// <summary>
		/// The full path for the request
		/// </summary>
		[JsonIgnore]
		public virtual Uri RequestUri
		{
			get
			{
				var sb = new StringBuilder(BASE_PATH);
				sb.Append(RequestAction);
				sb.Append("?");
				//no need to escape this key, it is already escaped by MapQuest at generation
				sb.AppendFormat("key={0}&", Key); 

				if (!string.IsNullOrWhiteSpace(JsonpCallBack))
					sb.AppendFormat("callback={0}&", HttpUtility.UrlEncode(JsonpCallBack));

				if (InputFormat != OsmFormat.json)
					sb.AppendFormat("inFormat={0}&", InputFormat);

				if (OutputFormat != OsmFormat.json)
					sb.AppendFormat("outFormat={0}&", OutputFormat);

				sb.Length--;
				return new Uri(sb.ToString());
			}
		}

		[JsonIgnore]
		public abstract string RequestAction { get; }

		[JsonIgnore]
		string _verb = "POST";
		/// <summary>
		/// Default request verb is POST for security and large batch payloads
		/// </summary>
		[JsonIgnore]
		public virtual string RequestVerb
		{
			get { return _verb; }
			protected set { _verb = string.IsNullOrWhiteSpace(value) ? "POST" : value.Trim().ToUpper(); }
		}

		/// <summary>
		/// Request body if request verb is applicable (POST, PUT, etc)
		/// </summary>
		[JsonIgnore]
		public virtual string RequestBody
		{
			get
			{
				return this.ToJSON();
			}
		}

		public override string ToString()
		{
			return this.RequestBody;
		}
	}
}

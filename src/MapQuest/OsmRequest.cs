using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	/// <summary>
	/// Geo-code request object
	/// <see cref="http://open.mapquestapi.com/geocoding/"/>
	/// </summary>
	public abstract class OsmRequest
	{
		public OsmRequest(string key) //output only, no need for default ctor
		{
			Key = key;
		}

		[JsonIgnore]
		string _key;
		/// <summary>
		/// A REQUIRED unique key to authorize use of the Routing Service.
		/// <see cref="http://developer.mapquest.com/"/>
		/// </summary>
		[JsonProperty("key")]
		public virtual string Key
		{
			get { return _key; }
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("An application key is required for MapQuest");

				_key = value;
			}
		}

		/// <summary>
		/// Defaults to json
		/// </summary>
		[JsonProperty("inFormat", ItemConverterType=typeof(string))]
		public virtual OsmFormat InputFormat { get; private set; }

		/// <summary>
		/// Defaults to json
		/// </summary>
		[JsonProperty("outFormat", ItemConverterType=typeof(string))]
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
		[JsonProperty("callback")]
		public virtual string JsonpCallBack { get; set; }
	}
}

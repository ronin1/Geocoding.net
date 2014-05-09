using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	/// <summary>
	/// MapQuest address obj.
	/// <see cref="http://open.mapquestapi.com/geocoding/"/>
	/// </summary>
	public class OsmLocation : ParsedAddress
	{
		protected OsmLocation()
			: this("unknown", new Location(0, 0))
		{
		}
		public OsmLocation(string formattedAddress, Location coordinates)
			: base(formattedAddress, coordinates, "MapQuest")
		{
			DisplayCoordinates = coordinates;
		}

		[JsonProperty("latLng")]
		public override Location Coordinates
		{
			get { return base.Coordinates; }
			set { base.Coordinates = value; }
		}

		[JsonProperty("displayLatLng")]
		public virtual Location DisplayCoordinates { get; set; }

		[JsonProperty("street")]
		public override string Street { get; set; }

		[JsonProperty("adminArea5")]
		public override string City { get; set; }

		[JsonProperty("adminArea4")]
		public override string County { get; set; }

		[JsonProperty("adminArea3")]
		public override string State { get; set; }

		[JsonProperty("adminArea1")]
		public override string Country { get; set; }

		[JsonProperty("postalCode")]
		public override string PostCode { get; set; }

		/// <summary>
		/// Type of location
		/// </summary>
		[JsonProperty("type")]
		public OsmLocationType Type { get; set; }

		/// <summary>
		/// Granularity code of quality/accuracy guarantee
		/// <see cref="http://open.mapquestapi.com/geocoding/geocodequality.html#granularity"/>
		/// </summary>
		[JsonProperty("geocodeQuality")]
		public OsmQuality Quality { get; set; }

		/// <summary>
		/// Text string comparable, sort able score
		/// <see cref="http://open.mapquestapi.com/geocoding/geocodequality.html#granularity"/>
		/// </summary>
		[JsonProperty("geocodeQualityCode")]
		public string Confidence { get; set; }

		/// <summary>
		/// Identifies the closest road to the address for routing purposes.
		/// </summary>
		[JsonProperty("linkId")]
		public long LinkId { get; set; }

		/// <summary>
		/// Which side of the street this address is in
		/// </summary>
		[JsonProperty("sideOfStreet")]
		public OsmSideOfStreet SideOfStreet { get; set; }

		/// <summary>
		/// Url to a MapQuest map
		/// </summary>
		[JsonProperty("mapUrl")]
		public Uri MapUrl { get; set; }

		[JsonProperty("adminArea1Type")]
		public string CountryLabel { get; set; }

		[JsonProperty("adminArea3Type")]
		public string StateLabel { get; set; }
	}
}
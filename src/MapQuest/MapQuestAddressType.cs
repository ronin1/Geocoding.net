using System;

namespace Geocoding.MapQuest
{
	/// <remarks>
	/// http://code.google.com/apis/maps/documentation/geocoding/#Types
	/// </remarks>
	public enum MapQuestAddressType
	{
		Unknown,
		StreetAddress,
		Route,
		Intersection,
		Political,
		Country,
		AdministrativeAreaLevel1,
		AdministrativeAreaLevel2,
		AdministrativeAreaLevel3,
		ColloquialArea,
		Locality,
		SubLocality,
		Neighborhood,
		Premise,
		Subpremise,
		PostalCode,
		NaturalFeature,
		Airport,
		Park,
		PointOfInterest,
		PostBox,
		StreetNumber,
		Floor,
		Room
	}
}
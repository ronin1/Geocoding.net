using System;

namespace Geocoding.MapQuest
{
	public enum OsmResponseStatus : int
	{
		Ok = 0,
		OkBatch = 100,
		ErrorInput = 400,
		ErrorAccountKey = 403,
		ErrorUnknown = 500,
	}
}
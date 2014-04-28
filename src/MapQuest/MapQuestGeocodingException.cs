using System;

namespace Geocoding.MapQuest
{
	public class MapQuestGeocodingException : Exception
	{
		const string defaultMessage = "There was an error processing the geocoding request. See Status or InnerException for more information.";

		public MapQuestStatus Status { get; private set; }

		public MapQuestGeocodingException(MapQuestStatus status)
			: base(defaultMessage)
		{
			this.Status = status;
		}

		public MapQuestGeocodingException(Exception innerException)
			: base(defaultMessage, innerException)
		{
			this.Status = MapQuestStatus.Error;
		}
	}
}
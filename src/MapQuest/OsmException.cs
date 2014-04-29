using System;

namespace Geocoding.MapQuest
{
	public class OsmException : Exception
	{
		const string defaultMessage = "There was an error processing the geocoding request. See Status or InnerException for more information.";

		public OsmResponseStatus Status { get; private set; }

		public OsmException(OsmResponseStatus status)
			: base(defaultMessage)
		{
			this.Status = status;
		}

		public OsmException(Exception innerException)
			: base(defaultMessage, innerException)
		{
			this.Status = OsmResponseStatus.ErrorUnknown;
		}
	}
}
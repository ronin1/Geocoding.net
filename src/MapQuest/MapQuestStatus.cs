using System;

namespace Geocoding.MapQuest
{
	public enum MapQuestStatus
	{
		Error,
		Ok,
		ZeroResults,
		OverQueryLimit,
		RequestDenied,
		InvalidRequest
	}
}
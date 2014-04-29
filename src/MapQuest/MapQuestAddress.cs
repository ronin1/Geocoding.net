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
	public class MapQuestAddress : Address
	{
		public MapQuestAddressType Type { get; set; }
		public MapQuestSideOfStreet SideOfStreet { get; set; }

		public MapQuestAddress(string formattedAddress, Location coordinates)
			: base(formattedAddress, coordinates, "MapQuest")
		{
			
		}
	}
}
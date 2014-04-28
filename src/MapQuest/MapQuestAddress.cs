using System;
using System.Linq;
using System.Collections.Generic;

namespace Geocoding.MapQuest
{
	public class MapQuestAddress : Address
	{
		readonly MapQuestAddressType type;
		readonly MapQuestAddressComponent[] components;
		readonly bool isPartialMatch;

		public MapQuestAddressType Type
		{
			get { return type; }
		}

		public MapQuestAddressComponent[] Components
		{
			get { return components; }
		}

		public bool IsPartialMatch
		{
			get { return isPartialMatch; }
		}

		public MapQuestAddressComponent this[MapQuestAddressType type]
		{
			get { return Components.FirstOrDefault(c => c.Types.Contains(type)); }
		}

		public MapQuestAddress(MapQuestAddressType type, string formattedAddress, MapQuestAddressComponent[] components, Location coordinates, bool isPartialMatch)
			: base(formattedAddress, coordinates, "Google")
		{
			if (components == null)
				throw new ArgumentNullException("components");

			if (components.Length < 1)
				throw new ArgumentException("Value cannot be empty.", "components");

			this.type = type;
			this.components = components;
			this.isPartialMatch = isPartialMatch;
		}
	}
}
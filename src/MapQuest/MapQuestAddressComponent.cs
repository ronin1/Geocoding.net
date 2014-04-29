using System;

namespace Geocoding.MapQuest
{
	public class MapQuestAddressComponent
	{
		public OsmAddressType[] Types { get; private set; }
		public string LongName { get; private set; }
		public string ShortName { get; private set; }

		public MapQuestAddressComponent(OsmAddressType[] types, string longName, string shortName)
		{
			if (types == null)
				throw new ArgumentNullException("types");

			if (types.Length < 1)
				throw new ArgumentException("Value cannot be empty.", "types");

			this.Types = types;
			this.LongName = longName;
			this.ShortName = shortName;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Types[0], LongName);
		}
	}
}
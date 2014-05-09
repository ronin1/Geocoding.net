using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	public class OsmLocationRequest
	{
		public OsmLocationRequest(string street)
		{
			Street = street;
		}

		[JsonIgnore]
		string _street;
		[JsonProperty("street")]
		public virtual string Street
		{
			get { return _street; }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentException("Street can not be null or blank");

				_street = value;
			}
		}

		public override string ToString()
		{
			return string.Format("street: {0}", Street);
		}

	}
}

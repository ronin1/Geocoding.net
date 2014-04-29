using System;
using System.Text;

namespace Geocoding
{
	public abstract class Address
	{
		readonly string formattedAddress;
		readonly Location coordinates;
		readonly string provider;

		public virtual string FormattedAddress
		{
			get { return formattedAddress ?? ""; }
		}

		public virtual Location Coordinates
		{
			get { return coordinates; }
		}

		public virtual string Provider
		{
			get { return provider ?? ""; }
		}

		public Address(string formattedAddress, Location coordinates, string provider)
		{
			formattedAddress = (formattedAddress ?? "").Trim();

			if (string.IsNullOrEmpty(formattedAddress))
				throw new ArgumentNullException("formattedAddress");

			if (coordinates == null)
				throw new ArgumentNullException("coordinates");

			if (provider == null)
				throw new ArgumentNullException("provider");

			this.formattedAddress = formattedAddress;
			this.coordinates = coordinates;
			this.provider = provider;
		}

		public virtual Distance DistanceBetween(Address address)
		{
			return this.Coordinates.DistanceBetween(address.Coordinates);
		}

		public virtual Distance DistanceBetween(Address address, DistanceUnits units)
		{
			return this.Coordinates.DistanceBetween(address.Coordinates, units);
		}

		public override string ToString()
		{
			return FormattedAddress;
		}
	}
}
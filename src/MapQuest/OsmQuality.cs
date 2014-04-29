using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geocoding.MapQuest
{
	public enum OsmQuality
	{
		/// <summary>
		/// P1	A specific point location.
		/// </summary>
		POINT,
		/// <summary>
		/// L1	A specific street address location.
		/// </summary>
		ADDRESS,
		/// <summary>
		/// I1	An intersection of two or more streets.
		/// </summary>
		INTERSECTION,
		/// <summary>
		/// B1	The center of a single street block. House number ranges are returned if available.
		/// B2	The center of a single street block, which is located closest to the geographic center of all matching street blocks. No house number range is returned.
		/// B3	The center of a single street block whose numbered range is nearest to the input number. House number range is returned.
		/// </summary>
		STREET,
		/// <summary>
		/// A1	Admin area, largest. For USA, a country.
		/// </summary>
		COUNTRY,
		/// <summary>
		/// A3	Admin area. For USA, a state.
		/// </summary>
		STATE,
		/// <summary>
		/// A4	Admin area. For USA, a county.
		/// </summary>
		COUNTY,
		/// <summary>
		/// A5	Admin area. For USA, a city.
		/// </summary>
		CITY,
		/// <summary>
		/// Z1	Postal code, largest. For USA, a ZIP.
		/// Z4	Postal code, smallest. Unused in USA.
		/// </summary>
		ZIP,
		/// <summary>
		/// Z2	Postal code. For USA, a ZIP+2.
		/// Z3	Postal code. For USA, a ZIP+4.
		/// </summary>
		ZIP_EXTENDED,
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geocoding
{
	public static class Extensions
	{
		public static bool IsNullOrEmpty<T>(this ICollection<T> col)
		{
			return col == null || col.Count == 0;
		}

		public static void ForEach<T>(this IEnumerable<T> self, Action<T> actor)
		{
			if(actor == null)
				throw new ArgumentNullException("actor");

			if (self == null)
				return;

			foreach (T item in self)
			{
				actor(item);
			}
		}
	}
}

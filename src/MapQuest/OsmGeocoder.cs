using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Geocoding.MapQuest
{
	/// <summary>
	/// Native OSM geocode logic
	/// </summary>
	public class OsmGeocoder
	{
		public OsmGeocoder()
		{
		}

		public OsmResponse Execute(OsmRequest f)
		{
			HttpWebRequest request = Send(f);
			OsmResponse r = Parse(request);
			if (r != null && !r.Results.IsNullOrEmpty())
			{
				foreach(OsmResult o in r.Results) 
				{
					if (o == null)
						continue;

					foreach(OsmLocation l in o.Locations) 
					{
						if (!string.IsNullOrWhiteSpace(l.FormattedAddress) || o.ProvidedLocation == null)
							continue;

						if (string.Compare(o.ProvidedLocation.FormattedAddress, "unknown", true) != 0)
							l.FormattedAddress = o.ProvidedLocation.FormattedAddress;
						else
							l.FormattedAddress = o.ProvidedLocation.ToString();
					}
				}
			}
			return r;
		}

		HttpWebRequest Send(OsmRequest f)
		{
			if (f == null)
				throw new ArgumentNullException("f");

			HttpWebRequest request;
			bool hasBody = false;
			switch (f.RequestVerb)
			{
				case "GET":
				case "DELETE":
				case "HEAD":
					{
						var u = string.Format("{0}json={1}&", f.RequestUri, HttpUtility.UrlEncode(f.RequestBody));
						request = WebRequest.Create(u) as HttpWebRequest;
					}
					break;
				case "POST":
				case "PUT":
				default:
					{
						request = WebRequest.Create(f.RequestUri) as HttpWebRequest;
						hasBody = !string.IsNullOrWhiteSpace(f.RequestBody);
					}
					break;
			}
			request.Method = f.RequestVerb;
			request.ContentType = "application/" + f.InputFormat;
			request.Expect = "application/" + f.OutputFormat;

			if(hasBody) 
			{
				byte[] buffer = Encoding.UTF8.GetBytes(f.RequestBody);
				request.ContentLength = buffer.Length;
				using (Stream rs = request.GetRequestStream())
				{
					rs.Write(buffer, 0, buffer.Length);
					rs.Flush();
					rs.Close();
				}
			}
			return request;
		}

		OsmResponse Parse(HttpWebRequest request)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			string requestInfo = string.Format("[{0}] {1}", request.Method, request.RequestUri);
			try
			{
				string json;
				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
				{
					if ((int)response.StatusCode >= 300) //error
						throw new HttpException((int)response.StatusCode, response.StatusDescription);

					using (var sr = new StreamReader(response.GetResponseStream()))
						json = sr.ReadToEnd();
				}
				if (string.IsNullOrWhiteSpace(json))
					throw new ApplicationException("Remote system response with blank: " + requestInfo);

				OsmResponse o = json.FromJSON<OsmResponse>();
				if (o == null)
					throw new ApplicationException("Unable to deserialize remote response: " + requestInfo + " => " + json);

				return o;
			}
			catch (WebException wex) //convert to simple exception & close the response stream
			{
				using (HttpWebResponse response = wex.Response as HttpWebResponse)
				{
					var sb = new StringBuilder(requestInfo);
					sb.Append(" | ");
					sb.Append(response.StatusDescription);
					sb.Append(" | ");
					using (var sr = new StreamReader(response.GetResponseStream()))
					{
						sb.Append(sr.ReadToEnd());
					}
					throw new HttpException((int)response.StatusCode, sb.ToString());
				}
			}
		}
	}
}

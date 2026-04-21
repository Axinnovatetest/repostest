using Geocoding.Microsoft.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.BackgroundWorker
{
	public class DayOff
	{
		public DateTime date { get; set; }
		public string fname { get; set; }
		public string by { get; set; }

		public string hb { get; set; }
		public string bb { get; set; }
		public string be { get; set; }
		public string bw { get; set; }
		public string nw { get; set; }
		public string ni { get; set; }
		public string mv { get; set; }
		public string hh { get; set; }
		public string he { get; set; }
		public string rp { get; set; }
		public string th { get; set; }
		public string sh { get; set; }
		public string st { get; set; }
		public string sn { get; set; }
		public string sl { get; set; }
		public string all_states { get; set; }
		public bool getDay(string day)
		{
			return day == "1";
		}

	}

	public class DaysOff
	{
		public string status { get; set; }
		public List<DayOff> feiertage { get; set; }

	}
	public class DaysOffService
	{
		public static DaysOff SyncDaysOff()
		{
			HttpClient client = new HttpClient(
		   new HttpClientHandler()
		   {
			   AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate

		   });
			client.BaseAddress = new Uri("https://get.api-feiertage.de/");
			HttpResponseMessage response = client.GetAsync("?states=by").Result;
			response.EnsureSuccessStatusCode();
			return JsonConvert.DeserializeObject<DaysOff>(response.Content.ReadAsStringAsync().Result);
		}
	}
}

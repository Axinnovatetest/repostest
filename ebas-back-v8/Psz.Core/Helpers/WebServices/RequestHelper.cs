using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Psz.Core.Helpers.WebServices
{
	public class RequestHelper
	{
		public static string Get(string url)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create(url);

			webRequest.ContentType = "text/json;charset=\"utf-8\"";
			webRequest.Accept = "text/json";
			webRequest.Method = "GET";

			var rawResponse = string.Empty;

			using(WebResponse response = webRequest.GetResponse())
			{
				using(StreamReader streamReader = new StreamReader(response.GetResponseStream()))
				{
					string result = streamReader.ReadToEnd();
					rawResponse = (result);
				}
			}

			return rawResponse;
		}

		public static string POST_Request(string url, string data)
		{
			try
			{
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				webRequest.ContentType = "json/xml;charset=\"utf-8\"";
				webRequest.Accept = "json/xml";
				webRequest.Method = "POST";

				byte[] dataStream = Encoding.UTF8.GetBytes(data);
				webRequest.ContentLength = dataStream.Length;

				var newStream = webRequest.GetRequestStream();
				newStream.Write(dataStream, 0, dataStream.Length);
				newStream.Close();

				string responseSR = "";
				using(WebResponse response = webRequest.GetResponse())
				{
					using(StreamReader rd = new StreamReader(response.GetResponseStream()))
					{
						string result = rd.ReadToEnd();
						responseSR = (result);
					}
				}

				return responseSR;
			} catch(Exception e)
			{
				throw;
			}
		}

		public static string Post(string url, object obj)
		{
			var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			//httpWebRequest.ContentType = "application/json";
			httpWebRequest.ContentType = "json/xml;charset=\"utf-8\"";
			httpWebRequest.Accept = "json/xml";

			//byte[] dataStream = Encoding.UTF8.GetBytes(data);
			//webRequest.ContentLength = dataStream.Length;

			//var newStream = webRequest.GetRequestStream();
			//newStream.Write(dataStream, 0, dataStream.Length);
			//newStream.Close();

			using(var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				var json = JsonConvert.SerializeObject(obj);

				streamWriter.Write(json);
				streamWriter.Flush();
				streamWriter.Close();
			}

			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

			if(httpResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new WebException("", WebExceptionStatus.UnknownError);
			}

			var responseStream = "";
			using(var streamReader = new StreamReader(httpResponse.GetResponseStream()))
			{
				responseStream = streamReader.ReadToEnd();
			}

			return responseStream;
		}
	}
}

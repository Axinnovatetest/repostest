using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Services.Helpers
{
	public static class SendGridHelper
	{

		public static string GetMessageIdFromHttpResponse(SendGrid.Response response)
		{
			var HeaderkeyValuePairs = response.Headers.AsQueryable().ToList();
			var messagesIds = HeaderkeyValuePairs.Where(x => x.Key == "X-Message-Id").FirstOrDefault().Value;
			return messagesIds.FirstOrDefault();
		}
		public static string BuildSendGridQuery(string queryBody, int limit)
		{
			return $@"{{'query':'{queryBody}','limit': {limit} }}";
		}

		public static string GetLinkFromString(string message)
		{
			if(message is null)
				return "";

			string pattern = @"<a\s+href='([^']+)'>";
			string restoreturn = "";
			Match match = Regex.Match(message, pattern);

			if(match.Success)
			{
				restoreturn = match.Groups[1].Value;
			}
			return restoreturn;
		}
	}
}

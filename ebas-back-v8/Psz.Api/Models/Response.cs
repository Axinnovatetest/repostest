using System;
using System.Collections.Generic;

namespace Psz.Api.Models
{
	public class Response<T>
	{
		public bool Success { get; set; }
		public T ResponseBody { get; set; }
		public List<string> Errors { get; set; }

		public Response() { }
		public Response(bool success, T responseBody, List<string> error = null)
		{
			Success = success;
			ResponseBody = responseBody;
			Errors = error;
		}
	}
}

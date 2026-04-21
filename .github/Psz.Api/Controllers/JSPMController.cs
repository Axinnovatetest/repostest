using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Psz.Api.Controllers
{
	public class JSPMController: Controller
	{
		[AllowAnonymous]
		public ActionResult Index(string timestamp)
		{
			//SET THE LICENSE INFO
			string license_owner = "PSZ electronic GmbH - 1 WebApp Lic - 1 WebServer Lic";
			string license_key = "8025CCE6A02A6889DA91D118D714DE24009C43A7";

			//DO NOT MODIFY THE FOLLOWING CODE
			string license_hash = "";

			using(System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
			{
				license_hash = BitConverter.ToString(sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(license_key + timestamp))).Replace("-", "").ToLower();
			}

			return File(System.Text.Encoding.UTF8.GetBytes(license_owner + '|' + license_hash), "text/plain");
		}
	}
}
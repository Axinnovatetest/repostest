using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetProjectTypes(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			try
			{
				var response = new List<KeyValuePair<int, string>>();
				var types = Enum.GetValues(typeof(Enums.ProjectManagmentEnums.ProjectTypes)).Cast<Enums.ProjectManagmentEnums.ProjectTypes>().ToList();
				if(types != null && types.Count > 0)
				{
					response = types.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription())).Distinct().ToList();
				}
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> GetProjectStatuses(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			try
			{
				var response = new List<KeyValuePair<int, string>>();
				var types = Enum.GetValues(typeof(Enums.ProjectManagmentEnums.ProjectStatuses)).Cast<Enums.ProjectManagmentEnums.ProjectStatuses>().ToList();
				if(types != null && types.Count > 0)
				{
					response = types.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription())).Distinct().ToList();
				}
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
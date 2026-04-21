using Psz.Core.BaseData.Models.ProjectManagment;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ProjectManagment
{
	public partial class ProjectManagmentService
	{
		public ResponseModel<List<OpenOrdersModel>> GetOpenOrders(UserModel user, int articleNr)
		{
			if(user == null)
				return ResponseModel<List<OpenOrdersModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Joins.BSD.ProjectManagmentAcces.GetArticleOpenOrders(articleNr);
				var response = entities?.Select(x => new OpenOrdersModel
				{
					FAId = x.ID,
					Fertigungsnummer = x.Fertigungsnummer,
					Status = x.FA_Gestartet.HasValue && x.FA_Gestartet.Value ? "Started" : "Not Started",
					State = x.Kennzeichen,
					FertigungDate=x.Termin_Bestatigt1
				})?.ToList();

				return ResponseModel<List<OpenOrdersModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
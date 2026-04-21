using Psz.Core.Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Core.Support.Handlers;

public class GetFrequentERPUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
{
	private Identity.Models.UserModel _user { get; set; }
	public GetFrequentERPUsersHandler(Identity.Models.UserModel user)
	{
		this._user = user;
	}

	public ResponseModel<List<KeyValuePair<string, string>>> Handle()
	{
		try
		{

			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.UsersMostUsingERP();


			



			if(fetchedData is null || fetchedData.Count() == 0)
				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(new List<KeyValuePair<string, string>>());

			var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(fetchedData.Select(x => int.Parse(x.UserId)).ToList());

			for(int i = 0; i < fetchedData.Count(); i++)
			{
				fetchedData[i].UserId = users.Where(x => x.Id == int.Parse(fetchedData[i].UserId)).FirstOrDefault().Name;
 			}

			var res = fetchedData.Select(x => new KeyValuePair<string, string>(x.UserId, x.UserCount)).ToList();

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(res);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<KeyValuePair<string, string>>> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
		}
		return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Logistics.Models.ControlProcedure.NewFolder;

namespace Psz.Core.Logistics.Handlers.ControlProcedure.ArticleControlProcedures;

public  class GetControlProcedureDescriptionHandler: IHandle<Core.Identity.Models.UserModel, ResponseModel<List<string>>>
{

	private Core.Identity.Models.UserModel _user;
	private GetProcedurNameModel _data;
	public GetControlProcedureDescriptionHandler(Core.Identity.Models.UserModel user, GetProcedurNameModel data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<List<string>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var fetchedData = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsArticleSupplierControlProceduresAccess.GetProcudreDescription(this._data.ProcedureName,this._data.ArtikelNummer);

			return ResponseModel<List<string>>.SuccessResponse(fetchedData);

		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}

	public ResponseModel<List<string>> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<string>>.AccessDeniedResponse();
		}

		return ResponseModel<List<string>>.SuccessResponse();
	}

}


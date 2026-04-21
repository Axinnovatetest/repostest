using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;


namespace Psz.Core.BaseData.Handlers.Article.ArticleReference;

public class DeleteArtikelCustomerReferencesHandler: IHandle<int, ResponseModel<int>>
{
	private UserModel _user { get; set; }
	public int _data { get; set; }
	public DeleteArtikelCustomerReferencesHandler(UserModel user, int id)
	{
		this._user = user;
		this._data = id;
	}
	public ResponseModel<int> Handle()
	{

		var validationResponse = this.Validate();
		if(!validationResponse.Success)
		{
			return validationResponse;
		}

		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

		var dbentity = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.Get(_data);
		if(dbentity is null)
			return ResponseModel<int>.FailureResponse(key: "1", value: $"the reference you are trying to delete does not exist !");

		try
		{
			botransaction.beginTransaction();


			var insert = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.DeleteWithTransaction(_data, botransaction.connection, botransaction.transaction);


			//logging
			var log = ObjectLogHelper.getLog(this._user, dbentity.Id, "Article_Customer_Reference", dbentity.CustomerReference, " ", Enums.ObjectLogEnums.Objects.Article_CustomerReference.GetDescription(), Enums.ObjectLogEnums.LogType.Delete);
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);

			if(botransaction.commit())
			{
				// -
				return ResponseModel<int>.SuccessResponse(insert);
			}
			else
			{
				return ResponseModel<int>.FailureResponse("Transaction error");
			}
		} catch(Exception e)
		{
			botransaction.rollback();
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<int> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<int>.AccessDeniedResponse();
		}
		return ResponseModel<int>.SuccessResponse();
	}

}

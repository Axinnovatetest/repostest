using Infrastructure.Data.Entities.Tables.BSD;
using Psz.Core.BaseData.Models.Article.ArticleReference;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;


namespace Psz.Core.BaseData.Handlers.Article.ArticleReference;

public class AddArtikelCustomerReferencesHandler
: IHandle<AddArtikelCustomerReferencesRequestModel, ResponseModel<int>>
{
	private UserModel _user { get; set; }
	public AddArtikelCustomerReferencesRequestModel _data { get; set; }
	public AddArtikelCustomerReferencesHandler(UserModel user, AddArtikelCustomerReferencesRequestModel referencedata)
	{
		this._user = user;
		this._data = referencedata;
	}
	public ResponseModel<int> Handle()
	{

		var validationResponse = this.Validate();
		if(!validationResponse.Success)
		{
			return validationResponse;
		}

		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

		try
		{
			botransaction.beginTransaction();

			var currentct = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.GetByCustomerNrAndArtikelID(_data.CustomerNumber ?? -1, _data.ArticleId ?? -1);

			if(currentct.Count > 0)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Customer Reference already exist !");
			}

			var entity = new ArtikelCustomerReferencesEntity()
			{
				ArticleId = _data.ArticleId,
				CustomerId = _data.CustomerId,
				CustomerName = _data.CustomerName,
				CustomerNumber = _data.CustomerNumber,
				CustomerReference = _data.CustomerReference,
				CreateDate = DateTime.Now,
				EditDate = DateTime.Now,
				EditUser = _user.Id,
				EditUserName = _user.Username,
				CreateUser = _user.Id,
				CreateUserName = _user.Username,
			};

			var insert = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.InsertWithTransaction(entity, botransaction.connection, botransaction.transaction);

			//logging
			var log = ObjectLogHelper.getLog(this._user, insert, "ArticlelCustomerReferences", null, this._data.CustomerReference, Enums.ObjectLogEnums.Objects.Article_CustomerReference.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
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

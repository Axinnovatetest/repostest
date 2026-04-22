using Infrastructure.Data.Entities.Tables.BSD;
using Psz.Core.BaseData.Models.Article.ArticleReference;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.ArticleReference;

public class EditArtikelCustomerReferencesHandler: IHandle<EditArtikelCustomerReferencesRequestModel, ResponseModel<int>>
{
	private UserModel _user { get; set; }
	public EditArtikelCustomerReferencesRequestModel _data { get; set; }
	public EditArtikelCustomerReferencesHandler(UserModel user, EditArtikelCustomerReferencesRequestModel referencedata)
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

			var currentctsnr = currentct.Where(x => x.CustomerNumber == _data.CustomerNumber && x.Id != _data.Id).ToList();

			var dbentity = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.Get(_data.Id);

			if(dbentity is null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"the reference you are trying to update does not exist !");
			if(currentctsnr != null && currentctsnr.Count > 0)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Customer Reference already exist !");

			var entity = new ArtikelCustomerReferencesEntity()
			{
				Id = dbentity.Id,
				ArticleId = dbentity.ArticleId,
				CustomerId = _data.CustomerId,
				CustomerName = _data.CustomerName,
				CustomerNumber = _data.CustomerNumber,
				CustomerReference = _data.CustomerReference,
				EditDate = DateTime.Now,
				EditUser = _user.Id,
				EditUserName = _user.Username,
				CreateDate = dbentity.CreateDate,
				CreateUser = dbentity.CreateUser,
				CreateUserName = dbentity.CreateUserName

			};

			//logging
			var update = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.UpdateWithTransaction(entity, botransaction.connection, botransaction.transaction);

			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
				new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
									ObjectLogHelper.getLog(this._user, entity.Id, "Customer Id from",
									$"{dbentity.CustomerId}",
									$"{_data.CustomerId}",
									Enums.ObjectLogEnums.Objects.Article_CustomerReference.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit),
									ObjectLogHelper.getLog(this._user, entity.Id, "Customer Name from",
									$"{dbentity.CustomerName}",
									$"{_data.CustomerName}",
									Enums.ObjectLogEnums.Objects.Article_CustomerReference.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit),
									ObjectLogHelper.getLog(this._user, entity.Id, "CustomerNumber from",
									$"{dbentity.CustomerNumber}",
									$"{_data.CustomerNumber}",
									Enums.ObjectLogEnums.Objects.Article_CustomerReference.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit),
									ObjectLogHelper.getLog(this._user, entity.Id, "CustomerReference from",
									$"{dbentity.CustomerReference}",
									$"{_data.CustomerNumber}",
									Enums.ObjectLogEnums.Objects.Article_CustomerReference.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit)
				});

			if(botransaction.commit())
			{
				// -
				return ResponseModel<int>.SuccessResponse(update);
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

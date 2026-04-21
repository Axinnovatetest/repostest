using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class GetZahlungskonditionenKundenHandler: IHandle<UserModel, ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenKundenModel>>>
{

	private UserModel _user { get; set; }
	public GetZahlungskonditionenKundenHandler(UserModel user)
	{
		this._user = user;
	}
	public ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenKundenModel>> Handle()
	{
		try
		{
			var validation = Validate();
			if(!validation.Success)
			{
				return validation;
			}

			return Perform(this._user);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	private ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenKundenModel>> Perform(UserModel user)
	{
		try
		{
			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetZahlungskonditionenKunden(null, null, 0);
			var restoreturn = fetchedData.Select(x => new GetZahlungskonditionenKundenModel(x)).ToList();

			int TotalCount = 0;
			if(restoreturn is not null && restoreturn.Count > 0)
			{
				TotalCount = restoreturn.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenKundenModel>>.SuccessResponse(
		 new IPaginatedResponseModel<GetZahlungskonditionenKundenModel>
		 {
			 Items = restoreturn,
			 TotalCount = TotalCount,
		 });
		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}
	public ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenKundenModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenKundenModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenKundenModel>>.SuccessResponse();
	}
}

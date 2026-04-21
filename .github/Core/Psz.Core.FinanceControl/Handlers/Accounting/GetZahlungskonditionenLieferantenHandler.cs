using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class GetZahlungskonditionenLieferantenHandler: IHandle<UserModel, ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>>>
{

	private UserModel _user { get; set; }
	public GetZahlungskonditionenLieferantenHandler(UserModel user)
	{
		this._user = user;
	}
	public ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>> Perform(UserModel user)
	{
		try
		{
			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetZahlungskonditionenLieferanten(null, null, 0);
			var restoreturn = fetchedData.Select(x => new GetZahlungskonditionenLieferantenModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>>.SuccessResponse(
		 new IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>
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
	public ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<GetZahlungskonditionenLieferantenModel>>.SuccessResponse();
	}
}

using System;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Accounting
{
	public class PSZ_BH_Kontenrahmen_GetHandler: IHandle<UserModel, ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>>>
	{

		private UserModel _user { get; set; }
		public PSZ_BH_Kontenrahmen_GetHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>> Handle()
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

		private ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>> Perform(UserModel user)
		{
			try
			{

				var fetchedData = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_KontenrahmenAccess.GetWithPagination(null, null, 0);
				var restoreturn = fetchedData.Select(x => new PSZ_BH_Kontenrahmen_Model(x)).ToList();

				int TotalCount = 0;
				if(fetchedData is not null && fetchedData.Count > 0)
				{
					TotalCount = fetchedData.FirstOrDefault().TotalCount;
				}
				return ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>>.SuccessResponse(
			 new IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>
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
		public ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>>.SuccessResponse();
		}
	}
}

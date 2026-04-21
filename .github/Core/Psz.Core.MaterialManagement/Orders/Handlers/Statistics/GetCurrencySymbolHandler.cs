using Infrastructure.Data.Entities.Joins.MTM.Order.Statistics;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;


namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetCurrencySymbolHandler: IHandle<UserModel, ResponseModel<GetCurrencySymbolModel>>
	{

		private int _data { get; set; }
		private UserModel _user { get; set; }
		public GetCurrencySymbolHandler(UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<GetCurrencySymbolModel> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this._user, this._data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<GetCurrencySymbolModel> Perform(UserModel user, int data)
		{
			try
			{



				List<DispowsDetailsCurrencyEntity> fetchedData = new();


				if(data > 0)
				{
					fetchedData = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.DispoAccess.GetCurrencySymbol(data);

					if(fetchedData is null || fetchedData.Count == 0)
					{
						return ResponseModel<GetCurrencySymbolModel>.NotFoundResponse();
					}

					var restoreturn = fetchedData.Select(x => new GetCurrencySymbolModel(x)).FirstOrDefault();

					return ResponseModel<GetCurrencySymbolModel>.SuccessResponse(restoreturn);
				}



				return ResponseModel<GetCurrencySymbolModel>.NotFoundResponse();

			} catch(Exception ex)
			{
				throw;
			}
		}
		public ResponseModel<GetCurrencySymbolModel> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<GetCurrencySymbolModel>.AccessDeniedResponse();
			}
			return ResponseModel<GetCurrencySymbolModel>.SuccessResponse();
		}
	}
}

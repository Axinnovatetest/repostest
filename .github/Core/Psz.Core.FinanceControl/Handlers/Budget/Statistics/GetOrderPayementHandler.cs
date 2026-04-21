using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetOrderPayementHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<OrderPayementsResponseModel>>>
	{

		public Psz.Core.Identity.Models.UserModel _user { get; set; }

		public GetOrderPayementHandler(UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<OrderPayementsResponseModel>> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();

				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}

				// GEt All Order
				var OrderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get();


				////Get All Status
				//var OrderDistinctsStatuses = Enum.GetValues(typeof(Enums.BudgetEnums.OrderStatuses)).Cast<Enums.BudgetEnums.OrderStatuses>().ToList();

				// get All Distincts Order Types 
				var OrderDistinctPaymentTypes = Enum.GetValues(typeof(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes)).Cast<Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes>().ToList();


				//  create List 
				var OrderPayementsEnties = new List<OrderPayementsResponseModel>();

				if(OrderEntities is null || OrderEntities.Count > 0)
				{
					if((OrderDistinctPaymentTypes is null || OrderDistinctPaymentTypes.Count > 0))
					{
						// filter project by Status and by type
						//--var OrderAccordingToStatuse = OrderEntities.FindAll(x => x.St)
						foreach(var entity in OrderDistinctPaymentTypes)
						{

							// filter project by Status and by type
							var OrderAccordingToStatuse = OrderEntities.FindAll(x => x.PoPaymentTypeName == entity.GetDescription());

							OrderPayementsEnties.Add(new OrderPayementsResponseModel((int)entity, OrderAccordingToStatuse.Count, entity.GetDescription()));

						}

					}
				}

				return ResponseModel<List<OrderPayementsResponseModel>>.SuccessResponse(OrderPayementsEnties);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}


		}

		public ResponseModel<List<OrderPayementsResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<OrderPayementsResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<OrderPayementsResponseModel>>.SuccessResponse();
		}
	}
}

using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetOrderTypesHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<OrderTypesResponseModel>>>
	{

		public Psz.Core.Identity.Models.UserModel _user { get; set; }

		public GetOrderTypesHandler(UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<OrderTypesResponseModel>> Handle()
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

				// get All Distincts Order Types according to project Enum
				var OrderDistinctTypes = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>().ToList();


				//  create List 
				var OrderTypesEnties = new List<OrderTypesResponseModel>();

				if(OrderEntities is null || OrderEntities.Count > 0)
				{
					if((OrderDistinctTypes is null || OrderDistinctTypes.Count > 0))
					{
						// filter project by Status and by type
						//--var OrderAccordingToStatuse = OrderEntities.FindAll(x => x.St)
						foreach(var entity in OrderDistinctTypes)
						{

							// filter project by Status and by type
							var OrderAccordingOfTypes = OrderEntities.FindAll(x => x.OrderType == entity.GetDescription());

							OrderTypesEnties.Add(new OrderTypesResponseModel((int)entity, OrderAccordingOfTypes.Count, entity.GetDescription()));

						}

					}
				}

				return ResponseModel<List<OrderTypesResponseModel>>.SuccessResponse(OrderTypesEnties);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}


		}

		public ResponseModel<List<OrderTypesResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<OrderTypesResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<OrderTypesResponseModel>>.SuccessResponse();
		}
	}
}

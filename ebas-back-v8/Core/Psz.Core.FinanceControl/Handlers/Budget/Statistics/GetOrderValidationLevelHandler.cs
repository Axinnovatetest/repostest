using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetOrderValidationLevelHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<OrderExpenseResponseModel>>>
	{

		public Psz.Core.Identity.Models.UserModel _user { get; set; }

		public GetOrderValidationLevelHandler(UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<OrderExpenseResponseModel>> Handle()
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
				var OrderDistinctValidationLevel = Enum.GetValues(typeof(Enums.BudgetEnums.ValidationLevels)).Cast<Enums.BudgetEnums.ValidationLevels>().ToList();

				//get booked ?? after


				//  create List 
				var OrderValidationLevelEnties = new List<OrderExpenseResponseModel>();

				if(OrderEntities is null || OrderEntities.Count > 0)
				{
					if((OrderDistinctValidationLevel is null || OrderDistinctValidationLevel.Count > 0))
					{
						// filter project by Status and by type
						//--var OrderAccordingToStatuse = OrderEntities.FindAll(x => x.St)
						foreach(var entity in OrderDistinctValidationLevel)
						{

							// filter project by Status and by type
							var OrderAccordingToValidationLevel = OrderEntities.FindAll(x => x.Level == (int)entity);

							OrderValidationLevelEnties.Add(new OrderExpenseResponseModel((int)entity, OrderAccordingToValidationLevel.Count, entity.GetDescription()));

						}

					}
				}

				return ResponseModel<List<OrderExpenseResponseModel>>.SuccessResponse(OrderValidationLevelEnties);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}


		}

		public ResponseModel<List<OrderExpenseResponseModel>> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<List<OrderExpenseResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<OrderExpenseResponseModel>>.SuccessResponse();
		}
	}
}

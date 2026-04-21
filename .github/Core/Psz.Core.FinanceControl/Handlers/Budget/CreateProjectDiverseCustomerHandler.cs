using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateProjectDiverseCustomerHandler
	{
		private Models.Budget.GetCustomer_Project_DiverseModel _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }
		public CreateProjectDiverseCustomerHandler(Models.Budget.GetCustomer_Project_DiverseModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var CustomerDiverseEntity = new Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity()
				{



					Customer_Contact_Description_Project_Diverse = _data.Customer_Contact_Description_Project_Diverse,
					Customer_Contact_Project_Diverse = _data.Customer_Contact_Project_Diverse,
					Custommer_Name_Project_Diverse = _data.Custommer_Name_Project_Diverse,
					Id_Customer_Project_Diverse = _data.Id_Customer_Project_Diverse,
					Id_Diverse_Customer_Project = _data.Id_Diverse_Customer_Project,
					Id_Project_Diverse = _data.Id_Project_Diverse,
					kumdennummer_Project_Diverse = _data.kumdennummer_Project_Diverse,
					Nr_Customer_Project_Diverse = _data.Nr_Customer_Project_Diverse,
					Ort_Project_Diverse = _data.Ort_Project_Diverse,


				};
				var InsertedCustomerDiverse = Infrastructure.Data.Access.Tables.FNC.Budget_Diverse_Project_CustomerAccess.Insert(CustomerDiverseEntity);
				return ResponseModel<int>.SuccessResponse(InsertedCustomerDiverse);



			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.ConfigCreateArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}

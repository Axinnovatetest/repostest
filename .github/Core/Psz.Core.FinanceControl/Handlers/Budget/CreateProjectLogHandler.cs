using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateProjectLogHandler
	{
		//private Models.Budget.GetProjects_LogModel _data { get; set; }

		private Models.Budget.GetProjects_Diverse_LogModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateProjectLogHandler(Models.Budget.GetProjects_Diverse_LogModel data, Identity.Models.UserModel user)
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
				var ProjectLogEntity = new Infrastructure.Data.Entities.Tables.FNC.Project_Log_Diverse_BudgetEntity()
				{

					Id_proj = _data.Id_proj,
					Name_proj = _data.Name_proj,
					Id_Customer = _data.Id_Customer,
					Nr_Customer = _data.Nr_Customer,
					Proj_Budget = _data.Proj_Budget,
					Id_Responsable = _data.Id_Responsable,
					Id_State = _data.Id_State,
					Id_Currency = _data.Id_Currency,
					Id_Land = _data.Id_Land,
					Id_Dept = _data.Id_Dept,
					Id_Type = _data.Id_Type,
					Description = _data.Description,
					Action_date = _data.Action_date,
					Id_LS = _data.Id_LS,
					Id_state = _data.Id_State,
					Id_user = _user.Id,
					Customer_Contact_Description_Project_Diverse = _data.Customer_Contact_Description_Project_Diverse,
					Customer_Contact_Project_Diverse = _data.Customer_Contact_Project_Diverse,
					Custommer_Name_Project_Diverse = _data.Custommer_Name_Project_Diverse,
					Id_Customer_Project_Diverse = _data.Id_Customer,
					Id_Diverse_Customer_Project = _data.Id_Diverse_Customer_Project,
					Id_Project_Diverse = _data.Id_proj,
					kumdennummer_Project_Diverse = _data.kumdennummer_Project_Diverse,
					Nr_Customer_Project_Diverse = _data.Nr_Customer,
					Ort_Project_Diverse = _data.Ort_Project_Diverse,
				};
				//var InsertedProject = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.InsertProjectLog(ProjectLogEntity);


				/* var CustomerDiverseEntity = new Infrastructure.Data.Entities.Tables.FNC.Budget_Diverse_Project_CustomerEntity()
					 {

						 Customer_Contact_Description_Project_Diverse = _data2.Customer_Contact_Description_Project_Diverse,
						 Customer_Contact_Project_Diverse = _data2.Customer_Contact_Project_Diverse,
						 Custommer_Name_Project_Diverse = _data2.Custommer_Name_Project_Diverse,
						 Id_Customer_Project_Diverse =_data.Id_Customer,
						 Id_Diverse_Customer_Project=_data2.Id_Diverse_Customer_Project,
						 Id_Project_Diverse=_data.Id_proj,
						 kumdennummer_Project_Diverse= _data2.kumdennummer_Project_Diverse,
						 Nr_Customer_Project_Diverse = _data.Nr_Customer,
						 Ort_Project_Diverse = _data2.Ort_Project_Diverse,


					 };

				   if (ProjectLogEntity.Id_Customer == 80)
					 {
						 var InsertedCustomerDiverse = Infrastructure.Data.Access.Tables.FNC.Budget_Diverse_Project_CustomerAccess.Insert(CustomerDiverseEntity);
						 return ResponseModel<int>.SuccessResponse(InsertedCustomerDiverse);

					 }*/
				return ResponseModel<int>.SuccessResponse(/*InsertedProject*/0);


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

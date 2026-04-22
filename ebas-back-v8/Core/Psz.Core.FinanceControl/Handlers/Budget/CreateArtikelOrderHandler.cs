using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateArtikelOrderHandler
	{
		private Models.Budget.ArtikelOrderModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateArtikelOrderHandler(Models.Budget.ArtikelOrderModel data, Identity.Models.UserModel user)
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
				var ArtikelOrderEntity = new Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity()
				{
					Id_Order = _data.Id_Order,
					Id_Article = _data.Id_Article,
					Id_Currency_Article = _data.Id_Currency_Article,
					Currency_Article = _data.Currency_Article,
					//Id_Dept = _data.Id_Dept,
					//Dept_name = _data.Dept_name,
					//Id_Land = _data.Id_Land,
					//Land_name = _data.Land_name,
					//Id_Project = _data.Id_Project,
					Quantity = _data.Quantity,
					Unit_Price = _data.Unit_Price,
					TotalCost_Article = _data.TotalCost_Article,
					//Action_Article = _data.Action_Article,
					//Article_date = _data.Article_date,
					//Id_User = _data.Id_User,
					VAT = _data.VAT

				};
				//var InsertedLand = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.InsertArtikelOrder(ArtikelOrderEntity);
				return ResponseModel<int>.SuccessResponse(/*InsertedLand*/0);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.AssignCreateLand)
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

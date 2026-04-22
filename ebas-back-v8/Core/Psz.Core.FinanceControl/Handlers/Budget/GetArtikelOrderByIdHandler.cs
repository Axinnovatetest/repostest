using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetArtikelOrderByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.ArtikelOrderModel>>

	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetArtikelOrderByIdHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;

		}
		public ResponseModel<Models.Budget.ArtikelOrderModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.ArtikelOrderModel();

				//var articleOrder = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetArtikelOrderByIdArtikel(this._data);
				//responseBody.Id_AO = articleOrder.Id_AO;
				//responseBody.Id_Article = articleOrder.Id_Article;
				//responseBody.Id_Order = articleOrder.Id_Order;
				////responseBody.Id_Project = articleOrder.Id_Project;
				////responseBody.Id_Dept = articleOrder.Id_Dept;
				////responseBody.Id_Land = articleOrder.Id_Land;
				//responseBody.Id_Currency_Article = articleOrder.Id_Currency_Article;
				////responseBody.Id_User = articleOrder.Id_User;
				//responseBody.TotalCost_Article = articleOrder.TotalCost_Article;
				//responseBody.Unit_Price = articleOrder.Unit_Price;
				//responseBody.Quantity = articleOrder.Quantity;
				////responseBody.Article_date = articleOrder.Article_date;
				////responseBody.Action_Article = articleOrder.Action_Article;
				//responseBody.Currency_Article = articleOrder.Currency_Article;
				////responseBody.Dept_name = articleOrder.Dept_name;
				////responseBody.Land_name = articleOrder.Land_name;

				return ResponseModel<Models.Budget.ArtikelOrderModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.ArtikelOrderModel> Validate()
		{

			return ResponseModel<Models.Budget.ArtikelOrderModel>.SuccessResponse();


		}

	}
}

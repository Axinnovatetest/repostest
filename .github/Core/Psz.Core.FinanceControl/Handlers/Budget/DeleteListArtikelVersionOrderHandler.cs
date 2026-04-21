using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteListArtikelVersionOrderHandler: IHandle<Models.Budget.InsertedListArticleOrderModel, ResponseModel<int>>
	{
		private int _ArtikelOrderID { get; set; }
		private Models.Budget.InsertedListArticleOrderModel _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }
		private int Max_Ver_Ord { get; set; }
		public DeleteListArtikelVersionOrderHandler(int Id_ArtikelOrder, int Max_Ver_Ord, Models.Budget.InsertedListArticleOrderModel data, Identity.Models.UserModel user)
		{
			this._ArtikelOrderID = Id_ArtikelOrder;
			this._user = user;
			this._data = data;
			this.Max_Ver_Ord = Max_Ver_Ord;
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

				//var artikelOrderentity = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.Get(_ArtikelOrderID);


				//if (artikelOrderentity == null)
				//{
				//    return ResponseModel<int>.SuccessResponse();
				//}
				//var _data2 = new List<Models.Budget.ArtikelOrderParamsModel>();

				//var LastVersionOrder = Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.GetLastVersionOrder(this.Max_Ver_Ord);
				//foreach (var order_tableEntity in LastVersionOrder)
				//{
				//    _data2.Add(new Models.Budget.ArtikelOrderParamsModel(order_tableEntity));
				//}


				//var ArtikelOrderEntity = new Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity();
				//foreach (var Artikel in _data.ArtikelsOrder)
				//{
				//    ArtikelOrderEntity.Max_VO = _data2[0].Max_VO;

				//    ArtikelOrderEntity.TotalCost_Order = Artikel.TotalCost_Order;

				//}
				return ResponseModel<int>.SuccessResponse(/*Infrastructure.Data.Access.Tables.FNC.Article_OrderAccess.DeleteArtikel_Order(artikelOrderentity.Id_AO,this.Max_Ver_Ord,ArtikelOrderEntity)*/0);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			/*if (this._user == null || !this._user.Access.Budget.Commande)
            {
                return ResponseModel<int>.AccessDeniedResponse();
            }*/
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}

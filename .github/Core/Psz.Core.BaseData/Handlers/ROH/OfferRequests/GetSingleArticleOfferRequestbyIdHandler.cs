using Infrastructure.Services.Reporting.Models.FNC;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.BaseData.Helpers;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests
{
	public class GetSingleArticleOfferRequestbyIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<EditOfferForSingleManufacturerNumberModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetSingleArticleOfferRequestbyIdHandler(Identity.Models.UserModel user, int Id)
		{
			this._user = user;
			this._data = Id;
		}

		public ResponseModel<EditOfferForSingleManufacturerNumberModel> Handle()
		{

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var fetchedOffers = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(_data);
				if(fetchedOffers is null)
					return ResponseModel<EditOfferForSingleManufacturerNumberModel>.SuccessResponse(new EditOfferForSingleManufacturerNumberModel());

				bool IsValidUnit = false;
				var datatoreturn = new EditOfferForSingleManufacturerNumberModel()
				{
					Id = fetchedOffers.Id,
					CustomQuantity = fetchedOffers.CustomQuantity,
					Graduatedprice1 = fetchedOffers.Graduatedprice1,
					Graduatedprice2 = fetchedOffers.Graduatedprice2,
					Graduatedprice3 = fetchedOffers.Graduatedprice3,
					Graduatedquantity1 = fetchedOffers.Graduatedquantity1,
					Graduatedquantity2 = fetchedOffers.Graduatedquantity2,
					Graduatedquantity3 = fetchedOffers.Graduatedquantity3,
					YearlyQuantity = fetchedOffers.YearlyQuantity,
					SupplierContactName = fetchedOffers.SupplierContactName,
					ManufactuerNumber = fetchedOffers.ManufactuerNumber,
					OfferRequestArticleDescription = fetchedOffers.OfferRequestArticleDescription,
					ProjectName = fetchedOffers.ProjectName,
					EndCustomer = fetchedOffers.EndCustomer,
					warengrupId = OfferRequestsManager.IsPositiveInteger(fetchedOffers.unit) ? fetchedOffers.unit : "-1",

				};

				return ResponseModel<EditOfferForSingleManufacturerNumberModel>.SuccessResponse(datatoreturn);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<EditOfferForSingleManufacturerNumberModel> Validate()
		{
			if(this._user == null /*|| this._user.Access.____*/)
			{
				return ResponseModel<EditOfferForSingleManufacturerNumberModel>.AccessDeniedResponse();
			}
			return ResponseModel<EditOfferForSingleManufacturerNumberModel>.SuccessResponse();
		}
	}

}

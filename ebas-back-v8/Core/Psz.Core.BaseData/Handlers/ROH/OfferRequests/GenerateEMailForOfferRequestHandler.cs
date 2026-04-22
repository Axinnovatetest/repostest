using Infrastructure.Services.Reporting.Models.FNC;
using Infrastructure.Services.Reporting.Models.MTM;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class GenerateEMailForOfferRequestHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<OfferRequestEMail>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }
	public GenerateEMailForOfferRequestHandler(Identity.Models.UserModel user, int id)
	{
		this._user = user;
		this._data = id;
	}

	public ResponseModel<List<OfferRequestEMail>> Handle()
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
				return ResponseModel<List<OfferRequestEMail>>.SuccessResponse(new List<OfferRequestEMail>());

			var offerfamily = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.GetByRequestUI(fetchedOffers.RequestUI);

			var offerrequestsPerSupplier = new List<OfferRequestPerSupplierModel>();

			var uniqueSuppliers = offerfamily.Select(x => x.SupplierId).Distinct().ToList();

			var offerRequestsSlicedUponUniqueSuppliers = new List<OfferRequestPerSupplierModel>();

			foreach(var item in uniqueSuppliers)
			{

				var uniquePerSupplierItem = new OfferRequestPerSupplierModel();
				uniquePerSupplierItem.SupplierId = item;
				var requestforthisSupplier = offerfamily.Where(x => x.SupplierId == item).ToList();
				var dataForTheEmail = requestforthisSupplier.Select(x => new MaterialRequestModel() { MatNr = x.ManufactuerNumber, Jahresmenge = x.YearlyQuantity, Hersteller = x.SupplierName }).ToList();
				uniquePerSupplierItem.OfferRequests.AddRange(dataForTheEmail);
				offerRequestsSlicedUponUniqueSuppliers.Add(uniquePerSupplierItem);
			}
			var restoreturn = new List<OfferRequestEMail>();


			foreach(var item in offerRequestsSlicedUponUniqueSuppliers)
			{
				var email = new OfferRequestEMail();
				//email.addresses.Add (offerfamily.Where(x=>x.SupplierId == item.SupplierId).First().SupplierContactEmail);
				var dataforemail = item.OfferRequests.Select(x => new Infrastructure.Services.Email.Models.MaterialRequestbaseModel() { Hersteller = x.Hersteller, MatNr = x.MatNr, Jahresmenge = x.Jahresmenge, Bez = x.Bez }).ToList();
			//	email.EmailContent = Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailAsHtmlContent(dataforemail, _user.Name);
				email.multipleSuppliers = uniqueSuppliers.Count() > 1 ? true : false;
				restoreturn.Add(email);
			}

			return ResponseModel<List<OfferRequestEMail>>.SuccessResponse(restoreturn);

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<OfferRequestEMail>> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<List<OfferRequestEMail>>.AccessDeniedResponse();
		}
		return ResponseModel<List<OfferRequestEMail>>.SuccessResponse();
	}
}


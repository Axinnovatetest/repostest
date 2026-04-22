using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.Common.Models;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests
{
    class GenerateMaterialTableForOfferRequestByIdsForSingleSupplierHandler: IHandle<Identity.Models.UserModel, ResponseModel<GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private List<int> _data { get; set; }
		private  int _LanguageId { get; set; }
		public GenerateMaterialTableForOfferRequestByIdsForSingleSupplierHandler(Identity.Models.UserModel user, List<int> Id,int LanguageId)
		{
			this._user = user;
			this._data = Id;
			_LanguageId = LanguageId;
		}

		public ResponseModel<GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var offerfamily = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(_data.First());

				var AllSimilarEmails = Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsAccess.Get(_data);

				if(offerfamily is null)
					return ResponseModel<GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel>.NotFoundResponse();



				var MaterialTable = GenerateEmailSubjectBasedOnDefaultSupplierLanguage(_LanguageId, AllSimilarEmails);

				var restoreturn = new GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel() 
				{ 
				MaterialTable = MaterialTable,
				Customer = offerfamily.EndCustomer,
				ProjectName = offerfamily.ProjectName
				};
				return ResponseModel<GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel>.SuccessResponse(restoreturn);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		private string GenerateEmailSubjectBasedOnDefaultSupplierLanguage(int supplierID, List<ArticleOfferRequestsEntity> allOfferForOneSupplier)
		{

			if(supplierID <= 0)
				supplierID = 4;

			var emailBody = supplierID switch
			{
				1 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialTable_EN(Psz.Core.BaseData.Helpers.SpecialHelper.GenerateMaterials(allOfferForOneSupplier)),
				2 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialTable_FR(Psz.Core.BaseData.Helpers.SpecialHelper.GenerateMaterials(allOfferForOneSupplier)),
				3 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialTable_SP(Psz.Core.BaseData.Helpers.SpecialHelper.GenerateMaterials(allOfferForOneSupplier)),
				_ => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialTable_DE(Psz.Core.BaseData.Helpers.SpecialHelper.GenerateMaterials(allOfferForOneSupplier))
			};
			return emailBody;
		}
		
		public ResponseModel<GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel> Validate()
		{
			if(this._user == null /*|| this._user.Access.____*/)
			{
				return ResponseModel<GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel>.AccessDeniedResponse();
			}
			return ResponseModel<GenerateMaterialTableForOfferRequestByIdsForSingleSupplierModel>.SuccessResponse();
		}
	}
}

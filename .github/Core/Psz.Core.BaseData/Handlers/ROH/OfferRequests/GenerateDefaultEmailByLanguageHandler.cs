using Infrastructure.Data.Access.Tables.BSD;
using Microsoft.AspNetCore.Mvc.Formatters;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class GenerateDefaultEmailByLanguageHandler: IHandle<Identity.Models.UserModel, ResponseModel<ChangeEmailLanguageModel>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }
	private List<int> _offerId { get; set; }
	public GenerateDefaultEmailByLanguageHandler(Identity.Models.UserModel user, GenerateEmailForOneSupplierModel data)
	{
		this._user = user;
		this._data = data.Id;
		_offerId = data.OfferIds;
	}

	public ResponseModel<ChangeEmailLanguageModel> Handle()
	{
		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

		try
		{


			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var restoreturn = new ChangeEmailLanguageModel();

			
			
			var generatedData  = new GenerateMaterialTableForOfferRequestByIdsForSingleSupplierHandler(_user, _offerId, _data).Handle().Body;
			restoreturn.EmailContent = GenerateEmailBasedOnDefaultSupplierLanguage(_data,generatedData.ProjectName, generatedData.Customer);
			restoreturn.MaterialTable = generatedData.MaterialTable;
			restoreturn.EmailSubject = GenerateEmailSubjectBasedOnDefaultSupplierLanguage(_data, generatedData.ProjectName, generatedData.Customer);
			return ResponseModel<ChangeEmailLanguageModel>.SuccessResponse(restoreturn);

		} catch(Exception e)
		{
			botransaction.rollback();
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public string GenerateEmailBasedOnDefaultSupplierLanguage(int supplierID,string Project,string Customer)
	{
		if(supplierID <= 0)
			supplierID = 4;


		var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(_user.Id);

		var emailBody = supplierID switch
		{
			1 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataEN(_user.Name, userEntity.TelephoneIP,_user.Email, Project, Customer),
			2 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataFR(_user.Name, userEntity.TelephoneIP, _user.Email, Project, Customer),
			3 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataES(_user.Name, userEntity.TelephoneIP, _user.Email, Project, Customer),
			_ => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailAsHtmlContentWithoutMaterialDataDE(_user.Name, userEntity.TelephoneIP, _user.Email, Project, Customer)
		};
		return emailBody;
	}

	private string GenerateEmailSubjectBasedOnDefaultSupplierLanguage(int supplierID,string ProjectName, string Customer)
	{

		if(supplierID <= 0)
			supplierID = 4;

		var emailBody = supplierID switch
		{
			1 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetEN(ProjectName, Customer),
			2 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetFR(ProjectName, Customer),
			3 => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetSP(ProjectName, Customer),
			_ => Infrastructure.Services.Helpers.EmailHelper.GenerateMaterialRequestEmailSubjetDE(ProjectName, Customer)
		};
		return emailBody;
	}


	public ResponseModel<ChangeEmailLanguageModel> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<ChangeEmailLanguageModel>.AccessDeniedResponse();
		}
		return ResponseModel<ChangeEmailLanguageModel>.SuccessResponse();
	}
}

using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Customer.ContactPerson
{
	public class DeleteCustomerContactPersonHandler: IHandle<int, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public DeleteCustomerContactPersonHandler(Identity.Models.UserModel user, int Id)
		{
			this._data = Id;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var contactPersonEntity = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Get(this._data);
			var KundenId = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(contactPersonEntity.Nummer ?? -1).Nr;
			//log
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Delete;
			var log = ObjectLogHelper.getContactPersonLog(this._user, KundenId, "Customer", contactPersonEntity.Ansprechpartner, null, Enums.ObjectLogEnums.Objects.Customer_ContcatPerson.GetDescription(), logTypeEdit);
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
			var response = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Delete(this._data);
			return ResponseModel<int>.SuccessResponse(response);
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var contactPersonEntity = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Get(this._data);
			if(contactPersonEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Contact Person not found"}
					}
				};
			}

			var contactArticles = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByCustomerContactPersonId(this._data);
			if(contactArticles?.Count > 0)
			{
				var salesArticleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(contactArticles.Select(x => x.ArtikelNr).ToList());
				return ResponseModel<int>.FailureResponse($"Delete aborted: Contact is used in following articles [{string.Join(", ", salesArticleEntities.Take(5).Select(x => x.ArtikelNummer))}].");
			}

			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerTechnicianId(this._data);
			if(articleEntities?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Delete aborted: Contact is set for articles [{string.Join(",", articleEntities.Take(5).Select(x => x.ArtikelNummer))}]");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}

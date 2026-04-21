using Psz.Core.BaseData.Models;
using Psz.Core.Common.Models;

using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article
{
	public class GetTechnikerKundeHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<TechnikerKundeModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public string _data { get; set; }
		public GetTechnikerKundeHandler(Identity.Models.UserModel user, string _data)
		{
			this._user = user;
			this._data = _data;
		}
		public ResponseModel<List<TechnikerKundeModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<TechnikerKundeModel>();
				var PackingListEntity = Infrastructure.Data.Access.Tables.ArtikelAccess.GetTechnikerKunde(_data);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new TechnikerKundeModel(k)).ToList();

				return ResponseModel<List<TechnikerKundeModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<List<TechnikerKundeModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<TechnikerKundeModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<TechnikerKundeModel>>.SuccessResponse();
		}
	}
}

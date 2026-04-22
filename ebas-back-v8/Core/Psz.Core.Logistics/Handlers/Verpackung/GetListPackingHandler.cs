using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Verpackung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Verpackung
{
	public class GetListPackingHandler: IHandle<Identity.Models.UserModel, ResponseModel<PackungModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetListPackingHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<PackungModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new PackungModel();
				var mitarbeiter = new List<KeyValuePair<string, string>>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetListePacking();
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response.listeVerpackung = PackingListEntity.Select(k => new PackingModel(k)).ToList();
				var MitarbeiterListEntity = Infrastructure.Data.Access.Joins.Logistics.PackingAccess.GetMitarbeiterLogistic();
				if(MitarbeiterListEntity != null && MitarbeiterListEntity.Count > 0)
				{
					foreach(var item in MitarbeiterListEntity)
					{
						mitarbeiter.Add(new KeyValuePair<string, string>(item.username, item.name));
					}
				}
				response.listeMitarbeiter = mitarbeiter;

				return ResponseModel<PackungModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<PackungModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<PackungModel>.AccessDeniedResponse();
			}

			return ResponseModel<PackungModel>.SuccessResponse();
		}
	}
}
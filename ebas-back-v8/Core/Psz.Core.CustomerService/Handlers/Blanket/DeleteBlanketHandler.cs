using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class DeleteBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteBlanketHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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
				var BlanketExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data);
				var RahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.DeleteByNr(this._data);
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.DeleteByAngeboteNr(this._data);
				Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.DeleteByAngeboteNr(this._data);
				Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.DeleteByRahmenNrNr(this._data);
				//logging
				var _log = new Helpers.LogHelper((int)BlanketExtension.AngeboteNr, RahmenEntity.Angebot_Nr ?? -1, int.TryParse(RahmenEntity.Projekt_Nr, out var v) ? v : 0, RahmenEntity.Typ, Helpers.LogHelper.LogType.DELETIONOBJECT, "CTS", _user)
			 .LogCTS(null, null, null, BlanketExtension.Id);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data);
			if(rahmenExtension.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale)
			{
				var rahmenItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data);
				if(rahmenItemsEntities != null && rahmenItemsEntities.Count > 0)
				{
					var abLinkCheck = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(rahmenItemsEntities.Select(x => x.Nr).ToList());
					if(abLinkCheck != null && abLinkCheck.Count > 0)
						return ResponseModel<int>.FailureResponse("One or more position(s) of this Rahmen are linked to AB, deletion not allowed.");
				}
			}
			else
			{
				var rahmenItemsEntities = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByBestellungNr(this._data);
				if(rahmenItemsEntities != null && rahmenItemsEntities.Count > 0)
				{
					var bsLinkCheck = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetbyRahmenPositions(rahmenItemsEntities.Select(x => x.Nr).ToList());
					if(bsLinkCheck?.Count > 0)
						return ResponseModel<int>.FailureResponse("One or more position(s) of this Rahmen are linked to BE, deletion not allowed.");
				}
			}

			var BlanketExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data);
			if(BlanketExtension.StatusId != 0)
			{
				return ResponseModel<int>.FailureResponse("this agreement is not draft.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}

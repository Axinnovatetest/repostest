using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class DeleteBlanketPositionHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteBlanketPositionHandler(Identity.Models.UserModel user, int id)
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
				var orderExtensionDb = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(this._data);
				var rahmenAngebotEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderExtensionDb.RahmenNr);
				Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.DeleteByAngeboteArtikelNr(this._data);
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(this._data);
				Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.DeleteByPosition(_data);

				//updating sum price
				Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(orderExtensionDb.RahmenNr);
				//logging
				var _log = new Helpers.LogHelper((int)orderExtensionDb.RahmenNr, (int)rahmenAngebotEntity.Angebot_Nr, int.TryParse(rahmenAngebotEntity.Projekt_Nr, out var v) ? v : 0, "Rahmenauftrag", Helpers.LogHelper.LogType.DELETIONPOS, "CTS", _user)
			 .LogCTS(null, null, null, (int)(orderExtensionDb?.Id), orderExtensionDb.AngeboteArtikelNr);
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

			var orderExtensionDb = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(this._data);
			if(orderExtensionDb == null)
				return ResponseModel<int>.FailureResponse("position not found");

			var ListpositionsAb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetAbByRahmenPosition(this._data);
			if(ListpositionsAb != null && ListpositionsAb.Count > 0)
			{
				return ResponseModel<int>.FailureResponse("position not allowed to be deleted. It's used by AB positions");
			}
			var listPositionsBS = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetbyRahmenPositions(new System.Collections.Generic.List<int> { _data });
			if(listPositionsBS != null && listPositionsBS.Count > 0)
			{
				return ResponseModel<int>.FailureResponse("position is linked to orders, delete not allowed.");
			}
			var rahmenExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(orderExtensionDb.RahmenNr);
			if(rahmenExtension.StatusId != (int)Enums.BlanketEnums.RAStatus.Draft)
				return ResponseModel<int>.FailureResponse("Delete allowed only in Draft Status .");
			if(rahmenExtension.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale)
			{
				DateTime _newDate, _oldDate;
				_newDate = _oldDate = orderExtensionDb.GultigBis ?? new DateTime(1900, 1, 1);
				var horizonCheck = Helpers.HorizonsHelper.userHasRAPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
				if(!horizonCheck)
					return ResponseModel<int>.FailureResponse(messages);
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}

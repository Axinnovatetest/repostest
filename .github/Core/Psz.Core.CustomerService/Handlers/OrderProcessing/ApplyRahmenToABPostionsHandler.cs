using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class ApplyRahmenToABPostionsHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private ApplayRahmenToABPosEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ApplyRahmenToABPostionsHandler(Identity.Models.UserModel user, ApplayRahmenToABPosEntryModel data)
		{
			this._user = user;
			this._data = data;
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

				// - 2022-08-05 - Do not allow updateRahmen from AB Pos - Schremmer
				//////var ABPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.ABPositionNr);
				//////var RahmenPositionentity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.RahmenPositonNr ?? -1);
				//////var aBPosRahmenList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPosition(_data.RahmenPositonNr ?? -1) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				//////var rahmenPositionsToUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				//////var abPositionToUpdate = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity();
				//////if (_data.RahmenPositonNr.HasValue)
				//////{
				//////    if (ABPositionEntity != null && ABPositionEntity.ABPoszuRAPos.HasValue && ABPositionEntity.ABPoszuRAPos.Value != 0 && ABPositionEntity.ABPoszuRAPos.Value != -1)
				//////    {
				//////        var rahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(ABPositionEntity.ABPoszuRAPos ?? -1);
				//////        //choosing same rahmen
				//////        if (rahmenPosition != null && rahmenPosition.Nr == _data.RahmenPositonNr)
				//////        {
				//////            // - Rahme.OriginalQty >= Sum(aBPosRahmenList) + NewQty - OldQty
				//////            var availableQty = CalculateRahmenVailableQty(RahmenPositionentity.Nr, ABPositionEntity.Nr);
				//////            if (ABPositionEntity.OriginalAnzahl > availableQty)
				//////                return ResponseModel<int>.FailureResponse($"Ordred quantity [{ABPositionEntity.OriginalAnzahl}] is bigger then rahmen available quantity [{availableQty}]");

				//////            //return old qty
				//////            rahmenPosition.Anzahl += ABPositionEntity.OriginalAnzahl;
				//////            rahmenPosition.Geliefert -= ABPositionEntity.OriginalAnzahl;
				//////            //take new qty
				//////            rahmenPosition.Anzahl -= ABPositionEntity.OriginalAnzahl;
				//////            rahmenPosition.Geliefert += ABPositionEntity.OriginalAnzahl;

				//////            //get right price from rahmen prices history
				//////            var rahmenPriceToApply = RahmenPositionentity.Einzelpreis;
				//////            if (ABPositionEntity.Liefertermin.HasValue || ABPositionEntity.Wunschtermin.HasValue)
				//////            {
				//////                var rahmenPrices = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(rahmenPosition.Nr, ABPositionEntity.Liefertermin ?? ABPositionEntity.Wunschtermin);
				//////                var rightPrice = rahmenPrices?[0].Price;
				//////                if (rightPrice != null)
				//////                    rahmenPriceToApply = rightPrice;
				//////            }

				//////            abPositionToUpdate = Helpers.BlanketHelper.GetCalculatedPositon(ABPositionEntity.Nr, ABPositionEntity.OriginalAnzahl ?? 0m, true, rahmenPriceToApply ?? 0m, rahmenPosition.Nr);
				//////            rahmenPositionsToUpdate.Add(rahmenPosition);

				//////        }
				//////        // choosing diffrent rahmen
				//////        else if (rahmenPosition != null && rahmenPosition.Nr != _data.RahmenPositonNr)
				//////        {
				//////            // - Rahme.OriginalQty >= Sum(aBPosRahmenList) + NewQty - OldQty
				//////            var newRahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.RahmenPositonNr ?? -1);
				//////            var availableQty = CalculateRahmenVailableQty(newRahmenPosition.Nr, ABPositionEntity.Nr);
				//////            if (ABPositionEntity.OriginalAnzahl > availableQty)
				//////                return ResponseModel<int>.FailureResponse($"Ordred quantity [{ABPositionEntity.OriginalAnzahl}] is bigger then rahmen available quantity [{availableQty}]");
				//////            //return old qty
				//////            rahmenPosition.Anzahl += ABPositionEntity.OriginalAnzahl;
				//////            rahmenPosition.Geliefert -= ABPositionEntity.OriginalAnzahl;
				//////            //take new qty
				//////            newRahmenPosition.Anzahl -= ABPositionEntity.OriginalAnzahl;
				//////            newRahmenPosition.Geliefert += ABPositionEntity.OriginalAnzahl;

				//////            rahmenPositionsToUpdate.Add(rahmenPosition);
				//////            rahmenPositionsToUpdate.Add(newRahmenPosition);

				//////            //get right price from rahmen prices history
				//////            var rahmenPriceToApply = RahmenPositionentity.Einzelpreis;
				//////            if (ABPositionEntity.Liefertermin.HasValue || ABPositionEntity.Wunschtermin.HasValue)
				//////            {
				//////                var rahmenPrices = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(newRahmenPosition.Nr, ABPositionEntity.Liefertermin ?? ABPositionEntity.Wunschtermin);
				//////                var rightPrice = rahmenPrices?[0].Price;
				//////                if (rightPrice != null)
				//////                    rahmenPriceToApply = rightPrice;
				//////            }
				//////            abPositionToUpdate = Helpers.BlanketHelper.GetCalculatedPositon(ABPositionEntity.Nr, ABPositionEntity.OriginalAnzahl ?? 0m, true, rahmenPriceToApply ?? 0m, newRahmenPosition.Nr);

				//////        }
				//////    }
				//////    else
				//////    {
				//////        // - Old AB Position w/o RA
				//////        var newRahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.RahmenPositonNr ?? -1);
				//////        var availableQty = CalculateRahmenVailableQty(newRahmenPosition.Nr, ABPositionEntity.Nr);
				//////        if (ABPositionEntity.OriginalAnzahl > availableQty)
				//////            return ResponseModel<int>.FailureResponse($"Ordred quantity [{ABPositionEntity.OriginalAnzahl}] is bigger then rahmen available quantity [{availableQty}]");

				//////        //take new qty
				//////        newRahmenPosition.Anzahl -= ABPositionEntity.OriginalAnzahl;
				//////        newRahmenPosition.Geliefert += ABPositionEntity.OriginalAnzahl;

				//////        //get right price from rahmen prices history
				//////        var rahmenPriceToApply = RahmenPositionentity.Einzelpreis;
				//////        if (ABPositionEntity.Liefertermin.HasValue || ABPositionEntity.Wunschtermin.HasValue)
				//////        {
				//////            var rahmenPrices = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(newRahmenPosition.Nr, ABPositionEntity.Liefertermin ?? ABPositionEntity.Wunschtermin);
				//////            var rightPrice = rahmenPrices?[0].Price;
				//////            if (rightPrice != null)
				//////                rahmenPriceToApply = rightPrice;
				//////        }

				//////        abPositionToUpdate = Helpers.BlanketHelper.GetCalculatedPositon(ABPositionEntity.Nr, ABPositionEntity.OriginalAnzahl ?? 0m, true, rahmenPriceToApply ?? 0m, newRahmenPosition.Nr);
				//////        rahmenPositionsToUpdate.Add(newRahmenPosition);
				//////    }
				//////}
				//////else // not choosing rahmen
				//////{
				//////    // - Old AB Pos has RA link
				//////    if (ABPositionEntity != null && ABPositionEntity.ABPoszuRAPos.HasValue && ABPositionEntity.ABPoszuRAPos.Value > 0)
				//////    {
				//////        // - Delete link to RA -- return Qty to RA
				//////        var rahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(ABPositionEntity.ABPoszuRAPos ?? -1);
				//////        rahmenPosition.Anzahl += ABPositionEntity.OriginalAnzahl;
				//////        rahmenPosition.Geliefert -= ABPositionEntity.OriginalAnzahl;
				//////        abPositionToUpdate = Helpers.BlanketHelper.GetCalculatedPositon(ABPositionEntity.Nr, ABPositionEntity.OriginalAnzahl ?? 0m, false, rahmenPosition.Einzelpreis ?? 0m, null);
				//////        rahmenPositionsToUpdate.Add(rahmenPosition);
				//////    }
				//////    else
				//////    { // - Old AB Pos does not have RA Link
				//////      // - DO NOTHING
				//////    }
				//////}
				//////if (rahmenPositionsToUpdate != null && rahmenPositionsToUpdate.Count > 0)
				//////{
				//////    Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(rahmenPositionsToUpdate);
				//////    Helpers.SpecialHelper.CalculateRahmenGesamtPries(rahmenPositionsToUpdate.Select(x => x.AngebotNr ?? -1).ToList());
				//////}
				//////Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(abPositionToUpdate);

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			////var ABPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.ABPositionNr);
			////var RahmenPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.RahmenPositonNr ?? -1);

			////if (ABPositionEntity == null)
			////    return ResponseModel<int>.FailureResponse("AB position not found .");
			////if (_data.RahmenPositonNr.HasValue && RahmenPositionEntity == null)
			////    return ResponseModel<int>.FailureResponse("Rahmen position not found .");

			////if (ABPositionEntity.Geliefert > 0 && _data.RahmenPositonNr != ABPositionEntity.ABPoszuRAPos)
			////    return ResponseModel<int>.FailureResponse("Ordred has delivred quantity, cannot apply Rahmen.");

			return ResponseModel<int>.SuccessResponse();
		}
		public static decimal CalculateRahmenVailableQty(int NrRahmenPos, int NrABPos)
		{

			var rahmenPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrRahmenPos);
			var abPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrABPos);
			var aBPosRahmenList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetAbByRahmenPosition(NrRahmenPos) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var rahmenOriginalQty = rahmenPosEntity.OriginalAnzahl ?? 0;
			var abLinksTakenQty = aBPosRahmenList?.Sum(x => x.Anzahl) ?? 0;
			var abRequestedQty = abPosEntity.Anzahl ?? 0;

			var _available = 0m;
			if(abPosEntity.ABPoszuRAPos.HasValue && abPosEntity.ABPoszuRAPos.Value == NrRahmenPos)
				_available = (rahmenOriginalQty - abLinksTakenQty) + abRequestedQty;
			else
				_available = rahmenOriginalQty - abLinksTakenQty;
			return _available;
		}
	}
}

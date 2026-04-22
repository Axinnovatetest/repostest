using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class InternTransferHandler: IHandle<Identity.Models.UserModel, ResponseModel<InterTransferKompletModel>>
	{
		private Identity.Models.UserModel _user { get; set; }





		public InternTransferHandler(Identity.Models.UserModel user)
		{

			_user = user;

		}
		public InternTransferHandler()
		{

		}

		public ResponseModel<InterTransferKompletModel> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}




				var response = new InterTransferKompletModel();
				//----------------transfer Intern WS IN--------------------------
				var responseWSIN = new LagerBewegungTreeModel();
				List<int> listlagerVonWSIN = new List<int> { 42, 57 };
				List<int> listlagerNachWSIN = new List<int> { 90 };
				var transferWSIN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonWSIN, listlagerNachWSIN);
				if(transferWSIN != null && transferWSIN.Count > 0)
				{
					responseWSIN.details = transferWSIN
							.Select(d => new LagerbewegungDetailsModel(d)).ToList();



				}
				response.interntransferWSIN = responseWSIN;
				//----------------transfer Intern IN WS--------------------------
				var responseINWS = new LagerBewegungTreeModel();
				List<int> listlagerVonINWS = new List<int> { 90 };
				List<int> listlagerNachINWS = new List<int> { 42, 57 };
				var transferINWS = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonINWS, listlagerNachINWS);
				if(transferWSIN != null && transferWSIN.Count > 0)
				{
					responseINWS.details = transferINWS
							.Select(d => new LagerbewegungDetailsModel(d)).ToList();



				}
				response.interntransferINWS = responseINWS;
				//----------------transfer Intern TN IN--------------------------
				var responseTNIN = new LagerBewegungTreeModel();
				List<int> listlagerVonTNIN = new List<int> { 7, 60, 56, 61 };
				List<int> listlagerNachTNIN = new List<int> { 90 };
				var transferTNIN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonTNIN, listlagerNachTNIN);
				if(transferTNIN != null && transferTNIN.Count > 0)
				{
					responseTNIN.details = transferTNIN
							.Select(d => new LagerbewegungDetailsModel(d)).ToList();



				}
				response.interntransferTNIN = responseTNIN;
				//----------------transfer Intern IN WS--------------------------
				var responseINTN = new LagerBewegungTreeModel();
				List<int> listlagerVonINTN = new List<int> { 90 };
				List<int> listlagerNachINTN = new List<int> { 7, 60, 56, 61 };
				var transferINTN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonINTN, listlagerNachINTN);
				if(transferWSIN != null && transferWSIN.Count > 0)
				{
					responseINTN.details = transferINTN
							.Select(d => new LagerbewegungDetailsModel(d)).ToList();



				}
				response.interntransferINTN = responseINTN;






				return ResponseModel<InterTransferKompletModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<InterTransferKompletModel> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<InterTransferKompletModel>.AccessDeniedResponse();
			}

			return ResponseModel<InterTransferKompletModel>.SuccessResponse();
		}
		public class Groupping
		{
			public DateTime? datum { get; set; }
			public int totalLigne { get; set; }
			public decimal gesmtPreis { get; set; }
			public decimal percentPreis { get; set; }
		}
	}
}
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class InternTransfer2Handler: IHandle<Identity.Models.UserModel, ResponseModel<InterTransferResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }

		private InterTransferSearchModel _data { get; set; }



		public InternTransfer2Handler(InterTransferSearchModel data, Identity.Models.UserModel user)
		{

			_user = user;
			_data = data;
		}
		public InternTransfer2Handler()
		{

		}

		public ResponseModel<InterTransferResponseModel> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}




				var transfer = new LagerBewegungTreeModel();
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				int allCount = 0;
				if(this._data.type == 1)
				{
					//----------------transfer Intern WS IN--------------------------
					var responseWSIN = new LagerBewegungTreeModel();
					List<int> listlagerVonWSIN = new List<int> { 42, 57 };
					List<int> listlagerNachWSIN = new List<int> { 90 };
					var transferWSIN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegungPagination(listlagerVonWSIN, listlagerNachWSIN, this._data.artikelnummer, this._data.SortFieldKey, this._data.SortDesc, dataPaging);
					if(transferWSIN != null && transferWSIN.Count > 0)
					{
						responseWSIN.details = transferWSIN
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();
					}
					if(transferWSIN.Count() > 0)
					{
						allCount = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetCountListBewegung(listlagerVonWSIN, listlagerNachWSIN, this._data.artikelnummer);
					}
					transfer = responseWSIN;
				}
				else if(this._data.type == 2)
				{
					//----------------transfer Intern IN WS--------------------------
					var responseINWS = new LagerBewegungTreeModel();
					List<int> listlagerVonINWS = new List<int> { 90 };
					List<int> listlagerNachINWS = new List<int> { 42, 57 };
					var transferINWS = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegungPagination(listlagerVonINWS, listlagerNachINWS, this._data.artikelnummer, this._data.SortFieldKey, this._data.SortDesc, dataPaging);
					if(transferINWS != null && transferINWS.Count > 0)
					{
						responseINWS.details = transferINWS
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();
					}
					if(transferINWS.Count() > 0)
					{
						allCount = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetCountListBewegung(listlagerVonINWS, listlagerNachINWS, this._data.artikelnummer);
					}

					transfer = responseINWS;
				}
				else if(this._data.type == 3)
				{
					//----------------transfer Intern TN IN--------------------------
					var responseTNIN = new LagerBewegungTreeModel();
					List<int> listlagerVonTNIN = new List<int> { 7, 60, 56, 61 };
					List<int> listlagerNachTNIN = new List<int> { 90 };
					var transferTNIN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegungPagination(listlagerVonTNIN, listlagerNachTNIN, this._data.artikelnummer, this._data.SortFieldKey, this._data.SortDesc, dataPaging);
					if(transferTNIN != null && transferTNIN.Count > 0)
					{
						responseTNIN.details = transferTNIN
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();

					}
					if(transferTNIN.Count() > 0)
					{
						allCount = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetCountListBewegung(listlagerVonTNIN, listlagerNachTNIN, this._data.artikelnummer);
					}
					transfer = responseTNIN;
				}
				else if(this._data.type == 4)
				{
					//----------------transfer Intern IN WS--------------------------
					var responseINTN = new LagerBewegungTreeModel();
					List<int> listlagerVonINTN = new List<int> { 90 };
					List<int> listlagerNachINTN = new List<int> { 7, 60, 56, 61 };
					var transferINTN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegungPagination(listlagerVonINTN, listlagerNachINTN, this._data.artikelnummer, this._data.SortFieldKey, this._data.SortDesc, dataPaging);
					if(transferINTN != null && transferINTN.Count > 0)
					{
						responseINTN.details = transferINTN
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();

					}
					if(transferINTN.Count() > 0)
					{
						allCount = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetCountListBewegung(listlagerVonINTN, listlagerNachINTN, this._data.artikelnummer);
					}
					transfer = responseINTN;
				}




				return ResponseModel<Models.Lagebewegung.InterTransferResponseModel>.SuccessResponse(
					new Models.Lagebewegung.InterTransferResponseModel()
					{
						interntransfer = transfer,
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<InterTransferResponseModel> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<InterTransferResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<InterTransferResponseModel>.SuccessResponse();
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
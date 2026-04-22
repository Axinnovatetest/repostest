using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	using Infrastructure.Data.Entities.Tables.MTM;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public partial class Order
	{
		public class UpdateDeliveryAddressHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
		{
			private Identity.Models.UserModel _user { get; set; }
			private Models.Order.OrderAddressModel _data { get; set; }


			public UpdateDeliveryAddressHandler(Identity.Models.UserModel user, Models.Order.OrderAddressModel data)
			{
				this._user = user;
				this._data = data;
			}

			public ResponseModel<int> Handle()
			{
				try
				{
					lock(Locks.OrdersLock)
					{
						var validationResponse = this.Validate();
						if(!validationResponse.Success)
						{
							return validationResponse;
						}

						var angeboteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.Id);
						angeboteEntity.LVorname_NameFirma = this._data.Name;
						angeboteEntity.LName2 = this._data.Name2;
						angeboteEntity.LName3 = this._data.Name3;
						angeboteEntity.LAnsprechpartner = this._data.Contact;
						angeboteEntity.LAbteilung = this._data.Department;
						angeboteEntity.LStraße_Postfach = this._data.StreetPOBox;
						angeboteEntity.LLand_PLZ_Ort = this._data.CountryPostcode;
						angeboteEntity.LBriefanrede = this._data.OrderTitle;
						angeboteEntity.LAnsprechpartner = this._data.Contact;
						angeboteEntity.LAnrede = this._data.Type;
						angeboteEntity.LsAddressNr = this._data.DeliveryAddressId;
						angeboteEntity.UnloadingPoint = this._data.UnloadingPoint;
						angeboteEntity.StorageLocation = this._data.StorageLocation;

						return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateDeliveryAddress(angeboteEntity));

					}
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}

			public ResponseModel<int> Validate()
			{
				if(this._user == null/*
                || this._user.Access.____*/)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}

				var angeboteEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data.Id);
				if(angeboteEntity == null)
				{
					return new ResponseModel<int>()
					{
						Success = false,
						Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Order not found"}
					}
					};
				}

				if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.DeliveryAddressId) == null)
				{
					return new ResponseModel<int>()
					{
						Success = false,
						Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Address not found"}
					}
					};
				}

				//// Validate StorageLocation and UnloadingPoint BEFORE updating the entity
				//// AB
				//var addressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)angeboteEntity.Kunden_Nr);
				//var kundeEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByAddressNr(addressDb.Nr);

				//if((string.IsNullOrEmpty(this._data.StorageLocation)  || string.IsNullOrEmpty(_data.UnloadingPoint)) && 
				//	(kundeEntity.Edi_Aktiv_Desadv == true))

				//{
				//	return new ResponseModel<int>()
				//	{
				//		Success = false,
				//		Errors = new List<ResponseModel<int>.ResponseError>() {
				//		new ResponseModel<int>.ResponseError {Key = "1", Value = "Please enter the unloading point and the local storage location to proceed."}
				//	}
				//	};
				//}
				//

				// Validate StorageLocation and UnloadingPoint BEFORE updating the entity
				// AB
				var addressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)angeboteEntity.Kunden_Nr);
				var kundeEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByAddressNr(addressDb.Nr);

				if((string.IsNullOrEmpty(this._data.StorageLocation) || string.IsNullOrEmpty(_data.UnloadingPoint)) &&
					(kundeEntity.Edi_Aktiv_Desadv == true))

				{
					// Delfor

					var lineitemplan = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(angeboteEntity?.nr_dlf ?? 0);
					var lineitem = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(lineitemplan?.LineItemId ?? 0);
					var delforheader = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineitem?.HeaderId ?? 0);
					var delforAddress = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();
					//
					if(string.IsNullOrEmpty(delforheader is not null ? delforheader.ConsigneeUnloadingPoint : delforAddress?.UnloadingPoint) ||
							string.IsNullOrEmpty(delforheader is not null ? delforheader.ConsigneeStorageLocation : delforAddress?.StorageLocation))
					{
						return new ResponseModel<int>()
						{
							Success = false,
							Errors = new List<ResponseModel<int>.ResponseError>() {
					new ResponseModel<int>.ResponseError {Key = "1", Value = "To proceed, please enter the unloading point and the local storage location in the 'Delivery Address' section of the order confirmation."}
							}
						};
					}
				}


				// - 2023-05-10 - Heidenreich - allow only ONCE to toggle Book for RG
				if(angeboteEntity?.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE.Trim().ToLower())
				{
					if(angeboteEntity?.Datum < DateTime.Today || angeboteEntity.Gebucht == true)
					{
						return ResponseModel<int>.FailureResponse("Invoice edit is not allowed");
					}
				}

				return ResponseModel<int>.SuccessResponse();
			}
		}
	}
}

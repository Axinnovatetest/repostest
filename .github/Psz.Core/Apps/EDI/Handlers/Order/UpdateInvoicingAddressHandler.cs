using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public partial class Order
	{
		public class UpdateInvoicingAddressHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
		{
			private Identity.Models.UserModel _user { get; set; }
			private Models.Order.OrderAddressModel _data { get; set; }


			public UpdateInvoicingAddressHandler(Identity.Models.UserModel user, Models.Order.OrderAddressModel data)
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
						angeboteEntity.Vorname_NameFirma = this._data.Name;
						angeboteEntity.Name2 = this._data.Name2;
						angeboteEntity.Name3 = this._data.Name3;
						angeboteEntity.Ansprechpartner = this._data.Contact;
						angeboteEntity.Abteilung = this._data.Department;
						angeboteEntity.Straße_Postfach = this._data.StreetPOBox;
						angeboteEntity.Land_PLZ_Ort = this._data.CountryPostcode;
						angeboteEntity.Briefanrede = this._data.OrderTitle;

						return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateInvoicingAddress(angeboteEntity));
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

				return ResponseModel<int>.SuccessResponse();
			}
		}
	}
}

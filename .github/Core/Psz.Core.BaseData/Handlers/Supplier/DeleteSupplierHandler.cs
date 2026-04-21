using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class DeleteSupplierHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<string>>
	{
		private int _supplierId { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public DeleteSupplierHandler(int supplierId, Identity.Models.UserModel user)
		{
			this._supplierId = supplierId;
			this._user = user;
		}

		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				string response = string.Empty;

				// - Always archive
				//var BestellugenEntity = Infrastructure.Data.Access.Tables.BSD.BestellungenAccess.GetConutBySupplier(_supplierId);
				//if (BestellugenEntity > 0)
				{
					var lieferentenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(_supplierId);
					if(lieferentenExtensionEntity != null)
					{
						Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.UpdateArchived(_supplierId, true, DateTime.Now, this._user.Id);
					}
					else
					{
						Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.LieferantenExtensionEntity
						{
							Nr = _supplierId,
							IsArchived = true,
							ArchiveUserId = this._user.Id,
							ArchiveTime = DateTime.Now,
						});
					}
					response = "The Supplier has Active Orders, archive successfull.";
				}
				//else
				//{
				//    response = "The Supplier has no Active Orders, delete successfull.";
				//    var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_supplierId);
				//    if (supplierEntity == null)
				//    {
				//        return ResponseModel<string>.SuccessResponse();
				//    }

				//    Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Delete(supplierEntity.Nr);
				//    Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.DeleteByLieferantenNr(supplierEntity.Nr);

				//    var addressEntity = supplierEntity.Nummer.HasValue
				//        ? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierEntity.Nummer.Value)
				//        : null;
				//    if (addressEntity == null)
				//    {
				//        return ResponseModel<string>.SuccessResponse(response);
				//    }
				//    else
				//    {
				//        if (addressEntity.Kundennummer.HasValue)
				//        {
				//            Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateLieferantenNummer(new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
				//            {
				//                Nr = addressEntity.Nr,
				//                Lieferantennummer = null,
				//            });
				//        }
				//        else
				//            Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Delete(addressEntity.Nr);
				//    }
				//}

				return ResponseModel<string>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}
			var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_supplierId);
			if(supplierEntity == null)
			{
				return new ResponseModel<string>()
				{
					Errors = new List<ResponseModel<string>.ResponseError>{
							new ResponseModel<string>.ResponseError() { Key = "", Value = "Supplier not found" }
						}
				};
			}
			return ResponseModel<string>.SuccessResponse();
		}
	}
}

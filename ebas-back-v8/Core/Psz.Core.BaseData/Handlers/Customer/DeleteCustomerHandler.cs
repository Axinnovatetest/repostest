using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class DeleteCustomerHandler: IHandle<int, ResponseModel<string>>
	{
		private int _customerId { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public DeleteCustomerHandler(int customerId, Identity.Models.UserModel user)
		{
			this._customerId = customerId;
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
				var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_customerId);
				var addressEntity = customerEntity.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerEntity.Nummer.Value)
						: null;
				var log = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity();

				// Always archive
				//var angebotCount = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetConutByCustomer(_customerId);
				//if (angebotCount > 0)
				{
					var kundenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(_customerId);
					if(kundenExtensionEntity != null)
					{
						Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.UpdateArchived(_customerId, true, DateTime.Now, this._user.Id);
					}
					else
					{
						Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.KundenExtensionEntity
						{
							Nr = _customerId,
							IsArchived = true,
							ArchiveUserId = this._user.Id,
							ArchiveTime = DateTime.Now,
							UpdateTime = DateTime.Now,
							UpdateUserId = this._user.Id,
						});
					}
					response = "The Customer has Active deals, archive successfull.";
					log = ObjectLogHelper.getLog(this._user, _customerId, "Customer", addressEntity?.Name1 ?? "", addressEntity?.Name1 ?? "", Enums.ObjectLogEnums.Objects.Customer.GetDescription(), Enums.ObjectLogEnums.LogType.Archive);
				}
				//else
				//{
				//    response = "The Customer has no Active deals, delete successfull.";
				//    log = ObjectLogHelper.getLog(this._user, _customerId, "Customer", addressEntity?.Name1 ?? "", addressEntity?.Name1 ?? "", Enums.ObjectLogEnums.Objects.Customer.GetDescription(), Enums.ObjectLogEnums.LogType.Delete);
				//    Infrastructure.Data.Access.Tables.PRS.KundenAccess.Delete(customerEntity.Nr);
				//    Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.ByKundenNr(customerEntity.Nr);
				//    if (addressEntity == null)
				//    {
				//        return ResponseModel<string>.SuccessResponse(response);
				//    }
				//    else
				//    {
				//        if (addressEntity.Lieferantennummer.HasValue)
				//        {
				//            Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateKundenNummer(new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity
				//            {
				//                Nr = addressEntity.Nr,
				//                Kundennummer = null,
				//            });
				//        }
				//        else
				//            Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Delete(addressEntity.Nr);
				//    }
				//}
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
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
			var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_customerId);
			if(customerEntity == null)
			{
				return new ResponseModel<string>()
				{
					Errors = new List<ResponseModel<string>.ResponseError>{
							new ResponseModel<string>.ResponseError() { Key = "", Value = "Customer not found" }
						}
				};
			}
			return ResponseModel<string>.SuccessResponse();
		}
	}
}

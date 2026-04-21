using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetBlanketHeaderHandler: IHandle<Identity.Models.UserModel, ResponseModel<BlanketModel>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBlanketHeaderHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<BlanketModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new BlanketModel();
				var BlanketAbgebotEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
				var BlanketFileEntity = Infrastructure.Data.Access.Tables.CTS.CTSBlanketFilesAccess.GetByAngebotNr(this._data);
				var blanketExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data);
				var ExtensionAllEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Get();
				// - add extension if not exist
				if(blanketExtensionEntity == null)
				{
					var abCount = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data)?.Select(x => x.Nr)?.ToList())
						?.Count();
					// - 2022-11-30 - pulling from Angebote => RahmenType == Sale; Purchase Rahmen are stored in Bestellung
					var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(318);
					var supplierAddressEntitiy = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierEntity?.Nummer ?? -1);
					var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(BlanketAbgebotEntity.Kunden_Nr ?? -1);
					Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Insert(
						new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity
						{
							AngeboteNr = this._data,
							Anhage = null,
							Archived = null,
							ArchiveTime = null,
							ArchiveUserId = null,
							Auftraggeber = supplierAddressEntitiy?.Name1,// supplier,
							CreateTime = DateTime.Now,
							CreateUserId = this._user.Id,
							Id = this._data,
							LastEditTime = null,
							LastEditUserId = null,
							CustomerId = customerEntity?.Nr ?? -1,//blanketExtensionEntity.CustomerId
							CustomerName = BlanketAbgebotEntity.Vorname_NameFirma,
							StatusId = abCount <= 0 ? (int)Enums.BlanketEnums.RAStatus.Draft : (int)Enums.BlanketEnums.RAStatus.Validated,
							StatusName = abCount <= 0 ? Enums.BlanketEnums.RAStatus.Draft.GetDescription() : Enums.BlanketEnums.RAStatus.Validated.GetDescription(),
							SupplierId = supplierEntity?.Nr,
							SupplierName = supplierAddressEntitiy?.Name1,
							BlanketTypeId = (int)Enums.BlanketEnums.Types.sale,
							BlanketTypeName = Enums.BlanketEnums.Types.sale.GetDescription(),
							Warenemfanger = BlanketAbgebotEntity.Vorname_NameFirma, // customer
						});

				}
				var extensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data);
				var creationUserEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(extensionEntity?.CreateUserId ?? -1);
				response = new BlanketModel(BlanketAbgebotEntity, extensionEntity, creationUserEntity);
				if(BlanketFileEntity != null && BlanketFileEntity.Count > 0)
					response.FileIds = BlanketFileEntity.Select(a => new BlanketAttachementsModel { AttachementID = a.FileId ?? -1, AttachementName = a.FileName }).ToList();

				Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(this._data);

				return ResponseModel<BlanketModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<BlanketModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<BlanketModel>.AccessDeniedResponse();
			}

			var blanketEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
			if(blanketEntity == null)
				return ResponseModel<BlanketModel>.FailureResponse("Rahmen not found");

			// -
			return ResponseModel<BlanketModel>.SuccessResponse();
		}
	}
}

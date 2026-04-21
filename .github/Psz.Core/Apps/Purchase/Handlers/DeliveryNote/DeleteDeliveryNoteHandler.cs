using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class DeleteDeliveryNoteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteDeliveryNoteHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
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

				var LSEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
				var LSItemsEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data);
				var LSArticlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(LSItemsEntity?.Select(a => a.ArtikelNr ?? -1).ToList() ?? new List<int> { -1 });
				if(LSItemsEntity != null && LSItemsEntity.Count > 0)
				{
					foreach(var item in LSItemsEntity)
					{
						var del = new Psz.Core.Apps.Purchase.Handlers.DeliveryNote.DeleteDeliveryNoteItemHandler(this._user, item.Nr, false).Handle();
						if(del.Success == false)
						{
							return new ResponseModel<int>()
							{
								Success = false,
								Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Error deleting position {item.Position}"}
					}
							};
						}
					}
				}
				var response = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Delete(this._data);
				GenerateDatFile(LSEntity, LSItemsEntity, LSArticlesEntities);
				//logging-1
				var _log = new LogHelper(LSEntity.Nr, (int)LSEntity.Angebot_Nr, int.TryParse(LSEntity.Projekt_Nr, out var v) ? v : 0, LSEntity.Typ, LogHelper.LogType.DELETIONOBJECT, "CTS", _user)
					.LogCTS(null, null, null, 0);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
				//loggin-2
				var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(LSEntity.Kunden_Nr ?? -1);
				Infrastructure.Data.Access.Tables.CTS.PSZ_Protokollierung_Angebote2Access.Insert(new Infrastructure.Data.Entities.Tables.CTS.PSZ_Protokollierung_Angebote2Entity
				{
					Angebot_Nr = LSEntity.Angebot_Nr.ToString(),
					bestellung_Typ = LSEntity.Typ,
					Bezug = LSEntity.Bezug,
					Gelöscht_am = DateTime.Now,
					Gelöscht_durch = _user.Name,
					ID = -1,
					Kunden_Nr = LSEntity.Kunden_Nr.ToString(),
					Name = adressDb?.Name1,
					Projekt_Nr = int.TryParse(LSEntity.Projekt_Nr, out var v1) ? v1 : 0,
				});
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var angebotEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
			if(angebotEntity == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>
					{
					   new ResponseModel<int>.ResponseError{Key ="", Value = $"Delivery note not found"}
					}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}

		public void GenerateDatFile(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
			List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		{
			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(angeboteEntity.Kunden_Nr.Value);
			var WmsAngebotNr = angeboteEntity.Angebot_Nr;
			var path = $@"{Program.CTS.DeliveryNoteFilesPath}\WA{DateTime.Now.ToString("yyyyMMddhhmmss")}.dat";
			var content = $"AG;1;1;{WmsAngebotNr};{angeboteneArtikelEntities?.Count};50;{angeboteEntity.Datum.Value.ToString("yyyyMMdd")};{angeboteEntity.Versanddatum_Auswahl?.ToString("yyyyMMdd")};1;0;0;1;1;{addressenEntity.Kundennummer.Value};{angeboteEntity.Vorname_NameFirma.Substring(0, Math.Min(angeboteEntity.Vorname_NameFirma.Length, 37))}";
			foreach(var angeboteneArtikelEntity in angeboteneArtikelEntities)
			{
				var artikelEntity = artikelEntities.Where(x => x.ArtikelNr == angeboteneArtikelEntity.ArtikelNr)?.ToList().FirstOrDefault();
				content += $"\nAG;2;1;{WmsAngebotNr};{angeboteneArtikelEntity.Position};{artikelEntity.ArtikelNr};0";
			}
			File.WriteAllText(path, content);
		}
	}
}

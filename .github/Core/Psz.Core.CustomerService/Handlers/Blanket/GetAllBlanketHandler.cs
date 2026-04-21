using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetAllBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BlanketHeaderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }


		public GetAllBlanketHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}
		public ResponseModel<List<BlanketHeaderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<BlanketHeaderModel> response = new List<BlanketHeaderModel>();
				List<int> _ExtensionNrs = new List<int>();
				List<int> _BlanketNrs = new List<int>();

				var blanketEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTyp(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT);
				var ExtensionAllEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Get();

				//get the non exsisting in extension header and insert them
				_ExtensionNrs = ExtensionAllEntities?.Select(x => x.AngeboteNr).ToList();
				_BlanketNrs = blanketEntities?.Select(x => x.Nr).ToList();

				var NotExistsNrs = _BlanketNrs.Except(_ExtensionNrs).ToList();
				var notExsistantEntities = blanketEntities?.Where(x => NotExistsNrs.Exists(y => y == x.Nr))?.ToList(); //Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(NotExistsNrs);

				Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Insert(
					notExsistantEntities?.Select(a => new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity
					{
						AngeboteNr = (int)a.Angebot_Nr,
						Anhage = null,
						Archived = null,
						ArchiveTime = null,
						ArchiveUserId = null,
						Auftraggeber = null,
						CreateTime = DateTime.Now,
						CreateUserId = this._user.Id,
						Id = a.Nr,
						LastEditTime = null,
						LastEditUserId = null,
						Warenemfanger = null
					}).ToList()
					);


				if(ExtensionAllEntities != null && ExtensionAllEntities.Count > 0)
				{
					response = ExtensionAllEntities.Select(a => new BlanketHeaderModel(a)).ToList();
					foreach(var item in response)
					{
						//var blanketPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngebotNr(new List<int> { (int)item.AngebotNr });
						var blanketPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr((int)item.AngeboteNr);
						if(blanketPositions != null && blanketPositions.Count > 0)
						{
							var _sum = blanketPositions.Sum(x => x.Gesamtpreis);
							item.Gesamtpreis = _sum;
						}
					}
				}

				return ResponseModel<List<BlanketHeaderModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<BlanketHeaderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<BlanketHeaderModel>>.AccessDeniedResponse();
			}



			// -
			return ResponseModel<List<BlanketHeaderModel>>.SuccessResponse();
		}
	}
}

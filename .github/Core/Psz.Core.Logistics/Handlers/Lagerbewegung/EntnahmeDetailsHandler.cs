using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class EntnahmeDetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Lagebewegung.EntnahmeDetailsResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Lagebewegung.EntnahmeDetailsSearchModel _data { get; set; }
		private int _lager2 { get; set; }
		public EntnahmeDetailsHandler(Models.Lagebewegung.EntnahmeDetailsSearchModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}
		public ResponseModel<Models.Lagebewegung.EntnahmeDetailsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var ModelLGT = Psz.Core.Logistics.Module.LGT.LGTList.Where(x => x.Lager_Id == this._data.lager).ToList();
				this._lager2 = 0;
				if(ModelLGT != null && ModelLGT.Count() > 0)
				{
					this._lager2 = ModelLGT[0].Lager_P_Id;
				}
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				int allCount = 0;
				var listeEntnahme = new List<Models.Lagebewegung.EntnahmeWertTreeDetailsModel>();

				if(this._data.ek == true)
				{
					var listeEntnahmeEntity = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetPaginationGetEntnahmeWertWithtEK(this._data.datum, this._data.lager, this._lager2, this._data.type, this._data.artikelnummer, this._data.SortFieldKey, this._data.SortDesc, dataPaging);

					foreach(var entnahmeEntity in listeEntnahmeEntity)
					{
						entnahmeEntity.kosten = Math.Round(entnahmeEntity.kosten, 2);
						listeEntnahme.Add(new Models.Lagebewegung.EntnahmeWertTreeDetailsModel(entnahmeEntity));
					}
					if(listeEntnahmeEntity.Count() > 0)
					{
						allCount = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetCountEntnahmeWertWithtEK(this._data.datum, this._data.lager, this._lager2, this._data.type, this._data.artikelnummer);
					}
				}
				else if(this._data.ek == false)
				{
					var listeEntnahmeEntity = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetPaginationGetEntnahmeWertWithtoutEK(this._data.datum, this._data.lager, this._lager2, this._data.type, this._data.artikelnummer, this._data.SortFieldKey, this._data.SortDesc, dataPaging);

					foreach(var entnahmeEntity in listeEntnahmeEntity)
					{
						entnahmeEntity.kosten = Math.Round(entnahmeEntity.kosten, 2);
						listeEntnahme.Add(new Models.Lagebewegung.EntnahmeWertTreeDetailsModel(entnahmeEntity));
					}
					if(listeEntnahmeEntity.Count() > 0)
					{
						allCount = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetCountEntnahmeWertWithtoutEK(this._data.datum, this._data.lager, this._lager2, this._data.type, this._data.artikelnummer);
					}
				}



				return ResponseModel<Models.Lagebewegung.EntnahmeDetailsResponseModel>.SuccessResponse(
					new Models.Lagebewegung.EntnahmeDetailsResponseModel()
					{
						listEntnahme = listeEntnahme,
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});
				;
				;
				;// ;

			} catch(Exception e)
			{
				throw;
			}
		}
		public ResponseModel<Models.Lagebewegung.EntnahmeDetailsResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Lagebewegung.EntnahmeDetailsResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Lagebewegung.EntnahmeDetailsResponseModel>.SuccessResponse();
		}
	}
}

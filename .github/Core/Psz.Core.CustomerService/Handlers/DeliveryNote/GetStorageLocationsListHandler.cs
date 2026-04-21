using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.DeliveryNote
{
	public class GetStorageLocationsListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>>>
	{
		private string _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }

		public GetStorageLocationsListHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var _showed = Module.Hauplager;//  2024-01-15 - new List<int> { 3, 4, 8, 13, 15, 24, 41, 58, 825, 928 };
				var response = new List<Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>();

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data ?? "");
				var lagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetByArticleNrs(new List<int> { articleEntity?.ArtikelNr ?? -1 }, false);
				var lagerExtEntites = Infrastructure.Data.Access.Tables.BSD.LagerExtensionAccess.GetByArticleAndLagerIds(articleEntity?.ArtikelNr ?? -1, _showed)
					?? new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
				var lagerortEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get();

				//if (lagerExtEntites != null && lagerExtEntites.Count > 0)
				//{
				//    foreach (var item in lagerExtEntites)
				//    {
				//        var lagarName = lagerortEntities.Where(x => x.LagerortId == item.Lagerort_id).FirstOrDefault()?.Lagerort;
				//        response.Add(new Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation
				//        {
				//            Key = item.Lagerort_id,
				//            Value = $"{lagarName} || {item.Bestand?.ToString("0.###")}",
				//            KundenIndex = item.Index_Kunde
				//        });
				//    }
				//}

				var _lastIndex = OrderProcessing.CreateOrderItemHandler.GetLastIndex(articleEntity);
				lagerEntities = lagerEntities?.Where(x => _showed.Exists(y => y == x?.Lagerort_id))?.ToList();
				if(lagerEntities != null && lagerEntities.Count > 0)
				{
					foreach(var item in lagerEntities)
					{
						var lagarName = lagerortEntities.Where(x => x.LagerortId == item.Lagerort_id).FirstOrDefault()?.Lagerort;
						response.Add(new Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation
						{
							Key = item.Lagerort_id ?? -1,
							Value = $"{lagarName} || {item.Bestand?.ToString("0.###")}",
							KundenIndex = _lastIndex.KundenIndex
						});
					}
				}

				return ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}

		public ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.StorageLocation>>.SuccessResponse();
		}
	}
}

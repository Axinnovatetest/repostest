using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetKundenIndexesByArticleHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>>>
	{
		private int _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }

		public GetKundenIndexesByArticleHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var snapshotKIEntities = new List<Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>();

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				if(articleEntity != null)
				{
					snapshotKIEntities.AddRange(
						Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetKundenIndexSnapshotTimeByArticle(this._data)
						?.Select(x => new Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex(x)));

					// - 2022-06-14 - only add Current Article Index, if no BOM
					if(snapshotKIEntities.Count == 0) // (snapshotKIEntities.FindIndex(x=> x.Value == articleEntity.Index_Kunde)<= 0)
					{
						snapshotKIEntities.Add(new Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex
						{
							Key = articleEntity.Index_Kunde_Datum,
							Value = articleEntity.Index_Kunde,
							SnapshotTime = DateTime.MaxValue
						});
					}
				}
				// -
				snapshotKIEntities = snapshotKIEntities?.DistinctBy(x => x.Value)
						?.OrderByDescending(x => x.Key)?.ToList();

				return ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>>.SuccessResponse(snapshotKIEntities);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.OrderProcessing.StoragesAndIndexesModel.KundeIndex>>.SuccessResponse();
		}
	}
}

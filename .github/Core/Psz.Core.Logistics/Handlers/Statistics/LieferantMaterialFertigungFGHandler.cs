using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class LieferantMaterialFertigungFGHandler: IHandle<Identity.Models.UserModel, ResponseModel<LieferantMaterialFertigungFGModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public LieferantMaterialFertigungFGHandler(Identity.Models.UserModel user)
		{
			this._user = user;

		}
		public ResponseModel<LieferantMaterialFertigungFGModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				//-------LieferantMaterialModel
				var LieferantMaterialModelList = new List<LieferantMaterialModel>();
				var PackingLieferantMaterialModelEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetLieferantMaterialEntity();
				if(PackingLieferantMaterialModelEntity != null && PackingLieferantMaterialModelEntity.Count > 0)
					LieferantMaterialModelList = PackingLieferantMaterialModelEntity.Select(k => new LieferantMaterialModel(k)).ToList();
				//-------FertigungFGModel
				var FertigungFGModelList = new List<FertigungFGModel>();
				var PackingListFertigungFGModelEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetFertigungFGEntity();
				if(PackingListFertigungFGModelEntity != null && PackingListFertigungFGModelEntity.Count > 0)
					FertigungFGModelList = PackingListFertigungFGModelEntity.Select(k => new FertigungFGModel(k)).ToList();

				//-------LieferantMaterialFertigungFGModel

				var LieferantMaterialFertigungFGModel = new LieferantMaterialFertigungFGModel { LieferantMaterialModel = LieferantMaterialModelList, FertigungFGModel = FertigungFGModelList };



				return ResponseModel<LieferantMaterialFertigungFGModel>.SuccessResponse(LieferantMaterialFertigungFGModel);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<LieferantMaterialFertigungFGModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<LieferantMaterialFertigungFGModel>.AccessDeniedResponse();
			}

			return ResponseModel<LieferantMaterialFertigungFGModel>.SuccessResponse();
		}
	}
}

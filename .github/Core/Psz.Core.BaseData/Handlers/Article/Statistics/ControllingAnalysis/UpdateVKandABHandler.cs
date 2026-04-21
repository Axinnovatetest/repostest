using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateVKandABHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>>>
	{
		private UserModel _user { get; set; }
		private List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel> _data { get; set; }
		public UpdateVKandABHandler(UserModel user, List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel> data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.UpdateVKandAB(this._user.Username,
					(this._data ?? new List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>()).Select(x => x.ToEntity()).ToList())
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationStffelPreis>();

				// -- Article level Logging
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data?.Select(x => x.Artikel_Nr)?.ToList());
				if(articleEntities != null && articleEntities.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, 0,
						$"Article [UpdateVKandAB]",
						$"{string.Join(" | ", articleEntities?.Select(x => $"{x.ArtikelNr} : [{x.ArtikelNummer}]")?.ToList())}",
						$"",
						Enums.ObjectLogEnums.Objects.Article.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
				}

				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>>.SuccessResponse(
					results.Select(x => new Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel(x)).ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>>.SuccessResponse();
		}
	}
}

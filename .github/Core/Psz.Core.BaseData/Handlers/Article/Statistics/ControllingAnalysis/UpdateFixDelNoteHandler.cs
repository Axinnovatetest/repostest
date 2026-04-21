using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateFixDelNoteHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.FixDelNoteRequestModel _data { get; set; }
		public UpdateFixDelNoteHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.FixDelNoteRequestModel data)
		{
			this._user = user;
			this._data = data;
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

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumberPreffixWDelFixed(this._data.ArticleNumber);
				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.UpdateFixDelNote(this._data.ArticleNumber, this._data.HM, this._data.NewDelNote);
				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					articleEntity.Select(x =>
					ObjectLogHelper.getLog(this._user, x.ArtikelNr,
					$"Article DEL Note",
					$"DEL: {x.DEL} | DELFixiert: {x.DELFixiert} | HM: {x.Hubmastleitungen}",
					$"DEL: {this._data.NewDelNote} | DELFixiert: {x.DELFixiert} | HM: {x.Hubmastleitungen}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Edit))?.ToList());
				return ResponseModel<int>.SuccessResponse(results);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumberPreffixWDelFixed(this._data.ArticleNumber) == null)
				return ResponseModel<int>.FailureResponse("Articles not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}

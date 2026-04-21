using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class CheckForOriginalLandDiffrenceHandler: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<bool, string>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.Logistics.ArticleLogisticsModel _data { get; set; }
		public CheckForOriginalLandDiffrenceHandler(Identity.Models.UserModel user, Models.Article.Logistics.ArticleLogisticsModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<KeyValuePair<bool, string>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var check = false;
				var text = "";
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleID);
				if(articleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
				{
					if(articleEntity.ProductionCountryCode?.ToLower()?.Trim() == _data.UrsprungslandName?.ToLower()?.Trim())
						check = true;
					else
					{
						text = $"[Origin Country (Ursprungsland)]: selected value [{_data.UrsprungslandName?.Trim()}] is different from production country [{articleEntity.ProductionCountryCode?.Trim()}], are you sure you want to proceed ?";
					}
				}

				return ResponseModel<KeyValuePair<bool, string>>.SuccessResponse(new KeyValuePair<bool, string>(check, text));
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<KeyValuePair<bool, string>> Validate()
		{
			if(this._data == null)
				return ResponseModel<KeyValuePair<bool, string>>.UnexpectedErrorResponse();
			return ResponseModel<KeyValuePair<bool, string>>.SuccessResponse();
		}
	}
}
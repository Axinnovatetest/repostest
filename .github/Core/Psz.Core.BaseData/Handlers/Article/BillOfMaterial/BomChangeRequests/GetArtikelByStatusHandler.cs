using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;


namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public partial class GetArtikelByStatusHandler: IHandle<string, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetArtikelByStatusHandler(Identity.Models.UserModel user, string data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				this._data = this._data.Trim();

				var responseBody = new List<KeyValuePair<string, string>> { };
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.SearchArticleByStatus(this._data);
				if(articleEntities != null && articleEntities.Count > 0)
				{
					foreach(var articleEntity in articleEntities)
					{
						responseBody.Add(new KeyValuePair<string, string>(articleEntity.ArtikelNr.ToString(), articleEntity.ArtikelNummer.ToString()));
					}
				}

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			if(string.IsNullOrEmpty(this._data) || string.IsNullOrWhiteSpace(this._data) || this._data.Trim().Length < 1)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.FailureResponse(
					new List<KeyValuePair<string, string>> { });
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}

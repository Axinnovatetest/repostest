using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Psz.Core.Identity.Models;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetArticlesReceivedHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.PlantBookings.ArticleReceivedResponseModel>>>
	{
		private ArticleReceivedRequestModel _data;
		private Identity.Models.UserModel _user;
		public GetArticlesReceivedHandler(Identity.Models.UserModel user, ArticleReceivedRequestModel data)
		{
			_data = data;
			_user = user;
		}
		public ResponseModel<List<ArticleReceivedResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 
				var response = Infrastructure.Data.Access.Joins.Logistics.WeVOHIncomingAccess.GetArticlesReceived(_data.Article, _data.LagerId);
				return ResponseModel<List<ArticleReceivedResponseModel>>.SuccessResponse(response?.Select(x => new ArticleReceivedResponseModel(x)).ToList());

			} catch(Exception)
			{
				throw;
			}
		}

		public ResponseModel<List<ArticleReceivedResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<ArticleReceivedResponseModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<ArticleReceivedResponseModel>>.SuccessResponse();
		}
	}
}

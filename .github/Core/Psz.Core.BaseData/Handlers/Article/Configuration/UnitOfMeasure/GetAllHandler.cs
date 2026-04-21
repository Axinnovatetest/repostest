using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.UnitOfMeasure
{
	using Psz.Core.BaseData.Models.Article.Configuration;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<UnitOfMeasureResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}

		public ResponseModel<List<UnitOfMeasureResponseModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<UnitOfMeasureResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.Get()?.Select(x => new UnitOfMeasureResponseModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<UnitOfMeasureResponseModel>> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<UnitOfMeasureResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<UnitOfMeasureResponseModel>>.SuccessResponse();
		}
	}
}

using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.Frames
{
	public class GetCustomerFramesForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Frames.FramesModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomerFramesForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Frames.FramesModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Frames.FramesModel>();
				var frameEntities = Infrastructure.Data.Access.Tables.BSD.Fibu_kunden_rahmenAccess.Get();

				if(frameEntities != null && frameEntities.Count > 0)
				{
					foreach(var kundenEntity in frameEntities)
					{
						responseBody.Add(new Models.Frames.FramesModel(kundenEntity));
					}
				}

				return ResponseModel<List<Models.Frames.FramesModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Frames.FramesModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Frames.FramesModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Frames.FramesModel>>.SuccessResponse();
		}
	}
}

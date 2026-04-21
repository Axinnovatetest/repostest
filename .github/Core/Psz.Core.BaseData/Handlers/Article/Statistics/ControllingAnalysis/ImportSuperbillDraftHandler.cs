using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.IO;

	public class ImportSuperbillDraftHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public ImportSuperbillDraftHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// --
				return ResponseModel<byte[]>.SuccessResponse(File.ReadAllBytes(Module.AppSettings.ArticlesStatisticsSuperbillFertigung));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}

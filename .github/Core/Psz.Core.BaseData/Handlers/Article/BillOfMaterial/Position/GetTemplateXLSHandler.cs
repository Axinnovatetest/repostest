using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.IO;

	public class GetTemplateXLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetTemplateXLSHandler(Identity.Models.UserModel user)
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
				return ResponseModel<byte[]>.SuccessResponse(File.ReadAllBytes(Module.AppSettings.BomImportFileTemplatePath));
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

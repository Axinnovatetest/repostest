using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetTemplateFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel>>
	{
		public GetTemplateFileHandler()
		{
		}

		public ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel>.SuccessResponse(new Models.Article.BillOfMaterial.ImportFileTemplateModel
				{
					CreationTime = DateTime.Now,
					FileData = System.IO.File.ReadAllBytes(Module.AppSettings.BomImportFileTemplatePath),
					FileName = System.IO.Path.GetFileName(Module.AppSettings.BomImportFileTemplatePath),
					FileExtension = System.IO.Path.GetExtension(Module.AppSettings.BomImportFileTemplatePath)
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel> Validate()
		{
			return ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel>.SuccessResponse();
		}
	}
}

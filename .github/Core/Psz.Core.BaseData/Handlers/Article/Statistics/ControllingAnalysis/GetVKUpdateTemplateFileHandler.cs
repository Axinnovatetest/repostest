using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetVKUpdateTemplateFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel>>
	{
		public GetVKUpdateTemplateFileHandler()
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
					FileData = System.IO.File.ReadAllBytes(Module.AppSettings.ArticlesStatisticsVKUpdate),
					FileName = System.IO.Path.GetFileName(Module.AppSettings.ArticlesStatisticsVKUpdate),
					FileExtension = System.IO.Path.GetExtension(Module.AppSettings.ArticlesStatisticsVKUpdate)
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

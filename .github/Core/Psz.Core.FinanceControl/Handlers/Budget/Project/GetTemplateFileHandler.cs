using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetTemplateFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Project.FileModel>>
	{

		public GetTemplateFileHandler()
		{
		}

		public ResponseModel<Models.Budget.Project.FileModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 

				return ResponseModel<Models.Budget.Project.FileModel>.SuccessResponse(new Models.Budget.Project.FileModel
				{
					CreationTime = DateTime.Now,
					FileData = System.IO.File.ReadAllBytes(Infrastructure.Services.Reporting.ReportGenerator.ProjectTemplatePath),
					FileName = System.IO.Path.GetFileName(Infrastructure.Services.Reporting.ReportGenerator.ProjectTemplatePath)
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Project.FileModel> Validate()
		{
			return ResponseModel<Models.Budget.Project.FileModel>.SuccessResponse();
		}
	}
}

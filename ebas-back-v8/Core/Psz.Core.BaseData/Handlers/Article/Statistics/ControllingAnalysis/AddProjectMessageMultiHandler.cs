using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	using System.Threading.Tasks;

	public class AddProjectMessageMultiHandler: IHandleAsync<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private List<Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemRequestModel> _data { get; set; }
		public AddProjectMessageMultiHandler(UserModel user, List<Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemRequestModel> data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<byte[]>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				if(this._data == null || this._data.Count <= 0)
					return ResponseModel<byte[]>.SuccessResponse();

				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					this._data.Select(x =>
						ObjectLogHelper.getLog(this._user, -1, "Projektdaten_Details", "",
						$"{x.Artikelnummer}",
						Enums.ObjectLogEnums.Objects.ProjectData.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add)).ToList());
				var ids = new List<int> { };
				for(int i = 0; i < this._data.Count; i++)
				{
					this._data[i].ID = Infrastructure.Data.Access.Tables.BSD.PSZ_Projektdaten_DetailsAccess.Insert(this._data[i].ToEntity());
				}
				var response = await GetProjectMessagePDFDataHandler.GetData(_data?.Where(x => x.isPrint)?.Select(x => x.ToModel())?.ToList(), null);
				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public async Task<ResponseModel<byte[]>> ValidateAsync()
		{
			if(this._user == null)
			{
				return await ResponseModel<byte[]>.AccessDeniedResponseAsync();
			}

			return await ResponseModel<byte[]>.SuccessResponseAsync();
		}
	}
}

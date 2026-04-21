using Infrastructure.Data.Access.Tables.Support.Feedback;
using Psz.Core.Apps.Support.Interfaces;
using Psz.Core.Apps.Support.Models.FeedbackLogs;
using Psz.Core.Apps.Support.Models.Logs;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Infrastructure.Services.Files.Parsing.PerfLogParsingUtilities;

namespace Psz.Core.Apps.Support.Handlers.Logs
{
	public class LogService: ILogService
	{
		public ResponseModel<FeedbackLogResponseModel> GetLogs(UserModel user, FeedbackLogRequestModel _data)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<FeedbackLogResponseModel>.AccessDeniedResponse();
				}
				#endregion

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = _data.PageSize > 0 ? (_data.RequestedPage * _data.PageSize) : 0,
					RequestRows = _data.PageSize
				};

				var sortFieldName = "";

				if(!string.IsNullOrWhiteSpace(_data.SortField))
				{
					switch(_data.SortField.ToLower())
					{
						default:
						case "logcapturedate":
							sortFieldName = "[LogCaptureDate]";
							break;
						case "endpointname":
							sortFieldName = "[EndpointName]";
							break;
						case "endpointmethod":
							sortFieldName = "[EndpointMethod]";
							break;
						case "loglevel":
							sortFieldName = "[LogLevel]";
							break;
						case "logmessage":
							sortFieldName = "[LogMessage]";
							break;
						case "userid":
							sortFieldName = "[UserId]";
							break;
					}
				}
				#endregion


				var results = ERP_LogsAccess.GetFeedbacksLogs(_data.Level, _data.SearchValue, _data.SearchDate, sortFieldName, _data.SortDesc, treated: (_data is not null && _data.IsTreated.HasValue && (_data.IsTreated ?? false)), _data.RequestedPage, (int)_data.PageSize);
				int allCount = ERP_LogsAccess.GetFeedbacksLogs_Count(_data.Level, _data.SearchValue, _data.SearchDate, (_data is not null && _data.IsTreated.HasValue && (_data.IsTreated ?? false)));
				var responseBody = new FeedbackLogResponseModel();
				if(allCount == 0)
				{
					return ResponseModel<FeedbackLogResponseModel>.SuccessResponse(responseBody);
				}

				return ResponseModel<FeedbackLogResponseModel>.SuccessResponse(new FeedbackLogResponseModel
				{
					Items = allCount == 0 ? new List<FeedbackLogModel>() : results?.Select(x => new FeedbackLogModel(x))?.ToList(),
					PageRequested = _data.RequestedPage,
					PageSize = _data.PageSize,
					TotalCount = allCount,
					TotalPageCount = _data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / _data.PageSize)) : 0
				});


			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> UpdateLogTreated(FeedbackLogUpdateModel model, UserModel user)
		{
			int result = 0;

			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(model != null)
			{
				result = ERP_LogsAccess.UpdateLogTreatedById(model.LogId, model.Treated.HasValue == true ? 1 : 0);
			}

			if(result > 0)
			{

				return ResponseModel<int>.SuccessResponse(result);
			}
			else
			{
				return ResponseModel<int>.FailureResponse("Error when updating Log Treated.");
			}
		}
	}
}

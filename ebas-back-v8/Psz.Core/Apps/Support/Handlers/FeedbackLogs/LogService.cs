using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.Access.Tables.NLogs;
using Infrastructure.Data.Access.Tables.Support.Feedback;
using Infrastructure.Data.Entities.Joins.NLog;
using Psz.Core.Apps.Support.Interfaces;
using Psz.Core.Apps.Support.Models.FeedbackLogs;
using Psz.Core.Apps.Support.Models.Logs;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using static Infrastructure.Services.Files.Parsing.PerfLogParsingUtilities;


namespace Psz.Core.Apps.Support.Handlers.Logs
{
	public class LogService: ILogService
	{
		public ResponseModel<ApiCallResponseModel> GetApiLastCall(UserModel user, ApiCallRequestModel _data)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<ApiCallResponseModel>.AccessDeniedResponse();
				}
				#region > Data sorting & paging
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;


				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = _data.PageSize > 0 ? (_data.RequestedPage * _data.PageSize) : 0,
					RequestRows = _data.PageSize
				};


				var sortFieldName = "";
				if(!String.IsNullOrEmpty(sortFieldName))
				{
					switch(_data.SortField.ToLower())
					{
						default:
						case "url":
							sortFieldName = "[Url]";
							break;
						case "lastcall":
							sortFieldName = "[LastCall]";
							break;
					}
				}

				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = _data.SortDesc,
				};

				#endregion

				var lastApiCallList = ERP_Nlog_ExceptionsAccess.GetApiLastCall(_data.ApiUrl, sortFieldName, _data.SortDesc, _data.RequestedPage, (int)_data.PageSize).ToList();
				int allCount = ERP_Nlog_ExceptionsAccess.GetApiLastCall_Count(_data.ApiUrl);

				if(lastApiCallList.Count > 0)
				{
					return ResponseModel<ApiCallResponseModel>.SuccessResponse(new ApiCallResponseModel
					{

						Items = lastApiCallList,
						PageRequested = _data.RequestedPage,
						PageSize = _data.PageSize,
						TotalCount = allCount > 0 ? allCount : 0,
						TotalPageCount = _data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / _data.PageSize)) : 0,
					});
				}
				return ResponseModel<ApiCallResponseModel>.SuccessResponse();
				#endregion
			} catch(System.Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
		public ResponseModel<ApiCallCountResponseModel> GetApisCount(UserModel user, ApiCallRequestModel _data)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<ApiCallCountResponseModel>.AccessDeniedResponse();
				}
				#region > Data sorting & paging
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;


				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = _data.PageSize > 0 ? (_data.RequestedPage * _data.PageSize) : 0,
					RequestRows = _data.PageSize
				};

				var sortFieldName = "";
				if(!String.IsNullOrEmpty(sortFieldName))
				{
					switch(_data.SortField.ToLower())
					{
						default:
						case "url":
							sortFieldName = "[Url]";
							break;
						case "lastcall":
							sortFieldName = "[LastCall]";
							break;
					}
				}

				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = _data.SortDesc,
				};

				#endregion

				var apiCallsCount = ERP_Nlog_ExceptionsAccess.GetApiCalls6Months(_data.ApiUrl, sortFieldName, _data.SortDesc, _data.RequestedPage, (int)_data.PageSize).ToList();
				int allCount = ERP_Nlog_ExceptionsAccess.GetApiCalls6Months_Count(_data.ApiUrl);

				if(apiCallsCount.Count > 0)
				{
					return ResponseModel<ApiCallCountResponseModel>.SuccessResponse(new ApiCallCountResponseModel
					{

						Items = apiCallsCount,
						PageRequested = _data.RequestedPage,
						PageSize = _data.PageSize,
						TotalCount = allCount > 0 ? allCount : 0,
						TotalPageCount = _data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / _data.PageSize)) : 0,
					});
				}
				return ResponseModel<ApiCallCountResponseModel>.SuccessResponse();
				#endregion
			} catch(System.Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
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
							sortFieldName = "[Date]";
							break;
						case "endpointname":
							sortFieldName = "[MemberName]";
							break;
						case "endpointmethod":
							sortFieldName = "[SourceFilePath]";
							break;
						case "loglevel":
							sortFieldName = "[Level]";
							break;
						case "logmessage":
							sortFieldName = "[Message]";
							break;
						case "userid":
							sortFieldName = "[EventId]";
							break;
					}
				}
				#endregion

				var results = Infrastructure.Data.Access.Tables.NLogs.ERP_Nlog_ExceptionsAccess.GetLogsExceptions(_data.Level, _data.SearchValue, _data.SearchDate, sortFieldName, _data.SortDesc, _data.RequestedPage, (int)_data.PageSize);
				int allCount = Infrastructure.Data.Access.Tables.NLogs.ERP_Nlog_ExceptionsAccess.GetLogsExceptions_Count(_data.Level, _data.SearchValue, _data.SearchDate);
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
		public ResponseModel<NLogsSummary> GetLogsSummary(UserModel user)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<NLogsSummary>.AccessDeniedResponse();
				}
				#endregion

				var logsSummary = ERP_Nlog_ExceptionsAccess.GetNLogsSummary();

				if(logsSummary != null)
				{
					return ResponseModel<NLogsSummary>.SuccessResponse(logsSummary);
				}
				return ResponseModel<NLogsSummary>.SuccessResponse();

			} catch(System.Exception ex)
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
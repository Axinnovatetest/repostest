using Infrastructure.Data.Access.Tables.Support.Feedback;
using Infrastructure.Data.Entities.Tables.Support.Feedback;
using Newtonsoft.Json;
using Psz.Core.Apps.Support.Interfaces;
using Psz.Core.Apps.Support.Models.Feedback;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Support.Handlers.Feedback
{
	public class FeedbacksService: IFeedbacksService
	{
		public ResponseModel<int> CreateFeedback(UserModel user, CreateFeedbackRequestModel _data)
		{
			try
			{
				#region validations 

				if(user == null)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}

				if(_data == null || Convert.ToString(_data) == "" || String.IsNullOrWhiteSpace(Convert.ToString(_data)) || String.IsNullOrEmpty(Convert.ToString(_data)))
				{
					return ResponseModel<int>.FailureResponse("Empty feedback !");
				}

				#endregion

				var insertedFeedbackEntity = _data.ToEntity();
				insertedFeedbackEntity.UserId = user.Id;
				insertedFeedbackEntity.Username = user.Name;
				insertedFeedbackEntity.CreationDate = DateTime.Now;
				var insertedId = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Insert(insertedFeedbackEntity);


				return ResponseModel<int>.SuccessResponse(insertedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<GetFeedbacksResponseModel> GetFeedbackById(UserModel user, int Id)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<GetFeedbacksResponseModel>.AccessDeniedResponse();
				}
				var feedbackItem = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get(Id);
				if(feedbackItem == null)
				{
					return ResponseModel<GetFeedbacksResponseModel>.FailureResponse("Item not found !");
				}

				#endregion
				GetFeedbacksResponseModel feedbackById = new GetFeedbacksResponseModel(feedbackItem);

				return ResponseModel<GetFeedbacksResponseModel>.SuccessResponse(feedbackById);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;

			}
		}

		public ResponseModel<GetFeedbackByModuleResponseModel> GetFeedbackByModule(UserModel user, GetFeedbacksRequestModel data)
		{
			try
			{
				#region validations 

				if(user == null)
				{
					return ResponseModel<GetFeedbackByModuleResponseModel>.AccessDeniedResponse();
				}

				#endregion

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};
				#endregion

				var allFeedbacks = ERP_FeedbacksAccess.Get().ToList();

				// Group by PageUrl and calculate counts
				var pageUrlCounts = allFeedbacks
					.GroupBy(feedback => feedback.PageUrl)
					.ToDictionary(group => group.Key, group => group.Count());
				var treatedFeedbackCount = allFeedbacks
					.Where(f => f.Treated == true)
					.GroupBy(feedback => feedback.PageUrl)
					.ToDictionary(group => group.Key, group => group.Count());

				var feedbackEntities = ERP_FeedbacksAccess.GetBySearchValue(data.SearchValue, data.Module, data.SubModule, data.FeedbackType, data.Priority, dataPaging).ToList();
				int allCount = ERP_FeedbacksAccess.GetBySearchValue_Count(data.SearchValue, data.Module, data.SubModule, data.FeedbackType, data.Priority);
				var responseBody = new GetFeedbackByModuleResponseModel();
				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (data.PageSize <= 0 ? 1 : data.PageSize));
				responseBody.PageSize = data.PageSize;
				responseBody.PageRequested = data.RequestedPage;
				responseBody.Items = feedbackEntities
				?.Select(feedback =>
				{
					var model = new GetFeedbacksResponseModel(feedback);
					model.AssociatedFeedbackCount = pageUrlCounts.TryGetValue(feedback.PageUrl, out var count) ? count : 0;
					model.FeedbacksTreatedCount = treatedFeedbackCount.TryGetValue(feedback.PageUrl, out var treatedCount) ? treatedCount : 0;
					return model;
				})
				.ToList();
				return ResponseModel<GetFeedbackByModuleResponseModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> GetFeedbacksModules(UserModel user)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
				}
				#endregion

				var modules = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("cts", "Customer Service"),
				new KeyValuePair<string, string>("mtd", "Master Data"),
				new KeyValuePair<string, string>("sld", "Sales & Distribution"),
				new KeyValuePair<string, string>("crp", "Capacity Requirement Planning"),
				new KeyValuePair<string, string>("lgt", "Logistics"),
				new KeyValuePair<string, string>("mgo", "Management Overview"),
				new KeyValuePair<string, string>("pur", "Purchase"),
				new KeyValuePair<string, string>("fnc", "Finance & Control"),
				new KeyValuePair<string, string>("mtm", "Materials Management")

			};
				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(modules);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> GetFeedbacksSubmodules(UserModel user, string module)
		{
			try
			{
				#region validations

				if(user == null)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
				}

				if(String.IsNullOrEmpty(Convert.ToString(module)))
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse("Module is empty !");
				}

				#endregion

				List<KeyValuePair<int, string>> subModules = new List<KeyValuePair<int, string>>();

				switch(module)
				{
					case "cts":
						{
							List<string> subModuleNames = new List<string>
								{
									"order-responses",
									"edi",
									"fertigung",
									"statistics",
									"configuration",
									"replacements",
									"administration"
								};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "crp":
						{
							List<string> subModuleNames = new List<string>
							{
								"fa-planning",
								"ubg-planning",
								"delivery-forecast",
								"requirement/forecast",
								"fertigung",
								"statistics-dashboard",
								"configuration",
								"replacements",
								"administration"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "sld":
						{
							List<string> subModuleNames = new List<string>
							{
								"work-schedule",
								"country-definition",
								"Departement",
								"standard-operation",
								"standard-operation",
								"historystandard",
								"hall",
								"work-area",
								"work-station",
								"history-config",
								"history-work-schedule"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "lgt":
						{
							List<string> subModuleNames = new List<string>
							{
								"dashboard",
								"statistics",
								"packages",
								"materials",
								"customs",
								"plant-bookings"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "fnc":
						{
							List<string> subModuleNames = new List<string>
							{
								"dashboard",
								"budgets-configuration",
								"budgets-allocations",
								"budgets-projects",
								"budget-rebuild-receptions",
								"statistics"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "pur":
						{
							List<string> subModuleNames = new List<string>
							{
								"dashboard",
								"orders",
								"stock-warnings",
								"dispo",
								"statistics",
								"rahmen"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "mgo":
						{
							List<string> subModuleNames = new List<string>
							{
								"dashboard",
								"customer-service",
								"project-management",
								"periodic-sales",
								"statistics",
								"production"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "mtd":
						{
							List<string> subModuleNames = new List<string>
							{
								"dashboard",
								"articles",
								"customers",
								"suppliers",
								"settings",
								"Offers"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					case "mtm":
						{
							List<string> subModuleNames = new List<string>
							{
								"dashboard",
								"holidays",
								"planning",
								"capacities",
								"planning-validations",
								"configurations-units"
							};
							for(int i = 0; i < subModuleNames.Count; i++)
							{
								subModules.Add(new KeyValuePair<int, string>(i, subModuleNames[i]));
							}
						}
						break;
					default:
						break;

				}

				if(subModules.Count > 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(subModules);
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse("1", "Error when returning submodules list");


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, int>>> GetModulesFeedbackCount(UserModel user)
		{
			try
			{
				if(user == null/*
                || this._user.Access.____*/)
				{
					return ResponseModel<List<KeyValuePair<string, int>>>.AccessDeniedResponse();
				}

				var feedbackEntities = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get();
				var response = new List<KeyValuePair<string, int>>();
				if(feedbackEntities != null && feedbackEntities.Count > 0)
				{
					var _Modules = feedbackEntities.Select(x => x.Module).Distinct().ToList();
					foreach(var item in _Modules)
					{
						response.Add(new KeyValuePair<string, int>(item, feedbackEntities.Count(y => y.Module == item)));
					}
				}

				return ResponseModel<List<KeyValuePair<string, int>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> UpdateFeedbackTreated(UserModel user, int Id)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}
				var feedBackEntity = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get(Id);
				if(feedBackEntity == null)
				{
					return ResponseModel<int>.FailureResponse("feedback not found !");
				}
				#endregion

				feedBackEntity.Treated = true;
				feedBackEntity.TreatedUser = user.Name;
				feedBackEntity.TreatedDate = DateTime.Now;
				var resposne = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Update(feedBackEntity);

				return ResponseModel<int>.SuccessResponse(resposne);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(Id)}"); // - 1 - Input Data
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace); // - 2 - Call Log
				throw;
			}
		}

		public ResponseModel<int> UpdatePriority(UserModel user, UpdatePriorityRequestModel request)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}
				var feedBackEntity = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Get(request.Id);
				if(feedBackEntity == null)
				{
					return ResponseModel<int>.FailureResponse("Item not found !");
				}
				#endregion

				feedBackEntity.priority = request.Priority;

				var response = Infrastructure.Data.Access.Tables.Support.Feedback.ERP_FeedbacksAccess.Update(feedBackEntity);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Error, $"Error when updating item with id: {request.Id}");
				throw;

			}
		}

		public ResponseModel<GetFeedbackByUrlResponseModel> GetFeedbacksByPageUrl(UserModel user, GetFeedbackByUrlRequestModel _data)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<GetFeedbackByUrlResponseModel>.AccessDeniedResponse();
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
						case "comment":
							sortFieldName = "[Comment]";
							break;
						case "module":
							sortFieldName = "[Module]";
							break;
						case "pageurl":
							sortFieldName = "[PageUrl]";
							break;
						case "rating":
							sortFieldName = "[Rating]";
							break;
						case "submodule":
							sortFieldName = "[Submodule]";
							break;
						case "priority":
							sortFieldName = "[Priority]";
							break;
					}
				}

				var dataSorting = new Infrastructure.Data.Access.Settings.SortingModel() { SortDesc = _data.SortDesc, SortFieldName = _data.SortField };

				#endregion



				var results = ERP_FeedbacksAccess.GetByPageLink(_data.PageUrl, dataSorting, dataPaging);
				int allCount = ERP_FeedbacksAccess.GetByPageLink_Count(_data.PageUrl);
				var responseBody = new GetFeedbackByUrlResponseModel();
				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (_data.PageSize <= 0 ? 1 : _data.PageSize));
				responseBody.PageSize = _data.PageSize;
				responseBody.PageRequested = _data.RequestedPage;
				responseBody.Items = results?.Select(x => new GetFeedbacksResponseModel(x))?.ToList();
				return ResponseModel<GetFeedbackByUrlResponseModel>.SuccessResponse(responseBody);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;

			}
		}

		public ResponseModel<int> MarkAllTreated(UserModel user, List<int> ids)
		{
			try
			{
				#region validations
				if(user == null)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}
				int response;
				if(ids.Count > 0)
				{
					List<ERP_FeedbacksEntity> itemsToUpdate = ERP_FeedbacksAccess.Get(ids).ToList();
					foreach(var item in itemsToUpdate)
					{
						item.Treated = item.Treated.HasValue ? !item.Treated.Value : true;
						item.TreatedUser = user.Name;
						item.TreatedDate = DateTime.Now;
					}
					response = ERP_FeedbacksAccess.Update(itemsToUpdate);
					return ResponseModel<int>.SuccessResponse(response);
				}
				return ResponseModel<int>.FailureResponse("feedbacks Ids list is empty !");
				#endregion
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;

			}
		}
	}
}

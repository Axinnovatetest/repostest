using Psz.Core.Apps.EDI.Models.Customers.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public partial class Users
		{
			public static Core.Models.ResponseModel<Models.Customers.Users.UserModel> Get(int id,
				Core.Identity.Models.UserModel user)
			{
				try
				{
					if(user == null
						|| !user.Access.Purchase.ModuleActivated)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var responseBody = Get(id);
					if(responseBody == null)
					{
						throw new Core.Exceptions.NotFoundException();
					}

					return Core.Models.ResponseModel<Models.Customers.Users.UserModel>.SuccessResponse(responseBody);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
			public static Core.Models.ResponseModel<List<Models.Customers.Users.UserModel>> Get(Core.Identity.Models.UserModel user)
			{
				try
				{
					if(user == null
						//|| !user.Access.Purchase.ModuleActivated
						)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					return Core.Models.ResponseModel<List<Models.Customers.Users.UserModel>>.SuccessResponse(Get());
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}

			internal static Models.Customers.Users.UserModel Get(int id)
			{
				try
				{
					return Get(new List<int>() { id }).FirstOrDefault();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
			internal static List<Models.Customers.Users.UserModel> Get()
			{
				try
				{
					return Get(Infrastructure.Data.Access.Tables.COR.UserAccess.Get());
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
			internal static List<Models.Customers.Users.UserModel> Get(List<int> ids)
			{
				try
				{
					return Get(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(ids));
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
			public static Core.Models.ResponseModel<List<AppointmentCustomerUserResponseModel>> GetUserCustomersList(Core.Identity.Models.UserModel user, AppointmentCustomerUserRequestModel data)
			{
				try
				{
					if(user == null)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var customersUserList = new List<AppointmentCustomerUserResponseModel>();

					if(!data.IsAssignedEmployee.Value)
					{
						customersUserList = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetCustomerWithoutEmployees(data.CustomerName).Select(x => new AppointmentCustomerUserResponseModel(x)).ToList();
					}
					else
					{
						customersUserList = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetCustomersUsersList(data.CustomerName, data.EmployeeName).Select(x => new AppointmentCustomerUserResponseModel(x)).ToList();

					}

					if(customersUserList.Count > 0)
					{
						var response = customersUserList;
						return Core.Models.ResponseModel<List<AppointmentCustomerUserResponseModel>>.SuccessResponse(response);
					}

					return Core.Models.ResponseModel<List<AppointmentCustomerUserResponseModel>>.SuccessResponse(new List<AppointmentCustomerUserResponseModel>());


				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
			internal static List<Models.Customers.Users.UserModel> Get(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> usersDb)
			{
				try
				{
					var response = new List<Models.Customers.Users.UserModel>();

					var accessProfilesIds = usersDb.Select(e => e.AccessProfileId).ToList();
					var accessProfilesDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(accessProfilesIds);

					var usersIds = usersDb.Select(e => e.Id).ToList();
					var allCustomersUsersDb = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUsersIds(usersIds);

					var customersNumbers = allCustomersUsersDb.Select(e => e.CustomerNumber).ToList();
					var customersAdressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);

					foreach(var userDb in usersDb)
					{
						var user = new Models.Customers.Users.UserModel()
						{
							Id = userDb.Id,
							Description = userDb.Username,
							Name = userDb.Name,
							Title = userDb.Name,
							AccessProfileId = userDb.AccessProfileId,
							AccessProfileName = accessProfilesDb.Find(e => e.Id == userDb.AccessProfileId)?.Name,
							Customers = new List<Models.Customers.Users.UserModel.UserCustomerModel>()
						};

						var customersUsersDb = allCustomersUsersDb.FindAll(e => e.UserId == userDb.Id);
						foreach(var customerUserDb in customersUsersDb)
						{
							var customerAdressDb = customersAdressesDb.Find(e => e.Nr == customerUserDb.CustomerNumber);

							user.Customers.Add(new Models.Customers.Users.UserModel.UserCustomerModel()
							{
								CustomerNumber = customerUserDb.CustomerNumber,
								CustomerName = customerAdressDb?.Name1,
								UserIsPrimary = customerUserDb.IsPrimary,
								ValidIntoTime = customerUserDb.IsPrimary ? (DateTime?)null : customerUserDb.ValidIntoTime,
								ValidFromTime = customerUserDb.IsPrimary ? (DateTime?)null : customerUserDb.ValidFromTime,
							});
						}

						response.Add(user);
					}

					return response;
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}

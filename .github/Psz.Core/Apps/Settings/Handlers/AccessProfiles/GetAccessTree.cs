using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		//public static Core.Models.ResponseModel<List<Models.AccessProfiles.AccessTreeBranchModel>> GetAccessTree(int? accessProfileId,
		//    Core.Identity.Models.UserModel user)
		//{
		//    try
		//    {
		//        if (user == null
		//            || user.Access == null
		//            || !user.Access.Settings.ModuleActivated)
		//        {
		//            //throw new Core.Exceptions.UnauthorizedException();
		//        }

		//        var accessProfile = accessProfileId.HasValue
		//            ? Get(accessProfileId.Value)
		//            : null;
		//        if (accessProfileId.HasValue && accessProfile == null)
		//        {
		//            throw new Core.Exceptions.NotFoundException();
		//        }

		//        if (accessProfile == null)
		//        {
		//            accessProfile = new Models.AccessProfiles.AccessProfileModel() { Id = -1 };
		//        }

		//        var response = new Core.Models.ResponseModel<List<Models.AccessProfiles.AccessTreeBranchModel>>();
		//        response.Body = new List<Models.AccessProfiles.AccessTreeBranchModel>();

		//        response.Body.Add(new Models.AccessProfiles.AccessTreeBranchModel()
		//        {
		//            Code = "settings",
		//            ParentCode = null,
		//            Text = "Settings Module"
		//        });
		//        response.Body.AddRange(getObjectBranches(accessProfile.Settings, "settings"));

		//        response.Body.Add(new Models.AccessProfiles.AccessTreeBranchModel()
		//        {
		//            Code = "edi",
		//            ParentCode = null,
		//            Text = "EDI Module"
		//        });
		//        response.Body.AddRange(getObjectBranches(accessProfile.Purchase, "edi"));

		//        response.Body.Add(new Models.AccessProfiles.AccessTreeBranchModel()
		//        {
		//            Code = "workplan",
		//            ParentCode = null,
		//            Text = "WorkPlan"
		//        });
		//        response.Body.AddRange(getObjectBranches(accessProfile.WorkPlan, "workplan"));

		//        response.Body.Add(new Models.AccessProfiles.AccessTreeBranchModel()
		//        {
		//            Code = "budget",
		//            ParentCode = null,
		//            Text = "Budget"
		//        });
		//        response.Body.AddRange(getObjectBranches(accessProfile.Budget, "budget"));

		//        return response;
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}

		private static List<Models.AccessProfiles.AccessTreeBranchModel> getObjectBranches<T>(T data, string parentCode)
		{
			var response = new List<Models.AccessProfiles.AccessTreeBranchModel>();

			var propreties = typeof(Models.AccessProfiles.PurchaseAccessModel).GetProperties();
			foreach(var proprety in propreties)
			{
				if(proprety.PropertyType != typeof(bool))
				{
					continue;
				}

				var value = proprety.GetValue(data, null) as Boolean?;
				if(!value.HasValue)
				{
					continue;
				}

				var displayAttributeObject = proprety.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
				var displayAttribute = displayAttributeObject != null
					? (displayAttributeObject as DisplayAttribute)
					: null;
				string displayName = displayAttribute?.Name;
				string displayDescription = displayAttribute?.Description;
				if(string.IsNullOrWhiteSpace(displayName) || string.IsNullOrWhiteSpace(displayDescription))
				{
					continue;
				}

				response.Add(new Models.AccessProfiles.AccessTreeBranchModel()
				{
					ParentCode = parentCode,
					Code = displayDescription,
					Text = displayName,
					Value = value.Value
				});
			}

			return response;
		}
	}
}

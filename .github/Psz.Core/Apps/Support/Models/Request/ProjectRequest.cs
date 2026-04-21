using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Support.Models.Request
{
	public class ProjectRequest
	{
		public int Id { get; set; }
		public string Requester { get; set; }
		public string Department { get; set; }
		public DateTime Date { get; set; }
		public string Theme { get; set; }
		public int Application { get; set; }
		public string OtherApplications { get; set; }
		public int Reason { get; set; }
		public string OtherReasons { get; set; }
		public string Requirement { get; set; }
		public string BuisnessProcess { get; set; }
		public string Problem { get; set; }
		public string Goals { get; set; }
		public string Benefits { get; set; }
		public string Consequences { get; set; }
		public string Dependencies { get; set; }
		public string ItNr { get; set; }
		public string ItFeasibility { get; set; }
		public string ItConditions { get; set; }
		public string ItEffort { get; set; }
		public string ItConcept { get; set; }
		public int Status { get; set; }
		public int Priority { get; set; }
		public int ValidationUserId { get; set; }
		public string ValidationUserName { get; set; }
		public DateTime? CreationDate { get; set; }
		public DateTime? ValidationDate { get; set; }
		public int CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public string LastEditUserName { get; set; }
		public bool Validated { get; set; }
		public List<Files> FilesList { get; set; }
		public List<Signature> Signatures { get; set; }
		public List<RequirementData> RequirementProcess { get; set; }
		public List<UserData> Users { get; set; }
		public class Files
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}
		public class Signature
		{
			public string Function { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public DateTime Date { get; set; }
		}

		public class UserData
		{
			public string FullName { get; set; }
			public string Department { get; set; }
			public string Phone { get; set; }
			public string Email { get; set; }

		}
		public class RequirementData
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public string TestUseCase { get; set; }
		}
		public ProjectRequest() { }
		public ProjectRequest(Infrastructure.Data.Entities.Tables.Support.Request.RequestEntity requestEntity)
		{
			if(requestEntity is null || requestEntity.Id == 0)
			{
				return;
			}

			var user = Infrastructure.Data.Access.Tables.Support.Request.User_RequestAccess.GetByRequestId(requestEntity.Id);
			var requirement_Processes = Infrastructure.Data.Access.Tables.Support.Request.Requirement_ProcessAccess.GetByRequestId(requestEntity.Id);
			var signatures = Infrastructure.Data.Access.Tables.Support.Request.SignatureAccess.GetByRequestId(requestEntity.Id);
			var files = Infrastructure.Data.Access.Tables.Support.Request.FilesAccess.GetByRequestId(requestEntity.Id);
			var creationUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(requestEntity.CreationUserId.HasValue ? requestEntity.CreationUserId.Value : 0);
			var editUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(requestEntity.LastEditUserId.HasValue ? requestEntity.LastEditUserId.Value : 0);
			var validationUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(requestEntity.ValidationUserId.HasValue ? requestEntity.ValidationUserId.Value : 0);

			Id = requestEntity.Id;
			Application = requestEntity.Application;
			Benefits = requestEntity.Benefits;
			BuisnessProcess = requestEntity.BuisnessProcess;
			Consequences = requestEntity.Consequences;
			Date = requestEntity.Date;
			Department = requestEntity.Department;
			Dependencies = requestEntity.Dependencies;
			Goals = requestEntity.Goals;
			ItConcept = requestEntity.ItConcept;
			ItConditions = requestEntity.ItConditions;
			ItEffort = requestEntity.ItEffort;
			ItFeasibility = requestEntity.ItFeasibility;
			ItNr = requestEntity.ItNr;
			OtherApplications = requestEntity.OtherApplication;
			OtherReasons = requestEntity.OtherReason;
			Problem = requestEntity.Problem;
			Reason = requestEntity.Reason;
			Requester = requestEntity.Requester;
			Requirement = requestEntity.Requirement;
			Theme = requestEntity.Theme;
			Status = requestEntity.Status.HasValue ? requestEntity.Status.Value : 0;
			Priority = requestEntity.Priority.HasValue ? requestEntity.Priority.Value : 0;
			Validated = requestEntity.Validated.HasValue ? requestEntity.Validated.Value : false;
			CreationUserName = creationUser?.Username;
			ValidationUserName = validationUser?.Username;
			LastEditUserName = editUser?.Username;
			ValidationUserId = requestEntity.ValidationUserId.HasValue ? requestEntity.ValidationUserId.Value : 0;
			CreationDate = requestEntity.CreationTime;
			ValidationDate = requestEntity.ValidationDate;

			Signatures = signatures != null ? signatures.Select(x => new Signature { Date = x.Date, FirstName = x.FirstName, Function = x.Function, LastName = x.LastName }).ToList() : new List<Signature>();
			Users = user != null ? user.Select(x => new UserData { Department = x.Department, Email = x.Email, FullName = x.Name, Phone = x.Phone }).ToList() : new List<UserData>();
			RequirementProcess = requirement_Processes != null ? requirement_Processes.Select(x => new RequirementData { Description = x.Description, Name = x.Name, TestUseCase = x.TestUseCase }).ToList() : new List<RequirementData>();
			FilesList = files != null ? files.Select(x => new Files { Name = x.FileName, Id = x.Id }).ToList() : new List<Files>();
		}
		public void ToEntity()
		{

		}
	}
}

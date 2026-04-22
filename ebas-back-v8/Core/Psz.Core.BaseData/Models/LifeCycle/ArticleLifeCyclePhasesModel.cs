using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.LifeCycle
{
	public class ArticleLifeCyclePhasesModel
	{
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public string CreateUserName { get; set; }
		public int Id { get; set; }
		public string PhaseDescription { get; set; }
		public string PhaseName { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public string UpdateUserName { get; set; }
		public ArticleLifeCyclePhasesModel()
		{
			
		}
		public ArticleLifeCyclePhasesModel(Infrastructure.Data.Entities.Tables.BSD.ArticleLifeCyclePhasesEntity entity)
		{
			CreateTime= entity.CreateTime;
			CreateUserId= entity.CreateUserId;
			CreateUserName=entity.CreateUserName;
			Id =entity.Id;
			PhaseDescription= entity.PhaseDescription;
			PhaseName= entity.PhaseName;
			UpdateTime= entity.UpdateTime;	
			UpdateUserId= entity.UpdateUserId;
			UpdateUserName= entity.UpdateUserName;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArticleLifeCyclePhasesEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleLifeCyclePhasesEntity
			{
				CreateTime = CreateTime,
				CreateUserId= CreateUserId,
				CreateUserName= CreateUserName,
				Id = Id,
                PhaseDescription= PhaseDescription,
				PhaseName= PhaseName,
				UpdateTime= UpdateTime,
				UpdateUserId= UpdateUserId,
				UpdateUserName= UpdateUserName,
			};
		}
	}

	public class ArticleLifeCyclePhasesRequestModel
	{
		public int Id { get; set; }
		public string PhaseDescription { get; set; }
		public string PhaseName { get; set; }
	}
}

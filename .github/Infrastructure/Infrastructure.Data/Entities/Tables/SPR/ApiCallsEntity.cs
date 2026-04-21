using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Entities.Tables.SPR
{
	public class ApiCallsEntity
	{
		public string ApiArea { get; set; }
		public string ApiController { get; set; }
		public string ApiMethod { get; set; }
		public DateTime? FirstCallTime { get; set; }
		public int Id { get; set; }
		public DateTime? LastCallTime { get; set; }
		public DateTime? ProcessingTime { get; set; }
		public int? TotalCall02HCount { get; set; }
		public int? TotalCall04HCount { get; set; }
		public int? TotalCall06HCount { get; set; }
		public int? TotalCall08HCount { get; set; }
		public int? TotalCall10HCount { get; set; }
		public int? TotalCall12HCount { get; set; }
		public int? TotalCall14HCount { get; set; }
		public int? TotalCall16HCount { get; set; }
		public int? TotalCall18HCount { get; set; }
		public int? TotalCall20HCount { get; set; }
		public int? TotalCall22HCount { get; set; }
		public int? TotalCall24HCount { get; set; }
		public int? TotalCallCount { get; set; }
		public int? TotalCallDistinctUserCount { get; set; }
		public int? UserId { get; set; }

		public ApiCallsEntity() { }

		public ApiCallsEntity(DataRow dataRow)
		{
			ApiArea = (dataRow["ApiArea"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ApiArea"]);
			ApiController = (dataRow["ApiController"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ApiController"]);
			ApiMethod = (dataRow["ApiMethod"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ApiMethod"]);
			FirstCallTime = (dataRow["FirstCallTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FirstCallTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastCallTime = (dataRow["LastCallTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastCallTime"]);
			ProcessingTime = (dataRow["ProcessingTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ProcessingTime"]);
			TotalCall02HCount = (dataRow["TotalCall02HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall02HCount"]);
			TotalCall04HCount = (dataRow["TotalCall04HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall04HCount"]);
			TotalCall06HCount = (dataRow["TotalCall06HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall06HCount"]);
			TotalCall08HCount = (dataRow["TotalCall08HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall08HCount"]);
			TotalCall10HCount = (dataRow["TotalCall10HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall10HCount"]);
			TotalCall12HCount = (dataRow["TotalCall12HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall12HCount"]);
			TotalCall14HCount = (dataRow["TotalCall14HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall14HCount"]);
			TotalCall16HCount = (dataRow["TotalCall16HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall16HCount"]);
			TotalCall18HCount = (dataRow["TotalCall18HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall18HCount"]);
			TotalCall20HCount = (dataRow["TotalCall20HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall20HCount"]);
			TotalCall22HCount = (dataRow["TotalCall22HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall22HCount"]);
			TotalCall24HCount = (dataRow["TotalCall24HCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCall24HCount"]);
			TotalCallCount = (dataRow["TotalCallCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCallCount"]);
			TotalCallDistinctUserCount = (dataRow["TotalCallDistinctUserCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCallDistinctUserCount"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}

		public ApiCallsEntity ShallowClone()
		{
			return new ApiCallsEntity
			{
				ApiArea = ApiArea,
				ApiController = ApiController,
				ApiMethod = ApiMethod,
				FirstCallTime = FirstCallTime,
				Id = Id,
				LastCallTime = LastCallTime,
				ProcessingTime = ProcessingTime,
				TotalCall02HCount = TotalCall02HCount,
				TotalCall04HCount = TotalCall04HCount,
				TotalCall06HCount = TotalCall06HCount,
				TotalCall08HCount = TotalCall08HCount,
				TotalCall10HCount = TotalCall10HCount,
				TotalCall12HCount = TotalCall12HCount,
				TotalCall14HCount = TotalCall14HCount,
				TotalCall16HCount = TotalCall16HCount,
				TotalCall18HCount = TotalCall18HCount,
				TotalCall20HCount = TotalCall20HCount,
				TotalCall22HCount = TotalCall22HCount,
				TotalCall24HCount = TotalCall24HCount,
				TotalCallCount = TotalCallCount,
				TotalCallDistinctUserCount = TotalCallDistinctUserCount,
				UserId = UserId
			};
		}
	}

	public class ApiAreaCallsEntity
	{
		public string ApiArea { get; set; }
		public string  Date { get; set; }
		public int? TotalCallCount { get; set; }
		public int? TotalCallDistinctUserCount { get; set; }

		public ApiAreaCallsEntity() { }

		public ApiAreaCallsEntity(DataRow dataRow)
		{
			ApiArea = dataRow["ApiArea"] == DBNull.Value ? "" : Convert.ToString(dataRow["ApiArea"]);
			TotalCallCount = dataRow["TotalCallCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallCount"]);
			TotalCallDistinctUserCount = dataRow["TotalCallDistinctUserCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallDistinctUserCount"]);
			Date = dataRow["Date"] == DBNull.Value ? "" : Convert.ToString(dataRow["Date"]);
		}
	}
	public class ApiSingleControllerCallsEntity
	{
		public string ApiController { get; set; }
		public string Date { get; set; }
		public int? TotalCallCount { get; set; }
		public int? TotalCallDistinctUserCount { get; set; }

		public ApiSingleControllerCallsEntity() { }

		public ApiSingleControllerCallsEntity(DataRow dataRow)
		{
			ApiController = dataRow["ApiController"] == DBNull.Value ? "" : Convert.ToString(dataRow["ApiController"]);
			TotalCallCount = dataRow["TotalCallCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallCount"]);
			TotalCallDistinctUserCount = dataRow["TotalCallDistinctUserCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallDistinctUserCount"]);
			Date = dataRow["Date"] == DBNull.Value ? "" : Convert.ToString(dataRow["Date"]);
		}
	}

	public class ApiAllControllerCallsEntity
	{
		public string ApiController { get; set; }
		public int? TotalCallCount { get; set; }
		public int? TotalCallDistinctUserCount { get; set; }

		public ApiAllControllerCallsEntity() { }

		public ApiAllControllerCallsEntity(DataRow dataRow)
		{
			ApiController = dataRow["ApiController"] == DBNull.Value ? "" : Convert.ToString(dataRow["ApiController"]);
			TotalCallCount = dataRow["TotalCallCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallCount"]);
			TotalCallDistinctUserCount = dataRow["TotalCallDistinctUserCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallDistinctUserCount"]);
		}
	}

	public class ApiMethodCallsEntity
	{
		public string ApiMethod { get; set; }
		public DateTime? FirstCallTime { get; set; }
		public string Date { get; set; }
		public int? TotalCallCount { get; set; }
		public int? TotalCallDistinctUserCount { get; set; }

		public ApiMethodCallsEntity() { }

		public ApiMethodCallsEntity(DataRow dataRow)
		{
			ApiMethod = dataRow["ApiMethod"] == DBNull.Value ? "" : Convert.ToString(dataRow["ApiMethod"]);
			TotalCallCount = dataRow["TotalCallCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallCount"]);
			TotalCallDistinctUserCount = dataRow["TotalCallDistinctUserCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallDistinctUserCount"]);
			Date   = dataRow["Date"] == DBNull.Value ? "" : Convert.ToString(dataRow["Date"]);
			
		}
	}


	public class ApiALLAreaCallsEntity
	{
		public string ApiArea { get; set; }
		public int? TotalCallCount { get; set; }
		public int? TotalCallDistinctUserCount { get; set; }

		public ApiALLAreaCallsEntity() { }

		public ApiALLAreaCallsEntity(DataRow dataRow)
		{
			ApiArea = dataRow["ApiArea"] == DBNull.Value ? "" : Convert.ToString(dataRow["ApiArea"]);
			TotalCallCount = dataRow["TotalCallCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallCount"]);
			TotalCallDistinctUserCount = dataRow["TotalCallDistinctUserCount"] == DBNull.Value ? null : Convert.ToInt32(dataRow["TotalCallDistinctUserCount"]);
		}
	}

	public class GetFirstAndLastCall
	{
		public string FirstCallTime { get; set; }
		public string LastCallTime { get; set; }
		

		public GetFirstAndLastCall() { }

		public GetFirstAndLastCall(DataRow dataRow)
		{
			LastCallTime = dataRow["LastCallTime"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastCallTime"]);
			FirstCallTime = dataRow["FirstCallTime"] == DBNull.Value ? "" : Convert.ToString(dataRow["FirstCallTime"]);
		}
	}


    public class HeavlyUsedApisEntity
    {
        public string AreasCalls { get; set; }
        public string ApiArea { get; set; }


        public HeavlyUsedApisEntity() { }

        public HeavlyUsedApisEntity(DataRow dataRow)
        {
            AreasCalls = dataRow["AreasCalls"] == DBNull.Value ? "" : Convert.ToString(dataRow["AreasCalls"]);
            ApiArea = dataRow["ApiArea"] == DBNull.Value ? "" : Convert.ToString(dataRow["ApiArea"]);
        }
    }

    public class UsersMostUsingERPEntity
    {
        public string UserCount { get; set; }
        public string UserId { get; set; }
        public UsersMostUsingERPEntity() { }

        public UsersMostUsingERPEntity(DataRow dataRow)
        {
            UserCount = dataRow["UserCount"] == DBNull.Value ? "" : Convert.ToString(dataRow["UserCount"]);
            UserId = dataRow["UserId"] == DBNull.Value ? "" : Convert.ToString(dataRow["UserId"]);
        }
    }
}

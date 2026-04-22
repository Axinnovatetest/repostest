using Infrastructure.Data.Entities.Tables.SPR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Models;

public class ApiCallsModel
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

	public ApiCallsModel() { }

	public ApiCallsModel(ApiCallsEntity data)
	{
		this.ApiArea = data.ApiArea;
		this.ApiController = data.ApiController;
		this.ApiMethod = data.ApiMethod;
		this.TotalCall12HCount= data.TotalCall12HCount;
		TotalCall06HCount= data.TotalCall06HCount;
		TotalCall02HCount= data.TotalCall02HCount;
		TotalCall08HCount = data.TotalCall08HCount;
		TotalCall14HCount = data.TotalCall14HCount;
		TotalCall16HCount= data.TotalCall16HCount;
		TotalCall18HCount= data.TotalCall18HCount;
		TotalCall20HCount= data.TotalCall20HCount;
		TotalCall22HCount= data.TotalCall22HCount;
		TotalCall24HCount= data.TotalCall24HCount;
		TotalCallCount = data.TotalCallCount;
		TotalCallDistinctUserCount = data.TotalCallDistinctUserCount;
		ProcessingTime = data.ProcessingTime;
		FirstCallTime =data.FirstCallTime;
		this.LastCallTime =data.LastCallTime;
		this.Id = data.Id;
		this.TotalCall04HCount = data.TotalCall04HCount;
	}
}

public class ApiAreaCallsModel
{
	public string ApiArea { get; set; }
	public DateTime? Date { get; set; }
	public int? TotalCallCount { get; set; }
	public int? TotalCallDistinctUserCount { get; set; }

	public ApiAreaCallsModel() { }

	public ApiAreaCallsModel(ApiAreaCallsEntity data)
	{
		this.ApiArea = data.ApiArea.ToLower();
		Date = DateTime.ParseExact(data.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
		TotalCallCount = data.TotalCallCount;
		TotalCallDistinctUserCount = data.TotalCallDistinctUserCount;
	}
}

public class ApiALLAreaCallsModel
{
	public string ApiArea { get; set; }
	public int? TotalCallCount { get; set; }
	public int? TotalCallDistinctUserCount { get; set; }

	public ApiALLAreaCallsModel() { }

	public ApiALLAreaCallsModel(ApiALLAreaCallsEntity data)
	{
		this.ApiArea = data.ApiArea.ToLower();
		TotalCallCount = data.TotalCallCount;
		TotalCallDistinctUserCount = data.TotalCallDistinctUserCount;
	}
}
public class ApiSingleControllerCallsModel
{
	public string ApiController { get; set; }
	public DateTime? Date { get; set; }
	public int? TotalCallCount { get; set; }
	public int? TotalCallDistinctUserCount { get; set; }

	public ApiSingleControllerCallsModel() { }

	public ApiSingleControllerCallsModel(ApiSingleControllerCallsEntity data)
	{
		this.ApiController = data.ApiController.ToLower();
		Date = DateTime.ParseExact(data.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
		TotalCallCount = data.TotalCallCount;
		TotalCallDistinctUserCount = data.TotalCallDistinctUserCount;
	}
}


public class ApiAllControllerCallsModel
{
	public string ApiController { get; set; }
	public int? TotalCallCount { get; set; }
	public int? TotalCallDistinctUserCount { get; set; }

	public ApiAllControllerCallsModel() { }

	public ApiAllControllerCallsModel(ApiAllControllerCallsEntity data)
	{
		this.ApiController = data.ApiController.ToLower();
		TotalCallCount = data.TotalCallCount;
		TotalCallDistinctUserCount = data.TotalCallDistinctUserCount;
	}
}

public class ApiMethodCallsModel
{
	public string ApiMethod { get; set; }
	public DateTime? Date { get; set; }
	public int? TotalCallCount { get; set; }
	public int? TotalCallDistinctUserCount { get; set; }

	public ApiMethodCallsModel() { }

	public ApiMethodCallsModel(ApiMethodCallsEntity data)
	{
		this.ApiMethod = data.ApiMethod.ToLower();
		Date = DateTime.ParseExact(data.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
		TotalCallCount = data.TotalCallCount;
		TotalCallDistinctUserCount = data.TotalCallDistinctUserCount;
	}
}

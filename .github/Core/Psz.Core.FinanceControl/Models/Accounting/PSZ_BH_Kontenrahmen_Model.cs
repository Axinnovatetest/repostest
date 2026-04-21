using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Accounting;

public class PSZ_BH_Kontenrahmen_Model
{
	public string Habenkto { get; set; }
	public int ID { get; set; }
	//public int TotalCount { get; set; }
	public string Rahmen { get; set; }
	public string Warengruppe { get; set; }
	public PSZ_BH_Kontenrahmen_Model(Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_KontenrahmenEntity data)
	{
		Habenkto = data.Habenkto;
		ID = data.ID;
		Rahmen = data.Rahmen;
		Warengruppe = data.Warengruppe;
		//TotalCount = data.TotalCount;
	}

}
public class PSZ_BH_Kontenrahmen_CreateModel
{
	public string Habenkto { get; set; }
	public int ID { get; set; }
	public string Rahmen { get; set; }
	public string Warengruppe { get; set; }
	public PSZ_BH_Kontenrahmen_CreateModel()
	{
	}
	public List<string> GetUnMatchingAttributes(Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_KontenrahmenEntity data)
	{
		var unmatchingItems = new List<string>();
		if(data is not null)
		{
			if(Habenkto != data.Habenkto)
			{
				unmatchingItems.Add("Habenkto");
			}
			if(Rahmen != data.Rahmen)
			{
				unmatchingItems.Add("Rahmen");
			}
			if(Warengruppe != data.Warengruppe)
			{
				unmatchingItems.Add("Warengruppe");
			}

		}
		return unmatchingItems;
	}
	public List<string> GetUnMatchingUpdateAttributes(Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_KontenrahmenUpdateEntity data)
	{
		var unmatchingItems = new List<string>();
		if(data is not null)
		{
			if(Habenkto != data.Habenkto)
			{
				unmatchingItems.Add("Habenkto");
			}
			if(Rahmen != data.Rahmen)
			{
				unmatchingItems.Add("Rahmen");
			}
			if(Warengruppe != data.Warengruppe)
			{
				unmatchingItems.Add("Warengruppe");
			}

		}
		return unmatchingItems;
	}

}

public class PSZ_BH_Kontenrahmen_RequestModel: IPaginatedRequestModel
{

}


public class PSZ_BH_Kontenrahmen_LogModel
{
	public DateTime? DateTime { get; set; }
	public int Id { get; set; }
	//public int TotalCount { get; set; }
	public string LogObject { get; set; }
	public string LogText { get; set; }
	public string LogType { get; set; }
	public int? UserId { get; set; }
	public string Username { get; set; }
	public PSZ_BH_Kontenrahmen_LogModel(Infrastructure.Data.Entities.Tables.FNC.PSZ_BH_Kontenrahmen_LogEntity data)
	{
		DateTime = data.DateTime;
		Id = data.Id;
		//TotalCount = data.TotalCount;
		LogObject = data.LogObject;
		LogText = data.LogText;
		LogType = data.LogType;
		UserId = data.UserId;
		Username = data.Username;
	}
}
public class PSZ_BH_Kontenrahmen_LogRequestModel: IPaginatedRequestModel
{
}
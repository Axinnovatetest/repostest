using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC;

public class PSZ_BH_KontenrahmenEntity
{
	public string Habenkto { get; set; }
	public int ID { get; set; }
	public int TotalCount { get; set; }
	public string Rahmen { get; set; }
	public string Warengruppe { get; set; }

	public PSZ_BH_KontenrahmenEntity() { }

	public PSZ_BH_KontenrahmenEntity(DataRow dataRow)
	{
		TotalCount = Convert.ToInt32(dataRow["TotalCount"]);
		Habenkto = (dataRow["Habenkto"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Habenkto"]);
		ID = Convert.ToInt32(dataRow["ID"]);
		Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen"]);
		Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
	}

	public PSZ_BH_KontenrahmenEntity ShallowClone()
	{
		return new PSZ_BH_KontenrahmenEntity
		{
			Habenkto = Habenkto,
			ID = ID,
			Rahmen = Rahmen,
			Warengruppe = Warengruppe
		};
	}
}
public class PSZ_BH_KontenrahmenUpdateEntity
{
	public string Habenkto { get; set; }
	public int ID { get; set; }
	public string Rahmen { get; set; }
	public string Warengruppe { get; set; }

	public PSZ_BH_KontenrahmenUpdateEntity() { }

	public PSZ_BH_KontenrahmenUpdateEntity(DataRow dataRow)
	{
		Habenkto = (dataRow["Habenkto"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Habenkto"]);
		ID = Convert.ToInt32(dataRow["ID"]);
		Rahmen = (dataRow["Rahmen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rahmen"]);
		Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
	}

	public PSZ_BH_KontenrahmenUpdateEntity ShallowClone()
	{
		return new PSZ_BH_KontenrahmenUpdateEntity
		{
			Habenkto = Habenkto,
			ID = ID,
			Rahmen = Rahmen,
			Warengruppe = Warengruppe
		};
	}
}
public class PSZ_BH_KontenrahmenNext_Id_Entity
{
	public int ID { get; set; }

	public PSZ_BH_KontenrahmenNext_Id_Entity() { }

	public PSZ_BH_KontenrahmenNext_Id_Entity(DataRow dataRow)
	{
		ID = Convert.ToInt32(dataRow["ID"]);
	}
}





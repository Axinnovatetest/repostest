
namespace Infrastructure.Data.Entities.Tables.BSD;

public class ArtikelCustomerReferencesEntity
{
	public int? ArticleId { get; set; }
	public DateTime? CreateDate { get; set; }
	public int? CreateUser { get; set; }
	public string CreateUserName { get; set; }
	public int? CustomerId { get; set; }
	public string CustomerName { get; set; }
	public int? CustomerNumber { get; set; }
	public string CustomerReference { get; set; }
	public DateTime? EditDate { get; set; }
	public int? EditUser { get; set; }
	public string EditUserName { get; set; }
	public int Id { get; set; }

	public ArtikelCustomerReferencesEntity() { }

	public ArtikelCustomerReferencesEntity(DataRow dataRow)
	{
		ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
		CreateDate = (dataRow["CreateDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateDate"]);
		CreateUser = (dataRow["CreateUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUser"]);
		CreateUserName = (dataRow["CreateUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreateUserName"]);
		CustomerId = (dataRow["CustomerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerId"]);
		CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
		CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
		CustomerReference = (dataRow["CustomerReference"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerReference"]);
		EditDate = (dataRow["EditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EditDate"]);
		EditUser = (dataRow["EditUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EditUser"]);
		EditUserName = (dataRow["EditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EditUserName"]);
		Id = Convert.ToInt32(dataRow["Id"]);
	}

	public ArtikelCustomerReferencesEntity ShallowClone()
	{
		return new ArtikelCustomerReferencesEntity
		{
			ArticleId = ArticleId,
			CreateDate = CreateDate,
			CreateUser = CreateUser,
			CreateUserName = CreateUserName,
			CustomerId = CustomerId,
			CustomerName = CustomerName,
			CustomerNumber = CustomerNumber,
			CustomerReference = CustomerReference,
			EditDate = EditDate,
			EditUser = EditUser,
			EditUserName = EditUserName,
			Id = Id
		};
	}


}


public class ArtikelCustomerReferencesLikeEntity
{

	public string CustomerName { get; set; }
	public int? CustomerNumber { get; set; }
	public string CustomerReference { get; set; }
	public ArtikelCustomerReferencesLikeEntity() { }

	public ArtikelCustomerReferencesLikeEntity(DataRow dataRow)
	{

		CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
		CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
		CustomerReference = (dataRow["CustomerReference"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerReference"]);
	}

	public ArtikelCustomerReferencesEntity ShallowClone()
	{
		return new ArtikelCustomerReferencesEntity
		{
			CustomerName = CustomerName,
			CustomerNumber = CustomerNumber,
			CustomerReference = CustomerReference,
		};
	}


}

public class ArtikelCustomerReferencesAndCustomerIDLikeEntity
{
	public int? CustomerId { get; set; }
	public string CustomerReference { get; set; }
	public string supplierName { get; set; }
	public ArtikelCustomerReferencesAndCustomerIDLikeEntity() { }

	public ArtikelCustomerReferencesAndCustomerIDLikeEntity(DataRow dataRow)
	{

		CustomerId = (dataRow["CustomerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerId"]);
		CustomerReference = (dataRow["CustomerReference"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerReference"]);
	}
}

public class GetArtikelCustomerReferencesAndCustomerIDLikeEntity
{
	public int? CustomerId { get; set; }
	public string CustomerReference { get; set; }
	public string supplierName { get; set; }
	public int? ArticleId { get; set; }
	public GetArtikelCustomerReferencesAndCustomerIDLikeEntity() { }

	public GetArtikelCustomerReferencesAndCustomerIDLikeEntity(DataRow dataRow)
	{

		CustomerId = (dataRow["CustomerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerId"]);
		ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
		CustomerReference = (dataRow["CustomerReference"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerReference"]);
	}
}
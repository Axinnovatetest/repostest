namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Budget_departementAccess
	{

		//    #region Default Methods
		//    public static Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity Get(int id)
		//    {
		//        var dataTable = new DataTable();
		//        using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//        {
		//            sqlConnection.Open();
		//            string query = "SELECT * FROM [Budget_departement] WHERE [ID]=@Id";
		//            var sqlCommand = new SqlCommand(query, sqlConnection);
		//            sqlCommand.Parameters.AddWithValue("Id", id); 

		//            new SqlDataAdapter(sqlCommand).Fill(dataTable);

		//        }

		//        if (dataTable.Rows.Count > 0)
		//        {
		//            return new Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity(dataTable.Rows[0]);
		//        }
		//        else
		//        {
		//            return null;
		//        }
		//    }

		//    public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> Get()
		//    {  
		//        var dataTable = new DataTable();     
		//        using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//        {
		//            sqlConnection.Open();
		//            string query = "SELECT * FROM [Budget_departement]";
		//            var sqlCommand = new SqlCommand(query, sqlConnection); 

		//            new SqlDataAdapter(sqlCommand).Fill(dataTable);
		//        }

		//        if (dataTable.Rows.Count > 0)
		//        {
		//            return toList(dataTable);
		//        }
		//        else
		//        {
		//            return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity>();
		//        }
		//    }
		//    public static List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> Get(List<int> ids)
		//    {
		//        if(ids != null && ids.Count > 0)
		//        {
		//            int maxQueryNumber = Settings.MAX_BATCH_SIZE ; 
		//            List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> results = null;
		//            if(ids.Count <= maxQueryNumber)
		//            {
		//                results = get(ids);
		//            }else
		//            {
		//                int batchNumber = ids.Count / maxQueryNumber;
		//                results = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity>();
		//                for(int i=0; i<batchNumber; i++)
		//                {
		//                    results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
		//                }
		//                results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count-batchNumber * maxQueryNumber)));
		//            }
		//            return results;
		//        }
		//        return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity>();
		//    }
		//    private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> get(List<int> ids)
		//    {
		//        if(ids != null && ids.Count > 0)
		//        {
		//            var dataTable = new DataTable();
		//            using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//            {
		//                sqlConnection.Open();
		//                var sqlCommand = new SqlCommand();
		//                sqlCommand.Connection = sqlConnection;

		//                string queryIds = string.Empty;
		//                for(int i=0; i<ids.Count; i++)
		//                {
		//                    queryIds += "@Id" + i + ",";
		//                    sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
		//                }
		//                queryIds = queryIds.TrimEnd(',');

		//                sqlCommand.CommandText = "SELECT * FROM [Budget_departement] WHERE [ID] IN ("+ queryIds +")";                    
		//            new SqlDataAdapter(sqlCommand).Fill(dataTable);
		//            }

		//            if (dataTable.Rows.Count > 0)
		//            {
		//                return toList(dataTable);
		//            }
		//            else
		//            {
		//                return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity>();
		//            }
		//        }
		//        return new List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity>();
		//    }

		//    public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity item)
		//    {
		//        int response = -1;
		//        using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//        {
		//            sqlConnection.Open();
		//            var sqlTransaction = sqlConnection.BeginTransaction();

		//            string query = "INSERT INTO [Budget_departement] ([Departement_name])  VALUES (@Departement_name)";

		//            using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
		//{

		//	sqlCommand.Parameters.AddWithValue("Departement_name",item.Departement_name == null ? (object)DBNull.Value  : item.Departement_name);

		//                sqlCommand.ExecuteNonQuery();
		//            }

		//            using (var sqlCommand = new SqlCommand("SELECT [ID] FROM [Budget_departement] WHERE [ID] = @@IDENTITY", sqlConnection, sqlTransaction))
		//            {
		//                response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
		//            }

		//            sqlTransaction.Commit();

		//            return response;
		//        }
		//    }
		//    public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> items)
		//    {
		//        if (items != null && items.Count > 0)
		//        {
		//            int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
		//            int results=0;
		//            if(items.Count <= maxParamsNumber)
		//            {
		//                results = insert(items);
		//            }else
		//            {
		//                int batchNumber = items.Count / maxParamsNumber;
		//                for(int i = 0; i < batchNumber; i++)
		//                {
		//                    results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
		//                }
		//                results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
		//            }
		//            return results;
		//        }

		//        return -1;
		//    }
		//    private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> items)
		//    {
		//        if (items != null && items.Count > 0)
		//        {
		//            int results = -1;
		//            using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//            {
		//                sqlConnection.Open();
		//                string query = "";
		//                var sqlCommand = new SqlCommand(query, sqlConnection);

		//                int i = 0;
		//                foreach (var item in items)
		//                {
		//                    i++;
		//                    query += " INSERT INTO [Budget_departement] ([Departement_name]) VALUES ( "

		//			+ "@Departement_name"+ i 
		//                        + "); ";


		//			sqlCommand.Parameters.AddWithValue("Departement_name" + i, item.Departement_name == null ? (object)DBNull.Value  : item.Departement_name);
		//                }

		//                sqlCommand.CommandText = query;

		//                results = sqlCommand.ExecuteNonQuery();
		//            }

		//            return results;
		//        }

		//        return -1;
		//    }

		//    public static int Update(Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity item)
		//    {   
		//        int results = -1;
		//        using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//        {
		//            sqlConnection.Open();
		//            string query = "UPDATE [Budget_departement] SET [Departement_name]=@Departement_name WHERE [ID]=@ID";
		//            var sqlCommand = new SqlCommand(query, sqlConnection);

		//            sqlCommand.Parameters.AddWithValue("ID", item.ID);
		//sqlCommand.Parameters.AddWithValue("Departement_name",item.Departement_name == null ? (object)DBNull.Value  : item.Departement_name);

		//            results = sqlCommand.ExecuteNonQuery();
		//        }

		//        return results;
		//    }
		//    public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> items)
		//    {
		//        if (items != null && items.Count > 0)
		//        {
		//            int maxParamsNumber = Settings.MAX_BATCH_SIZE / 2; // Nb params per query
		//            int results = 0;
		//            if(items.Count <= maxParamsNumber)
		//            {
		//                results = update(items);
		//            }else
		//            {
		//                int batchNumber = items.Count / maxParamsNumber;
		//                for(int i = 0; i < batchNumber; i++)
		//                {
		//                    results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
		//                }
		//                results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
		//            }

		//            return results;
		//        }

		//        return -1;
		//    }
		//    private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> items)
		//    {
		//        if (items != null && items.Count > 0)
		//        {
		//            int results = -1;
		//            using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//            {
		//                sqlConnection.Open();
		//                string query = "";
		//                var sqlCommand = new SqlCommand(query, sqlConnection);

		//                int i = 0;
		//                foreach (var item in items)
		//                {
		//                    i++;
		//                    query += " UPDATE [Budget_departement] SET "

		//			+ "[Departement_name]=@Departement_name"+ i +" WHERE [ID]=@ID" + i 
		//                        + "; ";

		//                        sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
		//			sqlCommand.Parameters.AddWithValue("Departement_name" + i, item.Departement_name == null ? (object)DBNull.Value  : item.Departement_name);
		//                }

		//                sqlCommand.CommandText = query;

		//                results = sqlCommand.ExecuteNonQuery();
		//            }

		//            return results;
		//        }

		//        return -1;
		//    }

		//    public static int Delete(int id)
		//    {
		//        int results = -1;
		//        using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//        {
		//            sqlConnection.Open();
		//            string query = "DELETE FROM [Budget_departement] WHERE [ID]=@ID";
		//            var sqlCommand = new SqlCommand(query, sqlConnection);
		//            sqlCommand.Parameters.AddWithValue("ID", id);

		//            results = sqlCommand.ExecuteNonQuery();
		//        }

		//        return results;
		//    }
		//    public static int Delete(List<int> ids)
		//    {
		//        if(ids != null && ids.Count > 0)
		//        {
		//            int maxParamsNumber = Settings.MAX_BATCH_SIZE; 
		//            int results=0;
		//            if(ids.Count <= maxParamsNumber)
		//            {
		//                results = delete(ids);
		//            } else
		//            {
		//                int batchNumber = ids.Count / maxParamsNumber;
		//                for(int i = 0; i < batchNumber; i++)
		//                {
		//                    results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
		//                }
		//                results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
		//            }
		//        }
		//        return -1;
		//    }
		//    private static int delete(List<int> ids)
		//    {
		//        if(ids != null && ids.Count > 0)
		//        {
		//            int results = -1;
		//            using(var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//            {
		//                sqlConnection.Open();
		//                var sqlCommand = new SqlCommand();
		//                sqlCommand.Connection = sqlConnection;

		//                string queryIds = string.Empty;
		//                for(int i=0; i<ids.Count; i++)
		//                {
		//                    queryIds += "@Id" + i + ",";
		//                    sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
		//                }
		//                queryIds = queryIds.TrimEnd(',');

		//                string query = "DELETE FROM [Budget_departement] WHERE [ID] IN ("+ queryIds +")";                    
		//                sqlCommand.CommandText = query;

		//                results = sqlCommand.ExecuteNonQuery();
		//            }

		//            return results;
		//        }
		//        return -1;
		//    }
		//    #endregion

		//    #region Custom Methods

		//    public static Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity GetByName(string departmentName)
		//    {
		//        var dataTable = new DataTable();
		//        using (var sqlConnection = new SqlConnection(Settings.ConnectionStringBudget))
		//        {
		//            sqlConnection.Open();
		//            string query = "SELECT * FROM [Budget_departement] WHERE [Departement_name]=@departmentName";
		//            var sqlCommand = new SqlCommand(query, sqlConnection);
		//            sqlCommand.Parameters.AddWithValue("departmentName", departmentName);

		//            new SqlDataAdapter(sqlCommand).Fill(dataTable);

		//        }

		//        if (dataTable.Rows.Count > 0)
		//        {
		//            return new Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity(dataTable.Rows[0]);
		//        }
		//        else
		//        {
		//            return null;
		//        }
		//    }

		//    #endregion

		//    #region Helpers

		//    private static List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity> toList(DataTable dataTable)
		//    {
		//        var list = new List<Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity>(dataTable.Rows.Count);
		//        foreach (DataRow dataRow in dataTable.Rows)
		//        { list.Add(new Infrastructure.Data.Entities.Tables.FNC.Budget_departementEntity(dataRow)); }
		//        return list;
		//    }
		//    #endregion
	}
}

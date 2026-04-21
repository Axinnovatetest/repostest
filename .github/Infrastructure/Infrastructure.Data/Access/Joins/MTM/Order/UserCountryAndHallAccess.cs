using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Joins.MTM.Order
{
	public class UserCountryAndHallAccess
	{
		public static List<Infrastructure.Data.Entities.Joins.MTM.Order.GetUserHallAndCountryEntity> GetUserHallAndCountry(int UserId)
		{

			if(UserId <= 0)
			{
				return new List<Entities.Joins.MTM.Order.GetUserHallAndCountryEntity>();
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query =
					@$"select H.Id,H.Country_Id,H.Name HallName,C.Name CountryName from Hall H 
						JOIN   User_Country UC ON UC.Country_Id = H.Country_Id  
						JOIN   Countries C ON C.Id = H.Country_Id where 
						User_Id = {UserId} AND ISNULL(H.Is_Archived,0) <> 1";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Joins.MTM.Order.GetUserHallAndCountryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Joins.MTM.Order.GetUserHallAndCountryEntity>();
			}
		}
	}
}

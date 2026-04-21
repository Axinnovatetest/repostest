using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Services.Utils
{
	public class TransactionsManager
	{
		public enum Database
		{
			Default = 0,
			FNC = 1,
			MTM = 2,
			EDI = 3
		}
		public SqlTransaction transaction { get; set; }
		public SqlConnection connection { get; set; }

		public TransactionsManager(Database database = Database.Default)
		{
			switch(database)
			{
				case Database.FNC:
					connection = new SqlConnection(Data.Access.Settings.ConnectionString_FNC);
					break;
				case Database.MTM:
					connection = new SqlConnection(Data.Access.Settings.ConnectionStringMTM);
					break;
				case Database.EDI: // EdiPlatformCnxEBAS
					connection = new SqlConnection(Data.Access.Settings.ConnectionStringEdiPlatformCnxEBAS);
					break;
				case Database.Default:
				default:
					connection = new SqlConnection(Data.Access.Settings.ConnectionString);
					break;
			}
		}
		public void beginTransaction()
		{
			connection.Close();
			connection.Open();
			transaction = connection.BeginTransaction();
		}
		public bool commit()
		{
			try
			{
				transaction.Commit();
				connection.Close();
				return true;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return rollback();
			}
		}
		public bool rollback()
		{
			try
			{
				transaction.Rollback();
				connection.Close();
				return true;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				connection.Close();
				return false;
			}
		}
	}

	public class Transaction: IDbTransaction
	{
		public IDbConnection Connection => throw new NotImplementedException();

		public IsolationLevel IsolationLevel => throw new NotImplementedException();

		public void Commit()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void Rollback()
		{
			throw new NotImplementedException();
		}
	}
}

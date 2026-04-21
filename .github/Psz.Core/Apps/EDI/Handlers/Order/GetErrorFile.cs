using System;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static byte[] GetErrorFile(int id)
		{
			try
			{
				var errorDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.Get(id);

				return errorDb != null
					? System.IO.File.ReadAllBytes(errorDb.FileName)
					: null;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Enums
{
	public enum RequestStatus
	{
		open, onhold, ongoing, canceled, closed, error
	}
	public class OfferRequestStatusManager
	{
		public static string GetRequestStatus(RequestStatus rs)
		{
			return rs switch
			{
				RequestStatus.open => "open",
				RequestStatus.onhold => "onhold",
				RequestStatus.ongoing => "ongoing",
				RequestStatus.canceled => "canceled",
				RequestStatus.closed => "closed",
				_ => "error",
			};
		}
	}
}

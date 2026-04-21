using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Enums
{
	public class BomChangeEnums
	{
		public enum Status : int
		{
			[Description("Open")]
			Open = 1,
			[Description("Accepted")]
			Accepted = 2,
			[Description("Rejected")]
			Rejected = 3,
			[Description("Closed")]
			Closed = 4,
			
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ROH
{
	public class RohDescritionBuilderModel
	{
		public int Order { get; set; }
		public string Value { get; set; }
		public string Seperator { get; set; }
		public string Prefix { get; set; }
		public string Suffix { get; set; }
		public RohDescritionBuilderModel()
		{
			
		}

		public RohDescritionBuilderModel(int order, string value, string seperator, string prefix, string suffix)
		{
			Order = order;
			Value = value;
			Seperator = seperator;
			Prefix = prefix;
			Suffix = suffix;
		}
	}
}

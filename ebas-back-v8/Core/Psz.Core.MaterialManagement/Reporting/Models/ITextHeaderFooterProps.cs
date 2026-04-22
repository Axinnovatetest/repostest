using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Reporting.Models
{
	public class ITextHeaderFooterProps
	{
		public bool HasHeader { get; set; }
		public bool HasFooter { get; set; }
		public bool HeaderLogoWithText { get; set; }
		public bool HeaderLogoWithCounter { get; set; }
		public bool HeaderFirstPageOnly { get; set; }
		public string FooterLeftText { get; set; }
		public string FooterCenterText { get; set; }
		public bool FooterWithCounter { get; set; }
		public string DocumentTitle { get; set; }
		public string Logo { get; set; }
		public string HeaderText { get; set; }
		public string BodyTemplate { get; set; }
		public string FooterTemplate { get; set; }
		public object BodyData { get; set; }
		public object FooterData { get; set; }
		public bool Rotate { get; set; }
	}
}

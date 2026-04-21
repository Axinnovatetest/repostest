using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.FileServer
{
	public class MinioParamtersModel
	{
		public string Minioaccesskey { get; set; }
		public string Miniosecretkey { get; set; }
		public string Minioendpoint { get; set; }
		public string Miniobucket { get; set; }

	}
}

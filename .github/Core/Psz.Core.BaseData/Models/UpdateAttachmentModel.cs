using System;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models
{
	public class UpdateAttachmentModel
	{
		public int Id { get; set; }
		public int AttachmentType { get; set; }
		public byte[] AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }

	}

}

using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.BillOfMaterial
{
	public class ImportFileTemplateModel
	{
		public DateTime? CreationTime { get; set; }
		public byte[] FileData { get; set; }
		public string FileName { get; set; }
		public string FileExtension { get; set; }
	}
	public class ImportRequestModel
	{
		public List<BomPosition> Data { get; set; }

		// - 2022-0713 - Partial Validation - 2022-07-25 - deprecated
		public bool UpgradeBomVersion { get; set; } = false;
	}
	public class ImportPositionXLSRequestModel
	{

		public int ArticleId { get; set; }
		public string AttachmentFilePath { get; set; }
		public bool Overwrite { get; set; }

		// - 2022-07-13
		public bool UpgradeBomVersion { get; set; } = false;
	}
}

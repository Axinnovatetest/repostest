using Minio.DataModel.Response;

namespace Infrastructure.Services.FileServer
{
	public class FileServerResponseModel
	{
		public bool success { get; set; }
		public PutObjectResponse response { get; set; }
	}

}

using System.Linq;

namespace WebService.Helper
{
    public class MultipartHelper
    {
        public static string GetBoundary(string contentType)
        {
            string boundaryDelimiter = "boundary=";
            return contentType.Split(boundaryDelimiter).Last();
        }

        public static bool IsFileData(string contentDisposition)
        {
            return contentDisposition.Contains("filename=");
        }
    }
}

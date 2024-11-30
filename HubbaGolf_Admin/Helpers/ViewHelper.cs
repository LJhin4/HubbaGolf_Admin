namespace HubbaGolfAdmin.Helpers
{
    public static class ViewHelper
    {
        //-------------------------------------------------------------------------support method
        public static string UploadFile(IFormFile file, string uploadPath)
        {
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), uploadPath);
            var filePath = Path.Combine(directoryPath, fileName);

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            //create a link
            string[] parts = uploadPath.Split('/');
            string result = "/" + string.Join("/", parts[1..]) + "/" + fileName;
            return result;
        }

        /// <summary>
        /// physically delete files
        /// </summary>
        /// <param name="webPathWithName"></param>
        /// <returns></returns>
        public static bool DeleteFile(string webPathWithName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + webPathWithName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}

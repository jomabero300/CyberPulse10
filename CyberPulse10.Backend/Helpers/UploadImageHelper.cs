namespace CyberPulse10.Backend.Helpers;

public class UploadImageHelper
{
    public static async Task<string> UploadImageAsync(string image, int id, string webRootPath, string? imageOld = null)
    {
        try
        {

            const string folder = "Images/Countries";

            string DirectoryPath = Path.Combine(webRootPath, folder);

            Directory.CreateDirectory(DirectoryPath);

            string fileName = $"{Guid.CreateVersion7():N}.jpg";

            var fullPath = Path.Combine(DirectoryPath, fileName);

            var imageBytes = Convert.FromBase64String(image);

            await File.WriteAllBytesAsync(fullPath, imageBytes);


            if (id != 0 && !string.IsNullOrWhiteSpace(imageOld))
            {
                var oldImagePath = Path.Combine(webRootPath, imageOld.TrimStart('/', '\\'));

                if (File.Exists(oldImagePath))
                    File.Delete(oldImagePath);
            }

            return Path.Combine(folder, fileName).Replace("\\", "/");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            throw new Exception("Failed to upload image.", ex);
        }
    }
}

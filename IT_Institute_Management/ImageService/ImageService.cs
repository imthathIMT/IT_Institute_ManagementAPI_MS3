using Microsoft.AspNetCore.Hosting;

namespace IT_Institute_Management.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


       
        public async Task<string> SaveImage(IFormFile imageFile, string folderName)
        {
            var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", folderName);
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsDirectory, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/{folderName}/{fileName}";
        }

       
        public void DeleteImage(string imagePath)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}

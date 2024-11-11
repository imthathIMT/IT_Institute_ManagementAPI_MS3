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


        // Method to save student image to the file system and return the file path
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images/students");
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsDirectory, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return filePath;
        }

        // Method to delete the student's image from the file system
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

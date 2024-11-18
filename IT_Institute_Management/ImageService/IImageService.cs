namespace IT_Institute_Management.ImageService
{
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile imageFile, string folderName);
        void DeleteImage(string imagePath);
    }
}

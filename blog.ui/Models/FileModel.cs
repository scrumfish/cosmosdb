namespace blog.ui.Models
{
    public record FileModel
    {
        public string? Filename { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}

namespace blog.ui.Models
{
    public record UploadResponse
    {
        public UploadLink data { get; init; } = new UploadLink();
    }
}

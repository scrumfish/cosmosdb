namespace blog.objects
{
    public class BlogWithImages : Blog
    {
        public IList<string>? images { get; set; }
    }
}
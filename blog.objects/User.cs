namespace blog.objects
{
    public class User
    {
        public string? id {get; set;}
        public string? email {get; set;} 
        public string? password {get; set;}
        public string[]? roles {get; set;}
    }
}
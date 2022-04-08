using blog.objects;
using blog.objects.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace blog.data
{
    public class UserData : IUserData
    {
        private IConfiguration configuration;
        private string databaseId;
        private string key;
        private string uri;
        private readonly string collection = "users";

        public UserData(IConfiguration config)
        {
            configuration = config;
            databaseId = configuration.GetSection("Database").GetSection("DatabaseId").Value;
            key = configuration.GetSection("Database").GetSection("Key").Value;
            uri = configuration.GetSection("Database").GetSection("Endpoint").Value;
        }

        public objects.User? GetUser(string email)
        {
            using (var client = new CosmosClient(uri, key))
            {
                var container = client.GetContainer(databaseId, collection);
                var query = new QueryDefinition("select * from users u where u.email = @email")
                    .WithParameter("@email", email);
                using (var feed = container.GetItemQueryIterator<objects.User>(query))
                {
                    while (feed.HasMoreResults)
                    {
                        return feed.ReadNextAsync().Result?.FirstOrDefault();
                    }
                }
            }
            return null;
        }
    }
}
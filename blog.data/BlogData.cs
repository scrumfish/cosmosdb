using blog.data.Extensions;
using blog.data.Services;
using blog.objects;
using blog.objects.Extensions;
using blog.objects.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.data
{
    public class BlogData : IBlogData
    {
        private IConfiguration configuration;
        private string databaseId;
        private string key;
        private string uri;
        private readonly string collection = "blogs";
        private IImageData imageData;

        public BlogData(IConfiguration config, IImageData imageData)
        {
            configuration = config;
            databaseId = configuration.GetSection("Database").GetSection("DatabaseId").Value;
            key = configuration.GetSection("Database").GetSection("Key").Value;
            uri = configuration.GetSection("Database").GetSection("Endpoint").Value;
            //using (var client = new CosmosClient(uri, key))
            //{
            //    client.CreateDatabaseIfNotExistsAsync(databaseId).Wait();
            //}
            this.imageData = imageData;
        }

        public string Add(BlogEntry entry)
        {
            var blog = entry.ToBlogWithImages();
            blog.fragment = entry.article.ToFragment();
            blog.publishedAt = DateTime.UtcNow;
            blog.id = ShortId.NewId();
            using (var client = new CosmosClient(uri, key))
            {
                var container = client.GetContainer(databaseId, collection);
                //container.Database.CreateContainerIfNotExistsAsync(collection, "/id").Wait();
                container.CreateItemAsync(blog, new PartitionKey(blog.id)).Wait();
            }
            return blog.id;
        }

        public IList<Title> GetTitles()
        {
            using (var client = new CosmosClient(uri, key))
            {
                var container = client.GetContainer(databaseId, collection);
                var response = new List<Title>();

                var query = new QueryDefinition("select c.id, c.title, c.publishedAt, c.fragment from c");
                using (var feed = container.GetItemQueryIterator<Title>(query))
                {
                    while (feed.HasMoreResults)
                    {
                        var result = feed.ReadNextAsync().Result;
                        response.AddRange(result.ToList());
                    }
                }
                return response;
            }

        }

        public void UpdateEntry(Blog entry)
        {
            using (var client = new CosmosClient(uri, key))
            {
                var container = client.GetContainer(databaseId, collection);
                container.UpsertItemAsync(entry, new PartitionKey(entry.id)).Wait();
            }
        }

        public Blog? Get(string id)
        {
            using (var client = new CosmosClient(uri, key))
            {
                var container = client.GetContainer(databaseId, collection);
                if (id == "latest")
                {
                    var query = new QueryDefinition("select top 1 * from c order by c.publishedAt desc");
                    using (var feed = container.GetItemQueryIterator<Blog>(query))
                    {
                        while (feed.HasMoreResults)
                        {
                            return feed.ReadNextAsync().Result?.FirstOrDefault();
                        }
                    }
                }
                else
                {
                    var response = container.ReadItemAsync<Blog>(id, new PartitionKey(id)).Result;
                    return response;
                }
            }
            return null;
        }

        public void Delete(string id)
        {
            using (var client = new CosmosClient(uri, key))
            {
                var container = client.GetContainer(databaseId, collection);
                var blog = container.ReadItemAsync<BlogWithImages>(id, new PartitionKey(id)).Result?.Resource;
                if (blog != null && blog.images != null)
                {
                    foreach(var image in blog.images)
                    {
                        if (!IsInUse(image, id, container))
                        {
                            imageData.Delete(image);
                        }
                    }
                }
                container.DeleteItemAsync<BlogWithImages>(id, new PartitionKey(id)).Wait();
            }
        }

        private bool IsInUse(string fileName, string id, Container container)
        {
            var query = new QueryDefinition("select value count(1) from c " +
                "where c.id <> @id and array_contains(c.images, @fileName)"
                )
                .WithParameter("@id", id)
                .WithParameter("@fileName", fileName);
            using (var feed = container.GetItemQueryIterator<int>(query))
            {
                while (feed.HasMoreResults)
                {
                    return feed.ReadNextAsync().Result?.FirstOrDefault() > 0;
                }
            }
            return false;
        }
    }
}
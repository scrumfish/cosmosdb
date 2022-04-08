using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using blog.objects.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.data
{
    public class ImageData : IImageData
    {
        private IConfiguration configuration;
        private BlobServiceClient serviceClient;
        private BlobContainerClient containerClient;
        private string cdnUri;

        public ImageData(IConfiguration config)
        {
            configuration = config;
            var conn = config.GetConnectionString("Imagesblob");
            serviceClient = new BlobServiceClient(conn);
            var name = configuration.GetSection("ImageStorageName").Value;
            containerClient = serviceClient.GetBlobContainerClient(name);
            cdnUri = configuration.GetSection("CdnUri").Value;
        }

        public void Delete(string filename)
        {
            var client = containerClient.GetBlobClient(filename);
            client.DeleteIfExists();
        }

        public string Save(string filename, Stream stream)
        {
            var ext = Path.GetExtension(filename).Replace(".", string.Empty);
            var client = containerClient.GetBlobClient(filename);
            client.Upload(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = $"image/{ext}"
                }
            });
            return $"{cdnUri}/{filename}";
        }
    }
}

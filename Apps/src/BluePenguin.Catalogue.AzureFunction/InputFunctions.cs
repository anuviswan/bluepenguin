using BluePenguin.Catalogue.AzureFunction.DTO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BluePenguin.Catalogue.AzureFunction
{
    public static class InputFunctions
    {
        [FunctionName("SayHello")]
        public static async Task<HttpResponseMessage> SayHello([HttpTrigger(AuthorizationLevel.Function, "get",  Route = null)] HttpRequestMessage req)
        {
            return req.CreateResponse(HttpStatusCode.OK, "Hello Custom !! This is Blue Penguin");
        }

        [FunctionName("UploadProducts")]
        public static async Task<HttpResponseMessage> UploadProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            [Blob("productlist")] CloudBlobContainer blobContainer,
            TraceWriter log)
        {

            MultipartMemoryStreamProvider stream;
            try
            {
                var multipartTask = req.Content.ReadAsMultipartAsync();
                if (!multipartTask.IsCompleted)
                {
                    multipartTask.RunSynchronously();
                }
                stream = multipartTask.Result;
            }
            catch (Exception)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Posted data was not multipart or read failed");
            }

            string fileContent;
            try
            {
                var httpContent = stream.Contents[0];
                var contentTask = httpContent.ReadAsStringAsync();
                if (!contentTask.IsCompleted)
                {
                    contentTask.RunSynchronously();
                }
                fileContent = contentTask.Result;
            }
            catch (Exception)
            {
                return req.CreateResponse(HttpStatusCode.InternalServerError, "Unable to read file stream");
            }

            await blobContainer.CreateIfNotExistsAsync();

            var blob = blobContainer.GetBlockBlobReference($"productlist.xml");
            await blob.UploadTextAsync(fileContent);
            return req.CreateResponse(HttpStatusCode.Created, "Product List Uploaded"); 
        }


        [FunctionName("GetAllProducts")]
        public static async Task<IEnumerable<Product>> GetAllProducts([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            [Blob("productlist")] CloudBlobContainer blobContainer,
            TraceWriter log)
        {
            try
            {
                await blobContainer.CreateIfNotExistsAsync();
                var blob = blobContainer.GetBlockBlobReference($"productlist.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(Products));
                var products = new Products()
                {
                    Product = Enumerable.Empty<Product>().ToList()
                };

                using (var stream = await blob.OpenReadAsync())
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        products = (Products)serializer.Deserialize(reader);
                    }
                        
                }

                return products.Product;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

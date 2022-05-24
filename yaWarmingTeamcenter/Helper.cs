using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TestRestClientTC
{
    public static class Helper
    {
        public static async Task<bool> ysStorageSerializingClass(object target, string yaAccessKeyId, string yaSecretAccessKey, string yaBucketName, string fileName)
        {
            MemoryStream memStream4Cookie = new MemoryStream();
            var binanryCookieForm = new BinaryFormatter();
            binanryCookieForm.Serialize(memStream4Cookie, target);
            //

            AmazonS3Config S3Config = new AmazonS3Config
            {
                ServiceURL = "http://s3.yandexcloud.net"
            };
            AmazonS3Client client = new AmazonS3Client(yaAccessKeyId, yaSecretAccessKey, S3Config);

            try
            {
                PutObjectRequest creatingObject = new PutObjectRequest();
                creatingObject.BucketName = yaBucketName;
                creatingObject.Key = fileName;
                creatingObject.InputStream = memStream4Cookie;
                PutObjectResponse response = await client.PutObjectAsync(creatingObject);
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }

        }

        public static async Task<object> ysStorageDeserializingClass(string yaAccessKeyId, string yaSecretAccessKey, string yaBucketName, string fileName)
        {

            try
            {
                AmazonS3Config S3Config = new AmazonS3Config
                {
                    ServiceURL = "http://s3.yandexcloud.net"
                };
                AmazonS3Client client = new AmazonS3Client(yaAccessKeyId, yaSecretAccessKey, S3Config);

                GetObjectRequest readingObject = new GetObjectRequest();
                readingObject.BucketName = yaBucketName;
                readingObject.Key = fileName;

                GetObjectResponse response = await client.GetObjectAsync(readingObject);
                Stream responseStream = response.ResponseStream;

                return new BinaryFormatter().Deserialize(responseStream);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }


        }
    }
}

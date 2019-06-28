using CarApiClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarApiClient.Services
{
    public class AutoService
    {
        public const string API_URL = "http://localhost:49890/api/autos";

        public async Task<Auto> CreateAuto(Auto auto)
        {
            var request = WebRequest.Create(API_URL) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";

            var sendStream = await request.GetRequestStreamAsync();
            string json = JsonConvert.SerializeObject(auto);

            byte[] data = Encoding.UTF8.GetBytes(json);
            await sendStream.WriteAsync(data, 0, data.Length);

            var response = await request.GetResponseAsync() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.Created)
            {
                string result = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

                return JsonConvert.DeserializeObject<Auto>(result);
            }

            return null;
        }


        public async Task<List<Auto>> GetAll()
        {
            var request = WebRequest.Create(API_URL) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "GET";

            var response = await request.GetResponseAsync() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string result = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<Auto>>(result);
            }

            return null;
        }

        public async Task<bool> Delete(int id)
        {
            var request = WebRequest.Create($"{API_URL}/{id}") as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "DELETE";

            var response = await request.GetResponseAsync() as HttpWebResponse;

            return response.StatusCode == HttpStatusCode.OK;
        }


        public async Task<bool> UpdateCarAsync(Auto auto)
        {
            var request = WebRequest.Create($"{API_URL}/{auto.Id}") as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "PUT";

            var sendStream = await request.GetRequestStreamAsync();
            string json = JsonConvert.SerializeObject(auto);

            byte[] data = Encoding.UTF8.GetBytes(json);
            await sendStream.WriteAsync(data, 0, data.Length);

            var response = await request.GetResponseAsync() as HttpWebResponse;

            return response.StatusCode == HttpStatusCode.NoContent;
        }

    }
}

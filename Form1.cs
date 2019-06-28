using CarApiClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarApiClient
{
    public partial class Form1 : Form
    {
        public const string API_URL = "http://localhost:49890/api/autos";

        public Form1()
        {
            InitializeComponent();

            var request = WebRequest.Create(API_URL) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";

            var sendStream = request.GetRequestStream();

            var auto = new Auto { Name = "Mercedes", Price = 15555, Color = "Gray" };
            string json = JsonConvert.SerializeObject(auto);

            byte[]data = Encoding.UTF8.GetBytes(json);
            sendStream.Write(data, 0, data.Length);

            var response = request.GetResponse() as HttpWebResponse;

            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Auto createdAuto = JsonConvert.DeserializeObject<Auto>(result);

            MessageBox.Show($"Auto id = {createdAuto.Id}");
        }
    }
}

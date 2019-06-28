using CarApiClient.Models;
using CarApiClient.Services;
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
        private AutoService autoService { get; set; }

        public Form1()
        {
            InitializeComponent();
            autoService = new AutoService();
        }

        private async void ButtonCreate_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            string color = textBoxColor.Text;

            if (name == "" || color == "")
            {
                MessageBox.Show("Fill data");
                return;
            }


           Auto auto = await autoService.CreateAuto(new Auto { Name = name, Price = price, Color = color });

            if (auto != null)
            {
                AddAutoToList(auto);
            }
        }

        void AddAutoToList(Auto auto)
        {
            var item = new ListViewItem(auto.Id.ToString());

            item.SubItems.Add(auto.Name);
            item.SubItems.Add(auto.Price.ToString());
            item.SubItems.Add(auto.Color);

            listViewAutos.Items.Add(item);
        }
    }
}

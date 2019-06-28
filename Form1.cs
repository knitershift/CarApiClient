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

        private List<Auto> autos;

        private int currentSelectedId = 0;
        ListViewItem selected;

        public Form1()
        {
            InitializeComponent();
            autoService = new AutoService();

            Task.Run(async() =>
            {
                autos = await autoService.GetAll();
                await Task.Delay(1000);
                this.Invoke(new MethodInvoker(() => pictureBoxLoading.Hide()));
                autos.ForEach(a => AddAutoToList(a));
            });
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

            this.Invoke(new MethodInvoker(() => listViewAutos.Items.Add(item) ));
        }

        private async void ButtonDelete_Click(object sender, EventArgs e)
        {

            if (listViewAutos.SelectedItems.Count > 0)
            {
                ListViewItem selected = listViewAutos.SelectedItems[0];
                int autoId = Convert.ToInt32(selected.Text);

                bool result = await autoService.Delete(autoId);

                if (result == true)
                {
                    listViewAutos.Items.Remove(selected);
                    autos.RemoveAll(a => a.Id == autoId);
                }
                    
            }

        }

        private void ListViewAutos_ItemActivate(object sender, EventArgs e)
        {
        }

        private void ListViewAutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAutos.SelectedItems.Count > 0)
            {
                selected = listViewAutos.SelectedItems[0];
                int autoId = Convert.ToInt32(selected.Text);
                currentSelectedId = autoId;

                Auto auto = autos.Find(a => a.Id == autoId);

                textBoxName.Text = auto.Name;
                textBoxPrice.Text = auto.Price.ToString();
                textBoxColor.Text = auto.Color;
            }
                
        }

        private async void ButtonUpdate_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            string color = textBoxColor.Text;

            if (name == "" || color == "")
            {
                MessageBox.Show("Fill data");
                return;
            }

            var auto = new Auto { Id = currentSelectedId, Name = name, Price = price, Color = color };
            bool result = await autoService.UpdateCarAsync(auto);

            if (result)
            {
                selected.SubItems[1].Text = auto.Name;
                selected.SubItems[2].Text = auto.Price.ToString();
                selected.SubItems[3].Text = auto.Color;

                listViewAutos.Refresh();
            }
        }
    }
}

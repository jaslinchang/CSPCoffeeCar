using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSPCoffee
{
    public partial class Form1 : Form
    {
      
        public Form1()
        {
            InitializeComponent();
            //MyInitializer();
            
            LoadCarNum(memID);
        }
        int memID = 2;  //現在登入的會員編號

        private void LoadCarNum(int memID)
        {
            
        }

        NorthwindEntities db = new NorthwindEntities();
        internal void MyInitializer()
        {
            //var q = db.Products.Select(x => x.ProductName).ToArray();
            //var q1 = db.Categories.Select(x => x.CategoriesName).ToArray();

            AutoCompleteStringCollection strings = new AutoCompleteStringCollection();

            //strings.AddRange(q);
            //strings.AddRange(q1);

            txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSearch.AutoCompleteCustomSource = strings;
            txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            Car1 car1 = new Car1();
            car1.Show();
        }
    }


}

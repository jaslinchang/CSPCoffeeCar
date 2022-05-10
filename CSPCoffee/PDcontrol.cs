using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSPCoffee
{
    public partial class PDcontrol : UserControl
    {
        CoffeeEntities db = new CoffeeEntities();
        int PDid;
        public PDcontrol(int productID)
        {            
            InitializeComponent();
            PDid=productID;
            
            //產品名稱
            var name = db.Products.AsEnumerable().Where(p => p.ProductID== productID).ToArray();
            label1.Text = name[0].ProductName.ToString();
            //label1.Click += PDcontrol_Click;
            //產品價格
            string pric = $"{name[0].Price:c2}元";
            label2.Text = pric;
            //label2.Click += PDcontrol_Click;
            //產品類別
            var Catergory = db.Products.AsEnumerable().Where(p => p.ProductID == productID).Select(p => p.Category.CategoriesName).ToArray();
            string ctg = $"{Catergory[0]}";
            label3.Text = ctg;
            ///*label3.Click += PDcontrol_Click*/;
            ////產品圖片
            //var photo = db.PhotoDetails.AsEnumerable().Where(p => p.ProductID == productID).Select(p => new { p.ProductID, p.Photos.Photo }).ToList();
            //byte[] bytes = photo[0].Photo;
            //MemoryStream ms = new MemoryStream(bytes);
            //pictureBox1.Image = Image.FromStream(ms);
            ////pictureBox1.Click += PDcontrol_Click;
        }
        private void PDcontrol_Click(object sender, EventArgs e)
        {

            PDcontrol product = (PDcontrol)sender;

            Productdetail pd = new Productdetail(product.PDid);
            pd.Location = new Point(300, 300);
            pd.Show();


        }

    }
}

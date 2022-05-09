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
    public delegate void TheButtonClick2(CarControl2 source);
    public partial class CarControl2 : UserControl
    {
        CoffeeEntities db = new CoffeeEntities();
        public CarControl2(int CarID)
        {
            InitializeComponent();
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Size = Size;

            this.SizeChanged += CarControl2_SizeChanged;


         LoadlabelName(CarID);
            LoadlabelPrice(CarID);
            LoadlabelNumber(CarID);
            LoadlabelCount();
        }

        private void CarControl2_SizeChanged(object sender, EventArgs e)
        {
            tableLayoutPanel1.Size = Size;
        }

        private void LoadlabelName(int ID)
        {
            var q = from s in db.ShoppingCarDetails
                    where s.ShoppingCarDetialsID == ID
                    select s.Product.ProductName;
            foreach (var ss in q)
            {
                this.labelName.Text = ss.ToString();
            }
        }
        private void LoadlabelPrice(int ID)
        {
            var q = from s in db.ShoppingCarDetails
                    where s.ShoppingCarDetialsID == ID
                    select s.Product.Price;

            foreach (var ss in q)
            {
                this.labelPrice.Text = ss.ToString();
            }
        }
        private void LoadlabelNumber(int ID)
        {
            var q = from s in db.ShoppingCarDetails
                    where s.ShoppingCarDetialsID == ID
                    select s.Quantity;
            foreach (var ss in q)
            {
                this.labelNumber.Text = ss.ToString();
            }
        }
        private void LoadlabelCount()
        {
            int price = int.Parse(this.labelPrice.Text);
            int quantity = int.Parse(labelNumber.Text);
            this.labelCount.Text = $"{price * quantity}";
        }




        #region 發明label屬性            
        public string theTextOnlabelName
        {
            get { return labelName.Text; }
            set { labelName.Text = value; }
        }
        public string theTextOnlabelPrice
        {
            get { return labelPrice.Text; }
            set { labelPrice.Text = value; }
        }
        public string theTextOnlabelNumber
        {
            get { return labelNumber.Text; }
            set { labelNumber.Text = value; }
        }
        public string theTextOnlabelCount
        {
            get { return this.labelCount.Text = $"{ int.Parse(this.labelPrice.Text) * int.Parse(labelNumber.Text)}"; }
            set { labelCount.Text = value; }
        }
        #endregion

        //定義元件大小
        public override Size MinimumSize
        {
            get { return new Size(1138, 86); }
            set {  /*base.MinimumSize = value;*/  }
        }


    }
}

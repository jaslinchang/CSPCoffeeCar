﻿using System;
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
    public delegate void TheButtonClick(CarControl1 source);
    public partial class CarControl1 : UserControl
    {
        CoffeeEntities db = new CoffeeEntities();
        public event TheButtonClick theClick = null;
        public event TheButtonClick thecomoboxChanged = null;

        public CarControl1(int CarID)
        {
            InitializeComponent();

            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Size = Size;

            this.SizeChanged += CarControl1_SizeChanged;
            this.btnDelete.Click += BtnDelete_Click;
            
            LoadlabelName(CarID);
            LoadlabelPrice(CarID);
            LoadComobox(CarID);
            CatchcomboBox1(CarID);
            LoadlabelCount();
            LoadlabelStock(CarID);

        }

  

        #region  載入初始值
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
        private void LoadComobox(int ID)
        {
            var q = from s in db.ShoppingCarDetails
                    where s.ShoppingCarDetialsID == ID
                    select s.Product.Stock;
            foreach (int a in q)
            {
                for (int i = 1; i <= a &&i<=10; i++)
                {
                    comboBox1.Items.Add(i);
                }
            }           
        }
        private void CatchcomboBox1(int ID)
        {
            var q = from s in db.ShoppingCarDetails
                    where s.ShoppingCarDetialsID == ID
                    select s.Quantity;
            foreach (var ss in q)
            {
                comboBox1.SelectedIndex = (int)ss - 1;
            }
        }
        private void LoadlabelCount()
        {
            int price = int.Parse(this.labelPrice.Text);
            int quantity = int.Parse(comboBox1.Text);
            this.labelCount.Text = $"{price * quantity}";
        }
        private void LoadlabelStock(int ID)
        {
            var q = from s in db.ShoppingCarDetails
                    where s.ShoppingCarDetialsID == ID
                    select s.Product.Stock;
            foreach (var ss in q)
            {
                this.labelStock.Text = ss.ToString();
            }
        }

        #endregion

        # region 發明label屬性            

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
        public string theTextOnlabelCount
        {
            //get { return labelCount.Text; }
            get { return this.labelCount.Text = $"{ int.Parse(this.labelPrice.Text) * int.Parse(comboBox1.Text)}"; }
            set { labelCount.Text = value; }
        }
        public string theTextOnlabelmem
        {
            get { return labelCount.Text; }
            set { labelCount.Text = value; }
        }
        public string theTextOnlabelStock
        {
            get { return labelStock.Text; }
            set { labelStock.Text = value; }
        }
        public int theTextOnComboBox1
        {
            get { return comboBox1.SelectedIndex; }
            set { comboBox1.SelectedIndex = value; }
        }
        #endregion
       

        //定義元件大小
        public override Size MinimumSize
        {
            get {return new Size(1138, 86);}
            set {  /*base.MinimumSize = value;*/  }
        }

        //定義按鈕定義
        [Category("MSIT老師")]
        [Description("MSIT同學: the value for MSIT")]
        //public string theTextOnButton
        //{
        //    get { return btnDelete.Text; }
        //    set  { btnDelete.Text = value; }
        //}

        private void CarControl1_SizeChanged(object sender, EventArgs e)
        {
            tableLayoutPanel1.Size = Size;
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (theClick != null)
            {
                theClick(this);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (thecomoboxChanged != null)
            {
                thecomoboxChanged(this);
            }           
            this.labelCount.Text = $"{ int.Parse(this.labelPrice.Text) * int.Parse(comboBox1.Text)}";
        }

       
    }
}

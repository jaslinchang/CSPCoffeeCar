using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSPCoffee
{
    public partial class Car1 : Form
    {       
        public Car1()
        {
            InitializeComponent();
            LoadCar();
            LoadCouponToListbox();
            LoadAddproducts();

            //this.tabPage1.Parent = this.tabControl1;//顯示
            //this.tabPage2.Parent = null;//隱藏
            //this.tabPage3.Parent = null;//隱藏
        }       

        //TODO.購物車功能(付款方式、出貨方式、加購商品推薦、免運費計算、點數折抵消費金) 09
        CoffeeEntities db = new CoffeeEntities();
        int memID =4;  //現在登入的會員編號
        decimal DELE = 0;  //紀錄已刪除的價錢
        decimal Total1 = 0;
        CarControl1[] x;
        PDcontrol [] y;
        string Fee;  //運送方式
        string Pay;  //府款方式
        int PayID;  //選擇了哪個付款方式
        int CouponName;     //CouponName
        string CouponID="";  //CouponID
        int AddCarID1; //記下加入購物車的產品編號
        int AddCarID2;
        int AddCarID3;
        List<int> CarID;
        List<int> Coupon1ID = new List<int>();
        List<decimal> Coupon1Money = new List<decimal>();

        #region Page1
        //載入折價卷
        private void LoadCouponToListbox()
        {
            var q = from c in db.CouponDetails
                    where c.MemberID == memID
                    select new { c.CouponID,c.Coupon.CouponName, c.Coupon.Money };
                        
            foreach (var n in q)
            {           
                listBox1.Items.Add($" {n.CouponName} : 可折抵 {n.Money.ToString("#0.")} 元 ");
                Coupon1Money.Add(n.Money);
                Coupon1ID.Add(n.CouponID);
            }
            this.splitContainer2.Panel2Collapsed = true;           

        }
        //抓該會員的購物明細
        private void LoadCar()
        {
            this.flowLayoutPanel1.Controls.Clear();
            Total1 = 0;
            var q = from s in db.ShoppingCarDetails
                    where s.MemberID == memID
                    select new { s.ShoppingCarDetialsID, s.ProductsID };

            CarID = new List<int>();
            foreach (var n in q)
            {
                CarID.Add(n.ShoppingCarDetialsID);
            }

            x = new CarControl1[CarID.Count];

            for (int i = 0; i < CarID.Count; i++)
            {
                x[i] = new CarControl1(CarID[i]);
                this.flowLayoutPanel1.Controls.Add(x[i]);
                x[i].Size = new Size(1470, 100);
                x[i].Tag = CarID[i];
                Total1 += decimal.Parse(x[i].theTextOnlabelCount, System.Globalization.NumberStyles.Currency);
                this.labelfirstcount.Text = Total1.ToString("#0.");
                this.labelTotal1.Text = (Total1 - CouponName).ToString("#0.");
                x[i].theClick += X_theClick;
                x[i].thecomoboxChanged += X_thecomoboxChanged;
            }
        }     
        //刪除明細時
        private void X_theClick(CarControl1 source)
        {                 
            var product = (from p in db.ShoppingCarDetails
                           where p.ShoppingCarDetialsID == (int)source.Tag
                           select p).FirstOrDefault();
                                
            DialogResult = MessageBox.Show("確定刪除此筆訂單?", "刪除訂單", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                if (product == null) return;
                this.flowLayoutPanel1.Controls.Remove(source);                
                this.db.ShoppingCarDetails.Remove(product);
                DELE = DELE + decimal.Parse( source.theTextOnlabelCount, System.Globalization.NumberStyles.Currency);

                Total1 = Total1 - decimal.Parse(source.theTextOnlabelCount, System.Globalization.NumberStyles.Currency);
                this.labelfirstcount.Text = Total1.ToString("#0.");
                this.labelTotal1.Text = (int.Parse(labelfirstcount.Text) - CouponName).ToString("#0.");               

                MessageBox.Show("刪除成功");
            }
            else if (DialogResult == DialogResult.No)
            {
                MessageBox.Show("取消刪除");
            }           

        }
        //修改明細數量時
        private void X_thecomoboxChanged(CarControl1 source)
        {            
            Total1 = 0-DELE;
            foreach (var item in x) { Total1 += decimal.Parse(item.theTextOnlabelCount, System.Globalization.NumberStyles.Currency); }
            this.labelfirstcount.Text = Total1.ToString("#0.");
            this.labelTotal1.Text = (Total1 - CouponName).ToString("#0.");

            var product = (from p in db.ShoppingCarDetails
                           where p.ShoppingCarDetialsID == (int)source.Tag
                           select p).FirstOrDefault();
            if (product == null) return;
            product.Quantity = (source.theTextOnComboBox1 + 1);           
        }
        //收合折價卷
        private void button2_Click(object sender, EventArgs e)
        {
            this.splitContainer2.Panel2Collapsed = !this.splitContainer2.Panel2Collapsed;
        }    
        //使用折價卷
        private void button7_Click(object sender, EventArgs e)
        {
            int d = listBox1.SelectedIndex;
            CouponName = (int)Coupon1Money[d];
            CouponID = Coupon1ID[d].ToString();
            if (decimal.Parse(labelfirstcount.Text) < CouponName)
            {
                MessageBox.Show("不可使用，因折抵金額已小於結帳金額");
            }
            else
            {
                MessageBox.Show("使用成功!");
                this.labeldiscount.Text = $"-{CouponName}";
                this.labelTotal1.Text = (Total1 - CouponName).ToString("#0.");
            }
        }    
        //下一頁(到第二頁)
        private void button4_Click(object sender, EventArgs e)
        {
            //this.tabPage1.Parent = null;//隱藏
            //this.tabPage2.Parent = this.tabControl1;//顯示
            //this.tabPage3.Parent = null;//隱藏
            tabControl1.SelectedTab = tabControl1.TabPages[1];
      

            this.db.SaveChanges();    
            LoadMemberInformation();
            LoadFeeRadioBox();     
        }
        //推薦加購商品
        private void LoadAddproducts()
        {
            var q = from s in db.ShoppingCarDetails
                    where s.MemberID == memID
                    select new { s.Product.CategoryID, s.ProductsID };

            bool c1 = false;    //判斷他是咖啡類的
            bool c2 = false;    //判斷他是周邊類
            bool c3 = false;

            List<int> CatID = new List<int>();  //所有類別編號
            List<int> ProID = new List<int>();  //所有的產品編號
            List<int> CoffeeID = new List<int>();  //所有是咖啡的產品編號
            
            foreach (var n in q)
            {
                CatID.Add(n.CategoryID);
                ProID.Add((int)n.ProductsID);
            }

            for (int i = 0; i < CatID.Count; i++)   //只有買咖啡
            {
                if (CatID[i] == 1) 
                {
                    c1 = true;
                    CoffeeID.Add(ProID[i]);
                }   
                else if (CatID[i] == 2|| CatID[i] == 3) { c2 = true; }  //只有買周邊
            }
                        
            if (c1==true&&c2==false)     //只有買咖啡
            {
                //Random rng = new Random(Guid.NewGuid().GetHashCode());
                //var qr = db.Products.AsEnumerable().Where(p => p.CategoryID != 1 && hasExisted(p.ProductID, ProID)).OrderByDescending(x => rng.Next()).Select(p => p.ProductID).Take(1).ToList();
                //PDcontrol recommend1 = new PDcontrol(qr[0]);
                //flowLayoutPanel2.Controls.Add(recommend1);
                //recommend1.theClick += Recommend1_theClick;
                //foreach (int n in qr)
                //{
                //    AddCarID1 = int.Parse(n.ToString());
                //}
                //=======================================
                Random rng = new Random(Guid.NewGuid().GetHashCode());
                var qr = db.Products.AsEnumerable().Where(p => p.CategoryID != 1 && hasExisted(p.ProductID, ProID)).OrderByDescending(x => rng.Next()).Select(p => p.ProductID).Take(3).ToList();
                y = new PDcontrol[qr.Count];
                for (int i = 0; i < qr.Count; i++)
                {
                    y[i] = new PDcontrol(qr[i]);
                    flowLayoutPanel2.Controls.Add(y[i]);
                    y[i].Tag = qr[i];
                    y[i].theClick += Car1_theClick1;
                }

            }

            if (c2==true&&c1==false)       //只有買周邊
            {            
                Random rng = new Random(Guid.NewGuid().GetHashCode());
                var qr = db.Products.AsEnumerable().Where(p => p.CategoryID != 2 && p.CategoryID != 3 && hasExisted(p.ProductID, ProID)).OrderByDescending(x => rng.Next()).Select(p => p.ProductID).Take(3).ToList();
                y = new PDcontrol[qr.Count];
                for (int i = 0; i < qr.Count; i++)
                {
                    y[i] = new PDcontrol(qr[i]);
                    flowLayoutPanel2.Controls.Add(y[i]);
                    y[i].Tag = qr[i];
                    y[i].theClick += Car1_theClick2;
                }
            }

            if (c1 == true && c2 == true)        //買了咖啡+周邊，推薦相同烘培方式
            {
                int pID = CoffeeID[0];                
                Random rng = new Random(Guid.NewGuid().GetHashCode());
                var r1 = db.Products.Where(p => p.ProductID == pID).Select(x => new { x.Coffee.RoastingID }).ToList();
                var qr = db.Coffees.AsEnumerable().Where(p => p.RoastingID == r1[0].RoastingID && hasExisted(p.ProductID, ProID)).OrderByDescending(x => rng.Next()).Select(p => p.ProductID).Take(3).ToList();
                y = new PDcontrol[qr.Count];
                for (int i = 0; i < qr.Count; i++)
                {
                    y[i] = new PDcontrol(qr[i]);
                    flowLayoutPanel2.Controls.Add(y[i]);
                    y[i].Tag = qr[i];
                    y[i].theClick += Car1_theClick3;
                }

            }

            #region
            //int pID = 2;
            ////烘培方式
            //Random rng = new Random(Guid.NewGuid().GetHashCode());
            //var r1 = db.Products.Where(p => p.ProductID == pID).Select(x => new { x.Coffee.RoastingID }).ToList();
            //var qr = db.Coffees.AsEnumerable().Where(p => p.RoastingID == r1[0].RoastingID && p.ProductID != pID).OrderByDescending(x => rng.Next()).Select(p => p.ProductID).Take(1).ToList();
            //PDcontrol recommend1 = new PDcontrol(qr[0]);
            //flowLayoutPanel2.Controls.Add(recommend1);

            ////製作法
            //var r2 = db.Products.Where(p => p.ProductID == pID).Select(x => new { x.Coffee.ProcessID }).ToList();
            //qr = db.Coffees.AsEnumerable().Where(p => p.ProcessID == r2[0].ProcessID && p.ProductID != pID).OrderByDescending(x => rng.Next()).Select(p => p.ProductID).Take(1).ToList();
            //PDcontrol recommend2 = new PDcontrol(qr[0]);
            //flowLayoutPanel2.Controls.Add(recommend2);

            ////國家
            //var r3 = db.Products.Where(p => p.ProductID == pID).Select(x => new { x.Coffee.Country.ContinentID }).ToList();
            //qr = db.Coffees.AsEnumerable().Where(p => p.Country.Continent.ContinentID == r3[0].ContinentID && p.ProductID != pID).OrderByDescending(x => rng.Next()).Select(p => p.ProductID).Take(1).ToList();
            //PDcontrol recommend3 = new PDcontrol(qr[0]);
            //flowLayoutPanel2.Controls.Add(recommend3);            
            #endregion
        }

        //推薦商品加入購物車
        private void Car1_theClick1(PDcontrol source)
        {
            ShoppingCarDetail carDetail = new ShoppingCarDetail { MemberID = memID, ProductsID = (int)source.Tag, Quantity = 1 };
            this.db.ShoppingCarDetails.Add(carDetail);
            this.db.SaveChanges();
            MessageBox.Show("新增至購物車成功!");
            LoadCar();
        }        
        private void Car1_theClick2(PDcontrol source)
        {       
            ShoppingCarDetail carDetail = new ShoppingCarDetail { MemberID = memID, ProductsID = (int)source.Tag, Quantity = 1 };
            this.db.ShoppingCarDetails.Add(carDetail);
            this.db.SaveChanges();
            MessageBox.Show("新增至購物車成功!");
            LoadCar();
        }
        private void Car1_theClick3(PDcontrol source)
        {
            ShoppingCarDetail carDetail = new ShoppingCarDetail { MemberID = memID, ProductsID = (int)source.Tag, Quantity = 1 };
            this.db.ShoppingCarDetails.Add(carDetail);
            this.db.SaveChanges();
            MessageBox.Show("新增至購物車成功!");
            LoadCar();
        }







        //private void Recommend1_theClick(PDcontrol source)
        //{
        //    ShoppingCarDetail carDetail = new ShoppingCarDetail { MemberID = memID, ProductsID = AddCarID1, Quantity = 1 };
        //    this.db.ShoppingCarDetails.Add(carDetail);
        //    this.db.SaveChanges();
        //    MessageBox.Show("新增至購物車成功!");
        //    LoadCar();
        //}
        //private void Recommend2_theClick(PDcontrol source)
        //{
        //    ShoppingCarDetail carDetail = new ShoppingCarDetail { MemberID = memID, ProductsID = AddCarID2, Quantity = 1 };
        //    this.db.ShoppingCarDetails.Add(carDetail);
        //    this.db.SaveChanges();
        //    MessageBox.Show("新增至購物車成功!");
        //    LoadCar();
        //}
        //private void Recommend3_theClick(PDcontrol source)
        //{
        //    ShoppingCarDetail carDetail = new ShoppingCarDetail { MemberID = memID, ProductsID = AddCarID3, Quantity = 1 };
        //    this.db.ShoppingCarDetails.Add(carDetail);
        //    this.db.SaveChanges();
        //    MessageBox.Show("新增至購物車成功!");
        //    LoadCar();
        //}

        //判斷重複商品
        private bool hasExisted(int p, List<int> id)
        {
            if (id.Contains(p)) return false;
            return true;
        }

        #endregion

        #region  Page2
        RadioButton r;
        RadioButton c;
        private void LoadMemberInformation()
        {
            var q = from m in db.ShoppingCarDetails
                    where m.MemberID == memID
                    select new
                    {
                        Name = m.ShoppingCar.Member.MemberName,
                        Phone = m.ShoppingCar.Member.MemberPhone,
                        Email = m.ShoppingCar.Member.MemberEMail,
                        Address = m.ShoppingCar.Member.MemberAddress
                    };

            foreach (var a in q)
            {
                this.txtName.Text = a.Name;
                this.txtTel.Text = a.Phone;
                this.txtEmail.Text = a.Email;
                this.txtAddress.Text = a.Address;
            }
        }

        private void LoadFeeRadioBox()
        {
            var q = from p in db.Payments
                    select new { ID = p.PaymentID, Name = p.Payment1 };

            List<object> PayID = new List<object>();
            List<object> PayName = new List<object>();
            foreach (var p in q) 
            { 
                PayID.Add(p.ID);
                PayName.Add(p.Name); 
            }

            for (int i= 0; i < PayID.Count(); i++)
            {
                RadioButton r = new RadioButton();               
                r.Tag = PayID[i];
                r.Text = PayName[i].ToString();
                r.Left = 25;
                r.Top = 60+ 70* i;
                r.Size = new Size(120, 80);
                r.Font = new Font("微軟正黑體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                this.splitContainer3.Panel1.Controls.Add(r);
                r.CheckedChanged += R_CheckedChanged;     
            }           

        }

        private void R_CheckedChanged(object sender, EventArgs e)
        { 
            r = sender as RadioButton;
            if (r.Checked == true) { Pay = r.Text; PayID = (int)r.Tag; }            
        }
        private void radioButtonFee1_CheckedChanged(object sender, EventArgs e)
        {
            c = sender as RadioButton;
            if (c.Checked == true) { Fee = c.Tag.ToString(); };
        }     
        private void button3_Click(object sender, EventArgs e)
        {
            //this.tabPage1.Parent = this.tabControl1;//顯示
            //this.tabPage2.Parent = null;//隱藏
            //this.tabPage3.Parent = null;//隱藏
            tabControl1.SelectedTab = tabControl1.TabPages[0];        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Pay == null) { MessageBox.Show("請選擇付款方式!"); }
            else if (Fee == null) { MessageBox.Show("請選擇配送方式!"); }
            else
            {
                //this.tabPage1.Parent = null;//隱藏
                //this.tabPage2.Parent = null;//隱藏
                //this.tabPage3.Parent = this.tabControl1;//顯示
                tabControl1.SelectedTab = tabControl1.TabPages[2];         
                LoadFinalMoney();
                LoadFinalShip();
            }          
        }     


        #endregion

        #region  Page3
        private void LoadFinalMoney()
        {
            this.flowLayoutPanel3.Controls.Clear();       
            this.label3Count.Text = this.labelfirstcount.Text;
            //運費
            if (decimal.Parse(labelTotal1.Text) > 1200)
            {
                label3Fee.Text = "0";
                labelFreefee.Visible = true;              
            }
            else if (decimal.Parse(labelTotal1.Text) < 1200)
            {
                
                label3Fee.Text = radioButtonFee1.Tag.ToString();
                labelFreefee.Visible = false;
            }
            //折價卷
            if (decimal.Parse(labeldiscount.Text) ==0)
            {
                label3discount.Text = "0";
            }
            else if (decimal.Parse(labeldiscount.Text) !=0)
            {
                label3discount.Text = $"{labeldiscount.Text}";            
            }


            var q = from s in db.ShoppingCarDetails
                    where s.MemberID == memID
                    select s.ShoppingCarDetialsID;

            List<int> Name = new List<int>();
            foreach (var n in q) {Name.Add(n); }

            for (int i = 0; i < Name.Count; i++)
            {
                CarControl2 x = new CarControl2(Name[i]);
                this.flowLayoutPanel3.Controls.Add(x);
                x.Size = new Size(1470, 100);
                x.Tag = Name[i];
                decimal Total3 = decimal.Parse(this.labelTotal1.Text);
                this.label3FinalTotal.Text = (Total3 + int.Parse(label3Fee.Text)).ToString("#0.");
            }          

        }

        private void LoadFinalShip()
        {
            label3Name.Text = txtName.Text;
            label3Tel.Text = txtTel.Text;
            label3Email.Text = txtEmail.Text;
            label3Add.Text = txtAddress.Text;
            label3Pay.Text = Pay;           
            label3Ship.Text = Fee;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //this.tabPage1.Parent = null;//隱藏
            //this.tabPage2.Parent = this.tabControl1;//顯示
            //this.tabPage3.Parent = null;//隱藏
            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }
        private void button8_Click(object sender, EventArgs e)
        {
            ////Orders
            string dd = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(dd);
            Order order = new Order { MemberID = memID, OrderDate = dt, OrderStateID = 1, PaymentID = PayID, OrderAddress = label3Add.Text, CouponID = int.Parse(CouponID) };
            this.db.Orders.Add(order);

            this.db.SaveChanges();

            //OrderDetails
            Thread.Sleep(1000);
            var q1 = (from o in db.Orders
                      select o.OrderID).ToList().Last();

            var q = from o in db.ShoppingCarDetails
                     where o.MemberID == memID
                     select new {id = o.ProductsID, Quan = o.Quantity };

            List<object> P = new List<object>();
            List<object> Q = new List<object>();
            foreach (var s in q)
            {
                P.Add(s.id);
                Q.Add(s.Quan);
            }

            for (int i = 0; i < P.Count(); i++)
            {
                int Pid = (int)P[i];
                int Qid = (int)Q[i];
                OrderDetail orderDetail = new OrderDetail { OrderID = q1, ProductID = Pid, Quantity = Qid };
                this.db.OrderDetails.Add(orderDetail);
            }

            this.db.SaveChanges();
            MessageBox.Show("已送出訂單");

        }



        #endregion

  
    }
}

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
    public partial class Car1 : Form
    {
        public Car1()
        {
            InitializeComponent();
            LoadCouponToListbox();                   
            label38.Text = memID.ToString() ;
        }             


        //TODO.購物車功能(付款方式、出貨方式、加購商品推薦、免運費計算、點數折抵消費金) 09
        CoffeeEntities db = new CoffeeEntities();
        int memID = 3;
        int Total1= 0;
        int Discount;
        string Fee;  //運送方式
        string Pay;
        List<int> Coupon = new List<int>();

        #region Page1
        //載入折價卷
        private void LoadCouponToListbox()
        {
            var q = from c in db.CouponDetails
                    where c.MemberID == memID
                    select new { c.Coupon.CouponName, c.Coupon.Money };
                        
            foreach (var n in q)
            {           
                listBox1.Items.Add($" {n.CouponName} : 可折抵 {n.Money} 元 ");
                Coupon.Add(n.Money);
            }
            //MessageBox.Show($"折價卷有 {String.Join(", ", Name)}");
            this.splitContainer2.Panel2Collapsed = true;           

        }                   

        //抓該會員的購物明細
        private void button5_Click(object sender, EventArgs e)
        {  
            this.flowLayoutPanel1.Controls.Clear();
            Total1 = 0;
            var q = from s in db.ShoppingCarDetails
                    where s.MemberID == memID
                    select s.ShoppingCarDetialsID;

            List<int> Name = new List<int>();
            foreach (var n in q)
            {
                Name.Add(n);
            }

            for (int i = 0; i < Name.Count; i++)
            {
                CarControl1 x = new CarControl1(Name[i]);
                this.flowLayoutPanel1.Controls.Add(x);
                x.Size = new Size(1470, 100);
                x.Tag = Name[i];
                Total1 += int.Parse(x.theTextOnlabelCount);
                this.labelfirstcount.Text = Total1.ToString();
                this.labelTotal1.Text = (Total1 - Discount).ToString();
                x.theClick += X_theClick;
                x.thecomoboxChanged += X_thecomoboxChanged;
            }
        }

        private void X_theClick(CarControl1 source)
        {                 
            //MessageBox.Show(source.Tag.ToString());
            var product = (from p in db.ShoppingCarDetails
                           where p.ShoppingCarDetialsID == (int)source.Tag
                           select p).FirstOrDefault();
                                
            DialogResult = MessageBox.Show("確定刪除此筆訂單?", "刪除訂單", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                if (product == null) return;
                this.flowLayoutPanel1.Controls.Remove(source);                
                this.db.ShoppingCarDetails.Remove(product);
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
            Total1=Total1- int.Parse(source.theTextOnlabelmem) + int.Parse(source.theTextOnlabelCount);
            this.labelfirstcount.Text = Total1.ToString();
            this.labelTotal1.Text = (Total1 - Discount).ToString();

            var product = (from p in db.ShoppingCarDetails
                           where p.ShoppingCarDetialsID == (int)source.Tag
                           select p).FirstOrDefault();
            //MessageBox.Show(source.Tag.ToString());
            //MessageBox.Show((source.theTextOnComboBox1+1).ToString());
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
            Discount = Coupon[d];
            MessageBox.Show("使用成功!");
            this.labeldiscount.Text = Discount.ToString();
            this.labelTotal1.Text = (Total1 - Discount).ToString();
        }    

        //下一頁(到第二頁)
        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            this.db.SaveChanges();     //要更新時再打開
            LoadMemberInformation();
            LoadFeeRadioBox();

            //this.tabPage1.Parent = null;//隱藏
            //this.tabPage2.Parent = this.tabControl1;//顯示
            //this.tabPage3.Parent = null;//隱藏

            //呼叫第二個表單 (應該沒有要用ㄌ
            //Car2 car2 = new Car2(this);
            //car2.Show();
        }              

        public int a()
        {
            int aa = memID;
            return aa;
        }

        //Test
        private void button6_Click(object sender, EventArgs e)
        {
            memID = int.Parse(textBox1.Text);
            label38.Text = memID.ToString();
        }
        #endregion

        #region  Page2
        RadioButton r;
        CheckBox c;
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
            foreach (var p in q) { PayID.Add(p.ID); PayName.Add(p.Name); }

            for(int i= 0; i < PayID.Count(); i++)
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
            if (r.Checked == true) { Pay = r.Text; }            
        }

        private void checkBoxFee1_CheckedChanged(object sender, EventArgs e)
        {
            c = sender as CheckBox;
            if (c.Checked== true){ Fee = c.Tag.ToString(); };
        }
        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Pay == null) { MessageBox.Show("請選擇付款方式!"); }
            else if (Fee == null) { MessageBox.Show("請選擇配送方式!"); }
            else
            {
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
            this.label3Count.Text = this.labelTotal1.Text;
            //this.label3Fee.Text = this.checkBoxFee1.Tag.ToString();


            if (int.Parse(labelTotal1.Text) > 1200)
            {
                label3Fee.Text = "0";
                labelFreefee.Visible = true;              
            }
            else if (int.Parse(labelTotal1.Text) < 1200)
            {
                label3Fee.Text = checkBoxFee1.Tag.ToString();
                labelFreefee.Visible = false;
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
                int Total3 = int.Parse(this.labelTotal1.Text);
                this.label3FinalTotal.Text = (Total3 + int.Parse(label3Fee.Text)).ToString();
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
            tabControl1.SelectedTab = tabControl1.TabPages[1];
        }


        #endregion

       
    }
}

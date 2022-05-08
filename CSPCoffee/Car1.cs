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
            
        }

        CoffeeEntities db = new CoffeeEntities();
        int memID = 1;
        int Total = 0;
        int Discount;
        List<int> Coupon = new List<int>();
             
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
            Total = 0;
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
                Total += int.Parse(x.theTextOnlabelCount);
                this.labelfirstcount.Text = Total.ToString();
                this.labelTotal.Text = (Total - Discount).ToString();
                x.theClick += X_theClick;
                x.thecomoboxChanged += X_thecomoboxChanged;
            }
        }

        private void X_theClick(CarControl1 source)
        {      
            this.flowLayoutPanel1.Controls.Remove(source);
            MessageBox.Show("刪除成功");
        }

        //修改明細數量時
        private void X_thecomoboxChanged(CarControl1 source)
        {
            Total=Total- int.Parse(source.theTextOnlabelmem) + int.Parse(source.theTextOnlabelCount);
            this.labelfirstcount.Text = Total.ToString();
            this.labelTotal.Text = (Total - Discount).ToString();
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
            this.labelTotal.Text = (Total - Discount).ToString();
        }               

        //下一頁
        private void button4_Click(object sender, EventArgs e)
        {
            Car2 car2 = new Car2(this);
            car2.Show();
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
        }

        //TODO.購物車功能(付款方式、出貨方式、加購商品推薦、免運費計算、點數折抵消費金) 09

    }
}

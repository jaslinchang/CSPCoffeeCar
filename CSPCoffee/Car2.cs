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
    public partial class Car2 : Form
    {
        int memID;
        public Car2(Car1 parent)
        {
            InitializeComponent();
            this.Tag = parent;
            memID = parent.a();
            LoadMemberInformation();            
        }     
      
        CoffeeEntities db = new CoffeeEntities();
        
        private void LoadMemberInformation()
        {
            var q = from m in db.ShoppingCarDetails
                    where m.MemberID == memID
                    select new
                    {
                       Name= m.ShoppingCar.Member.MemberName,
                        Phone=m.ShoppingCar.Member.MemberPhone,
                        Email=m.ShoppingCar.Member.MemberEMail,
                        Address=m.ShoppingCar.Member.MemberAddress
                    };

            foreach(var a in q)
            {
                this.txtName.Text = a.Name;
                this.txtTel.Text = a.Phone;
                this.txtEmail.Text = a.Email;
                this.txtAddress.Text = a.Address;
            }
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Car3 car3 = new Car3();
            car3.Show();
        }
    }
}

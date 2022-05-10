//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSPCoffee
{
    using System;
    using System.Collections.Generic;
    
    public partial class Coupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coupon()
        {
            this.CouponDetails = new HashSet<CouponDetail>();
            this.Qquestionnaires = new HashSet<Qquestionnaire>();
        }
    
        public int CouponID { get; set; }
        public string CouponName { get; set; }
        public decimal Money { get; set; }
        public int Condition { get; set; }
        public System.DateTime CouponStartDate { get; set; }
        public System.DateTime CouponDeadline { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CouponDetail> CouponDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Qquestionnaire> Qquestionnaires { get; set; }
    }
}

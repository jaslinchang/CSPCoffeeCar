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
    
    public partial class AnswerTableDetail
    {
        public int AnswerTableDetailsID { get; set; }
        public Nullable<int> QuestionTableDetailsID { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string Q6 { get; set; }
        public string Q7 { get; set; }
    
        public virtual QuestionTableDetail QuestionTableDetail { get; set; }
    }
}

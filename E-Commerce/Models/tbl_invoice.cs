//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Commerce.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_invoice()
        {
            this.order_table = new HashSet<order_table>();
        }
    
        public int in_id { get; set; }
        public Nullable<int> in_fk_user { get; set; }
        public Nullable<System.DateTime> in_date { get; set; }
        public Nullable<double> in_totalbill { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_table> order_table { get; set; }
        public virtual tbl_user tbl_user { get; set; }
    }
}

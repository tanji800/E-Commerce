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
    
    public partial class cateogory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cateogory()
        {
            this.products = new HashSet<product>();
        }
    
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public string cat_image { get; set; }
        public Nullable<int> adm_id { get; set; }
        public Nullable<int> cat_status { get; set; }
    
        public virtual adminn adminn { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<product> products { get; set; }
    }
}

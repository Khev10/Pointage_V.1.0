//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pointage.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            this.cart = new HashSet<cart>();
        }
    
        [Display(Name ="User ID")]
        public int users_id { get; set; }
        [Display(Name = "Username")]
        [Required(ErrorMessage ="Username is required")]
        public string username { get; set; }
        [Display(Name ="Email")]
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage ="Invalid Email")]
        public string email { get; set; }
        [Display(Name ="Password")]
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Date Of Registration")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public Nullable<System.DateTime> date_of_reg { get; set; }
        [Display(Name = "Address")]
        [Required]
        public string address { get; set; }
        [Display(Name = "Contact No.")]
        [Required]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Invalid Number")]
        public string contact_no { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart> cart { get; set; }
    }
}

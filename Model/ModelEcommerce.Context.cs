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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class EcommerceDBEntities : DbContext
    {
        public EcommerceDBEntities()
            : base("name=EcommerceDBEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<cart> cart { get; set; }
        public virtual DbSet<cart_items> cart_items { get; set; }
        public virtual DbSet<category> category { get; set; }
        public virtual DbSet<checkout> checkout { get; set; }
        public virtual DbSet<coupon> coupon { get; set; }
        public virtual DbSet<events> events { get; set; }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<users> users { get; set; }
    }
}

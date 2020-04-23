namespace DefCan.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbModelShop : DbContext
    {
        public DbModelShop()
            : base("name=DbModelShops")
        {
        }

        public virtual DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.AslPicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Item>()
                .Property(e => e.Audio)
                .IsUnicode(false);
        }
    }
}

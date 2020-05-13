namespace DefCan.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Item")]
    public partial class Item
    {
        public int ItemID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? StockQty { get; set; }

        public float? Cost { get; set; }

        [StringLength(25)]
        public string Category { get; set; }

        [StringLength(2048)]
        public string PicUrl { get; set; }

        [StringLength(2048)]
        public string AslPicUrl { get; set; }

        [StringLength(2048)]
        public string Audio { get; set; }

        public int? AmountPurchased { get; set; }
        //public Nullable<float> CustBalance { get; set; } //Added To hold total balance
    }

 

    
}

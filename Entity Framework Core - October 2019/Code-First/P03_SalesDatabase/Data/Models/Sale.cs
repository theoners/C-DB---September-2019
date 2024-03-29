﻿namespace P03_SalesDatabase.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using static DataValidation.Sales;
    public class Sale
    {
        public int SaleId { get; set; }

        [Column(TypeName = DateType)]
        public DateTime Date { get; set; }

        public int ProductId { get; set; } 
        public Product Product { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}

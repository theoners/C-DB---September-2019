﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataValidation.Product;
    public class Product
    {
        public int ProductId { get; set; }
        
        [MaxLength(NameMaxLength)] 
        [Required]
        public string Name { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }
        
        [MaxLength(250)]
        public string Description { get; set; }

       public ICollection<Sale> Sales { get; set; }
    }
}

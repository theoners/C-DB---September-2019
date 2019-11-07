using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using static DataValidation.Customer;
    public class Customer
    {
        public int CustomerId { get; set; }
        
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Column(TypeName = EmailMaxLength)]
        public string Email { get; set; }

        public string CreditCardNumber { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}

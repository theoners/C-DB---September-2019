using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_SalesDatabase.Data.Models
{
    using System.Collections.Generic;
    using static DataValidation.Store;
    public class Store
    {
        public int StoreId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<Sale> Sales { get; set; }

    }
}

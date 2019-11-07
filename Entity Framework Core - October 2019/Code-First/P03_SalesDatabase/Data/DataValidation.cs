using System.Collections.Specialized;

namespace P03_SalesDatabase.Data
{
    public static class DataValidation
    {
        public static class Customer
        {
            public const int NameMaxLength = 100;
            public const string EmailMaxLength = "varchar(80)";
        }

        public static class Product
        {
            public const int NameMaxLength = 50;
        }

        public static class Store
        {
            public const int NameMaxLength = 80;
        }

      public static class Sales
      {
          public const string DateType = "DATETIME2";
      }
    }
}

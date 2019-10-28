namespace MiniORMAPPS
{
    using System.Linq;
    using Data.Entities;
    class StartUp
    {
        public static void Main()
        {
            var connectionString = @"Server=ASUS\SQLEXPRESS;Database=MiniORM;Integrated security=true;";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true,
            });

            var employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}

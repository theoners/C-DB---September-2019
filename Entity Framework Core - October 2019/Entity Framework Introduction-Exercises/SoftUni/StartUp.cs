using System.Globalization;

namespace SoftUni
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Models;

    public static class StartUp
    {
       public static void Main()
       {
          
       }

       public static string GetEmployeesFullInformation (SoftUniContext context)
       {
           var result = new StringBuilder();
           var employees = context
               .Employees
               .OrderBy(x => x.EmployeeId)
               .Select(e=>new
               {
                   e.FirstName,
                   e.MiddleName,
                   e.LastName,
                   e.JobTitle,
                   e.Salary
               })
               .ToList();
           employees.ForEach(e => result.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}"));

            return result.ToString().TrimEnd();
       }

       public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
       {
           var result = new StringBuilder();
           var employees = context.Employees
               .Where(x => x.Salary > 50000)
               
               .Select(e => new
               {
                   e.FirstName,
                   e.Salary,
               }).OrderBy(x => x.FirstName)
               .ToList();

           employees.ForEach(x=>result.AppendLine($"{x.FirstName} - {x.Salary:f2}"));

           return result.ToString().TrimEnd();
       }

       public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
       {
           const string departmentName = "Research and Development";
          var result = new StringBuilder();

               var employees = context.Employees
                   .Where(e => e.Department.Name ==departmentName )
                   .OrderBy(e => e.Salary)
                   .ThenByDescending(e => e.FirstName)
                   .Select(e => new
                   {
                       e.FirstName,
                       e.LastName,
                       e.Salary
                   })
                   .ToList();

               employees.ForEach(e => result.AppendLine($"{e.FirstName} {e.LastName} from {departmentName} - ${e.Salary:f2}"));

               return result.ToString().TrimEnd();
       }
       public static string AddNewAddressToEmployee(SoftUniContext context)
       {
           const string addressText = "Vitoshka 15";
           const int townId = 4;
           const string employeeLastName = "Nakov";

           var result = new StringBuilder();

           var address = new Address {AddressText = addressText, TownId = townId};

           var employeeToChange = context.Employees
               .First(e => e.LastName == employeeLastName);

           employeeToChange.Address = address;
           context.SaveChanges();

            context.Employees
               .OrderByDescending(e => e.AddressId)
               .Select(e => new
               {
                   e.Address.AddressText
               })
               .Take(10)
               .ToList()
               .ForEach(e => result.AppendLine(e.AddressText));
            
           return result.ToString().TrimEnd();
       }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            const int startYear = 2001;
            const int endYear = 2003;

            var result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                    .Any(p => p.Project.StartDate.Year >= startYear && 
                              p.Project.StartDate.Year <= endYear))
                .Take(10)
                .Select(e => new
                {
                    EmployeeName = e.FirstName+" "+e.LastName,
                    ManagerName = e.Manager.FirstName+" "+e.Manager.LastName,
                    Projects = e.EmployeesProjects
                    .Select(p => new
                    {
                        p.Project.Name,
                        p.Project.StartDate,
                        p.Project.EndDate
                    })
                });

            const string format = "M/d/yyyy h:mm:ss tt";

            foreach (var employee in employees)
            {
                
                result.AppendLine($"{employee.EmployeeName} - Manager: {employee.ManagerName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.StartDate.ToString(format, CultureInfo.InvariantCulture);

                    var endDate = project.EndDate != null
                                                        ? project.EndDate.Value.ToString(format, CultureInfo.InvariantCulture)
                                                        : "not finished";

                    result.AppendLine($"--{project.Name} - {startDate} - {endDate}");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var result = new StringBuilder();

            context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeesCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToList()
                .ForEach(a => result.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees"));
            
            return result.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            const int employeeId = 147;

            var result = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(p => p.Project.Name)
                        .OrderBy(p => p)
                        .ToList()
                }).First();

            result.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            employee.Projects.ForEach(p => result.AppendLine(p));
            
            return result.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            const int departmentsCount = 5;

            var result = new StringBuilder();

            var departments= context.Departments
                .Where(d => d.Employees.Count > departmentsCount)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepatmentName = d.Name,
                    ManagerName = d.Manager.FirstName+" "+d.Manager.LastName,
                    Employees = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            EmployeeName = e.FirstName+" "+e.LastName,
                            e.JobTitle
                        })
                })
                .ToList();

            foreach (var department in departments)
            {
               result.AppendLine($"{department.DepatmentName} - {department.ManagerName}");

                foreach (var employee in department.Employees)
                {
                   result.AppendLine($"{employee.EmployeeName} - {employee.JobTitle}");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var result = new StringBuilder();

            context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .OrderBy(p => p.Name)
                .ToList()
                .ForEach(p => result.AppendLine($"{p.Name}{Environment.NewLine}" +
                                                $"{p.Description}{Environment.NewLine}" +
                                                $"{p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}")); ;

            

            return result.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var departments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };
           
            var result = new StringBuilder();

            var employees = context.Employees
                .Where(e => departments.Contains(e.Department.Name))
                .ToList();

            employees.ForEach(e=>e.Salary*=1.12M);
           
            context.SaveChanges();

            employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList()
                .ForEach(e => result.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})"));

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            const string startingString = "Sa";
            var result = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.Substring(0, 2) == startingString)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            employees.ForEach(e => result.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})"));

            return result.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            const int projectId = 2;

            var result = new StringBuilder();

            var projectToDelete = context.Projects
                .First(p => p.ProjectId == projectId);

            context.EmployeesProjects
                .Where(e => e.Project == projectToDelete)
                .ToList()
                .ForEach(e => e.Project = null);
            
            context.Projects.Remove(projectToDelete);

            context.SaveChanges();

            context.Projects
                .Select(p => p.Name)
                .Take(10)
                .ToList()
                .ForEach(p => result.AppendLine(p));
            
            return result.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            const string townName = "Seattle";

            var townToDelete = context.Towns
                .First(t => t.Name == townName);

            var employeesFromTown = context.Employees
                .Where(e => e.Address.Town == townToDelete)
                .ToList();

            employeesFromTown.ForEach(e=>e.AddressId=null);
            
            var addressesFromTown = context.Addresses
                .Where(e => e.Town == townToDelete)
                .ToList();

            var deletedAddressesCount = addressesFromTown.Count();

            addressesFromTown.ForEach(a=>context.Addresses.Remove(a));
           
            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            return $"{deletedAddressesCount} addresses in {townName} were deleted";
        }

    }
}

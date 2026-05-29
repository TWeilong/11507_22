using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp50
{
    public class Employee
    {
        public Employee() { }

        public string name { get; private set; }
        public decimal salary { get; private set; }
        public float experience { get; private set; }

        public Employee(string name, decimal salary, float experience)
        {
            this.name = name;
            this.salary = salary;
            this.experience = experience;
        }

        public void FilterEmployees(List<Employee> employees,Predicate<Employee> isFilter)
        {
            foreach (var employee in employees)
            {
                if (isFilter(employee))
                {
                    Console.WriteLine($"Name: {employee.name,-10} | " + $"Experience: {employee.experience} years,-10 | " + $"Salary: {employee.salary} y.e.");
                }
            }
        }
    }

}

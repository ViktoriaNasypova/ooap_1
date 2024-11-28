using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1ooap
{
    public interface ISalaryCalculator
    {
        decimal Calculate();
    }

    public class Employee
    {
        private static int nextId = 1;
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Position { get; set; }
        private ISalaryCalculator salaryCalculator;

        public Employee(string name, string position, ISalaryCalculator salaryCalculator)
        {
            this.ID = nextId++;
            this.Name = name;
            this.Position = position;
            this.salaryCalculator = salaryCalculator;
        }

        public decimal CalculateSalary()
        {
            return salaryCalculator.Calculate();
        }

        public void UpdateSalary(ISalaryCalculator newSalaryCalculator)
        {
            this.salaryCalculator = newSalaryCalculator;
        }
    }

    public class RealSalaryCalculator : ISalaryCalculator
    {
        private decimal baseSalary;

        public RealSalaryCalculator(decimal baseSalary)
        {
            this.baseSalary = baseSalary;
        }

        public decimal Calculate()
        {
            return baseSalary;
        }
    }

    public class SalaryCalculatorProxy : ISalaryCalculator
    {
        private RealSalaryCalculator realSalaryCalculator;
        private decimal baseSalary;

        public SalaryCalculatorProxy(decimal baseSalary)
        {
            this.baseSalary = baseSalary;
            realSalaryCalculator = new RealSalaryCalculator(baseSalary);
        }

        public decimal Calculate()
        {
            decimal salary = realSalaryCalculator.Calculate();
            return salary;
        }
    }


    public class Department
    {
        private List<Employee> employees = new List<Employee>();

        public int EmployeeCount => employees.Count;

        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        public void RemoveEmployee(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee != null)
            {
                employees.Remove(employee);
            }
        }

        public Employee GetEmployeeById(int id)
        {
            return employees.FirstOrDefault(e => e.ID == id);
        }

        public void DisplayAllEmployees()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees to display.");
                return;
            }

            Console.WriteLine("\n--- Employees List ---");
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.ID}. {employee.Name} - {employee.Position} - Salary: {employee.CalculateSalary():N2} UAH");
            }
        }
    }
}

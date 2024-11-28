using lab1ooap;
using static lab1ooap.Employee;

public class Program
{
    public static void Main(string[] args)
    {
        Department department = new Department();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. Display All Employees");
            Console.WriteLine("3. Edit Employee");
            Console.WriteLine("4. Remove Employee");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddEmployeeMenu(department);
                    break;
                case "2":
                    department.DisplayAllEmployees();
                    break;
                case "3":
                    EditEmployeeMenu(department);
                    break;
                case "4":
                    RemoveEmployeeMenu(department);
                    break;
                case "5":
                    running = false;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void AddEmployeeMenu(Department department)
    {
        try
        {
            Console.Write("Enter employee name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Select position:");
            Console.WriteLine("1. Head of Department");
            Console.WriteLine("2. Chief Engineer");
            Console.WriteLine("3. Software Engineer");
            Console.WriteLine("4. System Administrator");
            string position = SelectPosition();

            decimal salary = GetSalaryInput();

            // Using Proxy for Salary Calculation
            ISalaryCalculator salaryCalculator = new SalaryCalculatorProxy(salary);
            department.AddEmployee(new Employee(name, position, salaryCalculator));

            Console.WriteLine("Employee added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static decimal GetSalaryInput()
    {
        decimal salary = 0;
        bool validSalary = false;

        while (!validSalary)
        {
            Console.Write("Enter salary: ");
            string input = Console.ReadLine();

            if (decimal.TryParse(input, out salary) && salary >= 0)
            {
                validSalary = true;
            }
            else
            {
                Console.WriteLine("Invalid salary. Please enter a positive number.");
            }
        }

        return salary;
    }

    private static string SelectPosition()
    {
        string position = "";
        bool validChoice = false;

        while (!validChoice)
        {
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    position = "Head of Department";
                    validChoice = true;
                    break;
                case "2":
                    position = "Chief Engineer";
                    validChoice = true;
                    break;
                case "3":
                    position = "Software Engineer";
                    validChoice = true;
                    break;
                case "4":
                    position = "System Administrator";
                    validChoice = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid position.");
                    break;
            }
        }

        return position;
    }

    private static void EditEmployeeMenu(Department department)
    {
        try
        {
            Console.Write("Enter employee ID to edit or type 'cancel' to exit: ");
            string input = Console.ReadLine();

            if (input.ToLower() == "cancel")
            {
                Console.WriteLine("Edit action canceled.");
                return;
            }

            if (int.TryParse(input, out int id))
            {
                Employee employee = department.GetEmployeeById(id);
                if (employee != null)
                {
                    Console.WriteLine($"Editing Employee: {employee.Name} ({employee.Position}) with salary {employee.CalculateSalary():C}");

                    string newName = GetNonEmptyInput("Enter new name or press Enter to keep current: ");
                    if (!string.IsNullOrEmpty(newName))
                    {
                        employee.Name = newName;
                    }

                    Console.WriteLine("Select new position or press Enter to keep current:");
                    Console.WriteLine("1. Head of Department");
                    Console.WriteLine("2. Chief Engineer");
                    Console.WriteLine("3. Software Engineer");
                    Console.WriteLine("4. System Administrator");
                    string newPosition = SelectPosition();
                    if (!string.IsNullOrEmpty(newPosition))
                    {
                        employee.Position = newPosition;
                    }

                    decimal newSalary = employee.CalculateSalary();
                    string salaryInput = GetNonEmptyInput("Enter new salary or press Enter to keep current: ");
                    if (!string.IsNullOrEmpty(salaryInput))
                    {
                        while (!decimal.TryParse(salaryInput, out newSalary) || newSalary < 0)
                        {
                            Console.WriteLine("Invalid salary. Please enter a valid positive number.");
                            salaryInput = GetNonEmptyInput("Enter new salary or press Enter to keep current: ");
                        }

                        employee.UpdateSalary(new SalaryCalculatorProxy(newSalary));
                    }

                    Console.WriteLine("Employee updated successfully.");
                }
                else
                {
                    Console.WriteLine("Employee not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID. Edit canceled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static string GetNonEmptyInput(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty. Please enter a valid value.");
            }
        } while (string.IsNullOrEmpty(input));

        return input;
    }

    private static void RemoveEmployeeMenu(Department department)
    {
        try
        {
            Console.Write("Enter employee ID to remove or type 'cancel' to exit: ");
            string input = Console.ReadLine();

            if (input.ToLower() == "cancel")
            {
                Console.WriteLine("Remove action canceled.");
                return;
            }

            if (int.TryParse(input, out int id))
            {
                Employee employee = department.GetEmployeeById(id);
                if (employee != null)
                {
                    Console.WriteLine($"Are you sure you want to remove {employee.Name} (Position: {employee.Position})? (y/n)");

                    string confirm = Console.ReadLine();
                    if (confirm.ToLower() == "y")
                    {
                        department.RemoveEmployee(id);
                        Console.WriteLine("Employee removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Remove action canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("Employee not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID. Remove canceled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

namespace StudentBudgetPlanner
{
    // Represents a single expense record with details like description, cost, and category.
    class Expense
    {
        public string Description;
        public double Amount;
        public string Category;
    }

    // Stores the user's overall budget settings.
    class BudgetProfile
    {
        public double MonthlyAllowance;
        public double SavingsGoal;
    }

    class Program
    {
        // Program title displayed at the start.
        static string programName = "Personal Finance & Student Budget Planner";

        // Sets up the initial monthly allowance and savings goal for the user.
        static BudgetProfile userProfile = new BudgetProfile
        {
            MonthlyAllowance = 1500.00,
            SavingsGoal = 300.00
        };

        // Tracks how many expenses have been logged so far.
        static int totalExpenses = 0;

        // Fixed-size array to store up to 10 expenses.
        static Expense[] expenses = new Expense[10];

        // Controls whether the main program loop keeps running.
        static bool isRunning = true;

        // The entry point of the program. Keeps the menu running until the user decides to exit.
        static void Main()
        {
            WelcomeScreen();

            while (isRunning)
            {
                MenuDisplay();
                Console.Write("Enter Option: ");

                // Read user input and convert it into a whole number (integer)
                int option = Convert.ToInt32(Console.ReadLine());
                MenuSwitch(option);
            }
            Console.WriteLine("System closed. Goodbye!");
        }

        // Clears the screen and displays the welcome header.
        static void WelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("=====================================================");
            Console.WriteLine();
            Console.WriteLine("    " + programName);
            Console.WriteLine();
            Console.WriteLine("=====================================================");
            Console.WriteLine();
        }

        // Displays the main menu options for the user to choose from.
        static void MenuDisplay()
        {
            Console.WriteLine("--------------------------Menu-----------------------");
            Console.WriteLine();
            Console.WriteLine("1. Log Daily Expense");
            Console.WriteLine("2. View All Transactions and Balance");
            Console.WriteLine("3. Search Expense By Decription");
            Console.WriteLine("4. Calculate Total Spent & Check Remaining Allowance");
            Console.WriteLine("5. View Budget Statistics(Highest & Lowest Expense)");
            Console.WriteLine("6. Exit Program");
            Console.WriteLine();
        }

        // Directs the user to the correct method based on their menu choice.
        static void MenuSwitch(int choice)
        {
            switch (choice)
            {
                case 1:
                    AddNewExpense();
                    break;
                case 2:
                    DisplayAllExpenses();
                    break;
                case 3:
                    SearchExpense();
                    break;
                case 4:
                    DisplayAllowanceSummary();
                    break;
                case 5:
                    DisplayStatistics();
                    break;
                case 6:
                    ExitProgram();
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.\n");
                    break;
            }
        }

        // Allows the user to add a new expense and checks if they are spending too much.
        static void AddNewExpense()
        {
            Console.Clear();

            // Check if the array is full before adding a new item
            if (totalExpenses >= expenses.Length)
            {
                Console.WriteLine("Expense tracker storage is full! Cannot add more records.\n");
                return;
            }

            Expense e = new Expense();

            Console.Write("Enter expense description (e.g., Groceries, Books): ");
            e.Description = Console.ReadLine();

            Console.Write("Enter amount (R): ");
            e.Amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter category (Food, Transport, Books, Entertainment): ");
            e.Category = Console.ReadLine();

            // Save the new expense into the array and increase the expense count
            expenses[totalExpenses] = e;
            totalExpenses++;

            // CALCULATION: Add up all recorded expenses to check current spending
            double totalSpent = 0;
            for (int i = 0; i < totalExpenses; i++)
            {
                totalSpent += expenses[i].Amount;
            }

            // WARNING CHECK: If total spending is greater than 90% of the monthly allowance, show a warning
            if (totalSpent > (userProfile.MonthlyAllowance * 0.90))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[WARNING] Over-budget warning! Your spending has exceeded 90% of your total allowance.");
                Console.ResetColor();
            }

            Console.WriteLine(e.Description + " logged successfully!\n");
        }

        // Prints out a list of all transactions saved in the system.
        static void DisplayAllExpenses()
        {
            Console.Clear();

            // If no expenses have been added yet, let the user know
            if (totalExpenses == 0)
            {
                Console.WriteLine("No records found.\n");
                return;
            }

            Console.WriteLine("\n── CURRENT TRANSACTIONS ──");
            for (int i = 0; i < totalExpenses; i++)
            {
                Console.WriteLine((i + 1) + ". [" + expenses[i].Category + "] " + expenses[i].Description + " | Amount: R" + expenses[i].Amount);
            }
            Console.WriteLine();
        }

        // Searches for a specific expense using its description (ignoring uppercase/lowercase differences).
        static void SearchExpense()
        {
            Console.Clear();
            Console.Write("Enter description to search: ");
            string query = Console.ReadLine();

            bool found = false;

            for (int i = 0; i < totalExpenses; i++)
            {
                // Convert both strings to lowercase so the search works even if capitalization doesn't match
                if (expenses[i].Description.ToLower() == query.ToLower())
                {
                    Console.WriteLine("Match Found: [" + expenses[i].Category + "] " + expenses[i].Description + " | Amount: R" + expenses[i].Amount);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("No transaction found with description: " + query);
            }
            Console.WriteLine();
        }

        // Calculates total money spent and shows how much money is left from the allowance.
        static void DisplayAllowanceSummary()
        {
            Console.Clear();

            if (totalExpenses == 0)
            {
                Console.WriteLine("No expenses recorded yet.\n");
                return;
            }

            // CALCULATION: Loop through all expenses to sum up the total money spent
            double totalSpent = 0;
            for (int i = 0; i < totalExpenses; i++)
            {
                totalSpent += expenses[i].Amount;
            }

            // CALCULATION: Subtract total spent from the monthly allowance to find the remaining balance
            double remainingBalance = userProfile.MonthlyAllowance - totalSpent;

            Console.WriteLine("Monthly Allowance : R" + userProfile.MonthlyAllowance);
            Console.WriteLine("Savings Goal      : R" + userProfile.SavingsGoal);
            Console.WriteLine("Total Spent       : R" + totalSpent);
            Console.WriteLine("Remaining Balance : R" + remainingBalance + "\n");
        }

        // Finds and displays the highest and lowest expense items from the list.
        static void DisplayStatistics()
        {
            Console.Clear();

            if (totalExpenses == 0)
            {
                Console.WriteLine("No records available to calculate statistics.\n");
                return;
            }

            // Start by assuming the first expense is both the highest and lowest
            double highestExpense = expenses[0].Amount;
            double lowestExpense = expenses[0].Amount;
            string highestDesc = expenses[0].Description;
            string lowestDesc = expenses[0].Description;

            // Loop through all expenses to find the true highest and lowest amounts
            for (int i = 0; i < totalExpenses; i++)
            {
                // If current item is greater than the highest found so far, update highest
                if (expenses[i].Amount > highestExpense)
                {
                    highestExpense = expenses[i].Amount;
                    highestDesc = expenses[i].Description;
                }

                // If current item is smaller than the lowest found so far, update lowest
                if (expenses[i].Amount < lowestExpense)
                {
                    lowestExpense = expenses[i].Amount;
                    lowestDesc = expenses[i].Description;
                }
            }

            Console.WriteLine("\n── BUDGET STATISTICS ──");
            Console.WriteLine("Highest Expense : " + highestDesc + " (R" + highestExpense + ")");
            Console.WriteLine("Lowest Expense  : " + lowestDesc + " (R" + lowestExpense + ")\n");
        }

        // Displays a goodbye message and stops the program loop.
        static void ExitProgram()
        {
            Console.Clear();
            Console.WriteLine("============================================================");
            Console.WriteLine("                            EXIT PROGRAM                    ");
            Console.WriteLine();
            Console.WriteLine("                        Press any key to Exit               ");
            Console.WriteLine("                              Thank You!                    ");
            Console.WriteLine();
            Console.WriteLine("============================================================");
            Console.ReadKey();

            // Setting this to false stops the `while (isRunning)` loop in Main()
            isRunning = false;
        }
    }
}
using Budget;

namespace HomeBudget_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HI!");
            HomeBudget homeBudget = new HomeBudget("newDB.db", true);
            homeBudget.expenses.Add(new DateTime(2018, 5, 10), 10, 12, "hat (on credit)");
            homeBudget.expenses.Add(new DateTime(2018, 5, 2), 10, 12, "trousers (on credit)");

            homeBudget.expenses.Add(new DateTime(2020, 2, 7), 9, -15, "scarf (on credit)");
            homeBudget.expenses.Add(new DateTime(2020, 7, 6), 14, 45, "McDonalds");
            homeBudget.expenses.Add(new DateTime(2016, 2, 18), 14, 45, "McDonalds");
            homeBudget.expenses.Add(new DateTime(2020, 6, 3), 14, 25, "Wendys");

            Console.WriteLine("Get Budget Items");

            List<BudgetItem> items = homeBudget.GetBudgetItems(null, null, true, 14);
            foreach (var i in items)
            {
                Console.WriteLine(i.Date.ToString("yyyy/MM") + "  " + i.ShortDescription);
            }
            Console.WriteLine("Get Budget Items By Months");

            List<BudgetItemsByMonth> itemsByMonth = homeBudget.GetBudgetItemsByMonth(null, null, false, 1);
            foreach (var i in itemsByMonth)
            {
                Console.Write(i.Month);
                foreach (BudgetItem item in i.Details)
                {
                    Console.Write(item.Category + "       " + item.Balance + "      " + item.ShortDescription + "     ");
                }
                Console.WriteLine(i.Total);
            }
            /*Console.WriteLine("Get Budget Items By Categories");
            List<BudgetItemsByCategory> itemsByCategory = homeBudget.GetBudgetItemsByCategory(null, null, false, 1);
            foreach (BudgetItemsByCategory i in itemsByCategory)
            {
                Console.Write(i.Category);
                foreach (BudgetItem item in i.Details)
                {
                    Console.Write(item.Date.ToString("yyyy/MM") + " " + item.Balance + " " + item.ShortDescription);
                }
                Console.WriteLine(i.Total);
            }*/
        }
    }
}

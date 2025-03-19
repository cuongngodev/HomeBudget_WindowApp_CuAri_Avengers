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
            homeBudget.expenses.Add(new DateTime(2020, 2, 7), 9, -15, "scarf (on credit)");
            homeBudget.expenses.Add(new DateTime(2020, 7, 6), 14, 45, "McDonalds");
            homeBudget.expenses.Add(new DateTime(2016, 2, 18), 14, 45, "McDonalds");
            homeBudget.expenses.Add(new DateTime(2020, 6, 3), 14, 25, "Wendys");



            List<BudgetItem> items = homeBudget.GetBudgetItems(null, null, false, 1);
            /*foreach (var i in items)
            {
                Console.WriteLine(i.Date.ToString("yyyy/MM") + "  " + i.ShortDescription);
            }*/
            List<BudgetItemsByMonth> itemsByMonth = homeBudget.GetBudgetItemsByMonth(null, null, false, 1);
            foreach (var i in itemsByMonth)
            {
                Console.Write(i.Month);
                foreach (BudgetItem item in i.Details)
                {
                    Console.Write(item.Category + " " + item.Balance + " " + item.ShortDescription);
                }
                Console.WriteLine(i.Total);
            }
        }
    }
}

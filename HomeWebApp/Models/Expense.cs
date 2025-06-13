namespace HomeWebApp.Models
{
    public class Expense
    {
        public static Expense Empty 
        { 
            get
            {
                return new Expense
                {
                    Id = -1,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    Description = string.Empty,
                    Category = new ExpenseCategory { Id = 1, Name = string.Empty, Color = "#000000", Description = string.Empty },
                    Amount = 0.0m,
                    Image = null,
                    User = new User { Id = 1, Name = "Dawid" }
                };
            } 
       }

        public required int Id { get; set; }
        public required DateOnly Date { get; set; }
        public required string Description { get; set; }
        public required ExpenseCategory Category { get; set; }
        public required decimal Amount { get; set; }
        public Image? Image { get; set; }
        public required User User { get; set; }
    }
}

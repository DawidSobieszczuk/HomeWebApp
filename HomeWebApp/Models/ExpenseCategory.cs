using MudBlazor.Utilities;
using System;

namespace HomeWebApp.Models
{
    public class ExpenseCategory : IEquatable<ExpenseCategory>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#000000";
        public string Description { get; set; } = string.Empty;

        public bool Equals(ExpenseCategory? other)
        {
            if (other == null) return false;

            return Id == other.Id;
        }

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;
    }
}

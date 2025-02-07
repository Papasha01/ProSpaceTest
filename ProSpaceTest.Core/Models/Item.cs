namespace ProSpaceTest.Core.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;

        public static (Item Item, string Error) Create(Guid id, string code, string name, decimal price, string category)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(code) || !System.Text.RegularExpressions.Regex.IsMatch(code, @"^\d{2}-\d{4}-[A-Z]{2}\d{2}$"))
            {
                error = "Code must be in the format 'XX-XXXX-YYXX' where X is a digit and Y is an uppercase letter.";
            }
            else if (string.IsNullOrEmpty(name) || name.Length > 250)
            {
                error = "Name cannot be empty or longer than 250 symbols.";
            }
            else if (price <= 0)
            {
                error = "Price must be greater than 0.";
            }

            var item = new Item { Id = id, Code = code, Name = name, Price = price, Category = category };
            return (item, error);
        }
    }

}

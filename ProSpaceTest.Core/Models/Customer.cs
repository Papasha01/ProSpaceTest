namespace ProSpaceTest.Core.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Discount { get; set; }

        public static (Customer Customer, string Error) Create(Guid id, Guid userId, string name, string code, string address, decimal discount = 0)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(name) || name.Length > 250)
            {
                error = "Name cannot be empty or longer than 250 symbols.";
            }
            else if (string.IsNullOrEmpty(code) || !System.Text.RegularExpressions.Regex.IsMatch(code, @"^\d{4}-\d{4}$"))
            {
                error = "Code must be in the format 'XXXX-YYYY' where X is a digit and YYYY is the year.";
            }

            var customer = new Customer { Id = id, Name = name, Code = code, Address = address, Discount = discount };
            return (customer, error);
        }
    }
}
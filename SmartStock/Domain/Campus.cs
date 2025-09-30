namespace SmartStock.Domain
{
    public class Campus
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;

        // 🔗 Navigation
        public ICollection<Account> Users { get; set; } = new List<Account>();
    }
}

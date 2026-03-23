namespace MyAdvisor.Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }
        public List<Category> SubCategories { get; private set; } = new();
        private Category() { }
        public Category(string name, int? parentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.", nameof(name));

            Name = name;
            ParentCategoryId = parentCategoryId;
        }
    }
}
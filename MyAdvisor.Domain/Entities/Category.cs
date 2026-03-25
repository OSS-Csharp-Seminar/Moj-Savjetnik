namespace MyAdvisor.Domain.Entities
{
    public class Category
    {
        private readonly List<Category> _subCategories = new();

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }
        public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

        private Category() { Name = null!; } // For EF Core

        public Category(string name, int? parentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.", nameof(name));

            Name = name;
            ParentCategoryId = parentCategoryId;
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.", nameof(name));

            Name = name;
        }
    }
}

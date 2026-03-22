namespace MyAdvisor.Domain
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public int? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }

        public List<Category> SubCategories { get; private set; } = new();
    }
}
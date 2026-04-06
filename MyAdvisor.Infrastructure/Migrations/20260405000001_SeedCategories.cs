using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAdvisor.Infrastructure.Migrations
{
    public partial class SeedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Root categories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: ["Id", "Name", "ParentCategoryId"],
                values: new object[,]
                {
                    { 1,  "Food & Dining",     null },
                    { 2,  "Transport",          null },
                    { 3,  "Shopping",           null },
                    { 4,  "Entertainment",      null },
                    { 5,  "Health & Wellness",  null },
                    { 6,  "Housing",            null },
                    { 7,  "Income",             null },
                    { 8,  "Other",              null }
                });

            // Subcategories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: ["Id", "Name", "ParentCategoryId"],
                values: new object[,]
                {
                    { 9,  "Groceries",          1 },
                    { 10, "Restaurants",        1 },
                    { 11, "Coffee",             1 },
                    { 12, "Fuel",               2 },
                    { 13, "Public Transport",   2 },
                    { 14, "Taxi & Ride",        2 },
                    { 15, "Clothes",            3 },
                    { 16, "Electronics",        3 },
                    { 17, "Movies & Events",    4 },
                    { 18, "Subscriptions",      4 },
                    { 19, "Gym",                5 },
                    { 20, "Pharmacy",           5 },
                    { 21, "Rent",               6 },
                    { 22, "Utilities",          6 },
                    { 23, "Salary",             7 },
                    { 24, "Freelance",          7 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValues: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24]);
        }
    }
}

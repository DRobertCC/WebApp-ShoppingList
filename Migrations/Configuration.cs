namespace WebApp_ShoppingList.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApp_ShoppingList.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp_ShoppingList.Models.WebApp_ShoppingListContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false; // Don't want to use the automatic migration, so we use the "Add-Migration Initial" command instead in the NuGet PM. Console
        }

        protected override void Seed(WebApp_ShoppingList.Models.WebApp_ShoppingListContext context) // Adding Mock data to the DB
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            context.ShoppingLists.AddOrUpdate(
                new ShoppingList
                {
                    Name = "Groceries",
                    Items = {
                        new Item { Name = "Milk" }, // No need to add Ids as EF will handle that for us!
                        new Item { Name = "Cornflakes" }, // new Item { Id = 1, Name = "Corn", ShoppingListId = 0 },
                        new Item { Name = "Strawberries" }
                    }
                },
                new ShoppingList
                {
                    Name = "Hardware",
                }
            );

        }
    }
}

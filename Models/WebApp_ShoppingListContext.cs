using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApp_ShoppingList.Models
{
    public class WebApp_ShoppingListContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WebApp_ShoppingListContext() : base("name=WebApp_ShoppingListContext")
        {
        }

        public System.Data.Entity.DbSet<WebApp_ShoppingList.Models.Item> Items { get; set; }

        public System.Data.Entity.DbSet<WebApp_ShoppingList.Models.ShoppingList> ShoppingLists { get; set; }
    }
}

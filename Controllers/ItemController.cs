using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp_ShoppingList.Models;

namespace WebApp_ShoppingList.Controllers
{
    public class ItemController : ApiController
    {
        // POST: api/Item
        public IHttpActionResult Post([FromBody]Item item)
        {
            ShoppingList shoppingList =
                ShoppingListController.shoppingLists
                .Where(s => s.Id == item.ShoppingListId)
                .FirstOrDefault(); // shoppingLists has to be public static in ShoppingListController!

            if (shoppingList == null)
            {
                return NotFound();
            }

            item.Id = shoppingList.Items.Max(i => i.Id) + 1;
            shoppingList.Items.Add(item);

            return Ok(shoppingList);
        }

        // PUT: api/Item/5
        public IHttpActionResult Put(int id, [FromBody]Item item)
        {   // First we look fot the shopping list ...
            ShoppingList shoppingList =
                ShoppingListController.shoppingLists
                .Where(s => s.Id == item.ShoppingListId)
                .FirstOrDefault(); // shoppingLists has to be public static in ShoppingListController!

            if (shoppingList == null)
            {
                return NotFound();
            }
            // ... then the item in the shopping list.
            Item changedItem = shoppingList.Items.Where(i => i.Id == id).FirstOrDefault();

            if (changedItem == null)
            {
                return NotFound();
            }

            changedItem.Checked = item.Checked;

            return Ok(shoppingList);
        }

        // DELETE: api/Item/5
        public IHttpActionResult Delete(int id)
        {
            ShoppingList shoppingList = ShoppingListController.shoppingLists[0];

            Item item = shoppingList.Items.FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            shoppingList.Items.Remove(item);

            return Ok(shoppingList);
        }
    }
}

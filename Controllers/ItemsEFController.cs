using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp_ShoppingList.Models;

namespace WebApp_ShoppingList.Controllers
{
    public class ItemsEFController : ApiController
    {
        private WebApp_ShoppingListContext db = new WebApp_ShoppingListContext();

        // GET: api/ItemsEF
        public IQueryable<Item> GetItems()
        {
            return db.Items;
        }

        // GET: api/ItemsEF/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult GetItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/ItemsEF/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult PutItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // return StatusCode(HttpStatusCode.NoContent); // instead of this we return the updated item to the client to show it there:
            return Ok(item); // and change the ResponseType to Item uptop
        }

        // POST: api/ItemsEF
        [ResponseType(typeof(ShoppingList))]
        public IHttpActionResult PostItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // First check if the ShoppingList exists...
            ShoppingList shoppingList = db.ShoppingLists
                .Where(s => s.Id == item.ShoppingListId)
                .Include(s => s.Items)
                .FirstOrDefault();
            if (shoppingList == null)
            {
                return NotFound();
            }
            // ... if exists we can add the item to it.
            db.Items.Add(item);
            db.SaveChanges();

            // return CreatedAtRoute("DefaultApi", new { id = item.Id }, item); // instead of this we return the whole list to the client:
            return Ok(shoppingList); // and change the ResponseType to ShoppingList uptop
        }

        // DELETE: api/ItemsEF/5
        [ResponseType(typeof(ShoppingList))]
        public IHttpActionResult DeleteItem(int id)
        {
            // First check if the Item exists...
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            // Then check if the ShoppingList exists...
            ShoppingList shoppingList = db.ShoppingLists
                .Where(s => s.Id == item.ShoppingListId)
                .Include(s => s.Items)
                .FirstOrDefault();

            db.Items.Remove(item);
            db.SaveChanges();
            // Finally we send back the shoppingList (ResponseType changed too)
            return Ok(shoppingList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }
    }
}
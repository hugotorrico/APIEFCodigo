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
using APIEFCodigo.Models;

namespace APIEFCodigo.Controllers
{
    public class ProductsController : ApiController
    {
        private TekTonDBEntities db = new TekTonDBEntities();


        // GET: api/Products
        public IQueryable<Products> GetProducts()
        {
            return db.Products.Where(x=>x.IsActive==true) ;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Products))]
        public IHttpActionResult GetProducts(int id)
        {
            Products products = db.Products
                            .Where(x => x.ProductId == id && x.IsActive == true)
                            .FirstOrDefault();

            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        //public string GetProducts(int id)
        //{
         
        //    Products products = db.Products
        //                    .Where(x => x.ProductId == id)
        //                    .FirstOrDefault();

        //    if (products == null)
        //        return "Producto no encontrado";

        //    if (products.IsActive==true)
        //        return "Producto encontrado";
        //    else
        //        return "Producto ha sido eliminado";

        //}

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProducts(int id, Products products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != products.ProductId)
            {
                return BadRequest();
            }

            db.Entry(products).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        //[ResponseType(typeof(Products))]
        //public IHttpActionResult PostProducts(Products products)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Products.Add(products);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = products.ProductId }, products);
        //}


        // POST: api/Products
        [ResponseType(typeof(List< Products>))]
        public IHttpActionResult PostProducts(List< Products> products)
        {
        
            db.Products.AddRange(products);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = products[0].ProductId }, products);
        }


        // DELETE: api/Products/5         
        [ResponseType(typeof(Products))]
        public IHttpActionResult DeleteProducts(int id)
        {

            //Buscar el producto que voy a eliminar

            Products product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Entry(product).State = EntityState.Modified;
            
            product.IsActive = false;

            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}
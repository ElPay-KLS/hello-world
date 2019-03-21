using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
using upsiltry.Models;

namespace upsiltry.Controllers
{
    public class bankcontroller : ApiController
    {
        private dbmodels db = new dbmodels();

        // GET: api/bankcontroller
        public IQueryable<bank> Getbanks()
        {
            return db.banks;
        }

        // GET: api/bankcontroller/5
        [ResponseType(typeof(bank))]
        public IHttpActionResult Getbank(int id)
        {
            bank bank = db.banks.Find(id);
            if (bank == null)
            {
                return NotFound();
            }

            return Ok(bank);
        }

        // PUT: api/bankcontroller/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putbank(int id, bank bank)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != bank.Id)
            {
                return BadRequest();
            }

            db.Entry(bank).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bankExists(id))
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

        // POST: api/bankcontroller
        [ResponseType(typeof(bank))]

        //public void Post([FromBody]  bank Bank)
        //{
        //    using (bankDBEntities entities = new bankDBEntities())
        //    {
        //        entities.bank.Add(Bank);
        //        entities.SaveChanges();
        //    }
        //}
        public IHttpActionResult Postbank(bank bank)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.banks.Add(bank);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bank.Id }, bank);
        }
        [JsonConverter(typeof(CustomDateConverter))]
        static public DateTime TransactionDate { get; set; }
        public class CustomDateConverter : IsoDateTimeConverter
        {
            public CustomDateConverter()
            {
                DateTimeFormat = "MMddyyyy";
            }
        }

        // DELETE: api/bankcontroller/5
        [ResponseType(typeof(bank))]
        public IHttpActionResult Deletebank(int id)
        {
            bank bank = db.banks.Find(id);
            if (bank == null)
            {
                return NotFound();
            }

            db.banks.Remove(bank);
            db.SaveChanges();

            return Ok(bank);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool bankExists(int id)
        {
            return db.banks.Count(e => e.Id == id) > 0;
        }

       
    }
   
}
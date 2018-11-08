using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Data.Contexto;
using Domain.Entidade;

namespace Web.Controllers.API
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Domain.Entidade;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Kaizen>("Kaizen");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class KaizenController : ODataController
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: odata/Kaizen
        [EnableQuery]
        public IQueryable<Kaizen> GetKaizen()
        {
            return db.Kaizen;
        }

        // GET: odata/Kaizen(5)
        [EnableQuery]
        public SingleResult<Kaizen> GetKaizen([FromODataUri] int key)
        {
            return SingleResult.Create(db.Kaizen.Where(kaizen => kaizen.KaizenId == key));
        }

        // PUT: odata/Kaizen(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Kaizen> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Kaizen kaizen = db.Kaizen.Find(key);
            if (kaizen == null)
            {
                return NotFound();
            }

            patch.Put(kaizen);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KaizenExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(kaizen);
        }

        // POST: odata/Kaizen
        public IHttpActionResult Post(Kaizen kaizen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Kaizen.Add(kaizen);
            db.SaveChanges();

            return Created(kaizen);
        }

        // PATCH: odata/Kaizen(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Kaizen> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Kaizen kaizen = db.Kaizen.Find(key);
            if (kaizen == null)
            {
                return NotFound();
            }

            patch.Patch(kaizen);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KaizenExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(kaizen);
        }

        // DELETE: odata/Kaizen(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Kaizen kaizen = db.Kaizen.Find(key);
            if (kaizen == null)
            {
                return NotFound();
            }

            db.Kaizen.Remove(kaizen);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KaizenExists(int key)
        {
            return db.Kaizen.Count(e => e.KaizenId == key) > 0;
        }
    }
}

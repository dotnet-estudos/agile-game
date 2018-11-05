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
using Data.Contexto;
using Domain.Entidade;

namespace Web.Controllers.API
{
    public class JogoesController : ApiController
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: api/Jogoes
        public IQueryable<Jogo> GetJogo()
        {
            return db.Jogo;
        }

        // GET: api/Jogoes/5
        [ResponseType(typeof(Jogo))]
        public IHttpActionResult GetJogo(int id)
        {
            Jogo jogo = db.Jogo.Find(id);
            if (jogo == null)
            {
                return NotFound();
            }

            return Ok(jogo);
        }

        // PUT: api/Jogoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJogo(int id, Jogo jogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jogo.JogoId)
            {
                return BadRequest();
            }

            db.Entry(jogo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JogoExists(id))
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

        // POST: api/Jogoes
        [ResponseType(typeof(Jogo))]
        public IHttpActionResult PostJogo(Jogo jogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Jogo.Add(jogo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = jogo.JogoId }, jogo);
        }

        // DELETE: api/Jogoes/5
        [ResponseType(typeof(Jogo))]
        public IHttpActionResult DeleteJogo(int id)
        {
            Jogo jogo = db.Jogo.Find(id);
            if (jogo == null)
            {
                return NotFound();
            }

            db.Jogo.Remove(jogo);
            db.SaveChanges();

            return Ok(jogo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JogoExists(int id)
        {
            return db.Jogo.Count(e => e.JogoId == id) > 0;
        }
    }
}
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
    public class LancamentoChamadosController : ApiController
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: api/LancamentoChamados
        public IQueryable<LancamentoChamado> GetLancamentoChamado()
        {
            return db.LancamentoChamado;
        }

        // GET: api/LancamentoChamados/5
        [ResponseType(typeof(LancamentoChamado))]
        public IHttpActionResult GetLancamentoChamado(int id)
        {
            LancamentoChamado lancamentoChamado = db.LancamentoChamado.Find(id);
            if (lancamentoChamado == null)
            {
                return NotFound();
            }

            return Ok(lancamentoChamado);
        }

        // PUT: api/LancamentoChamados/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLancamentoChamado(int id, LancamentoChamado lancamentoChamado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lancamentoChamado.LancamentoChamadoId)
            {
                return BadRequest();
            }

            db.Entry(lancamentoChamado).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LancamentoChamadoExists(id))
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

        // POST: api/LancamentoChamados
        [ResponseType(typeof(LancamentoChamado))]
        public IHttpActionResult PostLancamentoChamado(LancamentoChamado lancamentoChamado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LancamentoChamado.Add(lancamentoChamado);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lancamentoChamado.LancamentoChamadoId }, lancamentoChamado);
        }

        // DELETE: api/LancamentoChamados/5
        [ResponseType(typeof(LancamentoChamado))]
        public IHttpActionResult DeleteLancamentoChamado(int id)
        {
            LancamentoChamado lancamentoChamado = db.LancamentoChamado.Find(id);
            if (lancamentoChamado == null)
            {
                return NotFound();
            }

            db.LancamentoChamado.Remove(lancamentoChamado);
            db.SaveChanges();

            return Ok(lancamentoChamado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LancamentoChamadoExists(int id)
        {
            return db.LancamentoChamado.Count(e => e.LancamentoChamadoId == id) > 0;
        }
    }
}
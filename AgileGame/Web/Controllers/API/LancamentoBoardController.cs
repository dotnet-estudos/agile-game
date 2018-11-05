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
    public class LancamentoBoardController : ApiController
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: api/LancamentoBoards
        public IQueryable<LancamentoBoard> GetLancamentoBoard()
        {
            return db.LancamentoBoard;
        }

        // GET: api/LancamentoBoards/5
        [ResponseType(typeof(LancamentoBoard))]
        public IHttpActionResult GetLancamentoBoard(int id)
        {
            LancamentoBoard lancamentoBoard = db.LancamentoBoard.Find(id);
            if (lancamentoBoard == null)
            {
                return NotFound();
            }

            return Ok(lancamentoBoard);
        }

        // PUT: api/LancamentoBoards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLancamentoBoard(int id, LancamentoBoard lancamentoBoard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lancamentoBoard.LancamentoBoardId)
            {
                return BadRequest();
            }

            db.Entry(lancamentoBoard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LancamentoBoardExists(id))
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

        // POST: api/LancamentoBoards
        [ResponseType(typeof(LancamentoBoard))]
        public IHttpActionResult PostLancamentoBoard(LancamentoBoard lancamentoBoard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LancamentoBoard.Add(lancamentoBoard);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lancamentoBoard.LancamentoBoardId }, lancamentoBoard);
        }

        // DELETE: api/LancamentoBoards/5
        [ResponseType(typeof(LancamentoBoard))]
        public IHttpActionResult DeleteLancamentoBoard(int id)
        {
            LancamentoBoard lancamentoBoard = db.LancamentoBoard.Find(id);
            if (lancamentoBoard == null)
            {
                return NotFound();
            }

            db.LancamentoBoard.Remove(lancamentoBoard);
            db.SaveChanges();

            return Ok(lancamentoBoard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LancamentoBoardExists(int id)
        {
            return db.LancamentoBoard.Count(e => e.LancamentoBoardId == id) > 0;
        }
    }
}
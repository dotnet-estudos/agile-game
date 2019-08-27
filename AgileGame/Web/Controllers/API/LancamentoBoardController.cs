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
using Microsoft.Ajax.Utilities;

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


        public object Get(string time, int jogo)
        {
            var lancamentos = db.LancamentoBoard
                                .Include(x => x.Fase)
                                .Include(x => x.Jogo)
                                .Include(x => x.Equipe)
                                .Where(x => x.JogoId == jogo)
                                .Where(x => time == null || x.Equipe.Hash.Equals(time)).ToList();

            var Dias = lancamentos.Select(x => x.DiaLancamento).Distinct().ToArray();

            var fases = db.Jogo.Include(x => x.Board)
                        .Include(x => x.Board.Fases)
                        .FirstOrDefault(x => x.JogoId == jogo)
                        .Board.Fases.ToList();


            var serie = new List<object>();

            fases.ForEach(fase =>
            {
                var count = Dias.Select(dia => lancamentos.Where(x => x.DiaLancamento == dia && x.FaseId == fase.FaseId).Sum(x=>x.Quantidade)).ToArray();

                serie.Add(new { name=fase.Nome, data = count, color= fase.RGB  });

            });
            
            var retorno = new { dias = Dias, fases = fases.Select(x=> new { x.FaseId, x.Nome }), serie = serie };

            return retorno;
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
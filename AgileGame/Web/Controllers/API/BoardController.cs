using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Data.Contexto;
using Domain.Entidade;
using Newtonsoft.Json;
using Web.Models;

namespace Web.Controllers.API
{
    public class BoardController : ApiController
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: api/Boards
        public IQueryable<Board> GetBoard()
        {
            return db.Board;
        }

        // GET: api/Boards/5
        [ResponseType(typeof(Board))]
        public IHttpActionResult GetBoard(int id)
        {
            Board board = db.Board.Find(id);
            if (board == null)
            {
                return NotFound();
            }

            return Ok(board);
        }

        // PUT: api/Boards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBoard(int id, Board board)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != board.BoardId)
            {
                return BadRequest();
            }

            db.Entry(board).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
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

        // PUT: api/Boards/5
        [ResponseType(typeof(void))]
        [Route("api/Board/PutViewBoard")]
        [HttpPut]
        public IHttpActionResult GetViewBoard(int id, BoardViewModel board)
        {

            if (board.LancamentoKaizen != null && board.LancamentoKaizen.Count > 0)
            {
                var lancamentoKaizen = new LancamentoKaizen();

                foreach (var item in board.LancamentoKaizen)
                {
                    lancamentoKaizen.DiaConlcusao = item.DiaConlcusao;
                    lancamentoKaizen.DiaInicio = item.DiaInicio;
                    lancamentoKaizen.JogoId = board.jogo.JogoId;
                    var kaizen = db.Kaizen.FirstOrDefault(x => x.Letra.Equals(board.Kaizen.Letra));
                    if (kaizen != null)
                        lancamentoKaizen.KaizenId =
                            kaizen.KaizenId;

                    if (lancamentoKaizen.KaizenId == null)
                    {

                        return Content(HttpStatusCode.BadRequest, new {mensagem = "Kaizem Não Encontrado"});
                    }
                }

                db.LancamentoKaizen.Add(lancamentoKaizen);


            }


            //db.Entry(board).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                return Content(HttpStatusCode.OK, new { mensagem = "OK" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
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

        // POST: api/Boards
        [ResponseType(typeof(Board))]
        public IHttpActionResult PostBoard(Board board)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Board.Add(board);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = board.BoardId }, board);
        }

        // DELETE: api/Boards/5
        [ResponseType(typeof(Board))]
        public IHttpActionResult DeleteBoard(int id)
        {
            Board board = db.Board.Find(id);
            if (board == null)
            {
                return NotFound();
            }

            db.Board.Remove(board);
            db.SaveChanges();

            return Ok(board);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BoardExists(int id)
        {
            return db.Board.Count(e => e.BoardId == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.UI.WebControls.Adapters;
using Data.Contexto;
using Domain.Entidade;
using Web.Models;

namespace Web.Controllers
{
    public class BoardController : Controller
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: Board
        public ActionResult Index()
        {
            return View(db.Board.ToList());
        }

        // GET: Board/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Board.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // GET: Board/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Board/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BoardId,Nome,Ativo")] Board board)
        {
            if (ModelState.IsValid)
            {
                db.Board.Add(board);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(board);
        }

        // GET: Board/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Board.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Board/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BoardId,Nome,Ativo")] Board board)
        {
            if (ModelState.IsValid)
            {
                db.Entry(board).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(board);
        }


       
        
        [System.Web.Mvc.Route("/Board/PutViewBoard")]
        [System.Web.Mvc.HttpPut]
        public JsonResult PutViewBoard(int id, BoardViewModel board)
        {

            #region LancamentoKaizen    
            if (board.LancamentoKaizen != null && board.LancamentoKaizen.Count > 0)
            {
                

                foreach (var item in board.LancamentoKaizen)
                {
                    var lancamentoKaizen = new LancamentoKaizen
                    {
                        DiaConlcusao = item.DiaConlcusao,
                        DiaInicio = item.DiaInicio,
                        JogoId = board.jogo.JogoId,
                        EquipeId = board.Equipe.EquipeId
                    };
                    var kaizen = db.Kaizen.FirstOrDefault(x => x.Letra.Equals(board.Kaizen.Letra.ToUpper()));
                    if (kaizen != null)
                        lancamentoKaizen.KaizenId =
                            kaizen.KaizenId;

                    if (lancamentoKaizen.KaizenId == null)
                    {

                        return Json( new { mensagem = "Kaizem Não Encontrado", status = true },JsonRequestBehavior.DenyGet);
                    }



                    if (
                        !db.LancamentoKaizen.Any(
                            x =>
                                x.KaizenId == lancamentoKaizen.KaizenId && x.EquipeId == lancamentoKaizen.EquipeId &&
                                x.JogoId == lancamentoKaizen.JogoId))
                    {
                        db.LancamentoKaizen.Add(lancamentoKaizen);
                    }
                    else
                    {
                        return Json(new { mensagem = "Kaizem Já foi enviado Anteriormente", status = false }, JsonRequestBehavior.DenyGet);
                    }
                }


            }
            #endregion

            #region LancamanChamado

            
            if (board.LancamentoChamado != null && board.LancamentoChamado.Count > 0)
            {
                

                foreach (var item in board.LancamentoChamado)
                {
                    var lancamentoChamado = new LancamentoChamado
                    {
                        ChamadoNumero = item.ChamadoNumero.ToUpper(),
                        DiaAbertura = item.DiaAbertura,
                        DiaFechamento = item.DiaFechamento,
                        JogoId = board.jogo.JogoId,
                        EquipeId = board.Equipe.EquipeId
                    };


                    if (!db.LancamentoChamado.Any(x => x.ChamadoNumero.Equals(lancamentoChamado.ChamadoNumero)
                                                       && x.EquipeId == lancamentoChamado.EquipeId
                                                       && x.JogoId == lancamentoChamado.JogoId))
                    {
                        db.LancamentoChamado.Add(lancamentoChamado);
                    }
                    else
                    {
                        return Json(new { mensagem = "Chamado Já foi enviado Anteriormente", status = false }, JsonRequestBehavior.DenyGet);
                    }
                }

            }
            #endregion

            #region LancamentoBoard
            if (board.LancamentoBoard != null && board.LancamentoBoard.Count > 0)
            {


                foreach (var item in board.LancamentoBoard)
                {
                    var lancamentoBoard = new LancamentoBoard
                    {
                        DiaLancamento = item.DiaLancamento,
                        FaseId = item.FaseId,
                        EquipeId = board.Equipe.EquipeId,
                        JogoId = board.jogo.JogoId,
                        Quantidade = item.Quantidade
                    };


                    if (!db.LancamentoBoard.Any(x => x.DiaLancamento == item.DiaLancamento
                                                     && x.EquipeId == item.EquipeId
                                                     && x.JogoId == item.JogoId))
                    {
                        db.LancamentoBoard.Add(lancamentoBoard);
                    }
                    else
                    {
                        return Json(new { mensagem = "Dia Já foi enviado Anteriormente, caso preciso de correção solicite ao Master do Jogo", status = false }, JsonRequestBehavior.DenyGet);
                    }
                }
            }
            #endregion

            try
            {
                db.SaveChanges();

                return Json(new { mensagem = "Enviado Com Sucesso", status = true }, JsonRequestBehavior.DenyGet);
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }

            return Json(HttpStatusCode.NoContent);
        }





        // GET: Board/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Board.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Board/Delete/5
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Board board = db.Board.Find(id);
            db.Board.Remove(board);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

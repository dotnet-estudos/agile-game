using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Contexto;
using Domain.Entidade;

namespace Web.Controllers
{
    public class LancamentoBoardController : Controller
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: LancamentoBoard
        public ActionResult Index()
        {
            var lancamentoBoard = db.LancamentoBoard.Include(l => l.Equipe).Include(l => l.Fase).Include(l => l.Jogo);
            return View(lancamentoBoard.ToList());
        }

        // GET: LancamentoBoard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoBoard lancamentoBoard = db.LancamentoBoard.Find(id);
            if (lancamentoBoard == null)
            {
                return HttpNotFound();
            }
            return View(lancamentoBoard);
        }

        // GET: LancamentoBoard/Create
        public ActionResult Create()
        {
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash");
            ViewBag.FaseId = new SelectList(db.Fase, "FaseId", "Nome");
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma");
            return View();
        }

        // POST: LancamentoBoard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LancamentoBoardId,JogoId,EquipeId,DiaLancamento,FaseId,Quantidade")] LancamentoBoard lancamentoBoard)
        {
            if (ModelState.IsValid)
            {
                db.LancamentoBoard.Add(lancamentoBoard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoBoard.EquipeId);
            ViewBag.FaseId = new SelectList(db.Fase, "FaseId", "Nome", lancamentoBoard.FaseId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoBoard.JogoId);
            return View(lancamentoBoard);
        }

        // GET: LancamentoBoard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoBoard lancamentoBoard = db.LancamentoBoard.Find(id);
            if (lancamentoBoard == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoBoard.EquipeId);
            ViewBag.FaseId = new SelectList(db.Fase, "FaseId", "Nome", lancamentoBoard.FaseId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoBoard.JogoId);
            return View(lancamentoBoard);
        }

        // POST: LancamentoBoard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LancamentoBoardId,JogoId,EquipeId,DiaLancamento,FaseId,Quantidade")] LancamentoBoard lancamentoBoard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lancamentoBoard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoBoard.EquipeId);
            ViewBag.FaseId = new SelectList(db.Fase, "FaseId", "Nome", lancamentoBoard.FaseId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoBoard.JogoId);
            return View(lancamentoBoard);
        }

        // GET: LancamentoBoard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoBoard lancamentoBoard = db.LancamentoBoard.Find(id);
            if (lancamentoBoard == null)
            {
                return HttpNotFound();
            }
            return View(lancamentoBoard);
        }

        // POST: LancamentoBoard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LancamentoBoard lancamentoBoard = db.LancamentoBoard.Find(id);
            db.LancamentoBoard.Remove(lancamentoBoard);
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

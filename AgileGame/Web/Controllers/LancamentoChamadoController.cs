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
    public class LancamentoChamadoController : Controller
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: LancamentoChamado
        public ActionResult Index()
        {
            var lancamentoChamado = db.LancamentoChamado.Include(l => l.Equipe).Include(l => l.Jogo);
            return View(lancamentoChamado.ToList());
        }

        // GET: LancamentoChamado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoChamado lancamentoChamado = db.LancamentoChamado.Find(id);
            if (lancamentoChamado == null)
            {
                return HttpNotFound();
            }
            return View(lancamentoChamado);
        }

        // GET: LancamentoChamado/Create
        public ActionResult Create()
        {
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash");
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma");
            return View();
        }

        // POST: LancamentoChamado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LancamentoChamadoId,EquipeId,JogoId,ChamadoNumero,DiaAbertura,DiaFechamento,DataCadastro")] LancamentoChamado lancamentoChamado)
        {
            if (ModelState.IsValid)
            {
                db.LancamentoChamado.Add(lancamentoChamado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoChamado.EquipeId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoChamado.JogoId);
            return View(lancamentoChamado);
        }

        // GET: LancamentoChamado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoChamado lancamentoChamado = db.LancamentoChamado.Find(id);
            if (lancamentoChamado == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoChamado.EquipeId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoChamado.JogoId);
            return View(lancamentoChamado);
        }

        // POST: LancamentoChamado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LancamentoChamadoId,EquipeId,JogoId,ChamadoNumero,DiaAbertura,DiaFechamento,DataCadastro")] LancamentoChamado lancamentoChamado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lancamentoChamado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoChamado.EquipeId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoChamado.JogoId);
            return View(lancamentoChamado);
        }

        // GET: LancamentoChamado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoChamado lancamentoChamado = db.LancamentoChamado.Find(id);
            if (lancamentoChamado == null)
            {
                return HttpNotFound();
            }
            return View(lancamentoChamado);
        }

        // POST: LancamentoChamado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LancamentoChamado lancamentoChamado = db.LancamentoChamado.Find(id);
            db.LancamentoChamado.Remove(lancamentoChamado);
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

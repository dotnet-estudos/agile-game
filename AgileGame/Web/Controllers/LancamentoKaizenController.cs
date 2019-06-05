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
    [Authorize]
    public class LancamentoKaizenController : BaseController
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: LancamentoKaizen
        public ActionResult Index()
        {
            var lancamentoKaizen = db.LancamentoKaizen.Include(l => l.Equipe).Include(l => l.Jogo).Include(l => l.Kaizen);
            return View(lancamentoKaizen.ToList());
        }

        // GET: LancamentoKaizen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoKaizen lancamentoKaizen = db.LancamentoKaizen.Find(id);
            if (lancamentoKaizen == null)
            {
                return HttpNotFound();
            }
            return View(lancamentoKaizen);
        }

        // GET: LancamentoKaizen/Create
        public ActionResult Create()
        {
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash");
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma");
            ViewBag.KaizenId = new SelectList(db.Kaizen, "KaizenId", "Letra");
            return View();
        }

        // POST: LancamentoKaizen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LancamentoKaizenId,KaizenId,JogoId,EquipeId,DiaConlcusao,DiaInicio")] LancamentoKaizen lancamentoKaizen)
        {
            if (ModelState.IsValid)
            {
                db.LancamentoKaizen.Add(lancamentoKaizen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoKaizen.EquipeId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoKaizen.JogoId);
            ViewBag.KaizenId = new SelectList(db.Kaizen, "KaizenId", "Letra", lancamentoKaizen.KaizenId);
            return View(lancamentoKaizen);
        }

        // GET: LancamentoKaizen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoKaizen lancamentoKaizen = db.LancamentoKaizen.Find(id);
            if (lancamentoKaizen == null)
            {
                return HttpNotFound();
            }
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoKaizen.EquipeId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoKaizen.JogoId);
            ViewBag.KaizenId = new SelectList(db.Kaizen, "KaizenId", "Letra", lancamentoKaizen.KaizenId);
            return View(lancamentoKaizen);
        }

        // POST: LancamentoKaizen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LancamentoKaizenId,KaizenId,JogoId,EquipeId,DiaConlcusao,DiaInicio")] LancamentoKaizen lancamentoKaizen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lancamentoKaizen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EquipeId = new SelectList(db.Equipe, "EquipeId", "Hash", lancamentoKaizen.EquipeId);
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", lancamentoKaizen.JogoId);
            ViewBag.KaizenId = new SelectList(db.Kaizen, "KaizenId", "Letra", lancamentoKaizen.KaizenId);
            return View(lancamentoKaizen);
        }

        // GET: LancamentoKaizen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LancamentoKaizen lancamentoKaizen = db.LancamentoKaizen.Find(id);
            if (lancamentoKaizen == null)
            {
                return HttpNotFound();
            }
            return View(lancamentoKaizen);
        }

        // POST: LancamentoKaizen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LancamentoKaizen lancamentoKaizen = db.LancamentoKaizen.Find(id);
            db.LancamentoKaizen.Remove(lancamentoKaizen);
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

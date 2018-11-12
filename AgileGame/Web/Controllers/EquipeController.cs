using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Data.Contexto;
using Domain.Entidade;
using Microsoft.Ajax.Utilities;
using Web.Common;

namespace Web.Controllers
{
    public class EquipeController : Controller
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: Equipe
        public ActionResult Index()
        {
            var equipe = db.Equipe.Include(e => e.Jogo);
            return View(equipe.ToList());
        }

        // GET: Equipe/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = db.Equipe.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        // GET: Equipe/Create
        public ActionResult Create()
        {
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma");
            return View();
        }

        // POST: Equipe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EquipeId,Hash,Nome,JogoId")] Equipe equipe)
        {
            if (ModelState.IsValid)
            {
                db.Equipe.Add(equipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", equipe.JogoId);
            return View(equipe);
        }

        // GET: Equipe/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = db.Equipe.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", equipe.JogoId);
            return View(equipe);
        }

        // POST: Equipe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EquipeId,Hash,Nome,JogoId")] Equipe equipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", equipe.JogoId);
            return View(equipe);
        }



        public ActionResult Registrar(string id)
        {
            if (id.IsNullOrWhiteSpace())
            {
                return View();
            }
            var equipEdit = db.Equipe.FirstOrDefault(x => x.Hash.Equals(id));
            if (equipEdit.Nome.IsNullOrWhiteSpace())
            {
                return View(equipEdit).Mensagem("Registre o Nome da Equipe");
            }
            return RedirectToAction("Board", "Jogo", id);
        }



        // POST: Equipe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "EquipeId,Hash,Nome,JogoId")] Equipe equipe)
        {

            var equipEdit = db.Equipe.FirstOrDefault(x => x.Hash.Equals(equipe.Hash));
            if (equipEdit != null)
            {
                equipEdit.Nome = equipe.Nome;
                db.Entry(equipEdit).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Board", "Jogo",new {id= equipe.Hash});

            // ViewBag.JogoId = new SelectList(db.Jogo, "JogoId", "NomeTurma", equipe.JogoId);
            return View();
        }

        // GET: Equipe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipe equipe = db.Equipe.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        // POST: Equipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipe equipe = db.Equipe.Find(id);
            db.Equipe.Remove(equipe);
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

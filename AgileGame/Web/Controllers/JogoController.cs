using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Data.Contexto;
using Domain.Entidade;
using Microsoft.Ajax.Utilities;
using Web.Models;
using WebGrease.Css.Extensions;

namespace Web.Controllers
{
    public class JogoController : Controller
    {
        private AgileGameContexto db = new AgileGameContexto();

        // GET: Jogo
        public ActionResult Index()
        {
            var jogo = db.Jogo.Include(j => j.Board);
            return View(jogo.ToList());
        }

        // GET: Jogo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogo jogo = db.Jogo.Find(id);
            if (jogo == null)
            {
                return HttpNotFound();
            }
            return View(jogo);
        }


        // GET: Jogo/Details/5
        public ActionResult Board(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Jogo jogo = db.Jogo
                .Include(x=>x.Equipes)
                .Include(x=>x.Board)
                .Include(x=>x.Board.Fases)
                .Include(x=>x.LancamentoBoard)
                .Include(x => x.LancamentoChamado)
                .Include(x => x.LancamentoKaizen)
                .FirstOrDefault(x=>x.Equipes.Any(y=>y.Hash.Equals(id)));


            var listSemanas = new List<SemanaCombo>();

            for (int i = 1; i < (50/7); i++)
            {
                listSemanas.Add(new SemanaCombo{ Dia = i*7, Semana = i.ToString("Semana ##")});
            }
            
            if (jogo == null)
            {
                return HttpNotFound();
            }

            var ultimaSemana = 0;
            foreach (var dia in jogo.LancamentoBoard.OrderByDescending(x => x.DiaLancamento).Select(x => x.DiaLancamento))
            {
                ultimaSemana = dia;
            }

            ViewBag.SemanaDia = new SelectList(listSemanas, "Dia", "Semana", ultimaSemana);

            jogo.Equipes = jogo.Equipes.Where(x => x.Hash.Equals(id)).ToList();


            if (jogo.Equipes.Any(x => x.Hash.Equals(id) && x.Nome.IsNullOrWhiteSpace()))
                return RedirectToAction("Registrar", "Equipe", id);

            return View(jogo);
        }

        // GET: Jogo/Create
        public ActionResult Create()
        {
            ViewBag.BoardId = new SelectList(db.Board, "BoardId", "Nome");
            return View();
        }

        // POST: Jogo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JogoId,BoardId,NomeTurma,QuantidadeEquipes,DataCadastro,Finalizado,DataHoraFim")] Jogo jogo)
        {
            if (ModelState.IsValid)
            {

                jogo.Equipes =new List<Equipe>();;

                for (int i = 0; i < jogo.QuantidadeEquipes; i++)
                {
                   jogo.Equipes.Add(new Equipe
                   {
                       Hash = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[^0-9a-zA-Z]+", "").Substring(1, 5).ToUpper(),
                       Jogo =jogo,
                       
                   });
                }
                db.Jogo.Add(jogo);
                db.SaveChanges();
                return View("RegistroEquipes", jogo);
            }

            ViewBag.BoardId = new SelectList(db.Board, "BoardId", "Nome", jogo.BoardId);
            return View(jogo);
        }

        public ActionResult RegistroEquipes(int id)
        {
            var jogo = db.Jogo.Include(x=>x.Equipes).FirstOrDefault(x => x.JogoId == id);
            return View("RegistroEquipes", jogo);
        }

        // GET: Jogo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogo jogo = db.Jogo.Find(id);
            if (jogo == null)
            {
                return HttpNotFound();
            }
            ViewBag.BoardId = new SelectList(db.Board, "BoardId", "Nome", jogo.BoardId);
            return View(jogo);
        }

        // POST: Jogo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JogoId,BoardId,NomeTurma,QuantidadeEquipes,DataCadastro,Finalizado,DataHoraFim")] Jogo jogo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jogo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BoardId = new SelectList(db.Board, "BoardId", "Nome", jogo.BoardId);
            return View(jogo);
        }

        // GET: Jogo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogo jogo = db.Jogo.Find(id);
            if (jogo == null)
            {
                return HttpNotFound();
            }
            return View(jogo);
        }

        // POST: Jogo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jogo jogo = db.Jogo.Find(id);
            db.Jogo.Remove(jogo);
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

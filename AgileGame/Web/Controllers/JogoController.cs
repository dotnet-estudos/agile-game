using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Data.Contexto;
using Domain.Entidade;
using Microsoft.Ajax.Utilities;
using Web.Common;
using Web.Models;
using WebGrease.Css.Extensions;

namespace Web.Controllers
{
    [Authorize]
    public class JogoController : BaseController
    {
        private AgileGameContexto db = new AgileGameContexto();

        
        // GET: Jogo
        
        public ActionResult Index()
        {
            var jogo = db.Jogo
                .Include(x => x.Equipes)
                .Include(x => x.Board);
            return View(jogo.ToList().OrderByDescending(x=>x.JogoId));
        }


        // GET: Jogo/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Jogo jogo = db.Jogo
                .Include(x => x.Equipes)
                .Include(x => x.Board)
                .Include(x => x.Board.Fases)
                .Include(x => x.LancamentoBoard)
                .Include(x => x.LancamentoBoard.Select(y=>y.Fase))
                .Include(x => x.LancamentoChamado)
                .Include(x => x.LancamentoKaizen)
                .Include(x => x.LancamentoKaizen.Select(y => y.Kaizen))
                .FirstOrDefault(x=>x.JogoId == id);
            if (jogo == null)
            {
                return HttpNotFound();
            }

            ViewBag.fases = jogo.Board.Fases;

            return View(jogo);
        }

        
        // GET: Jogo/Details/5
        [AllowAnonymous]
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

            ViewBag.JogoBag = jogo; 

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
                return RedirectToAction("Registrar", "Equipe", new {id});

            return View(jogo);
        }

        public ActionResult _LancamentoBoard(int id)
        {
            var returno = db.LancamentoBoard
                .Include(x=>x.Jogo.Board.Fases)
                .Include(x=>x.Equipe)
                .Where(x => x.EquipeId == id).ToList();

            if (returno == null || returno.Count == 0)
            {
                Equipe firstOrDefault = db.Equipe.FirstOrDefault(x => x.EquipeId == id);
                if (firstOrDefault != null)
                {
                    var eq = firstOrDefault.JogoId;
                    return RedirectToAction("Details",new { id=eq}).MensagemErro("Board Não foi Encontrado");
                }
                return RedirectToAction("index").MensagemErro("Board Não foi Encontrado");
            }


            var boardId = returno.Select(x => x.Jogo.BoardId).First();

            var listFases = db.Fase.Where(x => x.BoardId == boardId).ToList();

            ViewBag.Fases = listFases;

            if(Request.IsAjaxRequest())
            return PartialView(returno);

            return View(returno);
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
        public ActionResult Create([Bind(Include = "JogoId,BoardId,NomeTurma,QuantidadeEquipes,DataCadastro,Finalizado,DataHoraFim,Hash")] Jogo jogo)
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
                jogo.Hash =
                    Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[^0-9a-zA-Z]+", "")
                        .Substring(1, 5)
                        .ToUpper();
                db.Jogo.Add(jogo);
                db.SaveChanges();
                return RedirectToAction("RegistroEquipes", new {id = jogo.JogoId});
            }

            ViewBag.BoardId = new SelectList(db.Board, "BoardId", "Nome", jogo.BoardId);
            return View(jogo);
        }

        [AllowAnonymous]
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

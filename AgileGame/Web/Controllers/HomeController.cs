using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Data.Contexto;
using Domain.Entidade;
using Domain.Util;
using Web.Common;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private AgileGameContexto db = new AgileGameContexto();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        [HttpPost]
        public ActionResult Login(LoginViewModel pLogin)
        {
            FormsAuthentication.Initialize();

            var lUsuario = new Usuario
            {
                Login = pLogin.Login,
                Senha = pLogin.Senha
            };

            var usuario =
                db.Usuarios.FirstOrDefault(x => x.Login.Equals(lUsuario.Login) && x.Senha.Equals(CriarMd5.Criptografar(lUsuario.Senha)));

            if (usuario == null)
                return RedirectToAction("Login", "Home").MensagemErro("Falha na Autenticação, usuário e/ou senha inválido(s).");

            if (!usuario.Ativo)
                return RedirectToAction("Login", "Home").MensagemErro("Usuário Inativo.");

            AutenticarServico(usuario);
            Session["Usuario"] = usuario;
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Processa o Logout do site.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Método privado para Autenticar o Serviço.
        /// </summary>
        /// <param name="usuario"></param>
        private void AutenticarServico(Usuario usuario)
        {
            var autentic = new FormsAuthenticationTicket(usuario.Login, false, 120);

            var encryptedTicket = FormsAuthentication.Encrypt(autentic);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            if (autentic.IsPersistent) authCookie.Expires = autentic.Expiration;

            Response.Cookies.Add(authCookie);
            FormsAuthentication.SetAuthCookie(usuario.Login, true);
        }
    }
}
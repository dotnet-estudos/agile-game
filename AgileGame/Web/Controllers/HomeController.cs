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
            /*
            Global.Instance.CarregarParametros(ConfigurationManager.AppSettings);

            var resultglobal = Global.Instance.ConsultaCpfLista("84118040549");
           */
            if (Session["Usuario"] != null)
            {
                var mensage = Session["Mensagem"];

                if (mensage != null)
                {
                    var men = (Mensagem)mensage;

                    Session["Mensagem"] = null;

                    if (men.Erro)
                    {
                        return View("Index").MensagemErro(men.Mensage);
                    }
                    else
                    {
                        return View("Index").MensagemSucesso(men.Mensage);
                    }

                }


                return View("Index");
            } 


            return View("Login");

           
        }



        public ActionResult Login()
        {

            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel pLogin)
        {
            FormsAuthentication.Initialize();

            var lUsuario = new Usuario
            {
                Login = pLogin.Login,
                Senha = CriarMd5.Criptografar(pLogin.Senha)
            };

            var usuario =
                db.Usuarios.FirstOrDefault(x => x.Login.Equals(lUsuario.Login) && x.Senha.Equals(lUsuario.Senha));

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
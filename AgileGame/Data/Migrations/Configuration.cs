using System.Web.UI.WebControls;
using Data.Contexto;
using Domain.Entidade;

namespace Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Contexto.AgileGameContexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.Contexto.AgileGameContexto context)
        {
            #region Usuario

            context.Usuarios.AddOrUpdate(x => x.UsuarioId,
                new Usuario
                {
                    Email = "master@agilegame.local",
                    Login = "master",
                    Senha = "1c26d3784673de807d2c74bfbbade7f5",
                    UsuarioId = 1,
                    Ativo = true
                });

            #endregion

            #region Board

            context.Board.AddOrUpdate(x=>x.BoardId,
                  new Board
                  {
                      BoardId = 1,
                      Ativo = true,
                      Nome = "Sustentação"
                  });
           
            #endregion

            #region Fase

         
            Fase(context);
          
            #endregion

            #region Kaizen

            Kaizen(context);


            #endregion
            
            context.SaveChangesSeed();

        }

        private void Fase(AgileGameContexto context)
        {
            context.Fase.AddOrUpdate(x => x.FaseId,
                new Fase()
                {
                    Ativo = true,
                    BoardId = 1,
                    FaseId = 1,
                    Nome = "Backlog",
                    Ordem = 0,
                    RGB = "#1E90FF",
                    NomeAbreviado = "Bklog",
                    FaIcon = @"<i class='fas fa-clipboard-list'></i>"
                },
                new Fase()
                {
                    Ativo = true,
                    BoardId = 1,
                    FaseId = 2,
                    Nome = "Desenvolvimento",
                    Ordem = 1,
                    RGB = "#3CB371",
                    NomeAbreviado = "Dev",
                    FaIcon = @"<i class='fas fab fa-dev'></i>"
                },
                new Fase()
                {
                    Ativo = true,
                    BoardId = 1,
                    FaseId = 3,
                    Nome = "Pre-Homologação",
                    Ordem = 2,
                    RGB = "#FF6347",
                    NomeAbreviado = "P-Hml",
                    FaIcon = @"<i class='fas fa-clipboard-check'></i>"
                },
                new Fase()
                {
                    Ativo = true,
                    BoardId = 1,
                    FaseId = 4,
                    Nome = "Homologação",
                    Ordem = 3,
                    RGB = "#D3D3D3",
                    NomeAbreviado = "Hml",
                    FaIcon = @"<i class='fas fa-clipboard-check'></i>"
                },
                new Fase()
                {
                    Ativo = true,
                    BoardId = 1,
                    FaseId = 5,
                    Nome = "Deploy",
                    Ordem = 4,
                    RGB = "#FFEFD5",
                    NomeAbreviado = "Dep",
                     FaIcon = @"<i class='fab fa-deploydog'></i>",
                },
                new Fase()
                {
                    Ativo = true,
                    BoardId = 1,
                    FaseId = 6,
                    Nome = "Produção",
                    Ordem = 5,
                    RGB = "#D8BFD8",
                    NomeAbreviado = "Prd",
                    FaIcon = @"<i class='fab fa-product-hunt'></i>"
                }
                );
        }

        private void Kaizen(AgileGameContexto context)
        {
            context.Kaizen.AddOrUpdate(x => x.KaizenId,
                new Kaizen()
                {
                    KaizenId = 1,
                    Descricao = "Limitar WIP",
                    Esforco = 3,
                    Ativo = true,
                    Letra = "A",
                    Repeticao = 1,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 2,
                    Descricao = "Contratar DEV",
                    Esforco = 15,
                    Ativo = true,
                    Letra = "B",
                    Repeticao = 1,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 3,
                    Descricao = "Disponibilizar DEV para outro Time",
                    Esforco = 8,
                    Ativo = true,
                    Letra = "C",
                    Repeticao = 3,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 4,
                    Descricao = "Treinar Dev Para Atuar em Pre-Homologação",
                    Esforco = 9,
                    Ativo = true,
                    Letra = "D",
                    Repeticao = 1,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 5,
                    Descricao = "Solicitar ao Cliente para ter mais esforco para Homologação",
                    Esforco = 15,
                    Ativo = true,
                    Letra = "E",
                    Repeticao = 1,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 6,
                    Descricao = "Participação em Treinamento-Babas",
                    Esforco = 12,
                    Ativo = true,
                    Letra = "F",
                    Repeticao = 3,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 7,
                    Descricao = "Equalizar Time",
                    Esforco = 15,
                    Ativo = true,
                    Letra = "G",
                    Repeticao = 3,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 8,
                    Descricao = "Teste Automatizao",
                    Esforco = 20,
                    Ativo = true,
                    Letra = "H",
                    Repeticao = 1,
                    Requisito = "",
                },
                new Kaizen()
                {
                    KaizenId = 9,
                    Descricao = "One Click Deploy",
                    Esforco = 18,
                    Ativo = true,
                    Letra = "I",
                    Repeticao = 1,
                    Requisito = "H",
                },
                new Kaizen()
                {
                    KaizenId = 10,
                    Descricao = "Continuous Delivery",
                    Esforco = 15,
                    Ativo = true,
                    Letra = "j",
                    Repeticao = 1,
                    Requisito = "I",
                }
                );
        }
    }
}

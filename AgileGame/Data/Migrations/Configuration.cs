using System.Collections.Generic;
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
            ContextKey = "Data.Contexto.AgileGameContexto";
        }

        protected override void Seed(Data.Contexto.AgileGameContexto context)
        {
              context.Board.AddOrUpdate(x => x.BoardId,
                  new Board()
                {
                    BoardId = 1,
                    Ativo = true,
                    Nome = "Sustentação"
                });

                context.Fase.AddOrUpdate(x => x.FaseId,
                
                    new Fase()
                    {
                        Ativo = true,
                        BoardId = 1,
                        FaseId  = 1,
                        Nome =  "Backlog",
                        Ordem = 0,
                 
                    },
                     new Fase()
                    {
                        Ativo = true,
                        BoardId = 1,
                        FaseId  = 2,
                        Nome =  "Desenvolvimento",
                        Ordem = 1,
                 
                    },
                     new Fase()
                    {
                        Ativo = true,
                        BoardId = 1,
                        FaseId  = 3,
                        Nome =  "Pre-Homologação",
                        Ordem = 2,
                 
                    },
                     new Fase()
                    {
                        Ativo = true,
                        BoardId = 1,
                        FaseId  = 4,
                        Nome =  "Homologação",
                        Ordem = 3,
                 
                    },
                    new Fase()
                    {
                        Ativo = true,
                        BoardId = 1,
                        FaseId  = 5,
                        Nome =  "Deploy",
                        Ordem = 4,
                 
                    },
                     new Fase()
                    {
                        Ativo = true,
                        BoardId = 1,
                        FaseId  = 6,
                        Nome =  "Produção",
                        Ordem = 5,
                 
                    }
                );

            context.SaveChanges();
             
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Web;
using Domain.Entidade;

namespace Data.Contexto
{
    public class AgileGameContexto : DbContext
    {
        static AgileGameContexto()
        {
            Database.SetInitializer<AgileGameContexto>(new BdInitializer<AgileGameContexto>());

        }

        public AgileGameContexto()
            : base("Name=DefaultConnection")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }

        public AgileGameContexto(bool disabledProxyLazyLoad)
            : base("Name=DefaultConnection")
        {
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
            Configuration.LazyLoadingEnabled = disabledProxyLazyLoad;
            Configuration.ProxyCreationEnabled = disabledProxyLazyLoad;
        }

        public DbSet<Equipe> Equipe { get; set; }
        public DbSet<Jogo> Jogo { get; set; }
        public DbSet<LancamentoChamado> LancamentoChamado { get; set; }
        public DbSet<LogGame> LogGame { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fase> Fase { get; set; }
        public DbSet<Board> Board { get; set; }
        public DbSet<LancamentoBoard> LancamentoBoard { get; set; }
        
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties()
               .Where(p => p.Name == p.ReflectedType.Name + "Id")
               .Configure(p => p.IsKey());

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100));

          /*  modelBuilder.Configurations.Add(new ClienteConfiguration());
            modelBuilder.Configurations.Add(new ProdutoConfiguration());
            */

            /*
            modelBuilder.Configurations.Add(new EquipeMap());
            modelBuilder.Configurations.Add(new JogoMap());
            modelBuilder.Configurations.Add(new LancamentoChamadoMap());
            modelBuilder.Configurations.Add(new LogGameMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new SecretariasMap());
             * */
            
        }

        /// <summary>
        /// Obt�m uma string com a representa��o das chaves da entidade e seus respectivos valores
        /// </summary>
        /// <param name="entry">Entidade ou DbEntityEntry</param>
        /// <returns></returns>
        private string GetPrimaryKeyValue(object entry)
        {
            var keyNames = GetKeyNames(entry);
            var sb = new StringBuilder();
            sb.Append("{");
            for (var i = 0; i < keyNames.Length; i++)
            {
                sb.Append(keyNames[i] + " : " + GetKeyValue(entry, keyNames, i));
                sb.Append(i != keyNames.Length - 1 ? "," : "");
            }
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Obt�m o valor da chave
        /// </summary>
        /// <param name="entry">Entidade ou DbEntityEntry</param>
        /// <param name="keyNames">Array com o nome das chaves</param>
        /// <param name="i">contador da chave atual</param>
        /// <returns></returns>
        private static object GetKeyValue(object entry, string[] keyNames, int i)
        {
            if (!(entry is DbEntityEntry))
                return entry.GetType().GetProperty(keyNames[i]).GetValue(entry);

            var x = (DbEntityEntry)entry;

            if (x.State == EntityState.Added)
                return x.CurrentValues[keyNames[i]].ToString();

            return x.OriginalValues[keyNames[i]].ToString();
        }

        /// <summary>
        /// Obt�m o nome das chaves da entidade
        /// </summary>
        /// <param name="entity">Entidade ou DbEntityEntry</param>
        /// <returns></returns>
        private string[] GetKeyNames(object entity)
        {
            EntitySetBase setBase;
            var manager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;

            if (entity is DbEntityEntry)
                setBase = manager.GetObjectStateEntry(((DbEntityEntry)entity).Entity).EntitySet;
            else
                setBase = manager.GetObjectStateEntry(entity).EntitySet;

            var keyNames = setBase.ElementType.KeyMembers.Select(k => k.Name).ToArray();
            return keyNames;
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {

                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }

            }
            



            var added = new List<KeyValuePair<object, LogGame>>();

            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

            foreach (var dbEntityEntry in entries)
            {
                var entityName = dbEntityEntry.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(dbEntityEntry);


                switch (dbEntityEntry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                        var log = new LogGame
                        {
                            Tabela = entityName,
                            Acao = dbEntityEntry.State.ToString(),
                            NomeUsuario = HttpContext.Current.User.Identity.Name,
                            Chaves = primaryKey,
                            NomeColuna = "*",
                            Propriedade = "*",
                            ValorOriginal = String.Empty,
                            ValorAtual = String.Empty,
                            DataOcorrencia = DateTime.Now
                        };

                        Set<LogGame>().Add(log);

                        if (dbEntityEntry.State != EntityState.Added)
                            continue;

                        var kv = new KeyValuePair<object, LogGame>(dbEntityEntry.Entity, log);
                        added.Add(kv);
                        break;
                    case EntityState.Modified:
                        GeraLogUpdate(dbEntityEntry, entityName, primaryKey);
                        break;
                }
            }

            var result = base.SaveChanges();
            UpdateInsertedKey(added);
            return result;
        }

        /// <summary>
        /// Atualiza o registro das chaves geradas pelo banco
        /// </summary>
        /// <param name="added">Lista com as entrys e as suas respectiva entidades</param>
        private void UpdateInsertedKey(IEnumerable<KeyValuePair<object, LogGame>> added)
        {
            foreach (var keyvalues in added)
            {
                var log = Set<LogGame>().Find(keyvalues.Value.LogGameId);

                if (log == null)
                    continue;

                log.Chaves = GetPrimaryKeyValue(keyvalues.Key);
                base.SaveChanges();
            }
        }

        /// <summary>
        /// Gera o objeto de Log com as altera�oes realizadas no update
        /// </summary>
        /// <param name="dbEntityEntry"></param>
        /// <param name="entityName"></param>
        /// <param name="primaryKey"></param>
        private void GeraLogUpdate(DbEntityEntry dbEntityEntry, string entityName, string primaryKey)
        {
            foreach (var prop in dbEntityEntry.OriginalValues.PropertyNames)
            {
                var originalValue = (dbEntityEntry.OriginalValues[prop] != null
                    ? dbEntityEntry.OriginalValues[prop].ToString()
                    : "");
                var currentValue = (dbEntityEntry.CurrentValues[prop] != null
                    ? dbEntityEntry.CurrentValues[prop].ToString()
                    : "");

                if (originalValue == currentValue)
                    continue;

                var log = new LogGame
                {
                    Tabela = entityName,
                    Acao = dbEntityEntry.State.ToString(),
                    NomeUsuario = HttpContext.Current.User.Identity.Name,
                    Chaves = primaryKey,
                    NomeColuna = prop,
                    Propriedade = prop,
                    ValorOriginal = originalValue,
                    ValorAtual = currentValue,
                    DataOcorrencia = DateTime.Now
                };
                Set<LogGame>().Add(log);
            }
        }

        public bool AmbienteProducao()
        {
            return Database.Connection.ConnectionString.Contains("Data Source=10.2.8.134,1434;");
        }




        private class BdInitializer<T> : DropCreateDatabaseAlways<AgileGameContexto>
        {

            protected override void Seed(AgileGameContexto context)
            {


                context.Board.Add(new Board()
                {
                    BoardId = 1,
                    Ativo = true,
                    Nome = "Sustentação"
                });

                context.Fase.AddRange(new List<Fase>()
                {
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
                 
                    },
                });

                base.Seed(context);
            }
        } 


    }
}

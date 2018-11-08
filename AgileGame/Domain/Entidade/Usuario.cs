using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidade
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Login { get; set; }

        public string Senha { get; set; }

        public string Email { get; set; }

        public bool Ativo { get; set; }
    }
}

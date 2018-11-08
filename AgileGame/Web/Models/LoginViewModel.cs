using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class LoginViewModel
    {
        [DisplayName("Login: ")]
        [Required(ErrorMessage = "*")]

        public string Login { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Senha: ")]
        [Required(ErrorMessage = "*")]
        public string Senha { get; set; }


        [DataType(DataType.Password)]
        [DisplayName("Nova Senha: ")]
        public string NovaSenha { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirmar Senha: ")]
        //[Compare("NovaSenha", ErrorMessage = "Senha incorreta, tente novamente.")]
        [Required(ErrorMessage = "O campo Confirmar Senha: é obrigatório.")]
        public string ConfirmarSenha { get; set; }


    }
}
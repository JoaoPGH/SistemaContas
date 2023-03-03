using SistemaContas.Presentation.Models.Validations;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace SistemaContas.Presentation.Models
{
    public class AccountRegisterModel
    {
        [RegularExpression("^[A-Za-zÀ-Üà-ü\\s]{8,150}$", ErrorMessage = "Informe um nome válido de 8 a 150 caracteres.")]

        [Required(ErrorMessage = "Por favor, informe o nome do usuário.")]
        public string? Nome { get; set; }
        [EmailAddress(ErrorMessage = "Por favor, informe um endereço de email válido.")]

        [Required(ErrorMessage = "Por favor, informe o email do usuário.")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe a senha do usuário.")]
        [StrongPassword(ErrorMessage = "Por favor, informe uma senha forte de 8 a 20 caracteres com pelo menos 1 letra minúscula, 1 letra maiúscula, 1 número e 1 caractere especial")]
        public string? Senha { get; set; }
        [Compare("Senha", ErrorMessage = "Senhas não conferem,por favor verifique.")]
        
        [Required(ErrorMessage = "Por favor, confirme a senha do usuário.")]
        public string? SenhaConfirmacao { get; set; }
    }
}

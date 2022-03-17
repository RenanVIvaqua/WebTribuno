using System.ComponentModel.DataAnnotations;

namespace WebTribuno.Models
{
    public class UsuarioViewModel
    {

        [Required]
        [StringLength(60)]
        public string Nome { get; set; }

        [Required]
        [StringLength(20)]
        public string LoginUsuario { get; set; }

        [Required]
        [StringLength(15)]
        public string Senha { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
    }

}


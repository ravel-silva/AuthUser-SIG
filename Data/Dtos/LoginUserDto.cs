using System.ComponentModel.DataAnnotations;

namespace UserAuth.Data.Dtos
{
    public class LoginUserDto
    {
        [Required]
        public string PrefixoUsuario { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
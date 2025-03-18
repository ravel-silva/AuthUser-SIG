using System.ComponentModel.DataAnnotations;

namespace UserAuth.Data.Dtos
{
    public class CreateUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public int Matricula { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string NivelDeAcesso { get; set; }
        [Required]
        public DateTime DataDeRegistro { get; set; } = DateTime.Now;
    }
}

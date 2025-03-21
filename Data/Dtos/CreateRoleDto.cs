using System.ComponentModel.DataAnnotations;

namespace UserAuth.Data.Dtos
{
    public class CreateRoleDto
    {
        [Required]
        public string PrefixoRole { get; set; }
    }
}

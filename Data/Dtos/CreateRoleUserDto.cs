﻿namespace UserAuth.Data.Dtos
{
    public class CreateRoleUserDto
    {
        public string prefixoUsuario { get; set; }

        //public string UserId { get; set; }
        //  public string RoleId { get; set; }
        public List<string> RoleName { get; set; }
    }
}
﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UserAuth.Model
{
    public class User : IdentityUser
    {
        //userName --> o IdentityUser já tem
        public string PrefixoUsuario { get; set; } //prefixo do usuario
        [Required]
        public int Matricula { get; set; } //matricula do funcionario
        [Required]
        public string NivelDeAcesso { get; set; } //basic //admin //supervisor
        [Required]
        public DateTime DataDeRegisto { get; set; } = DateTime.Now; //data de registo do Usuario

        public User() : base()
        {
        }
    }
}

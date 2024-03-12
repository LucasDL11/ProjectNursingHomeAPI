using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class Token
    {



        [Key]
        [Column("cedUsuario")]
        public int CedUsuario { get; set; }

        [Column("rol")]
        public string Rol { get; set; }

        [Column("fechaExpiracion")]
        public DateTime FechaExpiracion { get; set; }

        [Column("passKey")]
        public string PassKey { get; set; }

        [NotMapped]
        [Column("persona")]
        public Persona persona { get; set; }

        public Token(int cedUsuario, string rol, DateTime fechaExpiracion, string passKey)
        {
            CedUsuario = cedUsuario;
            Rol = rol;
            FechaExpiracion = fechaExpiracion;
            PassKey = passKey;
        }
    }







}

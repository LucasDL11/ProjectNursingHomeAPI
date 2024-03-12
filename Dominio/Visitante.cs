using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using System.Globalization;

namespace Dominio
{
    [Table("Visitante")]
    public class Visitante : Persona
    {

        [Column("cedula")]
        public int cedula { get; set; }

        [Column("habilitado")]
        public int? Habilitado { get; set; }

        [Column("observaciones")]
        public string Observacion { get; set; }

        public Visitante()
        {
        }

        public Visitante(int cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo, int? habilitado, string observacion) : base(cedulaPersona, nombrePersona, apellidos, fechaNacimiento, email, telefono, direccion, sexo)
        {
            this.CedulaPersona = cedulaPersona;
            this.NombrePersona = nombrePersona;
            this.apellidos = apellidos;
            this.FechaNacimiento = fechaNacimiento;
            this.Email = email;
            this.Telefono = telefono;
            this.Direccion = direccion;
            this.Sexo = sexo;
            this.cedula=cedulaPersona;
            this.Habilitado = Habilitado;
            this.Observacion = observacion; 

        }
    }

   



}

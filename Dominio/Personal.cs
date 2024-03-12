using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using System.Diagnostics.CodeAnalysis;

namespace Dominio
{
    [Table("Personal")]
    public class Personal : Persona
    {
        

        [Column("fechaVencimientoCarnetDeSalud")]
        public DateTime? FechaVencimientoCarnetDeSalud { get; set; }

        [Column("fechaVencimientoCarnetBromatologia")]
        public DateTime? FechaVencimientoCarnetBromatologia { get; set; }

        [Column("carnetDeVacunas")]
        public bool? carnetDeVacunas { get; set; }




        [NotMapped]
        public List<Documentos>? documentos { get; set; }

        [NotMapped]
        public List<Tarea>? Tareas { get; set; }

        public Personal(int cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo, List<Documentos> cursos, DateTime? fechaVencimientoCarnetDeSalud, DateTime? fechaVencimientoCarnetBromatologia, bool? carnetDeVacunas) : base(cedulaPersona, nombrePersona, apellidos, fechaNacimiento, email, telefono, direccion, sexo)
        {
            this.CedulaPersona = cedulaPersona;
            this.NombrePersona = nombrePersona;
            this.apellidos = apellidos; 
            this.FechaNacimiento=fechaNacimiento;   
            this.Email = email;
            this.Telefono = telefono;   
            this.Direccion = direccion;
            this.Sexo = sexo;
            this.documentos = documentos;
            this.FechaVencimientoCarnetBromatologia = fechaVencimientoCarnetBromatologia;
            this.FechaVencimientoCarnetDeSalud = fechaVencimientoCarnetDeSalud;
            this.carnetDeVacunas= carnetDeVacunas;
            this.FechaDeIngreso = DateTime.Today;
            this.FechaDeEgreso=null;




        }

        public Personal() { }
        public Personal(int cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo) : base(cedulaPersona, nombrePersona, apellidos, fechaNacimiento, email, telefono, direccion, sexo)
        {
            this.CedulaPersona = cedulaPersona;
            this.NombrePersona = nombrePersona;
            this.apellidos = apellidos;
            this.FechaNacimiento = fechaNacimiento;
            this.Email = email;
            this.Telefono = telefono;
            this.Direccion = direccion;
            this.Sexo = sexo;
            this.FechaDeIngreso = DateTime.Today;
            this.FechaDeEgreso = null;




        }
    }

   



}

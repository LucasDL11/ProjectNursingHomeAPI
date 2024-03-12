using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using System.Diagnostics.CodeAnalysis;

namespace Dominio
{
    [Table("Residente")]
    public class Residente : Persona
    {
        public Residente()
        {
        }
        public Residente(int cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo) : base(cedulaPersona, nombrePersona, apellidos, fechaNacimiento, email, telefono, direccion, sexo)
        {
        }

        [ForeignKey("Responsable")]
        [Column("cedulaResponsable")]
        public int? CedulaResponsable { get; set; }

        [Column("emergenciaMovil")]
        public string? EmergenciaMovil { get; set; }

        [Column("sociedadMedica")]
        public string? SociedadMedica { get; set; }

        [Column("tieneCuratela")]
        public bool? tieneCuratela { get; set; }

        //[Column("fechaCuratela")]
        //public DateTime? curatela { get; set; }

        [NotMapped]
        public List<Medicamento>? Medicamentos { get; set; }

        [NotMapped]
        public List<PatologiaCronica>? PatologiasCronica { get; set; }

        [NotMapped]
        public List<Tarea>? Tareas { get; set; }

        [NotMapped]
        public Responsable? Responsable { get; set; }
        [NotMapped]
        public List<Documentos>? documentos { get; set; }

        [NotMapped]
        public Curatela? Curatela { get; set; }
    }





}

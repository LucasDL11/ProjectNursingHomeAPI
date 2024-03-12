using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    [Table("Responsable")]
    public class Responsable : Persona
    {

        public Responsable()
        {
        }
        public Responsable(int cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo) : base(cedulaPersona, nombrePersona, apellidos, fechaNacimiento, email, telefono, direccion, sexo)
        {
        }

        public Responsable(Residente? residente, Parentesco? parentesco, Curatela? curatela)
        {
            Residente = residente;
            Parentesco = parentesco;
            Curatela = curatela;
        }

        public Responsable(int? cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo, DateTime now, object value, bool v) : base(cedulaPersona, nombrePersona, apellidos, fechaNacimiento, email, telefono, direccion, sexo, now, value, v)
        {
        }

        [NotMapped]
        public Residente? Residente { get; set; }
        [NotMapped]
        public Parentesco? Parentesco { get; set; }

        [NotMapped]
        public Curatela? Curatela { get; set; }

        [NotMapped]
        public List<Documentos>? documentos { get; set; }
    }





}

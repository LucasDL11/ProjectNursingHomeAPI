using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    [Table("PersonalTarea")]
    public class PersonalTarea
    {

        [Column("cedula")]
        public int? Cedula { get; set; }

        [Column("idTarea")]
        public int? IdTarea { get; set; }

        [NotMapped]
        public string? nombres { get; set; }

        [NotMapped]
        public string? apellidos { get; set; }


        public PersonalTarea() { }
        public PersonalTarea(int? idTarea, int? cedula )
        { 
            this.IdTarea = idTarea;
            this.Cedula = cedula;
        }


    }




}

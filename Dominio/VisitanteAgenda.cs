using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    [Table("VisitanteAgenda")]
    public class VisitanteAgenda
    {

        [Column("cedula")]
        public int? cedula { get; set; }

        [Column("idAgenda")]
        public int? idAgenda { get; set; }

        [NotMapped]
        public string? nombres { get; set; }

        [NotMapped]
        public string? apellidos { get; set; }

        [NotMapped]
        public Visitante? visitante { get; set; }


        public VisitanteAgenda() { }
        public VisitanteAgenda(int? idAgenda, int? cedula )
        { 
            this.idAgenda = idAgenda;
            this.cedula = cedula;
        }


    }




}

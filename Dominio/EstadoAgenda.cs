using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class EstadoAgenda 
    {

        
        [Column("idEstadoAgenda")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idEstadoAgenda { get; set; }

        [ForeignKey("Agenda")]
        [Column("idAgenda")]
        public int? idAgenda { get; set; }

        [Column("nombreEstado")]
        public string? nombreEstado { get; set; }

        [Column("fechaYHora")]
        public DateTime? fechaYHora { get; set; }



        public EstadoAgenda()
        {
        }
        public EstadoAgenda(int? idAgenda, string nombreEstado, DateTime fechaYHora)
        {
            this.idAgenda = idAgenda;
            this.nombreEstado = nombreEstado;
            this.fechaYHora = fechaYHora;
        }

        //public Agenda agenda { get; set; }




    }





}

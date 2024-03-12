using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class EstadoTarea 
    {

        [Column("idEstadoTarea")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? idEstadoTarea { get; set; }

 
        [Column("idTarea")]
        public int? idTarea { get; set; }

        [Column("asignadoA")]
        public int? asignadoA { get; set; }

        [Column("nombreEstado")]
        public string? nombreEstado { get; set; }

        [Column("fechaYHora")]
        public DateTime? fechaYHora { get; set; }

        public EstadoTarea()
        {
        }

        public EstadoTarea(int? idTarea, int? asignadoA, string? nombreEstado, DateTime? fechaYHora)
        {
            this.idTarea = idTarea;
            this.asignadoA = asignadoA;
            this.nombreEstado = nombreEstado;
            this.fechaYHora = fechaYHora;
        }
    }





}

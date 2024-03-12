using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using Microsoft.IdentityModel.Tokens;

namespace Dominio
{
    public class Tarea 
    {
        //private DateTime? horaDeTarea;




        [Key]
        [Column("idTarea")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? IdTarea { get; set; }

        [Column("cedulaPersonal")]
        public int? CedulaPersonal { get; set; }

        [Column("diaDeTarea")]
        public DateTime? diaDeTarea { get; set; }

        //[Column("horaDeTarea")]
        //public DateTime? horaDeTarea { get; set; }

        [Column("seRepite")]
        public bool? seRepite { get; set; }

        [Column("inicio")]
        public DateTime? inicio { get; set; }

        [Column("fin")]
       
        public DateTime? fin { get; set; }

        [Column("cedulaResidente")]
        public int? CedulaResidente { get; set; }

        [Column("nombreTarea")]
        public string? NombreTarea { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("estado")]

        public bool? Estado { get; set; }

        [NotMapped]
        public List<PersonalTarea>? PersonalAsociado { get; set; }

        [NotMapped]
        public List<InsumoTareas>? InsumosTarea { get; set; }

        [NotMapped]
        public EstadoTarea? EstadoTarea { get; set; }

        [NotMapped]
        public string? nombresResidente { get; set; }
        [NotMapped]
        public string? apellidosResidente { get; set; }

        public Tarea(bool seRepite,int? cedulaResidente,int? cedulaPersonal, string? nombreTarea, string? descripcion) {
        
            this.CedulaPersonal = cedulaPersonal;
            this.seRepite = seRepite;
            this.CedulaResidente = cedulaResidente;
            this.NombreTarea = nombreTarea;
            this.Descripcion = descripcion;
            this.Estado = true;
            this.inicio = DateTime.Today;
            this.diaDeTarea = DateTime.Today;
            this.fin = null;
            this.EstadoTarea = new EstadoTarea(IdTarea, null, "Pendiente", DateTime.Today);
            this.InsumosTarea = null;
            this.PersonalAsociado = null;

        }

        public Tarea() {
            this.Estado = true;
            this.inicio = DateTime.Today;
            this.diaDeTarea = DateTime.Today;
            this.fin = null;
            this.EstadoTarea = new EstadoTarea(IdTarea, null, "Pendiente", DateTime.Today);
            this.InsumosTarea = null;
            this.PersonalAsociado = null;
        }

        public bool ValidarTarea()
        {
            if (!NombreTarea.IsNullOrEmpty())
            {
                return true;
            }
            return false;
        }

        public void modificaridTareaEstado() {
            if (this.EstadoTarea != null) {
                this.EstadoTarea.idTarea = this.IdTarea;
            }
         
        }
    }


}

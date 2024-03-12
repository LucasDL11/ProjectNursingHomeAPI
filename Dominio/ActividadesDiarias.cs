using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using Microsoft.IdentityModel.Tokens;

namespace Dominio
{
    public class ActividadesDiarias 
    {
        [Key]
        [Column("idActividad")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idActividad { get; set; }

        [Column("fechaActividad")]
        public DateTime? fechaActividad { get; set; }

        //[Column("horaActividad")]
        //public TimeSpan? horaActividad { get; set; }

        [Column("cedResidente")]
        public int? cedResidente { get; set; }

        [Column("nombreActividad")]
        public string? nombreActividad { get; set; }
        
        [Column("descripcionActividad")]
        public string? DescripcionActividad { get; set; }

        [NotMapped]
        public Residente? Residente { get; set; }




        public bool ValidarActividadDiaria()
        {
            if(this.fechaActividad > DateTime.MinValue != null > TimeSpan.MinValue
                 && !nombreActividad.IsNullOrEmpty())
            {
                return true;
            }
            return false;
        }
    }


}

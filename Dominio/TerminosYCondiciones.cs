using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class TerminosYCondiciones
    {

        [Key]
        [Column("idTerminosYCondiciones")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTerminosYCondiciones { get; set; }

        [Column("terminos")]
        public string Terminos { get; set; }

        
        [Column("fechaIngreso")]
        public DateTime FechaIngreso { get; set; }

        public TerminosYCondiciones()
        {
        }
    }





}

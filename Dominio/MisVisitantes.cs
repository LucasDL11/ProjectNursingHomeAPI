using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    [Table("MisVisitantes")]
    public class MisVisitantes 
    {    

        
        [Column("cedResponsable")]
        public int CedResponsable { get; set; }

        [Column("cedVisitante")]
        public int CedVisitante { get; set; }

        public MisVisitantes()
        {
        }

        public MisVisitantes(int cedResponsable, int cedVisitante)
        {
            CedResponsable = cedResponsable;
            CedVisitante = cedVisitante;
        }

        

        [NotMapped]
        public Persona? persona { get; set; }
    }





}

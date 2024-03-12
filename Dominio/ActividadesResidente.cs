using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using Microsoft.IdentityModel.Tokens;

namespace Dominio
{
    public class ActividadesResidente 
    {

        [Column("idTarea")]
        public int? idTarea { get; set; }
        [Column("cedulaResidente")]
        public int? cedResidente { get; set; }    

        [Column("fecha")]
        public DateTime? fechaActividad { get; set; }


        [Column("nombre")]
        public string? nombreActividad { get; set; }
        
        [Column("descripcion")]
        public string? DescripcionActividad { get; set; }

        
    }


}

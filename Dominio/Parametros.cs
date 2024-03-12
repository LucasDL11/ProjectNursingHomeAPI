using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    [Table("Parametros")]
    public class Parametros 
    {


        [Key]
        [Column("idParametro")]
        public int? idParametro { get; set; }

        [Column("horasPreviasAgenda")]
        public int? HorasPreviasAgenda { get; set; }

        [Column("usuarioAprobSolicitudes")]
        public int? UsuarioAprobSolicitudes { get; set; }

        [Column("usuarioAprobAgenda")]
        public int? UsuarioAprobAgenda { get; set; }

        //[Column("horasPreviasAgenda")]
        //public int HorasPreviasAgenda { get; set; }

    }





}

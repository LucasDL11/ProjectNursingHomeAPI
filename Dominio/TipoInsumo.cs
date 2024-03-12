using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class TipoInsumo 
    {
        [Key]
        [Column("idTipoInsumo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTipoInsumo { get; set; }
        
        [Column("nombreTipoInsumo")]
        public string NombreTipoInsumo  { get; set; }

    }





}

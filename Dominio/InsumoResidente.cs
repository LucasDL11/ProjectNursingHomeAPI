using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class InsumoResidente 
    {
        
        [Column("idInsumo")]
        public int IdInsumo { get; set; }

        
        [Column("cedulaResidente")]
        public int CedResidente { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }


    }





}

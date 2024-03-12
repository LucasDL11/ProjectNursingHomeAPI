using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using System.Numerics;

namespace Dominio
{
    public class InsumoTareas 
    {

        [Column("idTarea")]
        public int idTarea { get; set; }

        [Column("idInsumo")]
        public int IdInsumo { get; set; }      

        [Column("cantidad")]
        public int Cantidad { get; set; }    


    }





}

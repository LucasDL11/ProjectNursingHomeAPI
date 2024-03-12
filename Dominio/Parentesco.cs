using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class Parentesco 
    {        
              
        [Column("cedulaResponsable")]
        public int cedulaResponsable { get; set; }

        [Column("cedulaResidente")]
        public int cedulaResidente { get; set; }

        [Column("parentesco")]
        public string? NombreParentesco { get; set; }

        public Parentesco(int cedulaResidente, int cedulaResponsable,  string? nombreParentesco)
        {
            this.cedulaResidente = cedulaResidente;
            this.cedulaResponsable = cedulaResponsable;
            NombreParentesco = nombreParentesco;
        }

        public Parentesco()
        {
        }
    }

    




}

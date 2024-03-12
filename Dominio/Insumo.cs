using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using System.Numerics;
using Microsoft.IdentityModel.Tokens;

namespace Dominio
{
    public class Insumo 
    {
        [Key]
        [Column("idInsumo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdInsumo { get; set; }

        [Column("codBarras")]
        public Int64 CodBarrasInsumo { get; set; }

        [Column("nombre")]
        public string NombreInsumo { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }        

        [Column("idTipoInsumo")]
        public int IdTipoInsumo { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        [NotMapped]
        [Column("tipo")]
        public TipoInsumo Tipo { get; set; }

        public Insumo(int idInsumo, long codBarrasInsumo, string nombreInsumo, int cantidad, int idTipoInsumo, bool activo)
        {
            IdInsumo = idInsumo;
            CodBarrasInsumo = codBarrasInsumo;
            NombreInsumo = nombreInsumo;
            Cantidad = cantidad;
            IdTipoInsumo = idTipoInsumo;
            Activo = activo;
        }

        public Insumo(long codBarrasInsumo, string nombreInsumo, int cantidad, int idTipoInsumo, bool activo)
        {
            CodBarrasInsumo = codBarrasInsumo;
            NombreInsumo = nombreInsumo;
            Cantidad = cantidad;
            IdTipoInsumo = idTipoInsumo;
            Activo = activo;
        }

        public bool Validar(int? idInsumo, long codBarrasInsumo, string nombreInsumo, int cantidad, int idTipoInsumo)
        {
            if (idInsumo != null)
            {
                return (idInsumo >= 0 && codBarrasInsumo.ToString().Length == 13 && !nombreInsumo.IsNullOrEmpty() && cantidad >= 0 && idTipoInsumo >= 0);
            }
            else
            {
                return (codBarrasInsumo.ToString().Length == 13 && !nombreInsumo.IsNullOrEmpty() && cantidad >= 0 && idTipoInsumo >= 0);

            }
        }
    }





}

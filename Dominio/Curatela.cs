using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class Curatela 
    {
        [Key]
        [Column("idCuratela")]
        public int? IdCuratela { get; set; }

        [Column("fechaCuratela")]
        public DateTime? FechaCuratela { get; set; }

        [Column("curatelaResidente")]
        public int? CuratelaResidente { get; set; }

        [Column("curatelaResponsable")]
        public int? CuratelaResponsable { get; set; }


        public Curatela()
        {
        }

        public Curatela(DateTime? fechaCuratela, int? curatelaResidente, int? curatelaResponsable, Documentos documentoCuratela) : this(fechaCuratela, curatelaResidente, curatelaResponsable)
        {
            this.documentoCuratela = documentoCuratela;
        }

        public Curatela(DateTime? fechaCuratela, int? curatelaResidente, int? curatelaResponsable)
        {
            FechaCuratela = fechaCuratela;
            CuratelaResidente = curatelaResidente;
            CuratelaResponsable = curatelaResponsable;
        }

        [NotMapped]
        public Documentos? documentoCuratela { get; set; }
    }





}

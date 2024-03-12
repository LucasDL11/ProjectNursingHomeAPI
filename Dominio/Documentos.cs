using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace Dominio
{
    public class Documentos 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idDocumento")]
        public int? IdDocumento { get; set; }

        [Column("cedulaPersona")]
        public int? CedulaPersona { get; set; }

        [Column("esCurso")]
        public bool? EsCurso { get; set; }

        [Column("nombreDocumento")]
        public string? NombreDocumento { get; set; }

        [NotMapped]
        public string? ArchivoBase64 { get; set; }

        [NotMapped]
        public string? TipoArchivo { get; set; }
      

        [Column("rutaDocumento")]
        public string? RutaDocumento { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        public Documentos(int cedulaPersona, bool esCurso, string nombreDocumento, string archivoBase64, string descripcion) 
        {
            this.CedulaPersona = cedulaPersona;
            this.EsCurso = esCurso;
            this.ArchivoBase64 = archivoBase64;
            this.NombreDocumento = nombreDocumento;
            this.Descripcion = descripcion;
         
        }
        public Documentos() { }


        public bool validarDocumento() {

            if (this.CedulaPersona == 0 || this.NombreDocumento=="" || this.Descripcion == "" ) {
                return false; 
            }

            return true; 

        }







    }

  



}

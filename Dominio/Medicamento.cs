using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class Medicamento 
    {

        [Key]
        [Column("idMedicamento")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMedicamento { get; set; }

        [Column("cedulaResidente")]
        public int CedulaResidente { get; set; }

        
        [Column("medicamento")]
        public string? NombreMedicamento { get; set; }

        
        [Column("instrucciones")]
        public string? Instrucciones { get; set; }

        [Column("idPatologiaCronica")]
        public int? idPatologiaCronica { get; set; }

    }





}

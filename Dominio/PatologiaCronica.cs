using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class PatologiaCronica 
    {



        [Key]
        [Column("idPatologiaCronica")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? IdPatologiaCronica { get; set; }
        
        [Column("cedulaResidente")]
        public int? CedulaResidente { get; set; }

        
        [Column("nombre")]
        public string? Nombre { get; set; }

        
        [Column("observaciones")]
        public string? Observaciones { get; set; }

        [NotMapped]
        public List<Medicamento>? Medicamentos { get; set; }
        public PatologiaCronica()
        {
        }
        public PatologiaCronica(int? idPatologiaCronica, int? cedulaResidente, string? nombre, string? observaciones)
        {
            IdPatologiaCronica = idPatologiaCronica;
            CedulaResidente = cedulaResidente;
            Nombre = nombre;
            Observaciones = observaciones;
        }
    }





}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class TipoUsuario
    {

        [Key]
        [Column("idTipoUsuario")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTipoUsuario{ get; set; }
                        
        [Column("nombreTipoUsuario")]
        public string NombreTipoUsuario { get; set; }

        [NotMapped]
        [Column("funcionalidadDeUsuario")]
        public List<FuncionalidadesUsuario>? FuncionalidadesUsuario { get; set; }

        public TipoUsuario()
        {
        }
    }





}

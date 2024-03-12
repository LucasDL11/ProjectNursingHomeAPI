using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class FuncionalidadesUsuario 
    {

        [Key]
        [Column("idFuncionalidad")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFuncionalidad { get; set; }

        [Column("idTipoUsuario")]
        public int IdTipoUsuario { get; set; }

        
        [Column("nombreFuncionalidad")]
        public string NombreFuncionalidad { get; set; }
    }

    



}

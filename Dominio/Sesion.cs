using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    [Table("Sesiones")]
    public class Sesion
    {



        [Key]
        [Column("sesionID")]
        public int SesionID { get; set; }

        [Column("cedUsuario")]
        public int CedUsuario { get; set; }

        [Column("nombreEvento")]
        public string NombreEvento { get; set; }

        [Column("fechaYHora")]
        public DateTime FechaYHora { get; set; }

        public Sesion(int cedUsuario, string nombreEvento, DateTime fechaYHora)
        {
            CedUsuario = cedUsuario;
            NombreEvento = nombreEvento;
            FechaYHora = fechaYHora;
        }
    }







}

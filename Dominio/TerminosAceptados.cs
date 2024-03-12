using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class TerminosAceptados
    {
        [Column("idTerminosAceptados")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTerminosAceptados { get; set; }

        [Column("idTerminosYCondiciones")]
        public int IdTerminosYCondiciones { get; set; }

        [Column("cedPersona")]
        public int CedPersona { get; set; }

        
        [Column("fechaAceptado")]
        public DateTime FechaAceptado { get; set; }

        public TerminosAceptados()
        {
        }

        public TerminosAceptados(int idTerminosYCondiciones, int cedPersona, DateTime fechaAceptado)
        {
            IdTerminosYCondiciones = idTerminosYCondiciones;
            CedPersona = cedPersona;
            FechaAceptado = fechaAceptado;
        }
    }





}

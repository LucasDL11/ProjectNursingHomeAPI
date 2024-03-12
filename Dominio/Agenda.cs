using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using Microsoft.IdentityModel.Tokens;

namespace Dominio
{
    [Table("Agenda")]
    public class Agenda 
    {
        [Key]
        [Column("idAgenda")]
        public int? IdAgenda { get; set; }

        [Column("cedPersonal")]
        public int? CedPersonal { get; set; }

        [Column("cedResidente")]
        public int? CedResidente { get; set; }

        [Column("cedResponsable")]
        public int? CedResponsable { get; set; }

        [Column("observacion")]
        public string? observacion { get; set; }

        [Column("fechaYHora")]
        public DateTime? fechaYHora { get; set; }

        [Column("motivoDeVisita")]
        public string? MotivoDeVisita { get; set; }

        [Column("visitado")]
        public bool? Visitado { get; set; }

        [Column("sintomasRespiratorios")]
        public bool? SintomasRespiratorios { get; set; }


        //[NotMapped]
        //public List<Visitante> VisitanteAgenda { get; set; }

        //[NotMapped]
        //public Personal PersonalAgenda { get; set; }

        //[NotMapped]
        //public Residente ResidenteAgenda { get; set; }

        //[NotMapped]
        //public Responsable ResponsableAgenda { get; set; }

        //[NotMapped]
        //public List<Comprobante> Comprobantes { get; set; }

        [NotMapped]
        public EstadoAgenda? EstadoAgenda { get; set; }
        [NotMapped]
        public List<VisitanteAgenda>? visitantesAgenda { get; set; }

        [NotMapped]
        public string? NombreResidente { get; set; }
        [NotMapped]
        public string? ApellidoResidente { get; set; }

        public Agenda()
        {

        }
       
        public Agenda(int? cedResidente, int? cedResponsable, DateTime? fechaYHora, string? motivoDeVisita,List<VisitanteAgenda> visitantesAgenda)
        {
            this.CedResidente = cedResidente;
            this.CedResponsable = cedResponsable;
            this.fechaYHora = fechaYHora;
            this.MotivoDeVisita = motivoDeVisita;
            this.visitantesAgenda = visitantesAgenda;
        }

        public Agenda(int idAgenda, int cedPersonal, int cedResidente, int cedResponsable, string? observacion, DateTime? fechaYHora, string? motivoDeVisita, 
            bool visitado, bool sintomasRespiratorios)
        {
            IdAgenda = idAgenda;
            CedPersonal = cedPersonal;
            CedResidente = cedResidente;
            CedResponsable = cedResponsable;
            this.observacion = observacion;
            this.fechaYHora = fechaYHora;
            MotivoDeVisita = motivoDeVisita;
            Visitado = visitado;
            SintomasRespiratorios = sintomasRespiratorios;
        }


        //public Agenda(int idAgenda, int cedPersonal, int cedResidente, int cedResponsable, string? observacion, DateTime? fechaYHora, string? estadoAgenda, string? motivoDeVisita, bool visitado, string? sintomasRespiratorios, string? comprobantes)
        //{
        //    IdAgenda = idAgenda;
        //    CedPersonal = cedPersonal;
        //    CedResidente = cedResidente;
        //    CedResponsable = cedResponsable;
        //    this.observacion = observacion;
        //    this.fechaYHora = fechaYHora;
        //    //EstadoAgenda = estadoAgenda;
        //    MotivoDeVisita = motivoDeVisita;
        //    Visitado = visitado;
        //    SintomasRespiratorios = sintomasRespiratorios;
        //    //Comprobantes = comprobantes;
        //}

        //public Agenda(int cedResidente, int cedResponsable, DateTime fechaYHora, string motivoDeVisita, string sintomasRespiratorios, string comprobantes, List<Visitante> visitanteAgenda, Responsable responsableAgenda,Residente residenteAgenda)
        //{
        //    this.CedResidente=cedResidente;
        //    this.CedResponsable=cedResponsable;
        //    this.fechaYHora=fechaYHora;
        //    this.MotivoDeVisita = motivoDeVisita;
        //    this.SintomasRespiratorios=SintomasRespiratorios;
        //    //this.Comprobantes=comprobantes;
        //    this.VisitanteAgenda=visitanteAgenda;
        //    this.ResponsableAgenda=responsableAgenda;
        //    this.CedPersonal = 0;
        //    this.ResidenteAgenda = residenteAgenda;
        //    this.Visitado = false;
        //}

        //public bool verificarFechaLimite() {

        //    if (this.fechaYHora > DateTime.Today) {
        //        this.Visitado = false;
        //        return true;
        //    }
        //    return false; 
        //}

        //public void modificarAgenda(Personal personal, string Observacion, string estado, bool visitado) {
        //    if (personal != null) { 
        //    this.PersonalAgenda=personal;
        //    }

        //    if (Observacion != null && observacion != "") { 
        //    this.observacion= observacion;
        //    }
        //    //if (!estado.Equals(this.EstadoAgenda)) {
        //    //    this.EstadoAgenda = estado;
        //    //}

        //    if (!visitado.Equals(this.Visitado)) { 
        //    this.Visitado= visitado;
        //    }

        //}

        public bool Validacion() {

            if (this.CedResidente > 0 && this.CedResponsable > 0 && !MotivoDeVisita.IsNullOrEmpty() && fechaYHora != null) { 
            return true;
            }

            return false; 
        }
    }

    



}

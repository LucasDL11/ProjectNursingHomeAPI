using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class SolicitudUsuario 
    {
        [Key]
        [Column("idSolicitud")]
        public int? IdSolicitudUsuario { get; set; }

        [Column("cedSolicitante")]
        public int? CedSolicitante { get; set; }

        [Column("cedSolicitado")]
        public int? CedSolicitado { get; set; }

        [Column("nombres")]
        public string? nombres { get; set; }

        [Column("apellidos")]
        public string? apellidos { get; set; }

        [Column("fechaNacimiento")]
        public DateTime? fechaNacimiento { get; set; }

        [Column("email")]
        public string? email { get; set; }

        [Column("telefono")]
        public string? telefono { get; set; }        
        
        [Column("direccion")]
        public string? direccion { get; set; }

        [Column("sexo")]
        public string? sexo { get; set; }

        [Column("cedResidente")]
        public int? CedResidente { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("parentesco")]
        public string? nombreParentesco { get; set; }

        [Column("cedPersonal")]
        public int? CedPersonal { get; set; }


        [NotMapped]
        public Responsable? UsuarioSolicitado { get; set; }

        [NotMapped]
        public Personal? Personal { get; set; }

        [NotMapped]
        public Residente? Residente { get; set; }

        [NotMapped]
        public Parentesco? Parentesco { get; set; }

        [NotMapped]
        public string? nombresResidente { get; set; }

        [NotMapped]
        public string? apellidosResidente { get; set; }

        public SolicitudUsuario()
        {
        }

        public SolicitudUsuario(int? cedSolicitante, int? cedSolicitado, string? nombres, string? apellidos, string? email, string? telefono, string? direccion, string? sexo, int? cedResidente, string? parentesco)
        {
            CedSolicitante = cedSolicitante;
            CedSolicitado = cedSolicitado;
            this.nombres = nombres;
            this.apellidos = apellidos;
            this.email = email;
            this.telefono = telefono;
            this.direccion = direccion;
            this.sexo = sexo;
            CedResidente = cedResidente;
            this.nombreParentesco = parentesco;
        }
    }





}

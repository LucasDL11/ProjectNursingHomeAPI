using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{

    public class Persona 
    {
        [Key]
        [Column("cedula")]
        public int? CedulaPersona { get; set; }

        [Column("nombres")]
        public string? NombrePersona { get; set; }

        [Column("apellidos")]
        public string? apellidos { get; set; }

        [Column("fechaNacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("direccion")]
        public string? Direccion { get; set; }

        [Column("sexo")]
        public string? Sexo { get; set; }

        [Column("fechaDeIngreso")]
        public DateTime? FechaDeIngreso { get; set; }

        [Column("fechaDeEgreso")]
        public DateTime? FechaDeEgreso { get; set; }

        [Column("activo")]
        public bool? Activo { get; set; }

     



        public Persona()
        {
            
        }


        public Persona(int? cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo)
        {
            this.CedulaPersona = cedulaPersona;
            this.NombrePersona = nombrePersona;
            this.apellidos = apellidos;
            this.FechaNacimiento = fechaNacimiento;
            this.Email = email;
            this.Telefono = telefono;
            this.Direccion = direccion;
            this.Sexo = sexo;
        }

        public Persona(int? cedulaPersona, string? nombrePersona, string? apellidos, DateTime? fechaNacimiento, string? email, string? telefono, string? direccion, string? sexo, DateTime now, object value, bool v) : this(cedulaPersona, nombrePersona, apellidos, fechaNacimiento, email, telefono, direccion, sexo)
        {
        }






        //********************************************************FALTAN VALIDACIONES como la ci
        public string validaciones() {

            string mensaje = "";

            if (!this.validarCedula()) {

                mensaje += "Cedula no valida";
            
            }
            // Los valores requeridos son nulos o vacíos, retorna false o lanza una excepción
            if (string.IsNullOrEmpty(this.NombrePersona))  
            {
                mensaje += "Nombre inválido.";
            }
            if (string.IsNullOrEmpty(this.apellidos)) 
            {
                mensaje += "\n Apellido inválido.";
            }
            if (string.IsNullOrEmpty(this.Telefono)) 
            {
                mensaje += "\n Teléfono inválido.";
            }
            if (string.IsNullOrEmpty(this.Email)) {
            }
            else if (!MailValido(this.Email))
            {
                mensaje += "\n Email inválido.";
            }
            if (string.IsNullOrEmpty(this.Direccion)) 
            {
                mensaje += "\n Dirección inválida.";
            }
            if (string.IsNullOrEmpty(this.Sexo))
            {
                mensaje += "\n Sexo inválido.";
            }
            if (this.FechaNacimiento >= DateTime.Now) 
            {
                mensaje += "\n Inválida fecha de nacimiento";
            }


            return mensaje;
        }

        //public Personal Personal { get; set; }
        //public Residente Residente { get; set; }
        //public Responsable Responsable { get; set; }

        public static bool MailValido(string email)
        {
            string trimEmail = email.Trim();

            if (trimEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool validarCedula() {


            string buffer = this.CedulaPersona.ToString();
            if (buffer.Length == 8)
            {

                int a = int.Parse(buffer.Substring(0, 1));
                int b = int.Parse(buffer.Substring(1, 1));
                int c = int.Parse(buffer.Substring(2, 1));
                int d = int.Parse(buffer.Substring(3, 1));
                int i = int.Parse(buffer.Substring(4, 1));
                int f = int.Parse(buffer.Substring(5, 1));
                int g = int.Parse(buffer.Substring(6, 1));
                int verificador = int.Parse(buffer.Substring(7, 1));
                int sum = (2 * a + 9 * b + 8 * c + 7 * d + 6 * i + 3 * f + 4 * g);
                int valor = sum + verificador;

                if (valor.ToString().Length == 3)
                {
                    if (!(valor.ToString().Substring(2, 1)).Equals("0"))
                    {
                        return false;

                    }
                    else
                    {
                        return true;

                    }


                }
                else if (valor.ToString().Length == 2)
                {
                    if (!(valor.ToString().Substring(1, 1)).Equals("0"))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }






            return false;

        }
    }





}

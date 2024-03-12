using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using Microsoft.IdentityModel.Tokens;

namespace Dominio
{
    public class CambiarPass
    {


        
        public int Cedula { get; set; }

        public string PassActual { get; set; }

        public string Pass { get; set; }

        public string RePass { get; set; }

        [NotMapped]
        public bool TerminosAceptados { get; set; }

        [NotMapped]
        public int IdTerminos { get; set; }

        public CambiarPass()
        {
        }

        public CambiarPass(int cedula, string passActual, string pass, string rePass)
        {
            Cedula = cedula;
            PassActual = passActual;
            Pass = pass;
            RePass = rePass;
        }

        public CambiarPass(int cedula, string passActual, string pass, string rePass, bool terminosAceptados) : this(cedula, passActual, pass, rePass)
        {
            TerminosAceptados = terminosAceptados;
        }

        public CambiarPass(int cedula, string passActual, string pass, string rePass, bool terminosAceptados, int idTerminos) : this(cedula, passActual, pass, rePass, terminosAceptados)
        {
            IdTerminos = idTerminos;
        }
    }





}

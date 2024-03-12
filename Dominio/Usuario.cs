using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;
using Microsoft.IdentityModel.Tokens;

namespace Dominio
{
    public class Usuario 
    {
        [Key]
        public int cedula { get; set; }

        [Column("pass")]
        public string pass { get; set; }

        [Column("primerPass")]
        public bool? primerPass { get; set; }

        [Column("idTipoUsuario")]
        public int? idTipoUsuario { get; set; }

        [NotMapped]
        public TipoUsuario? tipoUsuario { get; set; }

        [Column("tokenDispositivo")]
        public String? TokenDispositivos { get; set; }

        public Usuario(int cedula, string pass, bool? primerPass, int? idTipoUsuario) : this(cedula, pass)
        {
            this.primerPass = primerPass;
            this.idTipoUsuario = idTipoUsuario;
        }

        [NotMapped]
        public Persona? persona { get; set; }

        [NotMapped]
        public Token? tokenUsuario { get; set; }



        public Usuario()
        {
        }

        public Usuario(string tokenDispositivo) {
            this.TokenDispositivos = tokenDispositivo;
        }


        public Usuario(int recibeCedula)
        {
            this.cedula = recibeCedula;
        }
        public Usuario(int recibeCedula, string recibePassword)
        {
            this.cedula = recibeCedula;
            this.pass = recibePassword;
        }

        public Usuario(int cedula, string pass, int? idTipoUsuario) : this(cedula, pass)
        {
            this.cedula = cedula;
            this.pass = pass;
            this.primerPass = true;
            this.idTipoUsuario = idTipoUsuario;
            this.persona = persona;
        }

        public bool validaUsuario() {

            if (this.cedula == 0 && this.pass.IsNullOrEmpty() && idTipoUsuario < 0) {
                return false;
            }
            return true;
        }

        public bool validarCedula(int recibeCedula)
        {
            string buffer = recibeCedula.ToString();
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
                    if (!(valor.ToString().Substring(2,1)).Equals("0"))
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

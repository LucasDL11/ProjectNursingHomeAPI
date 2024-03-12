using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Dominio.Repositorio;

namespace Dominio
{
    public class Login
    {
        public int recibeCI { get; set; }

        public string recibePassword { get; set; }


        public Login()
        {
        }

        public Login(int recibeCedula, string recibePassword)
        {
            this.recibeCI = recibeCedula;
            this.recibePassword = recibePassword;
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

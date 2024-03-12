using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using Dominio.Repositorio;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorio
{
    public class RepositorioConfiguracion
    {
        private ResidencialContext _conn;

        public RepositorioConfiguracion(ResidencialContext residencialContext)
        {

            _conn = residencialContext;

        }

        public Parametros BuscarParametros()
        {

            Parametros miParametro = new Parametros();


            try
            {
                miParametro = _conn.Parametros.FirstOrDefault(); //  
            }
            catch (Exception)
            {
                throw;
            }

            return miParametro;
        }

    }
}

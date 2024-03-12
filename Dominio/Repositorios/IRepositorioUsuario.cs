using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Repositorio
{
    public interface IRepositorioUsuario:IRepositorio<Usuario>

    {
        public bool BuscarUsuarioPorCI(string recibeCI, string recibePassword);
        public bool ValidarPassword(Usuario obj);
    }
}

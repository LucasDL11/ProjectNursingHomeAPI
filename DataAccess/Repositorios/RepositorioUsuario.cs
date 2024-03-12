using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using Dominio.Repositorio;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DataAccess.Repositorio
{
    public class RepositorioUsuario : IRepositorioUsuario
    {

        private ResidencialContext _conn;

        public RepositorioUsuario(ResidencialContext residencialContext)
        {

            _conn = residencialContext;

        }

        public bool Add(Usuario obj)
        {
            try
            {
                

                _conn.Usuario.Add(obj);
                
                _conn.SaveChanges();
                return true;



            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Usuario> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarUsuarioPorCI(int recibeCI)
        {

            Usuario miUsuario = new Usuario();

            try
            {
                //involucrando hasheo
                //if(VerificarPassword(recibeCI, recibePassword)){
                // return miUsuario = _conn.Usuario.Where(u => u.cedula == recibeCI).FirstOrDefault();
                //  }                

                //sin hasheo
                //miUsuario = _conn.Usuario.Where(u => u.cedula == recibeCI && u.pass.Equals(recibePassword)).FirstOrDefault(); //

                //07 07 2023
                if (ExisteUsuario(recibeCI) && VerificarPersonaActivo(recibeCI))
                {
                    miUsuario = _conn.Usuario.Where(u => u.cedula == recibeCI).FirstOrDefault();
                    //miUsuario.persona = _conn.Persona.Where(p => p.CedulaPersona == miUsuario.cedula).FirstOrDefault();
                    miUsuario.tipoUsuario = _conn.TipoUsuario.Where(tp => tp.IdTipoUsuario == miUsuario.idTipoUsuario).FirstOrDefault();
                    //miUsuario.tipoUsuario.FuncionalidadesUsuario = _conn.FuncionalidadesUsuario.Where(f => f.IdTipoUsuario == miUsuario.idTipoUsuario).ToList();
                }
                else
                {
                    return miUsuario;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return miUsuario;
        }

        private bool VerificarPersonaActivo(int recibeCI)
        {
            try
            {
                return _conn.Persona.Where(p => p.CedulaPersona == recibeCI && p.Activo == true).Any();
            }
            catch (Exception)
            {
                throw;
            }
        }

            public IEnumerable<Usuario> FindAllUsuarios()
        {
            IEnumerable<Usuario> usuarios;
            try
            {
                usuarios = _conn.Usuario.ToList();
                IEnumerable<TipoUsuario> tiposPersona = _conn.TipoUsuario.ToList();
                IEnumerable<Persona> personas = _conn.Persona.ToList();
                foreach (Usuario u in usuarios)
                {
                    foreach (TipoUsuario tp in tiposPersona)
                    {
                        if (u.idTipoUsuario == tp.IdTipoUsuario)
                        {
                            u.tipoUsuario = tp;
                        }
                    }
                    foreach (Persona p in personas)
                    {
                        if (u.cedula == p.CedulaPersona)
                        {
                            u.persona = p;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return usuarios;
        }




        public string EncriptarPassword(string passwordAEncriptar)
        {
            return BCrypt.Net.BCrypt.HashPassword(passwordAEncriptar);
        }

        public bool VerificarPasswordSinHashear(int ci, string recibePassword)
        {
            return ObtenerPasswordDelUsuario(ci).Equals(recibePassword);
        }
        public bool VerificarPasswordHasheada(int ci, string recibePassword)
        {
            return(BCrypt.Net.BCrypt.Verify(recibePassword, ObtenerPasswordDelUsuario(ci)));
        }

        public string ObtenerPasswordDelUsuario(int recibeCI)
        {
            string password;
            //Usuario miUsuario = null;
            try
            {
                password = _conn.Usuario.Where(u => u.cedula == recibeCI).FirstOrDefault().pass;
            }
            catch (Exception)
            {
                throw;
            }
            return password;
        }

        public bool CambiarPassword(int recibeCI, string recibePassword)
        {
            //hasheo
            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "update usuario set pass = @recibePassword, primerPass = 0 " +
                    "where cedula = @ci",
                    new SqlParameter("@ci", recibeCI),
                    new SqlParameter("@recibePassword", EncriptarPassword(recibePassword))
                    );
                return _conn.SaveChanges() > 0;

            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public void AddUsuario(Usuario nuevoUsuario)
        {
            string passwd = EncriptarPassword(nuevoUsuario.pass);
            nuevoUsuario.pass= passwd;
            try
            {
                _conn.Usuario.Add(nuevoUsuario);
                _conn.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ExisteUsuario(int recibeCI)
        {
            try
            {
                return _conn.Usuario.Where(u => u.cedula == recibeCI).Any();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<TipoUsuario> GetTiposUsuarioAll()
        {
            try
            {
                return _conn.TipoUsuario.ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ExistePersona(int recibeCI)
        {
            try
            {
                return _conn.Persona.Where(u => u.CedulaPersona == recibeCI).Any();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Usuario obj)
        {
            try
            {
                if (_conn.Usuario.Where(t => t.cedula == obj.cedula).Any())
                {
                 

                    _conn.Usuario.Update(obj);
                }


                if (_conn.SaveChanges() > 0) {
                    return true;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public bool ValidarPassword(Usuario obj)
        {
            throw new NotImplementedException();
        }

        bool IRepositorioUsuario.BuscarUsuarioPorCI(string recibeCI, string recibePassword)
        {
            throw new NotImplementedException();
        }

        public void GuardarToken(Token miToken)
        {
            try
            {
                if(_conn.Token.Where(t => t.CedUsuario == miToken.CedUsuario).Any()) {

                    _conn.Token.Update(miToken);
                }
                else
                {
                    _conn.Token.Add(miToken);
                }
                _conn.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddSolicitudUsuario(SolicitudUsuario solicitudUsuario)
        {
            try
            {
                if (!_conn.SolicitudUsuario.Where(su => su.CedSolicitado == solicitudUsuario.CedSolicitado).Any() && !_conn.Persona.Where(p => p.CedulaPersona == solicitudUsuario.CedSolicitado).Any())
                    {
                        solicitudUsuario.Estado = "Pendiente de Aprobación";
                        solicitudUsuario.CedResidente = _conn.Parentesco.FirstOrDefault(p => p.cedulaResponsable == solicitudUsuario.CedSolicitante)?.cedulaResidente;
                        _conn.SolicitudUsuario.Add(solicitudUsuario);
                        _conn.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
                }
            catch (Exception)
            {
                throw;
            }
        }


        public bool AprobarSolicitudUsuario(int idSolicitud, int cedulaPersonal)
        {
            try
            {
                SolicitudUsuario solicitudUsuario = _conn.SolicitudUsuario.FirstOrDefault(su => su.IdSolicitudUsuario == idSolicitud);
                if(solicitudUsuario != null && _conn.Personal.Any(p => p.CedulaPersona == cedulaPersonal))
                {
                    solicitudUsuario.CedPersonal = cedulaPersonal;
                    solicitudUsuario.Estado = "Aprobada";
                    Responsable persona = new Responsable(solicitudUsuario.CedSolicitado, solicitudUsuario.nombres,
                        solicitudUsuario.apellidos, solicitudUsuario.fechaNacimiento, solicitudUsuario.email,
                        solicitudUsuario.telefono, solicitudUsuario.direccion, solicitudUsuario.sexo, DateTime.Now, null, true);
                    _conn.Responsable.Add(persona);
                    _conn.SaveChanges();
                    _conn.Usuario.Add(new Usuario((int)persona.CedulaPersona, persona.CedulaPersona.ToString(), true, _conn.TipoUsuario.FirstOrDefault(tu => tu.NombreTipoUsuario.Equals("Responsable")).IdTipoUsuario));
                    _conn.SaveChanges();
                    _conn.Parentesco.Add(new Parentesco((int)solicitudUsuario.CedResidente, (int)persona.CedulaPersona, solicitudUsuario.nombreParentesco));
                    _conn.SolicitudUsuario.Update(solicitudUsuario);
                    _conn.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ExisteSolicitud(int recibeIdSolicitud)
        {
            try
            {
                return _conn.SolicitudUsuario.Where(s => s.IdSolicitudUsuario == recibeIdSolicitud).Any();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RechazarSolicitudUsuario(int recibeIdSolicitud, int recibeCedulaPersonal)
        {
            try
            {
                SolicitudUsuario solicitudUsuario = _conn.SolicitudUsuario.FirstOrDefault(su => su.IdSolicitudUsuario == recibeIdSolicitud);
                if (solicitudUsuario != null)
                {
                    solicitudUsuario.Estado = "Rechazada";
                    solicitudUsuario.CedPersonal = recibeCedulaPersonal;
                    _conn.SolicitudUsuario.Update(solicitudUsuario);
                    return _conn.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }            
        }

        public IEnumerable<SolicitudUsuario> GetSolicitudesUsuarioPorEstado(string recibeEstado)
        {
            try
            {
                IEnumerable<SolicitudUsuario> solicitudes = _conn.SolicitudUsuario.Where(su => su.Estado.Equals(recibeEstado)).ToList();
                foreach(SolicitudUsuario solicitudUsuario in solicitudes)
                {
                    solicitudUsuario.nombresResidente = _conn.Persona.FirstOrDefault(p => p.CedulaPersona == solicitudUsuario.CedResidente).NombrePersona;
                    solicitudUsuario.apellidosResidente = _conn.Persona.FirstOrDefault(p => p.CedulaPersona == solicitudUsuario.CedResidente).apellidos;
                }
                return solicitudes;

            }catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<SolicitudUsuario> GetSolicitudesUsuarioProcesadas()
        {
            try
            {
                IEnumerable<SolicitudUsuario> solicitudes = _conn.SolicitudUsuario.Where(su => !su.Estado.Equals("Pendiente de Aprobación")).ToList();
                foreach (SolicitudUsuario solicitudUsuario in solicitudes)
                {
                    solicitudUsuario.nombresResidente = _conn.Persona.FirstOrDefault(p => p.CedulaPersona == solicitudUsuario.CedResidente).NombrePersona;
                    solicitudUsuario.apellidosResidente = _conn.Persona.FirstOrDefault(p => p.CedulaPersona == solicitudUsuario.CedResidente).apellidos;
                }
                return solicitudes;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CompararPassKey(string recibePassKey)
        {
            try
            {
                return _conn.Token.Any(u => u.PassKey.Equals(recibePassKey));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CrearLogSesion(int cedula, string nombreLog)
        {
            try
            {
                Sesion sesion = new Sesion(cedula, nombreLog, DateTime.Now);
                _conn.Sesion.Add(sesion);
                _conn.SaveChanges();
                if(nombreLog.Equals("Log off"))
                {
                    Usuario usuario = _conn.Usuario.FirstOrDefault(u => u.cedula == cedula);
                    usuario.TokenDispositivos = null;
                    _conn.SaveChanges();
                    //_conn.Usuario.FromSqlRaw("update usuario set tokenDispositivo = null where cedula = @cedula",
                    //    new SqlParameter("@cedula", cedula));
                    //_conn.SaveChanges();
                    Token token = _conn.Token.FirstOrDefault(t=>t.CedUsuario==cedula);
                    _conn.Token.Remove(token);
                    _conn.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<string> GetTokenDispositivoResponsable(int ciResidente) {
            try
            {
               var resultado = (from usuario in _conn.Usuario
                                                 join responsable in _conn.Responsable on usuario.cedula equals responsable.CedulaPersona
                                                 join residente in _conn.Residente on  responsable.CedulaPersona equals residente.CedulaResponsable
                                where residente.CedulaPersona == ciResidente
                                                 select usuario.TokenDispositivos).ToList();



                return resultado;
            }
            catch (Exception ex)
            {
                // En lugar de solo "throw;", puedes manejar la excepción o registrarla
                // de alguna manera adecuada antes de lanzarla nuevamente.
                throw ex;
            }
        }

        public List<string> GetTokenByTipo(int tipoUsuario)
        {
            try
            {
                var resultado = (from usuario in _conn.Usuario
                                where usuario.idTipoUsuario== tipoUsuario && usuario.TokenDispositivos!=null
                                 select usuario.TokenDispositivos).ToList();



                return resultado;
            }
            catch (Exception ex)
            {
                // En lugar de solo "throw;", puedes manejar la excepción o registrarla
                // de alguna manera adecuada antes de lanzarla nuevamente.
                throw ex;
            }
        }

        public TerminosYCondiciones GetTerminosYCondiciones()
        {
            try
            {
               return _conn.TerminosYCondiciones.FirstOrDefault(tyc => tyc.IdTerminosYCondiciones == 1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AceptarTerminos(CambiarPass recibeDatos)
        {
            try
            {
                TerminosAceptados terminosAceptados = new TerminosAceptados(recibeDatos.IdTerminos, recibeDatos.Cedula, DateTime.Now);
                _conn.TerminosAceptados.Add(terminosAceptados);
                _conn.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void CerrarTareasYAgendas()
        {
            try
            {
                _conn.Database.ExecuteSqlRaw("exec CerrarAgendasDelDiaAnteriorProcedure");
                _conn.SaveChanges();
                _conn.Database.ExecuteSqlRaw("exec CerrarTareasDelDiaAnteriorProcedure");
                _conn.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

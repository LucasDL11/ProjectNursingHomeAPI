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
   public class RepositorioAgenda : IRepositorioAgenda
    {

        private ResidencialContext _conn;

        public RepositorioAgenda(ResidencialContext residencialContext)
        {

            _conn = residencialContext;

        }

        public bool Add(Agenda obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Agenda> FindAll()
        {
            IEnumerable<Agenda> todasLasAgendas;
            try
            {
                todasLasAgendas = _conn.Agenda.ToList();
                IEnumerable<EstadoAgenda> todasLosEstadoAgendas = _conn.EstadoAgenda.ToList();

                foreach (var agenda in todasLasAgendas)
                {
                    //agenda.EstadoAgenda = new EstadoAgenda();
                    foreach (var estadoAgenda in todasLosEstadoAgendas)
                    {
                        if (agenda.IdAgenda == estadoAgenda.idAgenda)
                        {
                            agenda.EstadoAgenda = estadoAgenda;
                        }
                    }
                    //if (agenda.EstadoAgenda == null) {
                    //    agenda.EstadoAgenda = "Pendiente de Aprobación";
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
            return todasLasAgendas;
        }

        public IEnumerable<Agenda> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Agenda obj)
        {
            throw new NotImplementedException();
        }

        public bool ExisteAgenda(int recibeIdAgenda)
        {
            try
            {
                return _conn.Agenda.Where(a => a.IdAgenda == recibeIdAgenda).Any();

            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool AddAgenda(Agenda recibeAgenda)
        {
            try
            {
                recibeAgenda.CedResidente = _conn.Residente.FirstOrDefault(r=>r.CedulaResponsable == recibeAgenda.CedResponsable).CedulaPersona;
                _conn.Agenda.Add(recibeAgenda);
                if (recibeAgenda.visitantesAgenda != null)
                {
                    if (_conn.SaveChanges() > 0)
                    {
                        recibeAgenda.visitantesAgenda.Add(new VisitanteAgenda(recibeAgenda.IdAgenda, recibeAgenda.CedResponsable));
                        foreach(VisitanteAgenda va in recibeAgenda.visitantesAgenda)
                        {
                            va.idAgenda=recibeAgenda.IdAgenda;
                        }
                        _conn.VisitanteAgendas.AddRange(recibeAgenda.visitantesAgenda);
                        _conn.EstadoAgenda.Add(new EstadoAgenda(recibeAgenda.IdAgenda, "Pendiente de Aprobación", DateTime.Now));
                        if (_conn.SaveChanges() > 0)
                        {
                            return true;
                        }
                    }
                }

                return false;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddEstadoAgenda(int idAgenda, string nombreEstado, string recibeComentario)
        {
            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "insert into EstadoAgenda values(@idAgenda, @nombreEstado, @fechaYHora)",
                    new SqlParameter("@idAgenda", idAgenda),
                    new SqlParameter("@nombreEstado", nombreEstado),
                    new SqlParameter("@fechaYHora", DateTime.Now)
                    );
                if (nombreEstado.Equals("Rechazada"))
                {
                    _conn.Database.ExecuteSqlRaw("update agenda set observacion = @recibeComentario where idAgenda = @idAgenda",
                        new SqlParameter("@idAgenda", idAgenda),
                        new SqlParameter("@recibeComentario", recibeComentario));
                }                
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Agenda> GetAgendaByResponsable(int recibeCedResponsable)
        {
            IEnumerable<Agenda> misAgendas;
            try
            {
                //traigo dec del residente
                //int cedResidente= _conn.Parentesco.Where(e => e.cedulaResponsable == recibeCedResponsable).Select(e=> e.cedulaResidente).FirstOrDefault();

                misAgendas = _conn.Agenda.Where(e => e.CedResponsable == recibeCedResponsable).ToList().OrderByDescending(a => a.fechaYHora);
                IEnumerable<EstadoAgenda> todasLosEstadoAgendas = _conn.EstadoAgenda.FromSqlRaw("exec ObtenerUltimosEstadoAgendas");
                IEnumerable<VisitanteAgenda> visitantes = _conn.VisitanteAgendas.ToList();

                foreach (var agenda in misAgendas)
                {
                    if(agenda.visitantesAgenda == null)
                    {
                        agenda.visitantesAgenda = new List<VisitanteAgenda>();
                    }
                    //agenda.EstadoAgenda = new List<EstadoAgenda>();
                    foreach (var estadoAgenda in todasLosEstadoAgendas)
                    {
                        if (agenda.IdAgenda == estadoAgenda.idAgenda)
                        {
                            agenda.EstadoAgenda = estadoAgenda; //.Add(estadoAgenda);
                        }
                    }
                    foreach (var visitante in visitantes)
                    {
                        if (agenda.IdAgenda == visitante.idAgenda)
                        {
                            visitante.nombres = _conn.Persona.FirstOrDefault(p => p.CedulaPersona == visitante.cedula).NombrePersona;
                            visitante.apellidos = _conn.Persona.FirstOrDefault(p => p.CedulaPersona == visitante.cedula).apellidos;
                            agenda.visitantesAgenda.Add(visitante);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return misAgendas;
        }       

        public IEnumerable<Agenda> GetAgendasPorEstado(string recibeEstado)
        {
            IEnumerable<Agenda> misAgendas = _conn.Agenda.FromSqlRaw(
                    "EXEC ObtenerAgendasPorNombreEstado @nombreEstado = @recibeEstado;",
                    new SqlParameter("@recibeEstado", recibeEstado)).ToList().OrderByDescending(a => a.fechaYHora);
            

            try
            {
                //traigo dec del residente
                //int cedResidente= _conn.Parentesco.Where(e => e.cedulaResponsable == recibeCedResponsable).Select(e=> e.cedulaResidente).FirstOrDefault();

                // Suponiendo que tienes una lista de Agenda llamada 'misAgendas'
                IEnumerable<EstadoAgenda> misEstadoAgendas = _conn.EstadoAgenda.FromSqlRaw(
                    "EXEC ObtenerEstadoAgendasPorNombreEstado @nombreEstado = @recibeEstado;",
                    new SqlParameter("@recibeEstado", recibeEstado)).ToList();

                IEnumerable<VisitanteAgenda> visitanteAgendas = _conn.VisitanteAgendas.ToList();
                foreach (Agenda agenda in misAgendas)
                    {
                    foreach (EstadoAgenda estadoAgenda in misEstadoAgendas)
                    {
                        if (agenda.IdAgenda == estadoAgenda.idAgenda)
                        {
                            agenda.EstadoAgenda = estadoAgenda;
                        }
                    }
                    agenda.NombreResidente = _conn.Residente.FirstOrDefault(r => r.CedulaPersona == agenda.CedResidente).NombrePersona;
                    agenda.ApellidoResidente = _conn.Residente.FirstOrDefault(r => r.CedulaPersona == agenda.CedResidente).apellidos;
                    agenda.visitantesAgenda = new List<VisitanteAgenda>();
                    foreach(VisitanteAgenda va in visitanteAgendas)
                    {
                        if(agenda.IdAgenda == va.idAgenda)
                        {
                            va.nombres = _conn.Persona.FirstOrDefault(r => r.CedulaPersona == va.cedula).NombrePersona;
                            va.apellidos = _conn.Persona.FirstOrDefault(r => r.CedulaPersona == va.cedula).apellidos;
                            agenda.visitantesAgenda.Add(va);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return misAgendas;
        }


        public IEnumerable<Agenda> GetAgendasEntreFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            IEnumerable<Agenda> agendas;
            try
            {
                agendas = _conn.Agenda.Where(t => t.fechaYHora.Value.Date >= fechaDesde.Date && t.fechaYHora.Value.Date <= fechaHasta).ToList().OrderByDescending(a => a.fechaYHora);
                IEnumerable<Persona> personas = _conn.Persona.ToList();
                foreach(Agenda a in agendas)
                {
                    foreach(Persona persona in personas)
                    {
                        if(a.CedResidente == persona.CedulaPersona)
                        {
                            a.NombreResidente = persona.NombrePersona;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return agendas;
        }

        public bool FinalizarAgenda(int cedulaPersonal, int idAgenda, string comentario, bool visitado)
        {
            try
            {
                string estado = "";
                if (visitado) {
                    estado = "Realizada";
                }
                else
                {
                    estado = "No realizada";
                }
                EstadoAgenda estadoAgenda = new EstadoAgenda(idAgenda, estado, DateTime.Now);
                _conn.EstadoAgenda.Add(estadoAgenda);

                if (_conn.SaveChanges() > 0)
                {
                  Agenda agenda = _conn.Agenda.FirstOrDefault(a=>a.IdAgenda == idAgenda);
                    agenda.observacion = comentario;
                    agenda.Visitado = visitado;
                    agenda.CedPersonal = cedulaPersonal;
                    _conn.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Agenda> GetAgendasDelDia()
        {
            IEnumerable<Agenda> misAgendas = _conn.Agenda.FromSqlRaw(
        "EXEC ObtenerAgendasSinRealizarDelDia").ToList();        

            try
            {
                IEnumerable<EstadoAgenda> misEstadoAgendas = _conn.EstadoAgenda.FromSqlRaw(
                    "EXEC ObtenerEstadoAgendasSinRealizarDelDia").ToList();

                IEnumerable<VisitanteAgenda> visitanteAgendas = _conn.VisitanteAgendas.ToList();
                foreach (Agenda agenda in misAgendas)
                    {
                    foreach (EstadoAgenda estadoAgenda in misEstadoAgendas)
                    {
                        if (agenda.IdAgenda == estadoAgenda.idAgenda)
                        {
                            agenda.EstadoAgenda = estadoAgenda;
                        }
                    }
                    agenda.NombreResidente = _conn.Residente.FirstOrDefault(r => r.CedulaPersona == agenda.CedResidente).NombrePersona;
                    agenda.ApellidoResidente = _conn.Residente.FirstOrDefault(r => r.CedulaPersona == agenda.CedResidente).apellidos;
                    agenda.visitantesAgenda = new List<VisitanteAgenda>();
                    foreach(VisitanteAgenda va in visitanteAgendas)
                    {
                        if(agenda.IdAgenda == va.idAgenda)
                        {
                            va.nombres = _conn.Persona.FirstOrDefault(r => r.CedulaPersona == va.cedula).NombrePersona;
                            va.apellidos = _conn.Persona.FirstOrDefault(r => r.CedulaPersona == va.cedula).apellidos;
                            agenda.visitantesAgenda.Add(va);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return misAgendas;
        }
    }
}

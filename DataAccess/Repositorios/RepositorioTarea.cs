using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using Dominio.Repositorio;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using Microsoft.Extensions.Azure;

namespace DataAccess.Repositorio
{
    public class RepositorioTarea : IRepositorioTarea
    {

        private ResidencialContext _conn;

        public RepositorioTarea(ResidencialContext residencialContext)
        {

            _conn = residencialContext;

        }


        public bool AddTarea(Tarea nuevaTarea, bool personal)
        {
            if (ExistePersonal(nuevaTarea.CedulaPersonal))
            {
                try{
                    nuevaTarea.inicio = DateTime.Now;
                    _conn.Tarea.Add(nuevaTarea);
                    if (_conn.SaveChanges() > 0) {
                        nuevaTarea.modificaridTareaEstado();
                        if (nuevaTarea.EstadoTarea != null) {
                            if (personal)
                            {
                                EstadoTarea estadoTarea = new EstadoTarea(nuevaTarea.IdTarea, nuevaTarea.CedulaPersonal, "Asignada", DateTime.Now);
                                _conn.EstadoTarea.Add(estadoTarea);
                            }
                            else
                            {
                                _conn.EstadoTarea.Add(nuevaTarea.EstadoTarea);
                            }
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
            return false;
        }

        private bool ExisteResidente(int? cedulaResidente)
        {
            if (cedulaResidente != null)
            {
                try
                {
                    return _conn.Residente.Where(r => r.CedulaPersona == cedulaResidente).Any();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        private bool ExistePersonal(int? cedulaPersonal)
        {
            if (cedulaPersonal != null)
            {
                try
                {
                    return _conn.Personal.Where(r => r.CedulaPersona == cedulaPersonal).Any();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        public bool AddEstadoTarea(int idTarea, string nombreEstado)
        {
            if (!ExisteTarea(idTarea))
            {
                try
                {
                    _conn.Database.ExecuteSqlRaw(
                        "insert into Tarea values " +
                        "(@idTarea, @nombreEstado, @fechaYhora)",
                        new SqlParameter("@idTarea", idTarea),
                        new SqlParameter("@nombreEstado", nombreEstado),
                        new SqlParameter("@fechaYhora", DateTime.Now)
                        );

                    return true;

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        private bool ExisteTarea(int idTarea)
        {
            try
            {
                return _conn.Tarea.Where(t => t.IdTarea == idTarea).Any();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateTarea(int idTarea, int? cedulaPersonal, DateTime? fechaDeTarea, int? seRepite,
    DateTime? inicio, DateTime? fin, int? cedulaResidente, string? nombreTarea, string? descripcion)
        {
            if (ExisteTarea(idTarea)) //y no está borrada estado == 0)
            {
                if (cedulaPersonal == null || ExistePersonal(cedulaPersonal))
                {
                    if (cedulaResidente == null || ExisteResidente(cedulaResidente))
                    {
                        try
                        {
                            _conn.Database.ExecuteSqlRaw(
                                "update Tarea set cedulaPersonal = @cedulaPersonal, " +
                                "fechaDeTarea = @fechaDeTarea, seRepite = @seRepite," +
                                "inicio = @inicio, fin = @fin, cedulaResidente = @cedulaResidente, " +
                                "nombreTarea = @nombreTarea, descripcion = @descripcion) where " +
                                "idTarea = @idTarea",
                                new SqlParameter("@idTarea", idTarea),
                                new SqlParameter("@cedulaPersonal", cedulaPersonal),
                                new SqlParameter("@fechaDeTarea", fechaDeTarea),
                                new SqlParameter("@seRepite", seRepite),
                                new SqlParameter("@inicio", inicio),
                                new SqlParameter("@fin", fin),
                                new SqlParameter("@cedulaResidente", cedulaResidente),
                                new SqlParameter("@nombreTarea", nombreTarea),
                                new SqlParameter("@descripcion", descripcion)
                                );

                            return true;

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            return false;
        }

        public IEnumerable<Tarea> FindAllEstadosTareaDeTarea(int recibeIdTarea)
        {
            IEnumerable<Tarea> tareas;
            if (ExisteTarea(recibeIdTarea))
            {
                try
                {
                    tareas = _conn.Tarea.ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

     /*   public IEnumerable<dynamic> GetAllTareasByCedPersonal(int recibeCedulaPersonal)
        {

            try
            {
                //esta consulta muestra todas las tareas de un empleado que no estan finalizadas y son para la fecha del dia
                var tareas = from tarea in _conn.Tarea
                             join estadoTarea in _conn.EstadoTarea
                             on tarea.IdTarea equals estadoTarea.idTarea
                             group new { tarea, estadoTarea } by tarea.IdTarea into g
                             orderby g.First().estadoTarea.fechaYHora descending
                             select new
                             {
                                 IdTarea = g.Key,
                                 NombreEstado = g.First().estadoTarea.nombreEstado,
                                 NombreTarea = g.First().tarea.NombreTarea,
                                 DiaDeTarea = g.First().tarea.diaDeTarea,
                                 Descripcion = g.First().tarea.Descripcion,
                                 CedulaResidente = g.First().tarea.CedulaResidente,
                                 AsignadoA = g.First().estadoTarea.asignadoA,
                                 FechaActualizacion = g.Max(e => e.estadoTarea.fechaYHora)
                             };
            
 

                return tareas.Where(t=> t.FechaActualizacion > DateTime.Today && t.AsignadoA==recibeCedulaPersonal );
           }
           catch (Exception)
            {
               throw;
           }


        }*/
        
       public IEnumerable<Tarea> GetAllTareasByCedPersonal(int recibeCedulaPersonal)
        {

            try
            {
              IEnumerable<EstadoTarea> estadosTareas = _conn.EstadoTarea.FromSqlRaw("exec ObtenerUltimosEstadoTareaByCedPersonal @cedPersonal = @recibeCedulaPersonal",
                  new SqlParameter("@recibeCedulaPersonal", recibeCedulaPersonal)).ToList();
                List<Tarea> tareas = new List<Tarea>();
                foreach(EstadoTarea e  in estadosTareas)
                {
                    if (e.fechaYHora.Value.Date >= DateTime.Now.Date && !e.nombreEstado.Equals("Realizada") ) { 
                    Tarea miTarea = _conn.Tarea.FirstOrDefault(t => t.IdTarea == e.idTarea);
                        miTarea.EstadoTarea = e;
                        tareas.Add(miTarea);
                    }
                }
                return tareas;
            }
            catch (Exception)
            {
                throw;
            }
      

        }
        public bool AddTipoInsumo(string recibeNombre)
        {
            if (!ExisteTipoInsumo(recibeNombre))
            {
                try
                {
                    _conn.Database.ExecuteSqlRaw(
                        "insert into TipoInsumo (nombreTipoInsumo) values (@nombre)",
                        new SqlParameter("@nombre", recibeNombre)
                        );

                    return true;

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }

        public int GetIdTipoConsumo(string recibeNombre)
        {
            {
                try
                {
                    return _conn.TipoInsumo.Where(ti => ti.NombreTipoInsumo.Equals(recibeNombre)).FirstOrDefault().IdTipoInsumo;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool ExisteInsumoPorCodBarra(int recibeCodBarra)
        {
            {
                try
                {
                    return _conn.Insumo.Where(i => i.CodBarrasInsumo == recibeCodBarra).Any();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool ExisteTipoInsumo(string recibeNombre)
        {
            {
                try
                {
                    return _conn.TipoInsumo.Where(ti => ti.NombreTipoInsumo.Equals(recibeNombre)).Any();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IEnumerable<InsumoResidente> GetInsumoResidenteByResidente(int recibeCedResidente)
        {
            IEnumerable<InsumoResidente> Insumos;
            try
            {
                Insumos = _conn.InsumoResidente.Where(ir => ir.CedResidente == recibeCedResidente).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return Insumos;
        }

        public bool Add(Tarea obj)
        {
            try
            {
                if(obj.diaDeTarea == null)
                {
                    obj.diaDeTarea = DateTime.Now;
                }
                if (obj.inicio == null)
                {
                    obj.inicio = DateTime.Now;
                }
                if (obj.Estado == null)
                {
                    obj.Estado = false;
                }
                if (obj.CedulaResidente == 0)
                {
                    obj.CedulaResidente = null;
                }
                _conn.Tarea.Add(obj);
                _conn.SaveChanges();
                EstadoTarea estadoTarea = new EstadoTarea(obj.IdTarea, null, "Pendiente", DateTime.Now);
                _conn.EstadoTarea.Add(estadoTarea); _conn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Remove(int id)
        {
            if (ExisteTarea(id)) //y no está borrada estado == 0)
            {
                try
                {
                    _conn.Database.ExecuteSqlRaw(
                        "update Tarea set estado = 0 where " +
                        "idTarea = @idTarea",
                        new SqlParameter("@idTarea", id)
                        );
                    return true;

                }
                catch (Exception)
                {
                    throw;
                }

            }
            return false;
        }

        public bool Update(Tarea obj)
        {
            try
            {
                _conn.Tarea.Update(obj);
                _conn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //get all tareas
        public IEnumerable<Tarea> FindAll()
        {
            IEnumerable<Tarea> tareas;
            try
            {
                tareas = _conn.Tarea.ToList();
                IEnumerable<PersonalTarea> personal = _conn.PersonalTarea.ToList();
                IEnumerable<InsumoTareas> insumosTarea = _conn.InsumoTareas.ToList();
                foreach (Tarea t in tareas)
                {
                    t.PersonalAsociado = new List<PersonalTarea>();
                    foreach (PersonalTarea p in personal)
                    {
                        if (t.IdTarea == p.IdTarea)
                        {
                            p.nombres = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).NombrePersona;
                            p.apellidos = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).apellidos;
                            t.PersonalAsociado.Add(p);
                        }
                    }
                    //t.InsumosTarea = new List<InsumoTareas>();
                    //foreach (InsumoTareas i in insumosTarea)
                    //{
                    //    if (t.IdTarea == i.idTarea)
                    //    {
                    //        t.InsumosTarea.Add(i);
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
            return tareas;
        }

        //get all tareas
        public IEnumerable<Tarea> GetTareasActivasDelDia()
        {
            IEnumerable<Tarea> tareas;
            try
            {
                tareas = _conn.Tarea.Where(t => t.diaDeTarea.Value.Date == DateTime.Now.Date && t.fin == null).ToList().OrderByDescending(t => t.diaDeTarea);
                IEnumerable<PersonalTarea> personal = _conn.PersonalTarea.ToList();
                IEnumerable<InsumoTareas> insumosTarea = _conn.InsumoTareas.ToList();
                foreach (Tarea t in tareas)
                {
                    t.PersonalAsociado = new List<PersonalTarea>();
                    foreach (PersonalTarea p in personal)
                    {
                        if (t.IdTarea == p.IdTarea)
                        {
                            p.nombres = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).NombrePersona;
                            p.apellidos = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).apellidos;
                            t.PersonalAsociado.Add(p);
                        }
                    }
                    //t.InsumosTarea = new List<InsumoTareas>();
                    //foreach (InsumoTareas i in insumosTarea)
                    //{
                    //    if (t.IdTarea == i.idTarea)
                    //    {
                    //        t.InsumosTarea.Add(i);
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
            return tareas;
        }

        public bool AddActividadDiaria(ActividadesDiarias obj)
        {
            try
            {
                _conn.ActividadesDiarias.Add(obj);
                _conn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool RemoveActividadDiaria(int id)
        //{
        //    if (ExisteTarea(id)) //y no está borrada estado == 0)
        //    {
        //        try
        //        {
        //            _conn.Database.ExecuteSqlRaw(
        //                "update ActividadesDiarias set estado = 0 where " +
        //                "idActividad = @ActividadesDiarias",
        //                new SqlParameter("@ActividadesDiarias", id)
        //                );
        //            return true;

        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }

        //    }
        //    return false;
        //}

        public bool UpdateActividadDiaria(ActividadesDiarias obj)
        {
            try
            {
                _conn.ActividadesDiarias.Update(obj);
                _conn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //get all ActividadesDiarias
        public IEnumerable<ActividadesDiarias> FindAllActividadesDiarias()
        {
            IEnumerable<ActividadesDiarias> actividadesDiarias;
            try
            {
                actividadesDiarias = _conn.ActividadesDiarias.ToList();
                IEnumerable<Residente> residente = _conn.Residente.ToList();
                foreach (ActividadesDiarias ad in actividadesDiarias)
                {
                    foreach (Residente r in residente)
                    {
                        if (ad.cedResidente == r.CedulaPersona)
                        {
                            ad.Residente = r;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return actividadesDiarias;
        }

        public IEnumerable<Tarea> FindById(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ActividadesResidente> GetActividadesDiariasByResponsableByFecha(int idResponsable, DateTime fecha)
        {
            IEnumerable<ActividadesResidente> actividadesDiarias;
            try
            {

                actividadesDiarias = _conn.ActividadesResidente.FromSqlRaw(
                    "exec getActividadesByDateAndResponsable @ci,@fecha",
                    new SqlParameter("@ci", idResponsable),
                    new SqlParameter("@fecha", fecha)
                    ).ToList();                
              
            }
            catch (Exception)
            {
                throw;
            }
            return actividadesDiarias;
        }

        //GetTareasByFecha
        public IEnumerable<Tarea> GetTareasByFecha(DateTime fecha)
        {
            IEnumerable<Tarea> tareas;
            try
            {
                tareas = _conn.Tarea.Where(t => t.diaDeTarea.Value.Date == fecha.Date).ToList();
                IEnumerable<PersonalTarea> personal = _conn.PersonalTarea.ToList();
                IEnumerable<InsumoTareas> insumosTarea = _conn.InsumoTareas.ToList();
                foreach (Tarea t in tareas)
                {
                    t.PersonalAsociado = new List<PersonalTarea>();
                    foreach (PersonalTarea p in personal)
                    {
                        if (t.IdTarea == p.IdTarea)
                        {
                            p.nombres = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).NombrePersona;
                            p.apellidos = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).apellidos;
                            t.PersonalAsociado.Add(p);
                        }
                    }
                    //t.InsumosTarea = new List<InsumoTareas>();
                    //foreach (InsumoTareas i in insumosTarea)
                    //{
                    //    if (t.IdTarea == i.idTarea)
                    //    {
                    //        t.InsumosTarea.Add(i);
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }
            return tareas;
        }

        public IEnumerable<Tarea> GetTareasEntreFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            IEnumerable<Tarea> tareas;
            try
            {
                tareas = _conn.Tarea.Where(t => t.diaDeTarea.Value.Date >= fechaDesde.Date && t.diaDeTarea.Value.Date <= fechaHasta).ToList().OrderByDescending(t=>t.diaDeTarea);
                IEnumerable<PersonalTarea> personal = _conn.PersonalTarea.ToList();
                IEnumerable<InsumoTareas> insumosTarea = _conn.InsumoTareas.ToList();
                IEnumerable<EstadoTarea> estadosTareas = _conn.EstadoTarea.FromSqlRaw("exec ObtenerUltimosEstadoTarea");
                foreach (Tarea t in tareas)
                {
                    t.PersonalAsociado = new List<PersonalTarea>();
                    foreach (PersonalTarea p in personal)
                    {
                        if (t.IdTarea == p.IdTarea)
                        {
                            p.nombres = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).NombrePersona;
                            p.apellidos = _conn.Persona.FirstOrDefault(per => per.CedulaPersona == p.Cedula).apellidos;
                            t.PersonalAsociado.Add(p);
                        }
                    }
                    //t.InsumosTarea = new List<InsumoTareas>();
                    //foreach (InsumoTareas i in insumosTarea)
                    //{
                    //    if (t.IdTarea == i.idTarea)
                    //    {
                    //        t.InsumosTarea.Add(i);
                    //    }
                    //}
                    foreach (EstadoTarea e in estadosTareas)
                    {
                        if (t.IdTarea == e.idTarea)
                        {
                            t.EstadoTarea = e;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return tareas;
        }

        public bool AsignarTarea(int cedulaPersonal, int idTarea)
        {
            try
            {
                if (_conn.Tarea.FirstOrDefault(t => t.IdTarea == idTarea).diaDeTarea.Value.Date == DateTime.Now.Date)
                {
                    //pendiente asignada vencida realizada
                    EstadoTarea estadoTarea = new EstadoTarea(idTarea, cedulaPersonal, "Asignada", DateTime.Now);
                    _conn.EstadoTarea.Add(estadoTarea);
                    return _conn.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddPersonalTarea(IEnumerable<PersonalTarea> personal)
        {
            try
            {
                _conn.PersonalTarea.AddRange(personal);
                return _conn.SaveChanges() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool FinalizarTarea(int cedulaPersonal, int idTarea)
        {
            try
            {
                //pendiente asignada vencida realizada
                EstadoTarea estadoTarea = new EstadoTarea(idTarea, cedulaPersonal, "Realizada", DateTime.Now);
                _conn.EstadoTarea.Add(estadoTarea);

                if(_conn.SaveChanges() > 0)
                {
                    _conn.Tarea.FromSqlRaw("update tarea set fin = @fechaFin where idTarea = @idTarea",
                        new SqlParameter("@idTarea", idTarea),
                        new SqlParameter("@fin", DateTime.Now));
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //GetTareasSinAsignar
        public IEnumerable<dynamic> GetTareasSinAsignar(string rol) {


            //try
            //{
            //    //Consulta para tarea 
            //    var tareas = from tarea in _conn.Tarea
            //                 join estadoTarea in _conn.EstadoTarea
            //                 on tarea.IdTarea equals estadoTarea.idTarea
            //                 group new { tarea, estadoTarea } by tarea.IdTarea into g
            //                 select new
            //                 {
            //                     IdTarea = g.Key,
            //                     NombreEstado = g.First().estadoTarea.nombreEstado,
            //                     NombreTarea = g.First().tarea.NombreTarea,
            //                     DiaDeTarea = g.First().tarea.diaDeTarea,
            //                     Descripcion = g.First().tarea.Descripcion,
            //                     CedulaResidente = g.First().tarea.CedulaResidente,
            //                     AsignadoA = g.First().estadoTarea.asignadoA,
            //                     fechaActualizacion = g.Max(e => e.estadoTarea.fechaYHora)
            //                 };


            //    if (rol.Equals("Encargado"))
            //    {

            //        return tareas.Where(t => !t.NombreEstado.Equals("Realizada") && !t.NombreEstado.Equals("Asignada"));
            //    }
            //    else if (rol.Equals("Empleado"))
            //    {
            //        return tareas.Where(t => !t.NombreEstado.Equals("Realizada") && !t.NombreEstado.Equals("Asignada") &&
            //        t.DiaDeTarea.Value.Date == DateTime.Today.Date || t.DiaDeTarea.Value >= DateTime.Now && t.DiaDeTarea.Value.AddHours(6) <= DateTime.Now);
            //    }
            //    else return null;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}          
            IEnumerable<Tarea> tareas;
            try
            {
                IEnumerable<Residente> residentes = _conn.Residente.ToList();
                if (rol.Equals("Empleado"))
                {
                    tareas = _conn.Tarea.FromSqlRaw("exec ObtenerUltimasTareasSinAsignarDelDia").ToList();
                    IEnumerable<EstadoTarea> estadosTareas = _conn.EstadoTarea.FromSqlRaw("exec ObtenerUltimosEstadoTareasSinAsignarDelDia").ToList();                    
                    foreach (EstadoTarea e in estadosTareas)
                    {
                        foreach (Tarea t in tareas)
                        {
                            if (t.IdTarea == e.idTarea)
                            {
                                foreach(Residente r in residentes)
                                {
                                    if(r.CedulaPersona == t.CedulaResidente)
                                    {
                                        t.nombresResidente = r.NombrePersona;
                                        t.apellidosResidente = r.apellidos;
                                    }
                                }
                                t.EstadoTarea = e;
                            }
                        }
                    }
                }
                else if (rol.Equals("Encargado"))
                {
                    tareas = _conn.Tarea.FromSqlRaw("exec ObtenerUltimasTareasSinAsignar").ToList();
                    IEnumerable<EstadoTarea> estadosTareas = _conn.EstadoTarea.FromSqlRaw("exec ObtenerUltimosEstadoTareaSinAsignar").ToList();
                    foreach (EstadoTarea e in estadosTareas)
                    {
                        foreach (Tarea t in tareas)
                        {
                            if (t.IdTarea == e.idTarea)
                            {
                                foreach (Residente r in residentes)
                                {
                                    if (r.CedulaPersona == t.CedulaResidente)
                                    {
                                        t.nombresResidente = r.NombrePersona;
                                        t.apellidosResidente = r.apellidos;
                                    }
                                }
                                t.EstadoTarea = e;
                            }
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return tareas;

        }

    }
}

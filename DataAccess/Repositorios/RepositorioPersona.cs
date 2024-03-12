using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using Dominio.Repositorio;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositorio
{
    public class RepositorioPersona : IRepositorioPersona
    {

        private ResidencialContext _conn;

        public RepositorioPersona(ResidencialContext residencialContext)
        {

            _conn = residencialContext;

        }

        public bool Add(Persona obj)
        {

            throw new NotImplementedException();
        }

        public bool AddResidente(Residente recibePersona)
        {

            try
            {
                recibePersona.FechaDeIngreso = DateTime.Now;
                recibePersona.Activo = true;
                //if(recibePersona.curatela != DateTime.MinValue)
                //{
                //    recibePersona.tieneCuratela = true;
                //}
                _conn.Persona.Add(recibePersona);
                _conn.Residente.Add(recibePersona);
                if (_conn.SaveChanges() > 1)
                {
                    if (recibePersona.PatologiasCronica != null)
                    {
                        foreach (PatologiaCronica pc in recibePersona.PatologiasCronica)
                        {
                            pc.CedulaResidente = recibePersona.CedulaPersona;
                            _conn.PatologiaCronica.Add(pc);
                            _conn.SaveChanges();
                        }
                        //_conn.PatologiaCronica.AddRange(recibePersona.PatologiasCronica);
                    }
                    if (recibePersona.documentos != null)
                    {
                        foreach (var documento in recibePersona.documentos)
                        {
                            _conn.Documentos.Add(documento);
                            _conn.SaveChanges();
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ingresar residente: ", ex);
            }

        }

        public bool AddResponsable(Responsable recibePersona)
        {

            try
            {
                if (_conn.Residente.Where(r => r.CedulaPersona == recibePersona.Parentesco.cedulaResidente).Any())
                {
                    recibePersona.FechaDeIngreso = DateTime.Now;
                    recibePersona.Activo = true;
                    //_conn.Persona.Add(recibePersona);
                    _conn.Responsable.Add(recibePersona);
                    if (_conn.SaveChanges() == 2)
                    {
                        _conn.Parentesco.Add(recibePersona.Parentesco);
                        _conn.SaveChanges();
                        var cedulaResidente = recibePersona.Parentesco.cedulaResidente;
                        var cedulaResponsable = recibePersona.Parentesco.cedulaResponsable;

                        var residenteToUpdate = _conn.Residente
                            .FirstOrDefault(r => r.CedulaPersona == cedulaResidente);

                        if (residenteToUpdate != null)
                        {
                            residenteToUpdate.CedulaResponsable = cedulaResponsable;
                            _conn.SaveChanges();
                        }
                    }
                    else
                    {
                        return false;
                    }
                    if (recibePersona.Curatela != null)
                    {
                        _conn.Curatela.Add(recibePersona.Curatela);
                        _conn.SaveChanges();
                        var residenteToUpdate = _conn.Residente
      .FirstOrDefault(r => r.CedulaPersona == recibePersona.Curatela.CuratelaResidente);

                        if (residenteToUpdate != null)
                        {
                            residenteToUpdate.tieneCuratela = true;
                            _conn.SaveChanges();
                        }
                    }

                    if (recibePersona.documentos != null) {
                        foreach (var documento in recibePersona.documentos)
                        {
                            _conn.Documentos.Add(documento);
                            _conn.SaveChanges();
                        }
                    }
                    Usuario usuario =
                        new Usuario
                        ((int)recibePersona.CedulaPersona, recibePersona.CedulaPersona.ToString(), true,
                        _conn.TipoUsuario.FirstOrDefault(tu => tu.NombreTipoUsuario.Equals("Responsable")).IdTipoUsuario);
                    if (usuario != null)
                    {
                        _conn.Usuario.Add(usuario);
                        _conn.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ingresar responsable: ", ex);
            }
            return false;
        }

        public bool AddPersonal(Personal nuevaPersona)
        {

            try
            {
                
                    nuevaPersona.FechaDeIngreso = DateTime.Now;
                    nuevaPersona.Activo = true;

                    _conn.Persona.Add(nuevaPersona);
                    _conn.Personal.Add(nuevaPersona);

                if (_conn.SaveChanges() > 0) {
                    if (nuevaPersona.documentos != null)
                    {
                        foreach (var documento in nuevaPersona.documentos)
                        {
                            _conn.Documentos.Add(documento);
                        }
                    }

                }
                Usuario usuario = new Usuario ((int)nuevaPersona.CedulaPersona, nuevaPersona.CedulaPersona.ToString(), true,
                    _conn.TipoUsuario.FirstOrDefault(tu => tu.NombreTipoUsuario.Equals("Empleado")).IdTipoUsuario);
                    _conn.Usuario.Add(usuario);
                return _conn.SaveChanges() > 0;







            }
            catch (Exception e)
            {
                throw;
            }

        }
        public Personal GetPersonalByCedula(int recibeCedula)
        {
            Personal personal;
            try
            {
                personal = _conn.Personal.Where(p => p.CedulaPersona == recibeCedula).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return personal;
        }

        public Responsable GetResponsableByCedula(int recibeCedula)
        {
            Responsable persona;
            try
            {
                persona = _conn.Responsable.Where(p => p.CedulaPersona == recibeCedula).FirstOrDefault();
                if (persona != null)
                {
                    persona.Parentesco = _conn.Parentesco.FirstOrDefault(p => p.cedulaResponsable == persona.CedulaPersona);
                    if (persona.Parentesco != null)
                    {
                        persona.Residente = _conn.Residente.FirstOrDefault(r => r.CedulaPersona == persona.Parentesco.cedulaResidente);
                    }
                    
                   
                    
                        //residente.Medicamentos = _conn.Medicamento.Where(m => m.CedulaResidente == recibeCedula).ToList();
                        persona.documentos = _conn.Documentos.Where(d => d.CedulaPersona == persona.CedulaPersona).ToList();
                    persona.Curatela = _conn.Curatela.FirstOrDefault(c => c.CuratelaResponsable == persona.CedulaPersona);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el responsable por cédula", ex);
            }
            return persona;            
        }

        public Residente GetResidenteByCedula(int recibeCedula)
        {
            Residente residente;
            try
            {
                residente = _conn.Residente.Where(p => p.CedulaPersona == recibeCedula).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
            return residente;
        }



        public bool AddPatologiaCronica(int cedulaResidente, string nombreEnfermedad, string? observaciones, string? medicamento, string? instrucciones)
        {

            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "exec agregarPatologiaCronica @cedulaResidente,@nombreEnfermedad, @observaciones, @medicamento, @instrucciones",
                    new SqlParameter("@cedulaResidente", cedulaResidente),
                    new SqlParameter("@nombreEnfermedad", nombreEnfermedad),
                    new SqlParameter("@observaciones", observaciones ?? (object)DBNull.Value),
                    new SqlParameter("@medicamento", medicamento ?? (object)DBNull.Value),
                    new SqlParameter("@instrucciones", instrucciones ?? (object)DBNull.Value)
                    );

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<PatologiaCronica> GetEnfermedadesCronicasByResidente(int recibeCedResidente)
        {
            IEnumerable<PatologiaCronica> PatologiaCronicas;
            try
            {
                PatologiaCronicas = _conn.PatologiaCronica.Where(ec => ec.CedulaResidente == recibeCedResidente).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return PatologiaCronicas;
        }

        //agrega medicamentos que consuma el residente NO los consumidos
        public bool AddMedicamentosResidente(int cedulaResidente, string medicamento, string instrucciones, int? idEnfermedad)
        {

            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "insert into Medicamento (cedulaResidente, medicamento, instrucciones, idEnfermedad) values (@cedulaResidente, @medicamento, @instrucciones, @idEnfermedad)",
                    new SqlParameter("@cedulaResidente", cedulaResidente),
                    new SqlParameter("@medicamento", medicamento),
                    new SqlParameter("@instrucciones", instrucciones),
                    new SqlParameter("@idEnfermedad", idEnfermedad ?? (object)DBNull.Value) //en caso que no esté asociado, va como null
                    );

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }
        public bool AddVisitante(int ci, string nombre, string apellido, string fechaNac, string email, string telefono, string direccion, string sexo, int? idAgenda, string? observaciones)
        {
            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "exec agregarVisitante @ci,@nombre, @apellido, @fechaNac, @email, @telefono, @direccion, @sexo, @idAgenda, @observaciones",
                    new SqlParameter("@ci", ci),
                    new SqlParameter("@nombre", nombre),
                    new SqlParameter("@apellido", apellido),
                    new SqlParameter("@fechaNac", fechaNac),
                    new SqlParameter("@email", email),
                    new SqlParameter("@telefono", telefono),
                    new SqlParameter("@direccion", direccion),
                    new SqlParameter("@sexo", sexo),
                    new SqlParameter("@idAgenda", idAgenda),
                    new SqlParameter("@observaciones", observaciones ?? (object)DBNull.Value)
                    );

                return true;

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
                return _conn.Persona.Where(u => u.CedulaPersona == recibeCI).ToList().Count > 0;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePersona(int ci, string nombres, string apellidos, string fechaNac, string email, string telefono, string direccion, string sexo)
        {

            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "update persona set nombres = @nombres, " +
                    "apellidos = @apellidos," +
                    "fechaNacimiento = @fechaNacimiento," +
                    "email = @email," +
                    "telefono = @telefono," +
                    "direccion = @direccion," +
                    "sexo = @sexo " +
                    "where cedula = @ci",
                    new SqlParameter("@ci", ci),
                    new SqlParameter("@nombres", nombres),
                    new SqlParameter("@apellidos", apellidos),
                    new SqlParameter("@fechaNacimiento", fechaNac),
                    new SqlParameter("@email", email),
                    new SqlParameter("@telefono", telefono),
                    new SqlParameter("@direccion", direccion),
                    new SqlParameter("@sexo", sexo)
                    );

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool UpdateResidente(int ci, int cedulaResponsable, string emergenciaMovil, string sociedadMedica)
        {

            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "update Residente set cedulaResidente = @cedulaResponsable, emergenciaMovil = @emergenciaMovil, sociedadMedica = @sociedadMedica " +
                    "where ci=@ci",
                    new SqlParameter("@ci", ci),
                    new SqlParameter("@cedulaResponsable", cedulaResponsable),
                    new SqlParameter("@emergenciaMovil", emergenciaMovil),
                    new SqlParameter("@sociedadMedica", sociedadMedica)
                    );

                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePersonal(int ci, string fechaVencCarnetDeSalud, string fechaVencCarnetBromatologia)
        {

            try
            {
                _conn.Database.ExecuteSqlRaw(
                   "update Personal set fechaVencimientoCarnetDeSalud = @fechaVencCarnetDeSalud, fechaVencimientoCarnetBromatologia = @fechaVencCarnetBromatologia " +
                   "where ci=@ci",
                   new SqlParameter("@ci", ci),
                    new SqlParameter("@fechaVencCarnetDeSalud", fechaVencCarnetDeSalud),
                    new SqlParameter("@fechaVencCarnetBromatologia", fechaVencCarnetBromatologia)
                    );

                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool UpdatePersonaFechaEgreso(int ci)
        {
            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "update persona set fechaEgreso = @fechaEgreso," +
                    "where cedula = @ci",
                    new SqlParameter("@ci", ci),
                    new SqlParameter("@fechaEgreso", DateTime.Now)
                    );
                return true;

            }
            catch (Exception)
            {
                throw;
            }

        }





        public bool HabilitarPersona(int ci)
        {
            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "update persona set activo = 1 " +
                    "where cedula = @ci",
                    new SqlParameter("@ci", ci)
                    );
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool EliminarPersona(int ci)
        {
            try
            {
                _conn.Database.ExecuteSqlRaw(
                    "update persona set activo = 0 " +
                    "where cedula = @ci",
                    new SqlParameter("@ci", ci)
                    );
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<Persona> FindAll()
        {
            IEnumerable<Persona> Personas;
            try
            {
                Personas = _conn.Persona.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return Personas;
        }

        public IEnumerable<Residente> FindAllResidentes()
        {
            IEnumerable<Residente> residentes;
            try
            {
               residentes = _conn.Residente.ToList();
               IEnumerable<Persona> responsables = _conn.Responsable.ToList();
                foreach(Residente residente in residentes)
                {
                    foreach(Persona responsable in responsables)
                    {
                        if(responsable.CedulaPersona == residente.CedulaResponsable)
                        {
                            residente.Responsable = (Responsable)responsable;
                        }
                    }
                }
                return residentes;
            }
            catch (Exception)
            {
                throw;
            }
            //return residentes;
        }

        public IEnumerable<Responsable> FindAllResponsables()
        {
            IEnumerable<Responsable> responsables;
            try
            {
                responsables = _conn.Responsable.ToList();
                IEnumerable<Residente> residentes = _conn.Residente.ToList();
                foreach (Responsable responsable in responsables)
                {
                    //responsable.Residentes = new Residente();
                    foreach (Residente residente in residentes)
                    {
                        if (responsable.CedulaPersona == residente.CedulaResponsable)
                        {
                            responsable.Residente = residente;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responsables;
        }
        //public bool AddSolicitud(int ciSol, int ci, string nombre, string apellido, string fechaNac, string email, string telefono, string direccion, string sexo, int cedRes, string parentesco)
        //{
        //    string estado = "Pendiente de Aprobación";
        //    string cedPersonal = null;
        //    try
        //    {
        //        if(!_conn.SolicitudUsuario.Where(su => su.CedSolicitado == ci).Any())
        //        {                
        //        _conn.Database.ExecuteSqlRaw(
        //            "insert into SolicitudUsuario (cedSolicitante, cedSolicitado, nombres, apellidos, fechaNacimiento, email, telefono, direccion, sexo, cedResidente, estado, parentesco, cedPersonal) " +
        //            "values (@ciSol, @ci, @nombre, @apellido, @fechaNac, @email, @telefono, @direccion, @sexo, @cedRes, @estado, @parentesco, @cedPersonal)",
        //            new SqlParameter("@ciSol", ciSol),
        //            new SqlParameter("@ci", ci),
        //            new SqlParameter("@nombre", nombre),
        //            new SqlParameter("@apellido", apellido),
        //            new SqlParameter("@fechaNac", fechaNac),
        //            new SqlParameter("@email", email),
        //            new SqlParameter("@telefono", telefono),
        //            new SqlParameter("@direccion", direccion),
        //            new SqlParameter("@sexo", sexo),
        //            new SqlParameter("@cedRes", cedRes),
        //            new SqlParameter("@estado", estado),
        //            new SqlParameter("@parentesco", parentesco),
        //            new SqlParameter("@cedPersonal", cedPersonal ?? (object)DBNull.Value)
        //            );

        //        return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public IEnumerable<Persona> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Persona obj)
        {
            throw new NotImplementedException();
        }

        public bool AddMisVisitantes(MisVisitantes recibeVisitante)
        {
            try
            {
                if (!ExistePersona(recibeVisitante.CedVisitante)) 
                {
                    recibeVisitante.persona.Activo = true;
                    _conn.Persona.Add(recibeVisitante.persona);
                    _conn.SaveChanges();
                }
                _conn.MisVisitantes.Add(recibeVisitante);
                _conn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IQueryable GetMisVisitantesByCedulaResponsable(int cedulaResponsable)
        {
            try
            {
                //var resultado = from persona in _conn.Persona
                //                join MisVisitantes in _conn.MisVisitantes
                //                on persona.CedulaPersona equals MisVisitantes.CedVisitante
                //                where MisVisitantes.CedResponsable == cedulaResponsable
                //                select MisVisitantes;
                var resultado = from persona in _conn.Persona
                                join MisVisitantes in _conn.MisVisitantes
                                on persona.CedulaPersona equals MisVisitantes.CedVisitante
                                where MisVisitantes.CedResponsable == cedulaResponsable
                                select new
                                {
                                    MisVisitantes.CedResponsable,
                                    MisVisitantes.CedVisitante,
                                    Persona = persona
                                };
                return (resultado);

            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<ActividadesResidente> GetActividadesDiarias(int cedula)
        {
            IEnumerable<ActividadesResidente> actividadesDiarias;
            try
            {

                actividadesDiarias = _conn.ActividadesResidente.FromSqlRaw(
                    "exec getActividadesByDateAndResponsable @ci,@fecha",
                    new SqlParameter("@ci", cedula),
                    new SqlParameter("@fecha", DateTime.Now.Date)
                    ).ToList();

            }
            catch (Exception)
            {
                throw;
            }
            return actividadesDiarias;


        }

        public Residente GetResidenteByResponsable(int cedula)
        {
            try
            {
                Residente resultado = _conn.Residente.FirstOrDefault(d => d.CedulaResponsable == cedula);
                return resultado;


            }
            catch (Exception)
            {
                throw;
            }


        }

        public IQueryable GetPersonalAll()
        {
            try
            {
                //var resultado = from persona in _conn.Persona
                //                join MisVisitantes in _conn.MisVisitantes
                //                on persona.CedulaPersona equals MisVisitantes.CedVisitante
                //                where MisVisitantes.CedResponsable == cedulaResponsable
                //                select MisVisitantes;
                var resultado = from persona in _conn.Persona
                                join Personal in _conn.Personal
                                on persona.CedulaPersona equals Personal.CedulaPersona                            
                                select new
                                {
                                  Personal.CedulaPersona,
                                  Personal.NombrePersona,
                                  Personal.apellidos,
                                  Personal.Email,
                                  Personal.Activo,
                                  Personal.Direccion,
                                  Personal.FechaDeIngreso,
                                  Personal.Telefono
                                };
                return (resultado);

            }
            catch
            {
                throw;
            }
        }

        public bool UpdateResponsable(Responsable recibePersona)
        {
            using (var transaction = _conn.Database.BeginTransaction())
            {
                try
                {
                    _conn.Database.ExecuteSqlRaw(
                        "update persona set nombres = @nombres, " +
                        "apellidos = @apellidos," +
                        "fechaNacimiento = @fechaNacimiento," +
                        "email = @email," +
                        "telefono = @telefono," +
                        "direccion = @direccion," +
                        "sexo = @sexo " +
                        "where cedula = @ci",
                        new SqlParameter("@ci", recibePersona.CedulaPersona),
                        new SqlParameter("@nombres", recibePersona.NombrePersona),
                        new SqlParameter("@apellidos", recibePersona.apellidos),
                        new SqlParameter("@fechaNacimiento", recibePersona.FechaNacimiento),
                        new SqlParameter("@email", recibePersona.Email),
                        new SqlParameter("@telefono", recibePersona.Telefono),
                        new SqlParameter("@direccion", recibePersona.Direccion),
                        new SqlParameter("@sexo", recibePersona.Sexo)
                    );
                    if (recibePersona.Parentesco.NombreParentesco != null && recibePersona.Parentesco.cedulaResidente > 0 && recibePersona.Parentesco.cedulaResponsable > 0)
                    {
                        if (_conn.Residente.Any(r => r.CedulaPersona == recibePersona.Parentesco.cedulaResidente))
                        {
                            _conn.Database.ExecuteSqlRaw(
                                "update parentesco set parentesco = @nombreParentesco, cedulaResidente=@cedulaResidente " +
                                "where cedulaResponsable = @cedulaResponsable",
                                new SqlParameter("@nombreParentesco", recibePersona.Parentesco.NombreParentesco),
                                new SqlParameter("@cedulaResidente", recibePersona.Parentesco.cedulaResidente),
                                new SqlParameter("@cedulaResponsable", recibePersona.Parentesco.cedulaResponsable)
                            );
                            _conn.Database.ExecuteSqlRaw(
                                "update residente set cedulaResponsable = @cedulaResponsable " +
                                 "where cedula = @cedulaResidente",
                                  new SqlParameter("@cedulaResidente", recibePersona.Parentesco.cedulaResidente),
                                  new SqlParameter("@cedulaResponsable", recibePersona.Parentesco.cedulaResponsable)
);
                        }
                    }
                    if (recibePersona.documentos != null)
                    {
                        foreach (var documento in recibePersona.documentos)
                        {
                            if (documento.IdDocumento != null)
                            {
                                _conn.Documentos.Update(documento);
                            }
                            else
                            {
                                _conn.Documentos.Add(documento);
                                _conn.SaveChanges();
                            }
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al actualizar responsable: " + ex.Message);
                }
            }
        }

        public Residente GetDetalleResidenteByCedula(int recibeCedula)
        {
            Residente residente;
            try
            {
                residente = _conn.Residente.FirstOrDefault(p => p.CedulaPersona == recibeCedula);
                if(residente != null)
                {
                    //residente.Medicamentos = _conn.Medicamento.Where(m => m.CedulaResidente == recibeCedula).ToList();
                    residente.Responsable = _conn.Responsable.FirstOrDefault(r => r.CedulaPersona == residente.CedulaResponsable);
                    residente.PatologiasCronica = _conn.PatologiaCronica.Where(pc => pc.CedulaResidente == residente.CedulaPersona).ToList();
                    residente.documentos = _conn.Documentos.Where(d => d.CedulaPersona == residente.CedulaPersona).ToList();
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
            return residente;
        }

        public bool UpdateResidente(Residente recibePersona)
        {
            using (var transaction = _conn.Database.BeginTransaction())
            {
                try
                {
                    _conn.Database.ExecuteSqlRaw(
                        "update persona set nombres = @nombres, " +
                        "apellidos = @apellidos," +
                        "fechaNacimiento = @fechaNacimiento," +
                        "telefono = @telefono," +
                        "direccion = @direccion," +
                        "sexo = @sexo " +
                        "where cedula = @ci",
                        new SqlParameter("@ci", recibePersona.CedulaPersona),
                        new SqlParameter("@nombres", recibePersona.NombrePersona),
                        new SqlParameter("@apellidos", recibePersona.apellidos),
                        new SqlParameter("@fechaNacimiento", recibePersona.FechaNacimiento),
                        new SqlParameter("@telefono", recibePersona.Telefono),
                        new SqlParameter("@direccion", recibePersona.Direccion),
                        new SqlParameter("@sexo", recibePersona.Sexo)
                    );
                    _conn.Database.ExecuteSqlRaw(
                        "update residente set emergenciaMovil = @emergenciaMovil, " +
                        "sociedadMedica = @sociedadMedica " +
                       // ",fechaCuratela = @fechaCuratela " +
                        "where cedula = @ci",
                        new SqlParameter("@ci", recibePersona.CedulaPersona),
                        new SqlParameter("@emergenciaMovil", recibePersona.EmergenciaMovil),
                        new SqlParameter("@sociedadMedica", recibePersona.SociedadMedica)
                        //,new SqlParameter("@fechaCuratela", recibePersona.curatela)
                    );

                    if (recibePersona.PatologiasCronica != null)
                    {
                        foreach (var patologia in recibePersona.PatologiasCronica)
                        {
                            if(patologia.IdPatologiaCronica != null)
                            {
                                _conn.PatologiaCronica.Update(patologia);
                            }
                            else
                            {
                                patologia.CedulaResidente = recibePersona.CedulaPersona;
                                //patologia.IdPatologiaCronica = _conn.PatologiaCronica.OrderBy(item=>item).Last().IdPatologiaCronica + 1;
                                _conn.PatologiaCronica.Add(patologia);
                                _conn.SaveChanges();
                            }
                        }
                    }
                    if (recibePersona.documentos != null)
                    {
                        foreach (var documento in recibePersona.documentos)
                        {
                            if (documento.IdDocumento != null)
                            {
                                _conn.Documentos.Update(documento);
                            }
                            else
                            {
                                _conn.Documentos.Add(documento);
                                _conn.SaveChanges();
                            }
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al actualizar residente: " + ex.Message);
                }
            }
        }

        public Persona? GetPersonaByCedula(int cedula)
        {
            try
            {
                return _conn.Persona.FirstOrDefault(p => p.CedulaPersona == cedula);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Personal GetDetalleEmpleadoByCedula(int recibeCedula)
        {
            Personal personal;
            try
            {
                personal = _conn.Personal.FirstOrDefault(p => p.CedulaPersona == recibeCedula);
                if (personal != null)
                {
                    //residente.Medicamentos = _conn.Medicamento.Where(m => m.CedulaResidente == recibeCedula).ToList();
                    personal.documentos = _conn.Documentos.Where(d => d.CedulaPersona == personal.CedulaPersona).ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
            return personal;
        }

        public bool UpdatePersonal(Personal recibePersona)
        {
            using (var transaction = _conn.Database.BeginTransaction())
            {
                try
                {
                    _conn.Database.ExecuteSqlRaw(
                        "update persona set nombres = @nombres, " +
                        "apellidos = @apellidos," +
                        "fechaNacimiento = @fechaNacimiento," +
                        "telefono = @telefono," +
                        "direccion = @direccion," +
                        "email = @email," +
                        "sexo = @sexo " +
                        "where cedula = @ci",
                        new SqlParameter("@ci", recibePersona.CedulaPersona),
                        new SqlParameter("@nombres", recibePersona.NombrePersona),
                        new SqlParameter("@apellidos", recibePersona.apellidos),
                        new SqlParameter("@fechaNacimiento", recibePersona.FechaNacimiento),
                        new SqlParameter("@telefono", recibePersona.Telefono),
                        new SqlParameter("@email", recibePersona.Email),
                        new SqlParameter("@direccion", recibePersona.Direccion),
                        new SqlParameter("@sexo", recibePersona.Sexo)
                    );
                    _conn.Database.ExecuteSqlRaw(
                        "update personal set fechaVencimientoCarnetDeSalud = @fechaVencimientoCarnetDeSalud, " +
                        "fechaVencimientoCarnetBromatologia = @fechaVencimientoCarnetBromatologia," +
                        "carnetDeVacunas = @carnetDeVacunas " +
                        "where cedula = @ci",
                        new SqlParameter("@ci", recibePersona.CedulaPersona),
                        new SqlParameter("@FechaVencimientoCarnetDeSalud", recibePersona.FechaVencimientoCarnetDeSalud),
                        new SqlParameter("@FechaVencimientoCarnetBromatologia", recibePersona.FechaVencimientoCarnetBromatologia),
                        new SqlParameter("@carnetDeVacunas", recibePersona.carnetDeVacunas)
                    );

                    if (recibePersona.documentos != null)
                    {
                        foreach (var documento in recibePersona.documentos)
                        {
                            if (documento.IdDocumento != null)
                            {
                                _conn.Documentos.Update(documento);
                            }
                            else
                            {
                                _conn.Documentos.Add(documento);
                                _conn.SaveChanges();
                            }
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al actualizar residente: " + ex.Message);
                }
            }
        }

        public IEnumerable<Personal> GetEmpleadosAllActivos()
        {
            try
            {
                return _conn.Personal.Where(p => p.Activo == true).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Residente> GetAllResidentesActivos()
        {
            IEnumerable<Residente> residentes;
            try
            {
                residentes = _conn.Residente.Where(r => r.Activo == true).ToList();
                IEnumerable<Persona> responsables = _conn.Responsable.ToList();
                foreach (Residente residente in residentes)
                {
                    foreach (Persona responsable in responsables)
                    {
                        if (responsable.CedulaPersona == residente.CedulaResponsable)
                        {
                            residente.Responsable = (Responsable)responsable;
                        }
                    }
                }
                return residentes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Responsable> GetAllResponsablesActivos()
        {
            IEnumerable<Responsable> responsables;
            try
            {
                responsables = _conn.Responsable.Where(r => r.Activo == true).ToList();
                IEnumerable<Residente> residentes = _conn.Residente.ToList();
                foreach (Responsable responsable in responsables)
                {
                    //responsable.Residentes = new Residente();
                    foreach (Residente residente in residentes)
                    {
                        if (responsable.CedulaPersona == residente.CedulaResponsable)
                        {
                            responsable.Residente = residente;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responsables;
        }
    }
}

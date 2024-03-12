using Dominio;
using Dominio.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataAccess.Repositorio;

using System.Net.Mime;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using ResidencialAPI.Utils;

namespace ResidencialAPI.Controllers
{
    [ApiController]
    public class TareaController : ControllerBase
    {

        RepositorioTarea repo = new RepositorioTarea(new Dominio.ResidencialContext());

        [HttpGet("GetAllTareas"), Authorize(Roles = "Administrador, Encargado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tarea))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Tarea> FindAll()
        {
            //RepositorioUsuario instanciaSesion = new RepositorioUsuario(new Connection());
            IEnumerable<Tarea> tareas = repo.FindAll();

            try
            {

                if (tareas != null)
                {
                    return tareas;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

   

        [HttpGet("GetAllTareasByCedPersonal/{recibeCedulaPersonal}"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<dynamic>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllTareasByCedPersonal(int recibeCedulaPersonal)
        {
            try
            {

                IEnumerable<dynamic> tareas = repo.GetAllTareasByCedPersonal(recibeCedulaPersonal);
                if (tareas != null)
                {
                    return Ok(tareas);
                }

                else
                {
                    return StatusCode(500, "No se encontraron tareas");
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        [HttpPost("AddTarea/{personal}"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tarea))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       
        public ActionResult AddTarea([FromBody] Tarea miTarea, bool personal)
        {
  
            if (miTarea.ValidarTarea()) {
            try
            {

                    if (repo.AddTarea(miTarea, personal))
                    {
                        RepositorioUsuario repoUsuario = new RepositorioUsuario(new Dominio.ResidencialContext());
                        List<string> tokens = repoUsuario.GetTokenByTipo(3);
                     
                        notifications nuevaNotificacion = new notifications();
                        nuevaNotificacion.SendExpoPushNotificationAsync(tokens, "Se creo una nueva tarea", "Nueva Tarea").Wait();
                        return StatusCode(200, "Tarea creada satisfactoriamente");
                }
                else
                {
                    return StatusCode(500, "Error al crear Tarea, verifique datos");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al crear Tarea");
            }
            }
            else { return StatusCode(500, "Verifique datos"); 
            }
        }

        [HttpPost("EliminarTarea"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tarea))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult EliminarTarea(int idTarea)
        {
                try
                {
                    if (repo.Remove(idTarea))
                    {
                        return StatusCode(200, "Tarea borrada satisfactoriamente");
                    }
                    else
                    {
                        return StatusCode(500, "Error al borrar Tarea, verifique datos");
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error al borrar Tarea");
                }
            }

        [HttpGet("GetTareasActivasDelDia"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tarea))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Tarea> GetTareasActivasDelDia()
        {
            //RepositorioUsuario instanciaSesion = new RepositorioUsuario(new Connection());
            IEnumerable<Tarea> tareas = repo.GetTareasActivasDelDia();

            try
            {

                if (tareas != null)
                {
                    return tareas;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        [HttpGet("GetActividadByResponsableByFecha/{idResponsable}/{fecha}"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Dominio.Tarea>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<ActividadesResidente> GetActividadByResponsableByFecha(int idResponsable, DateTime fecha)
        {

            try
            {
                IEnumerable<ActividadesResidente> actResidente = repo.GetActividadesDiariasByResponsableByFecha(idResponsable, fecha);
                if (actResidente != null)
                {
                    return actResidente;
                }

                if (actResidente != null)
                {
                    return (IEnumerable<ActividadesResidente>)StatusCode(500, "Error interno");
                }
                else
                {
                    return (IEnumerable<ActividadesResidente>)StatusCode(500, "Error interno");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetTareasByFecha/{fecha}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Tarea>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Tarea> GetTareasByFecha(DateTime fecha)
        {

            try
            {
                IEnumerable<Tarea> tareas = repo.GetTareasByFecha(fecha);
                if (tareas != null)
                {
                    return tareas;
                }
                else
                {
                    return (IEnumerable<Tarea>)StatusCode(500, "Error interno");
                }



            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetTareasEntreFechas/{fechaDesde}/{fechaHasta}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Tarea>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Tarea> GetTareasEntreFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde > DateTime.MinValue && fechaHasta > DateTime.MinValue)
            {
                try
                {
                    IEnumerable<Tarea> tareas = repo.GetTareasEntreFechas(fechaDesde, fechaHasta);
                    if (tareas != null)
                    {
                        return tareas;
                    }
                    else
                    {
                        return (IEnumerable<Tarea>)StatusCode(500, "Error interno");
                    }



                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost("AsignarTarea/{cedulaPersonal}/{idTarea}"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> AsignarTarea(int cedulaPersonal, int idTarea)
        {
            if (cedulaPersonal >= 0 && idTarea >= 0)
            {
                try
                {;
                    if (repo.AsignarTarea(cedulaPersonal, idTarea))
                    {
                        return "Asignada con exito";
                    }
                    else
                    {
                        return (StatusCode(500, "Error interno"));
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost("FinalizarTarea/{cedulaPersonal}/{idTarea}"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> FinalizarTarea(int cedulaPersonal, int idTarea)
        {
            if (cedulaPersonal >= 0 && idTarea >= 0)
            {
                try
                {
                    ;
                    if (repo.FinalizarTarea(cedulaPersonal, idTarea))
                    {
                        return "Finalizada con exito";
                    }
                    else
                    {
                        return (StatusCode(500, "Error interno"));
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost("AddPersonalTarea"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> AddPersonalTarea([FromBody] IEnumerable<PersonalTarea> personal)
        {
            if (personal.Any())
            {
                try
                {
                    ;
                    if (repo.AddPersonalTarea(personal))
                    {
                        return "Personal añadido con exito";
                    }
                    else
                    {
                        return (StatusCode(500, "Error interno"));
                    }



                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }


        [HttpGet("GetTareasSinAsignar/{rol}"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<dynamic>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<string> GetTareasSinAsignar(string rol)
        {
           
                try
                {
                if (!rol.IsNullOrEmpty())
                {
                    return Ok(repo.GetTareasSinAsignar(rol));

                }
                else
                {
                    {
                        return (StatusCode(500, "Verifique datos"));
                    }
                }
                }
                catch (Exception)
                {
                    throw;
                }
            

        }
    }
    }


    



using Dominio;
using Dominio.Repositorio;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataAccess.Repositorio;

using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ResidencialAPI.Utils;

namespace ResidencialAPI.Controllers
{
    [ApiController]
    public class AgendaController : ControllerBase
    {

        RepositorioAgenda repo = new RepositorioAgenda(new Dominio.ResidencialContext());

        [HttpGet("GetAgendaByResponsable/{cedResponsable}"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Agenda>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Agenda> GetAgendaByResponsable(int cedResponsable)
        {
            try
            {
                IEnumerable<Agenda> agendas = repo.GetAgendaByResponsable(cedResponsable);

                if (agendas != null)
                {
                    return agendas;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }               

        [HttpPost("AddAgenda"), Authorize(Roles = "Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddAgenda([FromBody] Agenda agenda)
        {
            if (agenda != null)
            {
                try
                {
                    if (agenda.Validacion())
                    {

                        if (repo.AddAgenda(agenda))
                        {
                            string mensaje = "Operacion exitosa";
                            RepositorioUsuario repoUsuario= new RepositorioUsuario(new Dominio.ResidencialContext());
                           List<string> tokens = repoUsuario.GetTokenByTipo(2);
                            notifications nuevaNotificacion = new notifications();
                           nuevaNotificacion.SendExpoPushNotificationAsync(tokens, "Ha recibido una nueva solicitud de agenda", "Agenda").Wait();


                            return Ok(JsonConvert.SerializeObject(mensaje));
                        }
                        else
                        {
                            return StatusCode(406, "Revise los datos ingresados");
                        }
                    }
                    else { return StatusCode(406, "Revise los datos ingresados"); }

                }
                catch (Exception)
                {
                    return StatusCode(500, "Ha ocurrido un error");
                }
            }
            else {
                return StatusCode(500, "Revise los datos ingresados");
            }      
        }

        [HttpGet("GetAgendasPorEstado/{recibeNombreEstado}"), Authorize(Roles = "Administrador, Empleado, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Agenda>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Agenda> GetAgendasPorEstado(string recibeNombreEstado)
        {
            try
            {
                IEnumerable<Agenda> agendas = repo.GetAgendasPorEstado(recibeNombreEstado);

                if (agendas != null)
                {
                    return agendas;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return (IEnumerable<Agenda>) StatusCode(500, "Ha ocurrido un error");
            }
        }

        [HttpGet("GetAgendasDelDia"), Authorize(Roles = "Administrador, Empleado, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Agenda>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Agenda> GetAgendasDelDia()
        {
            try
            {
                IEnumerable<Agenda> agendas = repo.GetAgendasDelDia();

                if (agendas != null)
                {
                    return agendas;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return (IEnumerable<Agenda>)StatusCode(500, "Ha ocurrido un error");
            }
        }

        [HttpPost("EvaluarAgenda/{recibeIdAgenda}/{recibeAprobarORechazar}/{recibeComentario}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EvaluarAgenda(int recibeIdAgenda, string recibeAprobarORechazar, string recibeComentario)
        {
            if (recibeIdAgenda > 0 && !recibeAprobarORechazar.IsNullOrEmpty() && !recibeComentario.IsNullOrEmpty())
            {
                try
                {
                    if (repo.ExisteAgenda(recibeIdAgenda))
                    {
                        if(repo.AddEstadoAgenda(recibeIdAgenda, recibeAprobarORechazar, recibeComentario))
                        {
                            return StatusCode(200, "Agenda evaluada correctamente");
                        }
                        else
                        {
                            return StatusCode(500, "Error al evaluar agenda");
                        }
                    }
                    else
                    {
                        return StatusCode(406, "Agenda no encontrada");
                    }
                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al evaluar agenda");
                }

            }
            return StatusCode(500, "Verifique campos");
        }

        [HttpGet("GetAgendasEntreFechas/{fechaDesde}/{fechaHasta}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Agenda>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Agenda> GetAgendasEntreFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde > DateTime.MinValue && fechaHasta > DateTime.MinValue)
            {
                try
                {
                    IEnumerable<Agenda> agendas = repo.GetAgendasEntreFechas(fechaDesde, fechaHasta);
                    if (agendas != null)
                    {
                        return agendas;
                    }
                    else
                    {
                        return (IEnumerable<Agenda>)StatusCode(500, "Error interno");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        [HttpPost("FinalizarAgenda/{cedulaPersonal}/{idAgenda}/{observaciones}/{visitado}"), Authorize(Roles = "Administrador, Empleado, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult FinalizarAgenda(int cedulaPersonal, int idAgenda, string observaciones, bool visitado)
        {
            if (cedulaPersonal >= 0 && idAgenda >= 0)
            {
                try
                {
                    ;
                    if (repo.FinalizarAgenda(cedulaPersonal, idAgenda, observaciones, visitado))
                    {
                        return StatusCode(200, "Finalizada con exito");
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
    }       
}


using Dominio;
using Dominio.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataAccess.Repositorio;

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net.WebSockets;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using ResidencialAPI.Utils;

namespace ResidencialAPI.Controllers
{
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        RepositorioUsuario repo = new RepositorioUsuario(new Dominio.ResidencialContext());
        private readonly IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //se encarga del login de usuarios        
        [HttpPost("IniciarSesion")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status307TemporaryRedirect)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult IniciarSesion([FromBody] Usuario login)
        {
            if (login.cedula != null && login.pass != null && login.cedula != 0
                && login.pass != "")
            {
                try
                {
                    Usuario inicioUsuario = repo.BuscarUsuarioPorCI(login.cedula);
                    if (inicioUsuario.cedula != 0)
                    {
                        if(inicioUsuario.primerPass == true)
                        {
                            if(repo.VerificarPasswordSinHashear(login.cedula, login.pass))
                            {
                            return StatusCode(307, "Primer login");
                            }
                            else
                            {
                                return StatusCode(500, "Verifique campos");
                            }
                        }
                        else
                        {
                            if (repo.VerificarPasswordHasheada(login.cedula, login.pass))
                            {
                                CreateToken(inicioUsuario);
                                repo.CrearLogSesion(inicioUsuario.cedula, "Login");
                                inicioUsuario.TokenDispositivos= login.TokenDispositivos;
                                if (repo.Update(inicioUsuario))
                                {
                                    return StatusCode(200, inicioUsuario.tokenUsuario);
                                }
                                else {
                                    return StatusCode(500, "Ocurrio un error al iniciar, intente nuevamente");
                                }
                               
                            }else{
                                return StatusCode(500, "Verifique campos");
                            }
                        }
                        //return StatusCode(200, inicioUsuario);
                        //CreateToken(inicioUsuario);
                        //return StatusCode(200, inicioUsuario);
                    }
                    else
                    {
                        return StatusCode(404, inicioUsuario);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error al buscar usuario");
                }
            }
            else
            {
                return StatusCode(500, "Verifique campos");
            }

        }

        [HttpPost("AddUsuario"), Authorize(Roles = "Administrador, Encargado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddUsuario([FromBody] Usuario nuevoUsuario)
        {
            ;
            if (nuevoUsuario.validaUsuario())
            {
                try
                {
                    if (!repo.ExisteUsuario(nuevoUsuario.cedula))
                    {
                        repo.AddUsuario(nuevoUsuario);
                        return StatusCode(200, JsonConvert.SerializeObject( "Usuario añadido correctamente"));
                    }
                    else
                    {
                        return StatusCode(500,JsonConvert.SerializeObject( "Usuario ya existe"));
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, JsonConvert.SerializeObject( "Error al buscar usuario"));
                }
            }
            else
            {
                return StatusCode(500, JsonConvert.SerializeObject( "Revise los datos ingresados"));
            }
        }


        [HttpGet("GetAllUsuarios"), Authorize(Roles = "Administrador, Encargado")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Usuario> FindAllResponsables()
        {
            //RepositorioUsuario instanciaSesion = new RepositorioUsuario(new Connection());
            IEnumerable<Usuario> usuarios = repo.FindAllUsuarios();

            try
            {

                if (usuarios != null)
                {
                    return usuarios;
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

        [HttpPost("ResetPassword")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ResetPassword([FromBody] CambiarPass recibeDatos)
        {
            if (recibeDatos.Cedula != 0 && !recibeDatos.PassActual.IsNullOrEmpty()  && !recibeDatos.Pass.IsNullOrEmpty() && !recibeDatos.RePass.IsNullOrEmpty())
            {
                if (recibeDatos.Pass.Equals(recibeDatos.RePass))
                {
                    try
                    {
                        Usuario inicioUsuario = repo.BuscarUsuarioPorCI(recibeDatos.Cedula);
                        if (inicioUsuario.primerPass == true)
                        {
                            if(repo.VerificarPasswordSinHashear(recibeDatos.Cedula, recibeDatos.PassActual))
                            {
                                if (recibeDatos.TerminosAceptados)
                                {
                                    repo.AceptarTerminos(recibeDatos);
                                }
                                repo.CambiarPassword(recibeDatos.Cedula, recibeDatos.Pass);
                                return Ok("Contraseña cambiada satisfactoriamente");
                            }
                            else
                            {
                                return NotFound("Usuario no encontrado");
                            }
                        }
                        else
                        {
                            if (repo.VerificarPasswordHasheada(recibeDatos.Cedula, recibeDatos.PassActual))
                            {
                                repo.CambiarPassword(recibeDatos.Cedula, recibeDatos.Pass);
                                return Ok("Contraseña cambiada satisfactoriamente");
                            }
                            else
                            {
                                return NotFound("Usuario no encontrado");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return StatusCode(500, "Error al cambiar contraseña");
                    }
                }
                else
                {
                    return Ok("Contraseñas no coinciden");
                }

            }
            return Ok("Retorno");
        }

        private string CreateToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.cedula.ToString()),
                new Claim(ClaimTypes.Role, usuario.tipoUsuario.NombreTipoUsuario)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(365),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            Token miToken = new Token(int.Parse(token.Claims.ToList()[0].Value), token.Claims.ToList()[1].Value, token.ValidTo, jwt.ToString());
            repo.GuardarToken(miToken);
            usuario.tokenUsuario = miToken;
            return jwt;
        }

        [HttpPost("AddSolicitudUsuario"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SolicitudUsuario))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddSolicitudUsuario([FromBody] SolicitudUsuario solicitudUsuario)
        {
            if (solicitudUsuario.CedSolicitante > 0 &&
                solicitudUsuario.CedSolicitado > 0 &&
                !solicitudUsuario.nombres.IsNullOrEmpty() &&
                !solicitudUsuario.apellidos.IsNullOrEmpty() &&
                !solicitudUsuario.email.IsNullOrEmpty() &&
                !solicitudUsuario.telefono.IsNullOrEmpty() &&
                !solicitudUsuario.sexo.IsNullOrEmpty() &&
                solicitudUsuario.fechaNacimiento > DateTime.MinValue
                )
            {
                try
                {
                    if (repo.AddSolicitudUsuario(solicitudUsuario))
                    {
                        RepositorioUsuario repoUsuario = new RepositorioUsuario(new Dominio.ResidencialContext());
                        List<string> tokens = repoUsuario.GetTokenByTipo(1);
                        notifications nuevaNotificacion = new notifications();
                        nuevaNotificacion.SendExpoPushNotificationAsync(tokens, "Ha recibido una nueva solicitud de usuario", "Nuevo Usuario").Wait();
                        return StatusCode(200, "Solicitud añadida correctamente");
                    }
                    else
                    {
                            return StatusCode(500, "Error al ingresar solicitud");
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error al buscar usuario");
                }
            }
            else
            {
                return StatusCode(500, "Verifique datos");
            }
        }

        [HttpGet("GetTipoDeUsuario/{idUsuario}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Dominio.Tarea>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTiposDeUsurio(int idUsuario){
            
            try
            {

                if (idUsuario != 0)
                {
                    return Ok(repo.GetTiposUsuarioAll());

                }
                else
                {
                    return StatusCode(500, "No se encontro Tipos");


                }


            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost("AprobarSolicitudUsuario/{recibeIdSolicitud}/{recibeCedulaPersonal}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AprobarSolicitudUsuario(int recibeIdSolicitud, int recibeCedulaPersonal)
        {
            if (recibeIdSolicitud > 0)
            {
                try
                {
                    if (repo.ExisteSolicitud(recibeIdSolicitud))
                    {
                        repo.AprobarSolicitudUsuario(recibeIdSolicitud, recibeCedulaPersonal);
                        return StatusCode(200, "Solicitud aprobada correctamente");
                    }
                    else
                    {
                        return StatusCode(406, "Solicitud no encontrada");
                    }

                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al aprobar Solicitud");

                }

            }
            return StatusCode(500, "Verifique campos");
        }

        [HttpPost("RechazarSolicitudUsuario/{recibeIdSolicitud}/{recibeCedulaPersonal}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RechazarSolicitudUsuario(int recibeIdSolicitud, int recibeCedulaPersonal)
        {
            if (recibeIdSolicitud > 0)
            {
                try
                {
                    if (repo.ExisteSolicitud(recibeIdSolicitud))
                    {
                        repo.RechazarSolicitudUsuario(recibeIdSolicitud, recibeCedulaPersonal);
                        return StatusCode(200, "Solicitud rechazada correctamente");
                    }
                    else
                    {
                        return StatusCode(406, "Solicitud no encontrada");
                    }

                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al rechazar solicitud");

                }

            }
            return StatusCode(500, "Verifique campos");
        }

        [HttpGet("GetSolicitudesUsuarioPorEstado/{recibeEstado}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetSolicitudesUsuarioPorEstado(string recibeEstado)
        {
            if (!recibeEstado.IsNullOrEmpty())
            {
                try
                {
                    return StatusCode(200, repo.GetSolicitudesUsuarioPorEstado(recibeEstado));                    

                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al obtener solicitudes");

                }

            }
            return StatusCode(500, "Verifique campos");
        }

        [HttpGet("GetSolicitudesUsuarioProcesadas"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetSolicitudesUsuarioProcesadas()
        {
                try
                {
                    return StatusCode(200, repo.GetSolicitudesUsuarioProcesadas());

                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al obtener solicitudes");

                }
        }

        [HttpPost("GetTokenUsuario"), Authorize(Roles = "Administrador, Encargado, Responsable, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTokenUsuario([FromBody] string recibePassKey)
        {
            try
            {
                if (repo.CompararPassKey(recibePassKey))
                {
                    return StatusCode(200, true);

                }
                else
                {
                    return StatusCode(500, false);
                }

            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, false);

            }
        }

        [HttpPost("LogOff"), Authorize(Roles = "Administrador, Encargado, Responsable, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Logoff([FromBody] int recibeCedula)
        {
            try
            {
                repo.CrearLogSesion(recibeCedula, "Log off");
                return StatusCode(200, "Log off exitoso");
            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, "Error al hacer log off");

            }
        }


        [HttpGet("SendNotificationsEmergencia/{ciResidente}"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SendNotifications(int ciResidente)
        {
            try
            {
                notifications nuevaNotificacoin= new notifications();

                List<string> tokens = repo.GetTokenDispositivoResponsable(ciResidente);
                if (tokens != null)
                {
                    if (tokens.Count > 0)
                    {
                        nuevaNotificacoin.SendExpoPushNotificationAsync(tokens, "Por favor comuniquese con el residencial a la brevedad", "Emergencia").Wait();
                        return StatusCode(200, "Notificación enviada con éxito");
                    }
                    else
                    {
                        return StatusCode(500, "No hay dispositivos para notificiar, intente por otro medio");
                    }

                }
                else {
                    return StatusCode(500, "No se encontraron dispositivos para notificar, intente por otro medio");
                
                }
             

            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, "Error al obtener solicitudes");

            }
        }

        [HttpGet("GetTerminosYCondiciones")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTerminosYCondiciones()
        {
            try
            {
                TerminosYCondiciones terminosYCondiciones = repo.GetTerminosYCondiciones();
                    return StatusCode(200, terminosYCondiciones);
            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, "Error al buscar términos y condiciones.");

            }

        }

        [HttpPost("CerrarTareasYAgendas"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CerrarTareasYAgendas()
        {
            try
            {
                repo.CerrarTareasYAgendas();
                return StatusCode(200, true);
            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, "Error al cerrar tareas y agendas.");

            }

        }
    }
}


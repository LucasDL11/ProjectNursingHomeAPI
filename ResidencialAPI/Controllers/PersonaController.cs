using Dominio;
using Dominio.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataAccess.Repositorio;

using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;

namespace ResidencialAPI.Controllers
{
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private IWebHostEnvironment _environment;

        RepositorioPersona repo = new RepositorioPersona(new Dominio.ResidencialContext());


        public PersonaController(IWebHostEnvironment enviroment)
        {
            _environment = enviroment;
        }

        [HttpGet("GetAllResidentes"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Residente>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Residente> FindAllResidentes()
        {
            IEnumerable<Residente> residentes = repo.FindAllResidentes();
            try
            {
                if (residentes != null)
                {
                    return residentes;
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

        [HttpGet("GetAllResidentesActivos"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Residente>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllResidentesActivos()
        {
            try
            {
                return StatusCode(200, repo.GetAllResidentesActivos());
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al buscar residentes");
            }
        }

        [HttpGet("GetAllResponsablesActivos"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Responsable>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllResponsablesActivos()
        {
            try
            {
                return StatusCode(200, repo.GetAllResponsablesActivos());
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al buscar responsables");
            }
        }

        [HttpPost("AddPatologiaCronica")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddPatologiaCronica(int cedulaResidente, string nombreEnfermedad, string? observaciones, string? medicamento, string? instrucciones)
        {

            try
            {
                if (repo.AddPatologiaCronica(cedulaResidente, nombreEnfermedad, observaciones, medicamento, instrucciones))
                {

                    return StatusCode(200, "Ingresado correctamente");

                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Error al agregar el enfermedad");
            }

        }

        [HttpPost("AddMedicamentosResidente")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddMedicamentosResidente(int cedulaResidente, string medicamento, string instrucciones, int? idEnfermedad)
        {

            try
            {
                if (repo.AddMedicamentosResidente(cedulaResidente, medicamento, instrucciones, idEnfermedad))
                {

                    return StatusCode(200, "Ingresado correctamente");

                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Error al agregar el medicamento");
            }

        }
        
        [HttpPost("HabilitarPersona"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult HabilitarPersona([FromBody] int ci)
        {
            try
            {
                if (repo.ExistePersona(ci))
                {
                    if (repo.HabilitarPersona(ci))
                    {
                        return StatusCode(200, "Cedula: " + ci.ToString() + " habilitada correctamente");
                    }
                    else
                        //406 Not Acceptable
                        return StatusCode(406, "Error al habilitar");
                }
                else
                {
                    return StatusCode(406, "Persona no encontrada");
                }


            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, "Error al habilitar");

            }
        }

        [HttpPost("EliminarPersona/{cedulaPersona}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EliminarPersona(int cedulaPersona)
        {

            try
            {
                if (repo.ExistePersona(cedulaPersona))
                {
                    if (repo.EliminarPersona(cedulaPersona))
                    {
                        return StatusCode(200, "Cedula: " + cedulaPersona.ToString() + " dado de baja correctamente");
                    }
                    else
                        //406 Not Acceptable
                        return StatusCode(406, "Error al eliminar");
                }
                else
                {
                    return StatusCode(406, "Persona no encontrada");
                }
            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, "Error al eliminar");
            }
        }


        [HttpGet("GetPersonaByCedula/{cedula}"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Persona))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPersonaByCedula(int cedula)
        {
            if (cedula > 0)
            {
                try
                {
                    if (repo.ExistePersona(cedula))
                    {
                        return StatusCode(200, repo.GetPersonaByCedula(cedula));
                    }
                    else
                    {
                        return StatusCode(404, "Persona no encontrada");
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }
            return StatusCode(400, "Verifique campos");
        }


        [HttpPost("AddMisVisitantes"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddMisVisitantes([FromBody] MisVisitantes recibeVisitante)
        {
            if (recibeVisitante.CedResponsable > 0 && recibeVisitante.CedVisitante > 0 && recibeVisitante.persona.validaciones().Equals(""))
                try
                {
                    repo.AddMisVisitantes(recibeVisitante);
                    return StatusCode(200, "Visitante añadida correctamente");
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error al añadir visitante");
                }
            return StatusCode(500, "Verifique campos");
        }


        [HttpGet("GetMisVisitantesByCedulaResponsable/{idResponsable}"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MisVisitantes>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMisVisitantesByCedulaResponsable(int idResponsable)
        {
            if (idResponsable > 0)
            {
                try
                {
                    if (repo.ExistePersona(idResponsable))
                    {
                        return StatusCode(200, repo.GetMisVisitantesByCedulaResponsable(idResponsable));
                    }
                    else
                    {
                        return StatusCode(406, "Persona no encontrada");
                    }
                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al buscar persona");

                }
            }
            return StatusCode(500, "Verifique campos");
        }


        [HttpGet("GetActividadByResponsable/{idResidente}"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Dominio.Tarea>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<ActividadesResidente> GetActividadByResponsable(int idResidente)
        {

            try
            {
                IEnumerable<ActividadesResidente> actResidente = repo.GetActividadesDiarias(idResidente);
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


        //Get para consultar residente que le pertenecen al responsable 
        [HttpGet("GetResidenteByResponsable/{idResponsable}"), Authorize(Roles = "Administrador, Encargado, Responsable")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Residente))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetResidentesByRequest(int idResponsable)
        {
            try
            {
                Residente residentes = null;
                if (idResponsable != 0)
                {
                    residentes = repo.GetResidenteByResponsable(idResponsable);
                    if (residentes != null)
                    {
                        return (StatusCode(200, residentes));
                    }
                    else
                    {
                        return (StatusCode(500, "Residente no encontrado"));
                    }
                }
                else
                {
                    return (StatusCode(500, "Verifique datos"));
                }
            }
            catch (Exception)
            {
                return (StatusCode(500, "Error interno"));
            }
        }

        [HttpGet("GetEmpleadosAll"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Personal>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmpleadosAll()
        {
            
            try
            {
                return Ok(repo.GetPersonalAll());              
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al buscar empleados");
            }
        }

        [HttpGet("GetEmpleadosAllActivos"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Personal>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmpleadosAllActivos()
        {
            try
            {
                return StatusCode(200, repo.GetEmpleadosAllActivos());
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al buscar empleados");
            }
        }

        [HttpPost("AddResponsable"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddResponsable([FromBody] Responsable recibePersona)
        {
            if (recibePersona.validaciones() == "")
                try
                {
                    try
                    {
                        List<Documentos> documentos = recibePersona.documentos;
                        if (documentos != null)

                        {
                            List<Documentos> documentosASubir = new List<Documentos>();
                            if(recibePersona.Curatela.documentoCuratela != null)
                            {
                                documentos.Add(recibePersona.Curatela.documentoCuratela);
                            }
                            for (int i = 0; i < documentos.Count(); i++)
                            {
                                if (documentos[i].TipoArchivo.ToLower().Equals(".pdf")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpg")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpeg")
                                    && !documentos[i].NombreDocumento.IsNullOrEmpty()
                                    && !documentos[i].Descripcion.IsNullOrEmpty())
                                {
                                    documentos[i].CedulaPersona = recibePersona.CedulaPersona;
                                    documentos[i] = this.GuardarArchivo(documentos[i]);
                                    documentosASubir.Add(documentos[i]);
                                }
                            }
                            recibePersona.documentos = documentosASubir;
                        }
                    }
                    catch
                    {
                        return StatusCode(500, JsonConvert.SerializeObject("Ocurrio un error interno y no se pudo agregar la nueva persona"));
                    }

                    repo.AddResponsable(recibePersona);
                    return StatusCode(200, "Responsable añadido correctamente");
                }
                catch (Exception)
                {
                }
            return StatusCode(500, "Verifique campos");
        }

        [HttpPost("AddResidente"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddResidente([FromBody] Residente recibePersona)
        {
            if (recibePersona.validaciones() == "")
            {
                try
                {
                    try
                    {
                        List<Documentos> documentos = recibePersona.documentos;
                        if (documentos != null)
                        {
                            List<Documentos> documentosASubir = new List<Documentos>();
                            for (int i = 0; i < documentos.Count(); i++)
                            {
                                if (documentos[i].TipoArchivo.ToLower().Equals(".pdf")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpg")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpeg")
                                    && !documentos[i].NombreDocumento.IsNullOrEmpty()
                                    && !documentos[i].Descripcion.IsNullOrEmpty())
                                {
                                    documentos[i].CedulaPersona = recibePersona.CedulaPersona;
                                    documentos[i] = this.GuardarArchivo(documentos[i]);
                                    documentosASubir.Add(documentos[i]);
                                }
                            }
                            recibePersona.documentos = documentosASubir;
                        }
                    }
                    catch
                    {
                        return StatusCode(500, JsonConvert.SerializeObject("Ocurrio un error interno y no se pudo agregar la nueva persona"));
                    }
                    repo.AddResidente(recibePersona);
                    return StatusCode(200, "Responsable añadido correctamente");
                }

                catch (Exception)
                {
                    return StatusCode(500, "Error al añadir residente");
                }
            }
            else
            {
                return StatusCode(500, "Verifique campos");
            }
        }

        [HttpPost("AddPersonal"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddPersonal([FromBody] Personal nuevaPersona)
        {
            string mensaje = "";
            RepositorioUsuario nuevoRepo = new RepositorioUsuario(new Dominio.ResidencialContext());
            if (nuevaPersona.validaciones() == "")
                {
                    try
                    {

                    Persona usuarioExistente = repo.GetPersonaByCedula((int)nuevaPersona.CedulaPersona);
                    if (usuarioExistente != null) {
                        if (usuarioExistente.CedulaPersona == nuevaPersona.CedulaPersona)
                        {
                            return StatusCode(500, "Error: El empleado ingresado ya se encuentra registrado");

                        }
                    }
                   
                        List<Documentos> documentos = nuevaPersona.documentos;
                        if (documentos != null){
                                List<Documentos> documentosASubir = new List<Documentos>();
                                for (int i = 0; i < documentos.Count(); i++)
                                {
                                    if (documentos[i].TipoArchivo.ToLower().Equals(".pdf")
                                        || documentos[i].TipoArchivo.ToLower().Equals(".jpg")
                                        || documentos[i].TipoArchivo.ToLower().Equals(".jpeg")
                                        && !documentos[i].NombreDocumento.IsNullOrEmpty()
                                        && !documentos[i].Descripcion.IsNullOrEmpty())
                                    {
                                        documentos[i].CedulaPersona = nuevaPersona.CedulaPersona;
                                        documentos[i] = this.GuardarArchivo(documentos[i]);
                                        documentosASubir.Add(documentos[i]);
                                    }
                                }
                                nuevaPersona.documentos = documentosASubir;

                            }
                    }
                        catch{
                        return StatusCode(500, JsonConvert.SerializeObject("Ocurrio un error interno y no se pudo agregar la nueva persona"));

                    }
                       
                       
                            if (repo.AddPersonal(nuevaPersona))
                            {

                              
                                    return StatusCode(200, JsonConvert.SerializeObject("Personal añadido correctamente"));
                              

                            }
                            else {

                                return StatusCode(500, JsonConvert.SerializeObject("Ocurrio un error interno y no se pudo agregar la nueva persona"));
                            }
                            
                           
                       
                       

                     
                    }
                    else {
                        return StatusCode(500, JsonConvert.SerializeObject( mensaje)); 
                    }  
        }

        [HttpGet("GetResponsableByCedula/{cedula}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Persona))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetResponsableByCedula(int cedula)
        {
            if (cedula > 0)
            {
                try
                {
                    if (repo.ExistePersona(cedula))
                    {
                        return StatusCode(200, repo.GetResponsableByCedula(cedula));
                    }
                    else
                    {
                        return StatusCode(406, "Persona no encontrada");
                    }

                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al buscar persona");

                }

            }
            return StatusCode(500, "Verifique campos");
        }






        //Transferir archivo 
        Documentos GuardarArchivo(Documentos archivo)
        {
           
           string rutaPrueba= "DefaultEndpointsProtocol=https;AccountName=residencialfilestorage;AccountKey=dhjvifk0mQIOVLD+W5FMQCJesi3TTMcPYKPnpgCjy6MPsNMESlItiSR2wUWYfodfnmkCyAHexh7D+AStF99NTw==;EndpointSuffix=core.windows.net";
         
            //FileStream permite manejar archivos
          
                if (archivo.NombreDocumento == null || archivo.ArchivoBase64 == null)
                    return null;

                try
                {

                    // Pasar archivo en base 64 a Stream
                    byte[] fileBytes = Convert.FromBase64String(archivo.ArchivoBase64);
                    Stream fileStream = new MemoryStream(fileBytes);

                    archivo.NombreDocumento = archivo.CedulaPersona + "_" + archivo.NombreDocumento;
                    string azureFileName = archivo.NombreDocumento + archivo.TipoArchivo;
                    // Guardar archivo en Azure File Share
                    AzureFileShareService fileShareService = new AzureFileShareService(rutaPrueba, "docs", "");
                    fileShareService.UploadFileAsync(azureFileName, fileStream).Wait();

                    archivo.RutaDocumento = azureFileName;

                    return archivo;
                }
                catch (Exception)
                {
                    throw;
                }


              
           
        }

        [HttpPost("UpdateResponsable"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateResponsable([FromBody] Responsable recibePersona)
        {
            if (recibePersona.validaciones() == "")
            {
                try
                {
                    try
                    {
                        List<Documentos> documentos = recibePersona.documentos;
                        if (documentos != null)
                        {
                            List<Documentos> documentosASubir = new List<Documentos>();
                            for (int i = 0; i < documentos.Count(); i++)
                            {
                                if (documentos[i].TipoArchivo.ToLower().Equals(".pdf")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpg")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpeg")
                                    && !documentos[i].NombreDocumento.IsNullOrEmpty()
                                    && !documentos[i].Descripcion.IsNullOrEmpty())
                                {
                                    documentos[i].CedulaPersona = recibePersona.CedulaPersona;
                                    documentos[i] = this.GuardarArchivo(documentos[i]);
                                    documentosASubir.Add(documentos[i]);
                                }
                            }
                            recibePersona.documentos = documentosASubir;
                        }
                    }
                    catch
                    {
                        return StatusCode(500, JsonConvert.SerializeObject("Ocurrio un error interno y no se pudo agregar la nueva persona"));
                    }
                    repo.UpdateResponsable(recibePersona);
                    return StatusCode(200, "Responsable actualizado correctamente");
                }
                catch (Exception ex)
                {
                    // Aquí puedes registrar la excepción o manejar el error adecuadamente
                    return StatusCode(500, "Ha ocurrido un error interno en el servidor");
                }
            }
            else
            {
                // Si hay errores en los campos enviados, devuelve un código de estado 400 Bad Request
                return StatusCode(400, "Verifique campos");
            }
        }

        [HttpGet("GetDetalleResidenteByCedula/{cedula}"), Authorize(Roles = "Administrador, Encargado, Empleado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Persona))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetDetalleResidenteByCedula(int cedula)
        {
            if (cedula > 0)
            {
                try
                {
                    if (repo.ExistePersona(cedula))
                    {
                        return StatusCode(200, repo.GetDetalleResidenteByCedula(cedula));
                    }
                    else
                    {
                        return StatusCode(406, "Persona no encontrada");
                    }

                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al buscar persona");

                }

            }
            return StatusCode(500, "Verifique campos");
        }

        [HttpPost("UpdateResidente"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateResidente([FromBody] Residente recibePersona)
        {
            if (recibePersona.validaciones() == "")
            {
                try
                {
                    try
                    {
                        List<Documentos> documentos = recibePersona.documentos;
                        if (documentos != null)
                        {
                            List<Documentos> documentosASubir = new List<Documentos>();
                            for (int i = 0; i < documentos.Count(); i++)
                            {
                                if (documentos[i].TipoArchivo.ToLower().Equals(".pdf")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpg")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpeg")
                                    && !documentos[i].NombreDocumento.IsNullOrEmpty()
                                    && !documentos[i].Descripcion.IsNullOrEmpty())
                                {
                                    documentos[i].CedulaPersona = recibePersona.CedulaPersona;
                                    documentos[i] = this.GuardarArchivo(documentos[i]);
                                    documentosASubir.Add(documentos[i]);
                                }
                            }
                            recibePersona.documentos = documentosASubir;
                        }
                    }
                    catch
                    {
                        return StatusCode(500, JsonConvert.SerializeObject("Ocurrio un error interno y no se pudo agregar la nueva persona"));
                    }
                    repo.UpdateResidente(recibePersona);
                    return StatusCode(200, "Residente actualizado correctamente");
                }
                catch (Exception ex)
                {
                    // Aquí puedes registrar la excepción o manejar el error adecuadamente
                    return StatusCode(500, "Ha ocurrido un error interno en el servidor");
                }
            }
            else
            {
                // Si hay errores en los campos enviados, devuelve un código de estado 400 Bad Request
                return StatusCode(400, "Verifique campos");
            }
        }

        [HttpGet("GetDetalleEmpleadoByCedula/{cedula}"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Persona))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetDetalleEmpleadoByCedula(int cedula)
        {
            if (cedula > 0)
            {
                try
                {
                    if (repo.ExistePersona(cedula))
                    {
                        return StatusCode(200, repo.GetDetalleEmpleadoByCedula(cedula));
                    }
                    else
                    {
                        return StatusCode(406, "Persona no encontrada");
                    }

                }
                catch (Exception)
                {
                    //500 Internal Server Error
                    return StatusCode(500, "Error al buscar persona");

                }

            }
            return StatusCode(500, "Verifique campos");
        }

        [HttpPost("UpdatePersonal"), Authorize(Roles = "Administrador, Encargado")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdatePersonal([FromBody] Personal recibePersona)
        {
            if (recibePersona.validaciones() == "")
            {
                try
                {
                    try
                    {
                        List<Documentos> documentos = recibePersona.documentos;
                        if (documentos != null)
                        {
                            List<Documentos> documentosASubir = new List<Documentos>();
                            for (int i = 0; i < documentos.Count(); i++)
                            {
                                if (documentos[i].TipoArchivo.ToLower().Equals(".pdf")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpg")
                                    || documentos[i].TipoArchivo.ToLower().Equals(".jpeg")
                                    && !documentos[i].NombreDocumento.IsNullOrEmpty()
                                    && !documentos[i].Descripcion.IsNullOrEmpty())
                                {
                                    documentos[i].CedulaPersona = recibePersona.CedulaPersona;
                                    documentos[i] = this.GuardarArchivo(documentos[i]);
                                    documentosASubir.Add(documentos[i]);
                                }
                            }
                            recibePersona.documentos = documentosASubir;
                        }
                    }
                    catch
                    {
                        return StatusCode(500, JsonConvert.SerializeObject("Ocurrio un error interno y no se pudo agregar la nueva persona"));
                    }
                    repo.UpdatePersonal(recibePersona);
                    return StatusCode(200, "Personal actualizado correctamente");
                }
                catch (Exception ex)
                {
                    // Aquí puedes registrar la excepción o manejar el error adecuadamente
                    return StatusCode(500, "Ha ocurrido un error interno en el servidor");
                }
            }
            else
            {
                // Si hay errores en los campos enviados, devuelve un código de estado 400 Bad Request
                return StatusCode(400, "Verifique campos");
            }
        }

    }



}





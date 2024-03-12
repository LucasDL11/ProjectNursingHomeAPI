using Dominio;
using Dominio.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataAccess.Repositorio;


namespace ResidencialAPI.Controllers
{
    [ApiController]
    public class ConfiguracionController : ControllerBase
    {

        RepositorioConfiguracion repo = new RepositorioConfiguracion(new Dominio.ResidencialContext());

        [HttpGet("GetConfiguracion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Parametros))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetParametros()
        {

            try
            {
                Parametros misParametros = repo.BuscarParametros();
                if (misParametros != null)
                {
                    return Ok(misParametros);
                }
                else
                {
                    return StatusCode(504, "Parametros no encontrados");
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Error al obtener parametros");
            }

        }
    }
}

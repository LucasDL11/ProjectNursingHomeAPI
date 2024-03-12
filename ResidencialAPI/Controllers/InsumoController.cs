using Dominio;
using Dominio.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataAccess.Repositorio;


namespace ResidencialAPI.Controllers
{
    [ApiController]
    public class InsumoController : ControllerBase
    {

        RepositorioInsumo repo = new RepositorioInsumo(new Dominio.ResidencialContext());

        [HttpGet("GetAllInsumos")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Agenda))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<Insumo> FindAll()
        {
            //RepositorioUsuario instanciaSesion = new RepositorioUsuario(new Connection());
            IEnumerable<Insumo> insumos = repo.FindAll();

            try
            {

                if (insumos != null)
                {
                    return insumos;
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

        [HttpGet("GetFindAllInsumoDelResidente")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Agenda))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<InsumoResidente> FindAllInsumoDelResidente()
        {
            //RepositorioUsuario instanciaSesion = new RepositorioUsuario(new Connection());
            IEnumerable<InsumoResidente> insumos = repo.FindAllInsumoDelResidente();

            try
            {

                if (insumos != null)
                {
                    return insumos;
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

        [HttpPost("AddTipoInsumo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddTipoInsumo(string recibeNombre)
        {
            try
            {
                if (recibeNombre != null && recibeNombre != "")
                {
                    if (repo.AddTipoInsumo(recibeNombre))
                    {
                        return Ok("Agregado correctamente");
                    }
                    else
                    {
                        return StatusCode(406, "Tipo Insumo ya existente");
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al agregar");
            }
            return null;
        }


        [HttpPost("AddInsumo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult AddInsumo(long codBarrasInsumo, string nombreInsumo, int cantidad, int idTipoInsumo)
        {
            Insumo miInsumo = new Insumo(codBarrasInsumo, nombreInsumo, cantidad, idTipoInsumo, true);
            if (miInsumo.Validar(null, codBarrasInsumo, nombreInsumo, cantidad, idTipoInsumo))
            {
                try
                {

                    if (repo.Add(miInsumo))
                    {
                        return StatusCode(200, "Insumo creado satisfactoriamente");
                    }
                    else
                    {
                        return StatusCode(500, "Error al crear Insumo, verifique datos");
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error al crear Insumo");
                }
            }
            else
            {
                return StatusCode(500, "Verifique datos");
            }
        }

        [HttpPost("EliminarInsumo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tarea))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult EliminarInsumo(Int64 codBarraInsumo)
        {
            try
            {
                if (repo.Remove(codBarraInsumo))
                {
                    return StatusCode(200, "Insumo borrado satisfactoriamente");
                }
                else
                {
                    return StatusCode(500, "Error al borrar Insumo, verifique datos");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al borrar Insumo");
            }
        }

        [HttpPost("UpdateInsumo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Tarea))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateInsumo(int idInsumo, long codBarrasInsumo, string nombreInsumo, int cantidad, int idTipoInsumo)
        {
            Insumo miInsumo = new Insumo(idInsumo, codBarrasInsumo, nombreInsumo, cantidad, idTipoInsumo, true);
            if (miInsumo.Validar(idInsumo, codBarrasInsumo, nombreInsumo, cantidad, idTipoInsumo))
            {
                try
                {

                    if (repo.Update(miInsumo))
                    {
                        return StatusCode(200, "Insumo actualizado satisfactoriamente");
                    }
                    else
                    {
                        return StatusCode(500, "Error al actualizar Insumo, verifique datos");
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error al actualizar Insumo");
                }
            }
            else
            {
                return StatusCode(500, "Verifique datos");
            }
        }
    }
}


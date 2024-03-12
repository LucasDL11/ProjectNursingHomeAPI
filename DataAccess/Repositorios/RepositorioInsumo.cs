using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using Dominio.Repositorio;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataAccess.Repositorio
{
    public class RepositorioInsumo : IRepositorioInsumo
    {

        private ResidencialContext _conn;

        public RepositorioInsumo(ResidencialContext residencialContext)
        {

            _conn = residencialContext;

        }

        public bool Add(Insumo obj)
        {
            try
            {
                _conn.Insumo.Add(obj);
                _conn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Insumo> FindAll()
        {
            IEnumerable<Insumo> Insumos;
            try
            {
                Insumos = _conn.Insumo.ToList();
                IEnumerable<TipoInsumo> TipoInsumos = _conn.TipoInsumo.ToList();
                foreach (Insumo insumo in Insumos)
                {
                    foreach (TipoInsumo tipoInsumo in TipoInsumos)
                    {
                        if (insumo.IdTipoInsumo == tipoInsumo.IdTipoInsumo)
                        {
                            insumo.Tipo = tipoInsumo;
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return Insumos;
        }

        public IEnumerable<InsumoResidente> FindAllInsumoDelResidente()
        {
            IEnumerable<InsumoResidente> Insumos;
            try
            {
                Insumos = _conn.InsumoResidente.ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return Insumos;
        }

        public IEnumerable<Insumo> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Int64 codBarra)
        {
            if (ExisteInsumoPorCodBarra(codBarra)) //y no está borrada estado == 0)
            {
                try
                {
                    _conn.Database.ExecuteSqlRaw(
                        "update Insumo set activo = 0 where " +
                        "codBarras = @codBarras",
                        new SqlParameter("@codBarras", codBarra)
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

        public bool Update(Insumo obj)
        {
            try
            {
                _conn.Insumo.Update(obj);
                _conn.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddInsumo(int codBarras, string nombre, int cantidad, string nombreTipoConsumo)
        {
            if (!ExisteTipoInsumo(nombreTipoConsumo))
            {
                if (!ExisteInsumoPorCodBarra(codBarras))
                {
                    int idTipoConsumo = GetIdTipoConsumo(nombreTipoConsumo);
                    try
                    {
                        _conn.Database.ExecuteSqlRaw(
                            "insert into Insumo (codBarras, nombre, cantidad, idTipoConsumo, activo) values (@codBarras, @nombre, @cantidad, @idTipoConsumo,1)",
                            new SqlParameter("@codBarras", codBarras),
                            new SqlParameter("@nombre", nombre),
                            new SqlParameter("@cantidad", cantidad),
                            new SqlParameter("@idTipoConsumo", idTipoConsumo)
                            );

                        return true;

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return false;
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

        public bool ExisteInsumoPorCodBarra(Int64 recibeCodBarra)
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

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}

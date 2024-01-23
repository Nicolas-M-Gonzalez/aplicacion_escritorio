using DominioArticulos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioCatalogo
{
    public class ListaMarca
    {


        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();
            Accesorapido datos = new Accesorapido();

            try
            {
                datos.setearconsulta(" Select Id, Descripcion from Marcas");
                datos.ejecutarlectura();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }

                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.Cerrarconexion();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominioArticulos
{
     public class  Categoria
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public override string ToString()// le aclaro que traiga la tabla descripcion y no por defecto.
        {
            return Descripcion;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominioArticulos
{
    public class Articulo
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Precio { get; set; }
        
        public string ImagenUrl { get; set; }

        public Marca Modelo { get; set; }

        public Categoria Tipo { get; set; }
      


      
       
    }
}

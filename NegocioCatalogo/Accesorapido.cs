using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DominioArticulos;


namespace NegocioCatalogo
{
     public class Accesorapido
    {
        private SqlConnection conexion; //creo las variables para manipular la base de datos.

        private SqlCommand comando;

        private SqlDataReader lector;

        public SqlDataReader Lector //se crea la propiedad publica para poder leer el lector.
        { 
          get { return lector; }
        
        }

        

        public Accesorapido ()//armmo el constructor para que cuando entre ala clase se conecte directamente.
        {
            conexion = new SqlConnection("server = DESKTOP-497OCOR\\SQLEXPRESS01; Database = CATALOGO_DB ; integrated security= true");
            comando = new SqlCommand();
        }

        public void setearconsulta(string consulta)
        //le paso el tipo y la consulta por parametro en un metodo.
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarlectura ( )
            //este metodo realiza la lectura y lo guarda en el lector.
        {
            comando.Connection = conexion;

            try
            { 
                conexion.Open();
                lector = comando.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
           
        }

        public void Ejecutaraccion()//para insertar un dato.
        {
            comando.Connection = conexion;
            //llamo ala conexion.
            try
            {

                conexion.Open();
                //abro la conexion.
                comando.ExecuteNonQuery();
                 //cuando se inserta un dato se guarda con esta sentencia.  
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void setearparametro (string Nombre,object Valor)
            //recibe un string y un object
        {
            comando.Parameters.AddWithValue(Nombre, Valor);
            //maneja el parametro.
        }

        public void Cerrarconexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();

        }

       

    }
}

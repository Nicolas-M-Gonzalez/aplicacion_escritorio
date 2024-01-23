using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DominioArticulos;
using System.Data.SqlClient; //libreria para conectarse ala base de datos.
using System.Net.Http;

namespace NegocioCatalogo
{
    public class Accesodatos
    {


      public List<Articulo> RutaAcceso()  //se crea el metodo.
        {


            try
            {

                List<Articulo> Lista = new List<Articulo>();  //se crea la lista.

                SqlConnection Conexion = new SqlConnection();  //se crea el objeto para conectar.

                SqlCommand Comando = new SqlCommand(); //se crea el objeto para realizar acciones.

                SqlDataReader Lector;  // variable donde los datos donde se van alojar.  




                Conexion.ConnectionString = " server = DESKTOP-497OCOR\\SQLEXPRESS01; Database = CATALOGO_DB ; integrated security= true ";
                //base de datos donde se va a conectar.

                Comando.CommandType = System.Data.CommandType.Text;
                //la manera que voy a mandar la consulta = tipo texto.

                Comando.CommandText = "select  nombre,Codigo, precio,A. descripcion, ImagenUrl, M.Descripcion Modelo, C.Descripcion, A.IdMarca , A.IdCategoria, A.Id from ARTICULOS A , CATEGORIAS C , MARCAS M where C.Id = A.IdCategoria AND M.Id = A.IdMarca ";
                //le escribo la consulta tipo texto.

                Comando.Connection = Conexion;
                //esa consulta que la haga en ese motor de base de datos.

                Conexion.Open(); //abro la conexion.

                Lector = Comando.ExecuteReader();
                // realiza la lectura y lo guarda en la variable Lector.

                while (Lector.Read()) //para leer el lector.
                {

                    Articulo Aux = new Articulo();//creo el objeto.

                    Aux.Id = (int)Lector["Id"];
                    Aux.Codigo = (string)Lector["codigo"];
                    Aux.Nombre = (string)Lector["nombre"];

                    if (!(Lector["Precio"] is DBNull))
                        Aux.Precio = (decimal)Lector["precio"];

                    Aux.Descripcion = (string)Lector["descripcion"]; //cargo los datos.

                    if (!(Lector["ImagenUrl"] is DBNull))//para que me lo traiga si esta nulo en la base de datos.
                        Aux.ImagenUrl = (string)Lector["ImagenUrl"];
                  

                    Aux.Tipo= new Categoria();//le aviso que tipo es un objeto de tipo categoria.
                    Aux.Tipo.Id = (int)Lector["IdCategoria"];
                    Aux.Tipo.Descripcion = (string)Lector["Descripcion"];//le aclaro que traiga la tabla Descripcion.
                    Aux.Modelo = new Marca();//le aviso que modelo es un objeto de tipo marca.
                    Aux.Modelo.Id = (int)Lector["IdMarca"];
                    Aux.Modelo.Descripcion = (string)Lector["Modelo"];//le aclaro que traiga la tabla Modelo.

                    Lista.Add(Aux); //lo guardo en el objeto.


                }




                Conexion.Close(); //cierro la conexion.
                return Lista;
                //cuando el while termine de leer la lista la va a retornar

            }


            catch (Exception ex)
            {


                    throw ex;
                
            }
         
        }
           

      public void Agregar ( Articulo Nuevo)
      {
           
             Accesorapido datos = new Accesorapido();
                //creo el objeto para conectarme ala base de datos.

            try
            {
               
                datos.setearconsulta(" Insert Into ARTICULOS ( Codigo, Nombre, Precio, Descripcion, IdMarca , IdCategoria, ImagenUrl ) Values ('"+ Nuevo.Codigo +"' , '"+Nuevo.Nombre + "' , '"+Nuevo.Precio+"', '"+ Nuevo.Descripcion + "', @IdMarca , @IdCategoria , @ImagenUrl )");
                //para insertar datos esta es la manera de hacer la consulta cuando es un texto lleva ""
                datos.setearparametro("@IdMarca", Nuevo.Modelo.Id);
                datos.setearparametro("@idcategoria", Nuevo.Tipo.Id);
                datos.setearparametro("@ImagenUrl", Nuevo.ImagenUrl);
                datos.Ejecutaraccion();
                //llamo ala  funcion para que lo agregue y guarde.

                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.Cerrarconexion();//cierro conexion.

            }
      }
    

      public void Modificar( Articulo Articulo)
        {
            Accesorapido datos = new Accesorapido();

            try
            {


                datos.setearconsulta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria , ImagenUrl = @ImagenUrl, Precio = @Precio where Id = @Id");
                datos.setearparametro("@Codigo", Articulo.Codigo);
                datos.setearparametro("@Nombre", Articulo.Nombre);
                datos.setearparametro("@Descripcion", Articulo.Descripcion);
                datos.setearparametro("@IdMarca", Articulo.Modelo.Id);
                datos.setearparametro("@IdCategoria", Articulo.Tipo.Id);
                datos.setearparametro("@ImagenUrl", Articulo.ImagenUrl);
                datos.setearparametro("@Precio", Articulo.Precio);
                datos.setearparametro("@Id", Articulo.Id);

                datos.Ejecutaraccion();

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
    
      public void Eliminar (int Id) //eliminar registro a traves del id.
      {
            try
            {

              Accesorapido datos = new Accesorapido();//creo el objeto para conectarme.
              datos.setearconsulta("Delete from Articulos Where Id = @Id");//le paso la consulta.
              datos.setearparametro("@id", Id);//le paso el parametro del id.
              datos.Ejecutaraccion();//ejecuto la accion.

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
      }


        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> Lista = new List<Articulo>();
            Accesorapido datos = new Accesorapido();
            try
            {
                string consulta = "select  nombre, Codigo, precio,A. descripcion, ImagenUrl, M.Descripcion Modelo, C.Descripcion, A.IdMarca , A.IdCategoria, A.Id from ARTICULOS A , CATEGORIAS C , MARCAS M where C.Id = A.IdCategoria AND M.Id = A.IdMarca  AND ";


                if (campo == "Codigo")
                {

                    switch (criterio) // y el criterio es mayor a..
                    {

                        case "Comienza con ":
                            consulta += " Codigo like '" + filtro + "%' "; //precio mayor al numero que colocaste.

                            break;

                        case "Termina con ":

                            consulta += " Codigo like '%" + filtro + "' ";
                            break;

                        default:
                            consulta += " Codigo like  '%" + filtro + "%' ";
                            break;
                    }


                }
                else if (campo == "Tipo")
                {

                    switch (criterio) // y el criterio es mayor a..
                    {

                        case "Comienza con ":
                            consulta += " M.descripcion like '" + filtro + "%' "; //precio mayor al numero que colocaste.

                            break;

                        case "Termina con ":

                            consulta += " C.descripcion like '%" + filtro + "' ";
                            break;

                        default:
                            consulta += " C.descripcion like  '%" + filtro + "%' ";
                            break;

                    }

                        
                }
                else
                {


                    switch (criterio) // y el criterio es mayor a..
                    {

                        case "Comienza con ":
                            consulta += " nombre like '" + filtro + "%' "; //precio mayor al numero que colocaste.
                           break;

                        case "Termina con ":

                            consulta += " nombre like '%" + filtro + "'";
                            break;

                        default:
                            consulta += " nombre like  '%" + filtro + "%' ";
                            break;

                    }


                }



                datos.setearconsulta(consulta);
                datos.ejecutarlectura();

                while (datos.Lector.Read()) //para leer el lector.
                {

                    Articulo Aux = new Articulo();//creo el objeto.

                    Aux.Id = (int)datos.Lector["Id"];
                    Aux.Codigo = (string)datos.Lector["codigo"];
                    Aux.Nombre = (string)datos.Lector["nombre"];

                    if (!(datos.Lector["Precio"] is DBNull))
                        Aux.Precio = (decimal)datos.Lector["Precio"];

                    Aux.Descripcion = (string)datos.Lector["descripcion"]; //cargo los datos.

                    if (!(datos.Lector["ImagenUrl"] is DBNull))//para que me lo traiga si esta nulo en la base de datos.
                        Aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];


                    Aux.Tipo = new Categoria();//le aviso que tipo es un objeto de tipo categoria.
                    Aux.Tipo.Id = (int)datos.Lector["IdCategoria"];
                    Aux.Tipo.Descripcion = (string)datos.Lector["Descripcion"];//le aclaro que traiga la tabla Descripcion.
                    Aux.Modelo = new Marca();//le aviso que modelo es un objeto de tipo marca.
                    Aux.Modelo.Id = (int)datos.Lector["IdMarca"];
                    Aux.Modelo.Descripcion = (string)datos.Lector["Modelo"];//le aclaro que traiga la tabla Modelo.

                    Lista.Add(Aux); //lo guardo en el objeto.


                }


                return Lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

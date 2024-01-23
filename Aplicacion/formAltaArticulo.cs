using DominioArticulos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NegocioCatalogo;
using System.IO;
using System.Configuration;

namespace Aplicacion
{
    public partial class formAltaArticulo : Form
    {
        private Articulo Articulo = null;
        //creo el atributo en nulo para que cuando quieras agregar un articulo este vacio.
        private OpenFileDialog Archivo = null;
        //creo el atributo en nulo para agregar si es necesario.

        public formAltaArticulo()
        {
            InitializeComponent();
            // este constructor va a utilizar el atributo nulo para agregar.
        }

        public formAltaArticulo( Articulo Articulo)
        //copio el constructor y le mando por parametro el articulo para  cuando se cree la instancia
        {
            InitializeComponent();
            this.Articulo = Articulo;
            Text = " Modificar Articulo ";//cambia el titulo.
            //le aclaro que este constructor tiene que usar el Articulo que viene por parametro.
            // el atributo nulo lo cambio por el que viene por parametro.
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
            //cierra la ventana.
        }

        private bool validartexto()
        {
            if (txtCodigo.Text == "" )
            {
                MessageBox.Show(" El campo codigo no puede estar vacio..");
                return true;

            }
            if (txtNombre.Text == "")
            {
                MessageBox.Show(" El campo nombre no puede estar vacio ");
                return true;
            }
            
            if (!(solonumero(txtPrecio.Text)))
            {
                MessageBox.Show(" este campo acepta solo numeros numeros ");
                return true;
            }

            return false;

        }

        private bool solonumero ( string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }

            return true;
        }
        
        
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Articulo nuevo = new Articulo(); una ves que creo el boton modificar no necesito el objeto.
            //creo el objeto para cargar.
            Accesodatos negocio = new Accesodatos();
            // creo el objeto negocio para poder mandarlo ala base de datos.
            try
            {

                if (validartexto())
                    return;

                


                if (Articulo == null) //si el atributo esta nulo creo el objeto quiere decir que quiero agregar un articulo.
                    Articulo = new Articulo();

                Articulo.Codigo= txtCodigo.Text;
                Articulo.Nombre = txtNombre.Text;
                Articulo.Precio = decimal.Parse(txtPrecio.Text);
                //sigo este format0 para convertirlo en un decimal.
                Articulo.Descripcion = txtDescripcion.Text;
                Articulo.Modelo =(Marca)cmbMarca.SelectedItem;
                Articulo.Tipo = (Categoria)cmbCategoria.SelectedItem;
                Articulo.ImagenUrl = txtImagen.Text;
                //le cargo los datos.


                if (Articulo.Id != 0)//si quiero modificar quiere decir que el articulo ya tiene un id.
                {
                    negocio.Modificar(Articulo);
                    MessageBox.Show(" Modificado Exitosamente ");
                }
                else
                {
                    negocio.Agregar(Articulo);
                    // lo agrego con la funcion agregar de acceso a datos.
                    MessageBox.Show(" Agregado Exitosamente ");
                    //le tiro el msj en pantalla.


                }

                if (Archivo != null && !(txtImagen.Text.ToUpper().Contains("HTTP"))) 
                File.Copy(Archivo.FileName, ConfigurationManager.AppSettings["Imagen"] + Archivo.SafeFileName);
                // guarde el archivo en la configuracion que pusiste por defecto en el framework.

                Close();//cierro ventana.
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            //si se pincha que le tire un msj de error.
            }
        }

        private void formAltaArticulo_Load(object sender, EventArgs e)
        {
            //evento cuando carga el formulario.
            ListaCategoria listaCategoria = new ListaCategoria();

            ListaMarca listamarca = new ListaMarca();

              try
              {

                cmbCategoria.DataSource = listaCategoria.Listar();
                cmbCategoria.ValueMember = "Id"; //clave del combobox
                cmbCategoria.DisplayMember = "Descripcion"; // lo que tiene que mostrar.

                cmbMarca.DataSource = listamarca.Listar();//carga los desplegables.
                cmbMarca.ValueMember = "Id";
                cmbMarca.DisplayMember = "Descripcion";


                if(Articulo != null) 
                //si el articulo es distinto de nulo quiere decir que quiero modificar.
                // por ende le tengo que cargar los datos.
                {
                    txtCodigo.Text = Articulo.Codigo;//muestre todos los datos pre-cargados.
                    txtNombre.Text = Articulo.Nombre;
                    txtDescripcion.Text = Articulo.Descripcion;
                    txtPrecio.Text = Articulo.Precio.ToString();
                    txtImagen.Text = Articulo.ImagenUrl;
                    cargarimagen(Articulo.ImagenUrl);//que me cargue la imagen directamente.
                    cmbCategoria.SelectedValue = Articulo.Tipo.Id;
                    cmbMarca.SelectedValue = Articulo.Modelo.Id;


                }



              }
               catch (Exception ex)
               {

                 MessageBox.Show(ex.ToString());
               }
            
        }

       

       private void txtImagen_Leave(object sender, EventArgs e)
       {
            
            cargarimagen(txtImagen.Text);
            //le cargo la imagen y le digo que es un texto.
            
       }       

           private void cargarimagen(string imagenUrl)
            //me copio la funcion en este formulario para poder agregarlo al evento leave.
            {
                try
                {
                    PtbImagen.Load(imagenUrl);//cargo la imagen.


                }
                catch (Exception ex)
                {

                    PtbImagen.Load("https://media.istockphoto.com/id/1409329028/vector/no-picture-available-placeholder-thumbnail-icon-illustration-design.jpg?s=612x612&w=0&k=20&c=_zOuJu755g2eEUioiOUdz_mHKJQJn-tDgIAhQzyeKUQ=");
                    //si no encuentra la imagen que tire la excepcion por defecto.
                }
            }

        private void btnImagenLocal_Click(object sender, EventArgs e)
        {
            Archivo = new OpenFileDialog();
            //te permite abrir una ventana de dialogo.
            Archivo.Filter = "jpg|*.jpg;|png|*.png"; //le digo que archivo quiero que me muestre.
           if ( Archivo.ShowDialog() == DialogResult.OK)
            {

                txtImagen.Text = Archivo.FileName; //te guarda la ruta del archivo.
                cargarimagen(Archivo.FileName);

               // File.Copy(Archivo.FileName, ConfigurationManager.AppSettings["ImagenLocalCatalogo"] + Archivo.SafeFileName );
                // guarde el archivo en la configuracion que pusiste por defecto en el framework.
            }
        }

        private void txtImagen_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

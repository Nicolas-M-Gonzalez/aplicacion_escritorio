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
using DominioArticulos;

namespace Aplicacion
{
    public partial class Form1 : Form
    {
        private List<Articulo> Listaarticulo;//creo el atributo para manipular la lista.    

        public Form1()
        {

            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Actualizar();
            cmbCampo.Items.Add("Codigo"); //le digo que cargue la grilla con estos datos.
            cmbCampo.Items.Add("Tipo");
            cmbCampo.Items.Add("Nombre");
        }

        private void Actualizar()
        {
            try
            {
                Accesodatos Datos = new Accesodatos();//creo el objeto datos de la clase negociocatalogo.
                Listaarticulo = Datos.RutaAcceso();
                dgwarticulo.DataSource = Listaarticulo;//le cargo los datos al dgwarticulo.
                OcultarColumnas();
                cargarimagen(Listaarticulo[0].ImagenUrl);
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void OcultarColumnas()
        {
            dgwarticulo.Columns["Id"].Visible = false;
            dgwarticulo.Columns["ImagenUrl"].Visible = false;

        }


        private void dgwarticulo_SelectionChanged(object sender, EventArgs e)
        {

            if (dgwarticulo.CurrentRow != null) //le pregunto si tiene un articulo seleccionado.
            {
                Articulo Articuloseleccionado = (Articulo)dgwarticulo.CurrentRow.DataBoundItem;
                //le digo de forma explicita que el objeto de la grilla del objeto seleccionado
                //es un articulo.
                cargarimagen(Articuloseleccionado.ImagenUrl);
                //la cargo en el metodo cargarimagen para manejar la excepcion.
            }

        }

        private void cargarimagen(string imagenurl)
        {
            try
            {
                pcbArticulo.Load(imagenurl);//cargo la imagen.


            }
            catch (Exception ex)
            {

                pcbArticulo.Load("https://media.istockphoto.com/id/1409329028/vector/no-picture-available-placeholder-thumbnail-icon-illustration-design.jpg?s=612x612&w=0&k=20&c=_zOuJu755g2eEUioiOUdz_mHKJQJn-tDgIAhQzyeKUQ=");
                //si no encuentra la imagen que tire la excepcion por defecto.
            }
        }



        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            formAltaArticulo Alta = new formAltaArticulo();//en este caso constructor vacio.
            //creas el objeto.
            Alta.ShowDialog();
            //no te permite salir de la ventana salvo que la cierres.
            Actualizar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            Articulo seleccionado;// llamo al articulo seleccionado con la variable.
            seleccionado = (Articulo)dgwarticulo.CurrentRow.DataBoundItem;
            //le digo del articulo seleccionado en el momento.


            formAltaArticulo Modificar = new formAltaArticulo(seleccionado);//en este caso constructor cargado con articulo.
            //creas el objeto.
            // cuando apretas el boton modificar te lo va a cargar con el pokemon seleccionado.
            Modificar.ShowDialog();
            //no te permite salir de la ventana salvo que la cierres.

            Actualizar();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {

            Accesodatos datos = new Accesodatos();//creo el objeto que tiene la funcion eliminar.

            Articulo seleccionado; //creo la variable

            try
            {
                DialogResult Respuesta = MessageBox.Show("¿Estas seguro de eliminar el registro..?", "ELIMINANDO", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                //le agrego titulo, un boton ,y un icono al cartel.
                if (Respuesta == DialogResult.Yes)
                {

                    seleccionado = (Articulo)dgwarticulo.CurrentRow.DataBoundItem;
                    //del articulo de la grilla.
                    datos.Eliminar(seleccionado.Id);
                    //elimino con la funcion y le paso la variable como parametro.
                    Actualizar();


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



        }

        private bool validarfiltro()
        {
            if (cmbCampo.SelectedIndex < 0) //si el campo es menor a 0 quiere decir que no hay nada
                                            // seleccionado.
            {
                MessageBox.Show(" Por favor seleccione el campo..");
                return true;
            }
            if (cmbCriterio.SelectedIndex < 0)
            {
                MessageBox.Show(" Por favor seleccione el criterio..");
                return true;

            }

            return false;
        }

    private void btnBuscar_Click(object sender, EventArgs e)
    {

        Accesodatos datos = new Accesodatos();


        try
        {
            if (validarfiltro())
            return ; //pones un return para que finalice la funcion validar filtro.
            string campo = cmbCampo.SelectedItem.ToString();
            string criterio = cmbCriterio.SelectedItem.ToString();
            string filtro = txtFiltro.Text;
            //capturo los campos seleccionados cuando apreto el boton buscar.
            dgwarticulo.DataSource = datos.filtrar(campo, criterio, filtro);
        }
        catch (Exception ex)
        {

            MessageBox.Show(ex.ToString());
        }


    }



    private void txbBuscar_TextChanged(object sender, EventArgs e)
    {
        List<Articulo> listafiltrada;



        if (txbBuscar.Text != "")
        //si el filtro esta distinto de  vacio.
        // "" significa vacio.
        {
            listafiltrada = Listaarticulo.FindAll(x => x.Nombre.ToUpper().Contains(txbBuscar.Text.ToUpper()));
            //evalua los nombres y los compara guardando los datos.

        }
        else
        {

            listafiltrada = Listaarticulo;
        }

        dgwarticulo.DataSource = null;
        //limpiamos el dgw.
        dgwarticulo.DataSource = listafiltrada;
        // le agrego el la variable.
        OcultarColumnas();
    }

    private void cbmCampo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string opcion = cmbCampo.SelectedItem.ToString();
        //esta variable me guarda la opcion seleccionada


        cmbCriterio.Items.Clear();
        cmbCriterio.Items.Add(" Comienza con: ");
        cmbCriterio.Items.Add(" Termina con: ");
        cmbCriterio.Items.Add(" Contiene: ");




    }
   }
       
}    



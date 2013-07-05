using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoDatos
{
    /// <summary>
    /// Lógica de interacción para V04_Desconectado1.xaml
    /// </summary>
    public partial class V04_Desconectado1 : Window
    {
        NWDataSet.ProductsDataTable ProductosFiltrados = null;

        public V04_Desconectado1()
        {
            InitializeComponent();

            //Enlazo el combo a la tabla del DataSet
            comboBox1.DisplayMemberPath = "CategoryName";
            comboBox1.SelectedValuePath = "CategoryID";
            comboBox2.DisplayMemberPath = "CategoryName";
            comboBox2.SelectedValuePath = "CategoryID";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Creamos Dataset y cargamos tabla...

            NWDataSet ds = new NWDataSet();
            NWDataSetTableAdapters.CategoriesTableAdapter taCat;
            taCat = new NWDataSetTableAdapters.CategoriesTableAdapter();
            //Cargamos la tabla
            taCat.LlenarCategorias(ds.Categories);
            //Asignamos la tabla al comboBox
            comboBox1.ItemsSource = ds.Categories;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Para acceder al valor seleccionado hacemos un cash del dato selecionado
            System.Data.DataRowView drv = (System.Data.DataRowView)comboBox1.SelectedItem;
            string nombre;
            //System.Data.DataRow fila = drv.Row;
            //nombre = fila["CategoryName"].ToString();
            //nombre = fila[1].ToString();
            //De esta forma con el cash tengo los nombres de los campos como propiedades de la fila
            NWDataSet.CategoriesRow fila = (NWDataSet.CategoriesRow)drv.Row;
            nombre = fila.CategoryName;

            NWDataSetTableAdapters.CategoriesTableAdapter taCat;
            taCat = new NWDataSetTableAdapters.CategoriesTableAdapter();

            string nombre2 = taCat.SelNombreCategory(fila.CategoryID);

            MessageBox.Show(nombre + "\n" + fila.Description + "\nNombre desde BBDD: " + nombre2,
                "Categoría: " + comboBox1.SelectedValue);

            //Rellenar los datos del DataGrid
            NWDataSetTableAdapters.ProductsTableAdapter taProd;
            taProd = new NWDataSetTableAdapters.ProductsTableAdapter();

            if (ProductosFiltrados == null)
                ProductosFiltrados = taProd.ObtenerProductosPorCategoryID(fila.CategoryID);
            else
            {
                taProd.ClearBeforeFill = !checkBox1.IsChecked.Value;
                taProd.LlenarProductosPorCategoryID(ProductosFiltrados, fila.CategoryID);
            }

            dataGrid1.ItemsSource = ProductosFiltrados;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //Cargamos tabla SIN DataSet...

            NWDataSet ds = new NWDataSet();
            NWDataSetTableAdapters.CategoriesTableAdapter taCat;
            taCat = new NWDataSetTableAdapters.CategoriesTableAdapter();

            //Asignamos la tabla al comboBox
            comboBox2.ItemsSource = taCat.ObtenerCategorias();
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Para acceder al valor seleccionado hacemos un cash del dato selecionado
            System.Data.DataRowView drv = (System.Data.DataRowView)comboBox2.SelectedItem;
            string nombre;
            //System.Data.DataRow fila = drv.Row;
            //nombre = fila["CategoryName"].ToString();
            //nombre = fila[1].ToString();
            //De esta forma con el cash tengo los nombres de los campos como propiedades de la fila
            NWDataSet.CategoriesRow fila = (NWDataSet.CategoriesRow)drv.Row;
            nombre = fila.CategoryName;
            MessageBox.Show(nombre + "\n" + fila.Description,
                "Categoría: " + comboBox2.SelectedValue);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            NWDataSetTableAdapters.ProductsTableAdapter taProd;
            taProd = new NWDataSetTableAdapters.ProductsTableAdapter();

            dataGrid1.ItemsSource = taProd.ObtenerProductos();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            NWDataSet.ProductsDataTable tabla;
            tabla = dataGrid1.ItemsSource as NWDataSet.ProductsDataTable;
            if (tabla == null)
            {
                MessageBox.Show("Muestre los productos para actualizarlos...");
            }
            else
            {
                NWDataSetTableAdapters.ProductsTableAdapter taProd;
                taProd = new NWDataSetTableAdapters.ProductsTableAdapter();
                try
                {
                    taProd.Update(tabla);
                    //Otra manera pero con cash
                    //taProd.Update((NWDataSet.ProductsDataTable)dataGrid1.ItemsSource);
                    MessageBox.Show("BBDD Actualizada.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Operaciónn abortada:\n" + ex.Message);
                }
            }
        }

        //Actualizar con Transacción
        //Añadimos la referencia a la librería está en .NET (System.Transactions)
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            NWDataSet.ProductsDataTable tabla;
            tabla = dataGrid1.ItemsSource as NWDataSet.ProductsDataTable;
            if (tabla == null)
            {
                MessageBox.Show("Muestre los productos para actualizarlos...");
            }
            else
            {
                NWDataSetTableAdapters.ProductsTableAdapter taProd;
                taProd = new NWDataSetTableAdapters.ProductsTableAdapter();

                //Transacción

                //otro metodo
                //System.Data.SqlClient.SqlConnection cnn;
                //using (cnn = 
                //    new System.Data.SqlClient.SqlConnection;
                //using (System.Data.SqlClient.SqlConnection cnn = 
                //    new System.Data.SqlClient.SqlConnection;

                using (System.Transactions.TransactionScope tr =
                    new System.Transactions.TransactionScope())
                {
                    //Dentro hay que llamar al método .Complete()
                    //Sino deshace todo lo que le hayamos dicho

                    try
                    {
                        taProd.Update(tabla);
                        tr.Complete();
                        MessageBox.Show("BBDD Actualizada.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Operaciónn abortada:\n" + ex.Message);
                    }
                }
            }

        }

        //Actualizar sin parada
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            NWDataSet.ProductsDataTable tabla;
            tabla = dataGrid1.ItemsSource as NWDataSet.ProductsDataTable;
            if (tabla == null)
            {
                MessageBox.Show("Muestre los productos para actualizarlos...");
            }
            else
            {
                NWDataSetTableAdapters.ProductsTableAdapter taProd;
                taProd = new NWDataSetTableAdapters.ProductsTableAdapter();
                //Decide si cuando ocurre un error para o continúa con el siguiente
                //Con Insert, Delete o Update
                taProd.Adapter.ContinueUpdateOnError = true;

                try
                {
                    taProd.Update(tabla);
                    //Si no tiene errores
                    if (!tabla.HasErrors)
                        MessageBox.Show("BBDD Actualizada.");
                    else
                    {
                        string cad = "";
                        foreach (NWDataSet.ProductsRow prod in tabla.GetErrors())
                        {
                            //Error al acceder a registros borrados no puedo saber su contenido
                            //cad += prod.ProductID + " - " + prod.ProductName +
                            cad +=
                                (prod.RowState != System.Data.DataRowState.Deleted ?
                                    prod.ProductID + " - " + prod.ProductName :
                                    "Registro borrado.")
                                + "\nEstado: " + prod.RowState +
                                "\n" + prod.RowError + "\n\n";
                        }

                        MessageBoxResult res;
                        res = MessageBox.Show(cad + "\n¿Quieres deshacer los cambios pendientes?",
                            "BBDD Actualizada CON errores.",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Error,
                            MessageBoxResult.No);
                        if (res == MessageBoxResult.Yes)
                            //Deshacer los cambios 
                            tabla.RejectChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Operaciónn abortada:\n" + ex.Message);
                }
            }
        }

        //Filtro (Vista)
        System.Data.DataView vistaProductos = null;

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            //Instancio la vista si no lo está
            if (dataGrid1.ItemsSource != null)
            {
                if (vistaProductos == null)
                {
                    vistaProductos = new System.Data.DataView();
                    vistaProductos.Table = (NWDataSet.ProductsDataTable)dataGrid1.ItemsSource;
                }

                int IdCategoria;

                if (int.TryParse(textBox1.Text, out IdCategoria))
                    vistaProductos.RowFilter = "CategoryID = " + IdCategoria;
                else
                {
                    vistaProductos.RowFilter = "";
                    textBox1.Text = "";
                }

                //Asigno la vista al datagrid
                dataGrid1.ItemsSource = vistaProductos;
            }
        }
    }
}

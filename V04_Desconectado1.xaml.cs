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
        NWDataSet.ProductsDataTable ProductosFiltrados = null ;

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
                taProd.Update(tabla);
                //Otra manera pero con cash
                //taProd.Update((NWDataSet.ProductsDataTable)dataGrid1.ItemsSource);
                MessageBox.Show("BBDD Actualizada.");
            }
        }
    }
}

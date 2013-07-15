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
    /// Lógica de interacción para V16_Entity05.xaml
    /// </summary>
    public partial class V16_Entity05 : Window
    {
        public V16_Entity05()
        {
            InitializeComponent();
        }

        private System.Data.Objects.ObjectQuery<Producto> GetProductosQuery(northwindEntities northwindEntities)
        {
            // Código generado automáticamente

            System.Data.Objects.ObjectQuery<ProyectoDatos.Producto> productosQuery = northwindEntities.Productos;
            // Para cargar datos explícitamente, puede ser necesario agregar métodos Include similares al siguiente:
            // productosQuery = productosQuery.Include("Productos.Categoria").
            // Para obtener más información, vea http://go.microsoft.com/fwlink/?LinkId=157380
            // Devuelve un elemento ObjectQuery.
            return productosQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ProyectoDatos.northwindEntities northwindEntities = new ProyectoDatos.northwindEntities();
            // Cargar datos en Productos. Puede modificar este código según sea necesario.
            System.Windows.Data.CollectionViewSource productosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("productosViewSource")));
            System.Data.Objects.ObjectQuery<ProyectoDatos.Producto> productosQuery = this.GetProductosQuery(northwindEntities);
            productosViewSource.Source = productosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly).Where(p => p.CategoryID == int.Parse(textBox1.Text));

        }

        private void productosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Producto  p = productosDataGrid.SelectedItem as Producto;

            textBox2.Text = p.ProductName;
            textBox3.Text = p.UnitPrice.ToString();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //Para tener el ID del producto
            Producto  pr = productosDataGrid.SelectedItem as Producto;

            //Creo de nuevo el objeto vacio
            ProyectoDatos.northwindEntities northwindEntities = new ProyectoDatos.northwindEntities();

            object opr;
            //Busco el objeto por el EntityKey, si no lo encuentra en northwindEntities
            //que en este caso está vacío lo va a buscar a la base de datos, si no lo encontrase
            //daría la excepción en este caso con TryGet..... no da excepción lo pone a null
            //northwindEntities.TryGetObjectByKey(pr.EntityKey, out opr);
            // Otra forma
            System.Data.EntityKey clave =
                new System.Data.EntityKey("northwindEntities.Productos", "ProductID", pr.ProductID);
            northwindEntities.TryGetObjectByKey(clave, out opr);
            Producto pe = null;
            if (opr != null)
                pe = opr as Producto;
            if (pe != null)
            {
                pe.UnitPrice = decimal.Parse(textBox3.Text);
                northwindEntities.SaveChanges();
                //Vuelvo a cargar los productos en la grid
                button1_Click(null, null);
            }
            else
                MessageBox.Show("No existe el producto");
        }

    }
}

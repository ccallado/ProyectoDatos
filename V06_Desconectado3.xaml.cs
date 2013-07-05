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
    /// Lógica de interacción para V06_Desconectado3.xaml
    /// </summary>
    public partial class V06_Desconectado3 : Window
    {
        System.Windows.Data.CollectionViewSource categoriesViewSource;
        System.Windows.Data.CollectionViewSource categoriesProductsViewSource;

        int UltimaCategoria;

        public V06_Desconectado3()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ProyectoDatos.NWDataSet nWDataSet = ((ProyectoDatos.NWDataSet)(this.FindResource("nWDataSet")));
            // Cargar datos en la tabla Categories. Puede modificar este código según sea necesario.
            ProyectoDatos.NWDataSetTableAdapters.CategoriesTableAdapter nWDataSetCategoriesTableAdapter = new ProyectoDatos.NWDataSetTableAdapters.CategoriesTableAdapter();
            nWDataSetCategoriesTableAdapter.LlenarCategorias(nWDataSet.Categories);
            //Cambiado manualmente, definida la variable a nivel de formulario
            categoriesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoriesViewSource")));
            categoriesViewSource.View.MoveCurrentToLast();
            UltimaCategoria = categoriesViewSource.View.CurrentPosition;

            // Cargar datos en la tabla Products. Puede modificar este código según sea necesario.
            ProyectoDatos.NWDataSetTableAdapters.ProductsTableAdapter nWDataSetProductsTableAdapter = new ProyectoDatos.NWDataSetTableAdapters.ProductsTableAdapter();
            nWDataSetProductsTableAdapter.LlenarProductos(nWDataSet.Products);
            //Cambiado manualmente, definida la variable a nivel de formulario
            categoriesProductsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoriesProductsViewSource")));
            categoriesProductsViewSource.View.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(View_CollectionChanged);

            //categoriesProductsViewSource.View.MoveCurrentToFirst();
            categoriesViewSource.View.CurrentChanged += new EventHandler(View_CurrentChanged);
            //categoriesViewSource.View.MoveCurrentToFirst();
            button1_Click(null, null);
        }

        void View_CurrentChanged(object sender, EventArgs e)
        {
            categoriesProductsViewSource.View.MoveCurrentToLast();
            label1.Content = "Productos: " + (categoriesProductsViewSource.View.CurrentPosition + 1);
            categoriesProductsViewSource.View.MoveCurrentToFirst();
        }

        void View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            categoriesProductsViewSource.View.MoveCurrentToLast ();
            label1.Content = "Productos: " + (categoriesProductsViewSource.View.CurrentPosition + 1);
            categoriesProductsViewSource.View.MoveCurrentToFirst();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            categoriesViewSource.View.MoveCurrentToFirst();
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = true ;
            button4.IsEnabled = true ;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            categoriesViewSource.View.MoveCurrentToPrevious();
            if (categoriesViewSource.View.CurrentPosition == 0)
            {
                button1.IsEnabled = false;
                button2.IsEnabled = false;
            }

            button3.IsEnabled = true;
            button4.IsEnabled = true;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            categoriesViewSource.View.MoveCurrentToNext ();
            if (categoriesViewSource.View.CurrentPosition == UltimaCategoria )
            {
                button3.IsEnabled = false;
                button4.IsEnabled = false;
            }

            button1.IsEnabled = true;
            button2.IsEnabled = true;
            
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            categoriesViewSource.View.MoveCurrentToLast ();
            button1.IsEnabled = true ;
            button2.IsEnabled = true ;
            button3.IsEnabled = false ;
            button4.IsEnabled = false ;
        }
    }
}

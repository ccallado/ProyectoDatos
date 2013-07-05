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
    /// Lógica de interacción para V05_Desconectado2.xaml
    /// </summary>
    public partial class V05_Desconectado2 : Window
    {
        public V05_Desconectado2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ProyectoDatos.NWDataSet nWDataSet = ((ProyectoDatos.NWDataSet)(this.FindResource("nWDataSet")));
            // Cargar datos en la tabla Categories. Puede modificar este código según sea necesario.
            ProyectoDatos.NWDataSetTableAdapters.CategoriesTableAdapter nWDataSetCategoriesTableAdapter = new ProyectoDatos.NWDataSetTableAdapters.CategoriesTableAdapter();
            nWDataSetCategoriesTableAdapter.LlenarCategorias(nWDataSet.Categories);
            System.Windows.Data.CollectionViewSource categoriesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoriesViewSource")));
            categoriesViewSource.View.MoveCurrentToFirst();
        }
    }
}

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
    /// Lógica de interacción para V11_LINQ5.xaml
    /// </summary>
    public partial class V11_LINQ5 : Window
    {
        NWDataSet ds;
        string cad;

        public V11_LINQ5()
        {
            InitializeComponent();
            ds = NWDataSet.CargaDatos();
        }

        //ToList
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var producto = ds.Products.SingleOrDefault(p => p.ProductID == int.Parse(textBox1.Text));
            if (producto != null)
            {
//                var prods = ds.Products.Where(p => p.CategoryID == producto.CategoryID).ToList();
                var prods = ds.Products
                    .Where(p => p.CategoryID == producto.CategoryID)
                    .Select(p => new {p.ProductName, p.UnitsInStock })
                    .ToList();
                cad = "";
                cad += "Producto: " + 
                       producto.ProductName + " - " +
                       producto.UnitsInStock + "\n\n";
                producto.UnitsInStock = 999;

                foreach (var p in prods)
                    cad += p.ProductName + " - " +
                           p.UnitsInStock + "\n";

                MessageBox.Show(cad);
            }
            else
                MessageBox.Show("No encontrado");
        }
    }
}

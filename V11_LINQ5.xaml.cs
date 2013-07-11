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
                    .Select(p => new { p.ProductName, p.UnitsInStock })
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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var Productos = ds.Products
                            .Where(p => p.CategoryID == int.Parse(textBox2.Text))
                            .ToDictionary(p => p.ProductID);

            cad = "";
            foreach (var x in Productos)
            {
                cad += "Clave: " + x.Key +
                      "\tProducto: " + x.Value.ProductName + "\n";
            }

            if (Productos.ContainsKey(1))
                cad += "\n\nEncontrado el 1";
            else
                cad += "\n\nNO Encontrado el 1";

            if (Productos.ContainsKey(8))
                cad += "\n\nEncontrado el 8";
            else
                cad += "\n\nNO Encontrado el 8";

            MessageBox.Show(cad);
        }

        //Select
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var pedidos = ds.Orders.Where(c => c.CustomerID == textBox3.Text);

            //Select de líneas de detalle
            //Obtengo un ARRAY de elementos (No lo puedo asignar a un Grid)
            var detalles1 = pedidos.Select(p => p.GetOrder_DetailsRows());

            cad = "";
            foreach (var x in detalles1)
            {
                cad += "Detalles: " + x.Count();
                foreach (var d in x)
                {
                    cad += "\n" +
                           d.OrderID + " - " +
                           d.ProductID + " " +
                           d.ProductsRow.ProductName + "\t (unidades: " +
                           d.Quantity + ")";
                }
                cad += "\n\n";
            }

            MessageBox.Show(cad);

            //SelectMany de líneas de detalle
            //Obtengo una coleccion (Puedo asignarlo a un Grid)
            var detalles2 = pedidos.SelectMany(p => p.GetOrder_DetailsRows());

            cad = "";
            foreach (var x in detalles2)
            {
                //                cad += "Detalles: " + x.Count();
                //                foreach (var d in x)
                //                {
                cad += "\n" +
                       x.OrderID + " - " +
                       x.ProductID + " " +
                       x.ProductsRow.ProductName + "\t (unidades: " +
                       x.Quantity + ")";
                //                }
                //                cad += "\n\n";
            }

            MessageBox.Show(cad);
        }
    }
}

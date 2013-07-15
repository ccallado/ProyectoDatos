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

using System.Data.Objects;

namespace ProyectoDatos
{
    /// <summary>
    /// Lógica de interacción para V15_Entity04.xaml
    /// </summary>
    public partial class V15_Entity04 : Window
    {
        public V15_Entity04()
        {
            InitializeComponent();
        }

        //Procedimientos almacenados SPNombreCategoria
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                string nombre = ne.funSPNombreCategoria(int.Parse(textBox1.Text)).SingleOrDefault();

                if (nombre != null)
                    MessageBox.Show("Categoría: " + nombre);
                else
                    MessageBox.Show("No existe la categoría " + textBox1.Text);
            }
        }

        //Procedimientos almacenados SPInfoCategoria
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                ObjectParameter pNombre = new ObjectParameter("Nombre", typeof(string));
                ObjectParameter pProductos = new ObjectParameter("Productos", typeof(int));
                Categoria Categ = ne.funSPInfoCategoria(int.Parse(textBox1.Text), pNombre, pProductos)
                                                        .SingleOrDefault();

                if (Categ != null)
                    MessageBox.Show("Categoría: " + pNombre.Value.ToString() + " - " +
                                    Categ.Description, "Cantidad: " + pProductos.Value  );
                else
                    MessageBox.Show("No existe la categoría " + textBox1.Text);
            }
        }

        //Procedimientos almacenados SPProductosPorCategoria
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                ObjectParameter pNombre = new ObjectParameter("Nombre", typeof(string));
                ObjectParameter pProductos = new ObjectParameter("Productos", typeof(int));
                var Prods = ne.funSPProductosPorCategoria(int.Parse(textBox1.Text), pNombre, pProductos);

                string cad = "";
                foreach (var p in Prods)
                {
                    cad += p.ProductName + " - " +
                           p.UnitPrice.Value.ToString("c") + "\n";
                }

                if (pNombre.Value.ToString() != "")
                    MessageBox.Show(pNombre.Value.ToString() + "\n\n" + 
                                    cad,
                                    "Cantidad: " + pProductos.Value);
                else
                    MessageBox.Show("No existe la categoría " + textBox1.Text);
            }
        }

        //Procedimientos almacenados SPVentasPorCategoria
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            using (northwindEntities ne = new northwindEntities())
            {
                var datos = ne.funSPVentasPorCategoria(int.Parse(textBox1.Text));
                string cad = "";
                foreach (var x in datos)
                {
                    cad += x.ProductName + ": " + x.Cantidad + "\n";
                }
                MessageBox.Show(cad);
            }
        }
    }
}

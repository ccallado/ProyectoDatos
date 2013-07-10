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
    /// Lógica de interacción para V10_LINQ4.xaml
    /// </summary>
    public partial class V10_LINQ4 : Window
    {
        //Cargar el DataSet
        NWDataSet ds;
        string cad;

        public V10_LINQ4()
        {
            InitializeComponent();
            ds = NWDataSet.CargaDatos();
        }

        //Hacemos los parses antes de hacer la consula
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            int IdProd = int.Parse(textBox1.Text);
            var info = from det in ds.Order_Details
                       where det.ProductID == IdProd
                       select new
                       {
                           IdProducto = det.ProductID,
                           Cantidad = det.Quantity,
                           Precio = det.UnitPrice
                       };

            cad = "";
            foreach (var x in info)
                cad += x.IdProducto + " - " +
                       x.Cantidad + " uds. - " +
                       x.Precio.ToString("C") + "\n";

            //Creamos un objeto anónimo con la misma estructura que la consulta
            //Creo la instancia anónima a buscar
            var infoaBuscar = new
            {
                IdProducto = IdProd,
                Cantidad = (short)120,
                Precio = 18M
            };

            //vamos a ver si esto existe dentro de la colección
            if (info.Contains(infoaBuscar))
                cad += "\n\nEncontrado";
            else
                cad += "\n\nNO Encontrado";


            MessageBox.Show(cad);

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int IdProd = int.Parse(textBox2.Text);
            var info = from det in ds.Order_Details
                       where det.ProductID == IdProd
                       //Añadirle InfoPedido, instanciamos a la clase creada anteriormente
                       select new InfoPedido()
                       {
                           IdProducto = det.ProductID,
                           Cantidad = det.Quantity,
                           Precio = det.UnitPrice
                       };

            cad = "";
            foreach (var x in info)
                cad += x.IdProducto + " - " +
                       x.Cantidad + " uds. - " +
                       x.Precio.ToString("C") + "\n";

            //Creamos un objeto anónimo con la misma estructura que la consulta
            //Creo la instancia anónima a buscar
            //La estructura deja de ser anónima pues instancio una clase INFOPEDEDO
            var infoaBuscar = new InfoPedido()
            {
                IdProducto = IdProd,
                Cantidad = (short)20,
                Precio = 18M
            };

            //vamos a ver si esto existe dentro de la colección
            //NO lo encuentra nunca porque ahora estamos comparando punteros de Clases no STRUCS
            if (info.Contains(infoaBuscar))
                cad += "\n\nEncontrado";
            else
                cad += "\n\nNO Encontrado";


            MessageBox.Show(cad);

        }

        //DISTINCT
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            int IdProd = int.Parse(textBox3.Text);
            var info = from det in ds.Order_Details
                       where det.ProductID == IdProd
                       select new InfoPedido()
                       {
                           IdProducto = det.ProductID,
                           Cantidad = det.Quantity,
                           Precio = det.UnitPrice
                       };

            cad = "";

            //SIN DISTINT
            foreach (var x in info)
                cad += x.IdProducto + " - " +
                       x.Cantidad + " uds. - " +
                       x.Precio.ToString("C") + "\n";
            MessageBox.Show(cad, "SIN Distinct: " + info.Count());

            cad = "";

            //CON DISTINT
            IEnumerable<InfoPedido> infoAux;
            if (!checkBox1.IsChecked.Value)
                infoAux = info.Distinct();
            else
                infoAux = info.Distinct(new ComparadorInfoPedido());

            foreach (var x in infoAux)
                cad += x.IdProducto + " - " +
                       x.Cantidad + " uds. - " +
                       x.Precio.ToString("C") + "\n";
            MessageBox.Show(cad, "CON Distinct: " + infoAux.Count());

        }

        //UNION
        private void button4_Click(object sender, RoutedEventArgs e)
        {

            var info = ds.Products.Where(p => p.CategoryID == 1)
                        .Union(ds.Products.Where(p => p.CategoryID == 4));

            cad = "";

            foreach (var x in info)
                cad += x.CategoryID + " - " +
                       x.ProductID + " - " +
                       x.ProductName + "\n";
            MessageBox.Show(cad, "UNION: " + info.Count());

            //Otra UNION de Clientes y Proveedores de una ciudad
            var info2 = from cli in ds.Customers
                        where cli.City == textBox4.Text
                        select new
                        {
                            Id = cli.CustomerID,
                            cli.CompanyName,
                            cli.ContactName,
                            cli.Phone,
                            cli.City,
                            Tipo = "Cliente"
                        };

            var info3 = from prov in ds.Suppliers
                        where prov.City == textBox4.Text
                        select new
                        {
                            Id = prov.SupplierID.ToString(),
                            prov.CompanyName,
                            prov.ContactName,
                            prov.Phone,
                            prov.City,
                            Tipo = "Proveedor"
                        };

            var info4 = info2.Union(info3);

            cad = "";

            //Ordeno info4 por nombre de la compañía
            foreach (var x in info4.OrderBy(ta => ta.CompanyName))
                cad += x.Tipo + "\t" +
                       x.Id + " \t- " +
                       x.CompanyName + " \t- " +
                       x.ContactName + "\n";

            MessageBox.Show(cad, "UNION: " + info4.Count());
        }

        //CONCAT  buscar los dos productos mas vendidos 
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            var info = (from prod in ds.Order_Details
                        group prod by prod.ProductID
                            into infoAux
                            select new
                            {
                                Id = infoAux.Key,
                                Cantidad = infoAux.Sum(p => p.Quantity)
                            }
                       )
                       .OrderByDescending(x => x.Cantidad)
                       .Take(2);

            var info2 = from p1 in ds.Order_Details
                        where p1.ProductID == info.First().Id
                        select p1.OrderID;

            var info3 = from p2 in ds.Order_Details
                        where p2.ProductID == info.Last().Id
                        select p2.OrderID;

            var infoConcat = info2.Concat(info3).OrderBy(ta => ta);
            var infoUnion = info2.Union(info3).OrderBy(ta => ta);

            cad = "";

            foreach (var x in infoConcat)
                cad += x + "\n";

            MessageBox.Show(cad, "CONCAT: " + infoConcat.Count());

            cad = "";

            foreach (var x in infoUnion)
                cad += x + "\n";

            MessageBox.Show(cad, "UNION: " + infoUnion.Count());
        }

        //Intersec
        //Clientes y proveedores en la misma ciudad
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            //var info = (from c in ds.Customers
            //           select c.City)
            //           .Intersect(from p in ds.Suppliers
            //                          select p.City);

            var info = ds.Customers.Select(c => c.City)
                       .Intersect(ds.Suppliers.Select(p => p.City));

            cad = "";

            foreach (string ciudad in info)
                cad += ciudad + "\n";

            MessageBox.Show(cad, "Intersect: " + info.Count());
        }

        //Except
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            var info = ds.Customers.Select(c => c.City)
                       .Except(ds.Suppliers.Select(p => p.City));

            cad = "";

            foreach (string ciudad in info.OrderBy(c=>c))
                cad += ciudad + "\n";

            System.Diagnostics.Debug.Print(cad);

            MessageBox.Show(cad, "Except: " + info.Count());
        }
    }
}

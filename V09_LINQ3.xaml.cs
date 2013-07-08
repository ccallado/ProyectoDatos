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
    /// Lógica de interacción para V09_LINQ3.xaml
    /// </summary>
    public partial class V09_LINQ3 : Window
    {
        NWDataSet ds;
        string cad;

        public V09_LINQ3()
        {
            InitializeComponent();

            ds = NWDataSet.CargaDatos();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Con 
            //var Prods = from p in ds.Products
            //            orderby p.CategoryID 
            //            group p by p.CategoryID;

            //Con métodos de extensión
            //var Prods = ds.Products
            //            .OrderBy(p => p.CategoryID)
            //            //Con un solo campo
            //            //.GroupBy(p => p.CategoryID);
            //            //Con más de un campo
            //            .GroupBy(p => {p.CategoryID), p.SupplierID };

            //Algo más optimizado cuando hay muchos registros
            //Se hace into en una variable que me reduce la agrupación
            //Y luego hago un order sobre la variable que tiene menos registros
            var Prods = from p in ds.Products
                        group p by p.CategoryID
                        into productos
                        orderby productos.Key
                        select productos;

            cad = "";
            foreach (var pAux in Prods)
            {
                cad += "Categoría: " + pAux.Key + "\n";
                foreach (var p in pAux)
                { 
                    cad +="  " + p.ProductName +"\n";
                }
            }

            MessageBox.Show(cad);
        }
    }
}

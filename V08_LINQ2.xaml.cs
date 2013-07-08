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
    /// Lógica de interacción para V08_LINQ2.xaml
    /// </summary>
    public partial class V08_LINQ2 : Window
    {
        //Creamos el dataset a nivel de formulario
        NWDataSet ds;

        public V08_LINQ2()
        {
            InitializeComponent();

            ds = NWDataSet.CargaDatos();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Estas consultas no se ejecutan hasta que me las recorro con el foreach
            IEnumerable<NWDataSet.CategoriesRow> cats1;

            cats1 = from c in ds.Categories
                    select c;

            //Lo mismo que lo anterior con INFERENCIA
            var cats2 = from c in ds.Categories
                        select c;

            var cats3 = from c in ds.Categories
                        where c.CategoryID < 4
                        select c;

            var cats4 = from c in ds.Categories
                        where c.CategoryID < 4
                        orderby c.Description //ascending 
                        select c;
            //Nos da una lista de punteros a los registros en este caso porque en la select pongo el alias

            string cad = "";
            cad += "Cats1\n";
            foreach (var c in cats1)
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;

            cad += "Cats2\n";
            foreach (var c in cats2)
            {
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;
                if (c.CategoryID == 2)
                    //Al modificar el dato los otros foreach devuelven el mismo dato
                    //PORQUE son punteros a datos.
                    //Esto no ocurre si lo que se apunta no es una lista de punteros es NO MODIFICABLE
                    //Ejemplo: tipos ANONIMOS
                    c.CategoryName = "Lo que sea";
            }

            cad += "Cats3\n";
            foreach (var c in cats3)
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;

            cad += "Cats4\n";
            foreach (var c in cats4)
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;

            MessageBox.Show(cad);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //METODOS DE EXTENSION
            //Estas consultas no se ejecutan hasta que me las recorro con el foreach
            IEnumerable<NWDataSet.CategoriesRow> cats1;

            cats1 = ds.Categories;

            //Lo mismo que lo anterior con INFERENCIA
            //Metodo de extensión SELECT
            //Expresión Lambda (Por cada c proyectame c)
            var cats2 = ds.Categories.Select(c => c);

            //Metodo de extensión WHERE
            //Expresión Lambda 
            var cats3 = ds.Categories.Where(c => c.CategoryID < 4);

            var cats4 = ds.Categories
                        .Where(c => c.CategoryID < 4)
                        .OrderBy(c => c.Description);
            //.OrderByDescending (c=>c.Description)

            //Nos da una lista de punteros a los registros en este caso porque en la select pongo el alias

            string cad = "";
            cad += "Cats1\n";
            foreach (var c in cats1)
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;

            cad += "Cats2\n";
            foreach (var c in cats2)
            {
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;
                if (c.CategoryID == 2)
                    //Al modificar el dato los otros foreach devuelven el mismo dato
                    //PORQUE son punteros a datos.
                    //Esto no ocurre si lo que se apunta no es una lista de punteros es NO MODIFICABLE
                    //Ejemplo: tipos ANONIMOS
                    c.CategoryName = "Lo que sea";
            }

            cad += "Cats3\n";
            foreach (var c in cats3)
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;

            cad += "Cats4\n";
            foreach (var c in cats4)
                cad += "\n  " + c.CategoryID + " - " + c.CategoryName;

            MessageBox.Show(cad);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            var cats = from c in ds.Categories
                       orderby c.Description
                       //select c;
                       //Vamos a hacerlo con tipos anomimos
                       //Estamos proyectando u  IEnumerable <a'> cats
                       //son copia no es un puntero Solo Lectura
                       select new
                       {
                           c.CategoryName,
                           c.CategoryID
                       };

            comboBox1.ItemsSource = cats;
            comboBox1.DisplayMemberPath = "CategoryName";
            comboBox1.SelectedValuePath = "CategoryID";
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked.Value)
            {
                var cats = from c in ds.Categories
                           where c.CategoryID < 5
                           orderby c.Description
                           select c;

                dataGrid1.ItemsSource = cats;
            }
            else
            {
                {
                    var cats = from c in ds.Categories
                               where c.CategoryID < 5
                               orderby c.Description
                               select new
                               {
                                   c.CategoryName,
                                   c.CategoryID
                               };

                    dataGrid1.ItemsSource = cats;
                }
            }

            dataGrid1.Columns[2].Visibility =
                checkBox1.IsChecked.Value ?
                System.Windows.Visibility.Visible :
                System.Windows.Visibility.Hidden;
            dataGrid1.Columns[3].Visibility =
                checkBox1.IsChecked.Value ?
                System.Windows.Visibility.Visible :
                System.Windows.Visibility.Hidden;
        }
    }
}

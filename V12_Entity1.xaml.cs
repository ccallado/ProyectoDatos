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
    /// Lógica de interacción para V12_Entity1.xaml
    /// </summary>
    public partial class V12_Entity1 : Window
    {
        public V12_Entity1()
        {
            InitializeComponent();
        }

        //Tabla Categorías (Entity Framework)
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Este objeto hay que crearlo y borrarlo cuanto antes, 
            //para liberar recursos

            using (northwindEntities ne = new northwindEntities())
            {
                //Ataca a la base de datos pidiendo todos los campos de la tabla.
                //ES modificable en un GRID
                comboBox1.ItemsSource = ne.Categorias;
                comboBox1.DisplayMemberPath = "CategoryName";
                comboBox1.SelectedValuePath = "CategoryID";
                //La consulta a la base de datos no se hace hasta que no despliegue
                //el combo.
            }
        }

        //Tabla Categorías (Entity Framework)
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //Este objeto hay que crearlo y borrarlo cuanto antes, 
            //para liberar recursos

            using (northwindEntities ne = new northwindEntities())
            {
                //LINQ. hacemos un anónimo
                //Ataca a la base de datos pidiendo solo dos campos.
                //No es modificable en un GRID
                comboBox2.ItemsSource = ne.Categorias
                                          .Select(c => new { c.CategoryID, c.CategoryName });
                comboBox2.DisplayMemberPath = "CategoryName";
                comboBox2.SelectedValuePath = "CategoryID";
                //La consulta a la base de datos no se hace hasta que no despliegue
                //el combo.
            }
        }

        //Mostrar la descripción del elemento seleccionado
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Cuando no hay ninguno seleccionado vale -1
            if (comboBox1.SelectedIndex != -1)
            {
                Categoria cat = comboBox1.SelectedItem as Categoria;
                if (cat != null)
                    MessageBox.Show(cat.Description);
            }
        }

        //Mostrar la descripción del elemento seleccionado
        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Cuando no hay ninguno seleccionado vale -1
            if (comboBox2.SelectedIndex != -1)
            {
                //No tengo un objeto Categoria
                //Tengo un anómimo
                using (northwindEntities ne = new northwindEntities())
                {
                    string des = ne.Categorias
                                 .Where(c => c.CategoryID == (int)comboBox2.SelectedValue)
                                 //Como es una colección debemos seleccionar uno, con single, last, first, etc...
                                 .Select(c => c.Description).SingleOrDefault();

                    if (des != null)
                        MessageBox.Show(des);
                }
            }
        }

        private System.Data.Objects.ObjectQuery<Categoria> GetCategoriasQuery(northwindEntities northwindEntities)
        {
            // Código generado automáticamente

            System.Data.Objects.ObjectQuery<ProyectoDatos.Categoria> categoriasQuery = northwindEntities.Categorias;
            // Devuelve un elemento ObjectQuery.
            return categoriasQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ProyectoDatos.northwindEntities northwindEntities = new ProyectoDatos.northwindEntities();
            // Cargar datos en Categorias. Puede modificar este código según sea necesario.
            System.Windows.Data.CollectionViewSource categoriasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("categoriasViewSource")));
            System.Data.Objects.ObjectQuery<ProyectoDatos.Categoria> categoriasQuery = this.GetCategoriasQuery(northwindEntities);
            categoriasViewSource.Source = categoriasQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }
    }
}

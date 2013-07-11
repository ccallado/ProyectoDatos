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
    /// Lógica de interacción para V13_Entity2.xaml
    /// </summary>
    public partial class V13_Entity2 : Window
    {
        public V13_Entity2()
        {
            InitializeComponent();
        }

        //Entity SQL
        //Vamos a dar un string en las consultas
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string cadena = "";

            //Esto se define de nuestro modelo no de la base de datos
            //No puedo utilizar "*" se utiliza "VALUE"
            cadena = "SELECT VALUE ListaCategorias " +
                     "FROM northwindEntities.Categorias AS ListaCategorias";

            using (northwindEntities ne = new northwindEntities())
            {
                System.Data.Objects.ObjectQuery<Categoria> cats;
                //La creo sin mergeOption y en la ejecución le indico como quiero ejecutarla
                //y le pongo el mergeOption en el Execute
                cats = new System.Data.Objects.ObjectQuery<Categoria>(cadena, ne,
                                                System.Data.Objects.MergeOption.NoTracking);

                comboBox1.ItemsSource = cats;
                comboBox1.DisplayMemberPath = "CategoryName";
                comboBox1.SelectedValuePath = "CategoryID";

            }
        }

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

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string cadena = "";

            //Esto se define de nuestro modelo no de la base de datos
            //No puedo utilizar "*" se utiliza "VALUE"
            cadena  = "SELECT ListaCategorias.CategoryName, ListaCategorias.CategoryID";
            if (checkBox1.IsChecked.Value)
                cadena += ", ListaCategorias.Description";
            cadena += " FROM northwindEntities.Categorias AS ListaCategorias";

            using (northwindEntities ne = new northwindEntities())
            {
                //Ahora no es el registro completo
                System.Data.Objects.ObjectQuery<System.Data.Common.DbDataRecord > cats;
                //La creo sin mergeOption y en la ejecución le indico como quiero ejecutarla
                //y le pongo el mergeOption en el Execute
                cats = new System.Data.Objects.ObjectQuery<System.Data.Common.DbDataRecord>(cadena, ne,
                                                System.Data.Objects.MergeOption.NoTracking);

                comboBox2.ItemsSource = cats;
                comboBox2.DisplayMemberPath = "CategoryName";
                comboBox2.SelectedValuePath = "CategoryID";

            }
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Cuando no hay ninguno seleccionado vale -1
            if (comboBox2.SelectedIndex != -1)
            {
                string desc = "";
                System.Data.Common.DbDataRecord cat;
                cat = comboBox2.SelectedItem as System.Data.Common.DbDataRecord;
                if (cat != null)
                {
                    if (cat.FieldCount == 3)
                        desc = cat[2].ToString();
                    else
                        //Tengo un anómimo
                        using (northwindEntities ne = new northwindEntities())
                        {
                            int id = (int)cat["CategoryID"];
                            desc = ne.Categorias
                                         .Where(c => c.CategoryID == id)
                        //Como es una colección debemos seleccionar uno, con single, last, first, etc...
                                         .Select(c => c.Description).SingleOrDefault();
                        }
                        MessageBox.Show(desc);
                }
            }
        }
    }
}

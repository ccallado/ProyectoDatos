namespace ProyectoDatos {
    
    //Todo lo que haga aquí no se perderá cuando rehaga el DataSet en el diseñador
    public partial class NWDataSet 
    {
        public static NWDataSet  CargaDatos()
        {
            NWDataSet  ds = new NWDataSet();

            NWDataSetTableAdapters.CategoriesTableAdapter taCat;
            taCat = new NWDataSetTableAdapters.CategoriesTableAdapter();
            taCat.LlenarCategorias(ds.Categories);

            NWDataSetTableAdapters.ProductsTableAdapter taProd;
            taProd = new NWDataSetTableAdapters.ProductsTableAdapter();
            taProd.LlenarProductos(ds.Products);

            return ds;
        }

    }
}

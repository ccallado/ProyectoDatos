namespace ProyectoDatos
{

    //Todo lo que haga aquí no se perderá cuando rehaga el DataSet en el diseñador
    public partial class NWDataSet
    {
        public static NWDataSet CargaDatos()
        {
            NWDataSet ds = new NWDataSet();

            NWDataSetTableAdapters.CategoriesTableAdapter taCat;
            taCat = new NWDataSetTableAdapters.CategoriesTableAdapter();
            taCat.LlenarCategorias(ds.Categories);

            NWDataSetTableAdapters.ProductsTableAdapter taProd;
            taProd = new NWDataSetTableAdapters.ProductsTableAdapter();
            taProd.LlenarProductos(ds.Products);

            NWDataSetTableAdapters.CustomersTableAdapter taCli;
            taCli = new NWDataSetTableAdapters.CustomersTableAdapter();
            taCli.Fill(ds.Customers);

            NWDataSetTableAdapters.Order_DetailsTableAdapter taDet;
            taDet = new NWDataSetTableAdapters.Order_DetailsTableAdapter();
            taDet.Fill(ds.Order_Details);

            NWDataSetTableAdapters.OrdersTableAdapter taPed;
            taPed = new NWDataSetTableAdapters.OrdersTableAdapter();
            taPed.Fill(ds.Orders);

            return ds;
        }

    }
}

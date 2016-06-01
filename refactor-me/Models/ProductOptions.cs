using System;
using System.Collections.Generic;
/*
 * moved from Products class
 * having seperate model class will be easier 
 * to read and simplify the code
 */ 
namespace refactor_me.Models
{
    using System.Data.SqlClient;

    public class ProductOptions
    {
       public List<ProductOption> Items { get; private set; }
        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions($"where productid = '{productId}'");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();

             using (var connection = Helpers.NewConnection())
            {
                SqlCommand command;
                connection.Open();
                using (command = new SqlCommand($"select id from productoption {where}",connection))
                {
                    var rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        var id = Guid.Parse(rdr["id"].ToString());
                        Items.Add(new ProductOption(id));
                    }
                }
             }
        }
    }
}
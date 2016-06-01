using System;
/*
 * moved from Products class
 * having seperate model class will be easier 
 * to read and simplify the code
 */
namespace refactor_me.Models
{
    using System.Data.SqlClient;

    public class Product : GenericObject
    {
        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id)
        {
            // use using statement to manage unmanaged resource,
            // not have to worry about connection closed for example
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command;
                connection.Open();
                using (command = new SqlCommand($"select * from product where id = '{id}'",connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IsNew = false;
                        Id = Guid.Parse(reader["Id"].ToString());
                        Name = reader["Name"].ToString();
                        Description = (DBNull.Value == reader["Description"]) ? null : reader["Description"].ToString();
                        Price = decimal.Parse(reader["Price"].ToString());
                        DeliveryPrice = decimal.Parse(reader["DeliveryPrice"].ToString());
                    }
                    IsNew = true;
                }
            }
        }

        public void Save()
        {
            // use using statement to manage unmanaged resource, 
            // not have to worry about connection closed for example
            using (var connection = Helpers.NewConnection())
            {
                connection.Open();
                SqlCommand command;
                if (IsNew)
                {
                    using (command = new SqlCommand($"insert into product (id, name, description, price, deliveryprice) values ('{Id}', '{Name}', '{Description}', {Price}, {DeliveryPrice})", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (command = new SqlCommand($"update product set name = '{Name}', description = '{Description}', price = {Price}, deliveryprice = {DeliveryPrice} where id = '{Id}'", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }  
        }

        public void Delete()
        {
            // move create new ProductOptions out of foreach statement, 
            // as only one instance required, avoid creating uncessary ProductOptions objects, performance wised
             var proOption = new ProductOptions(Id);
            foreach (var option in proOption.Items)
            option.Delete();
            using (var connection = Helpers.NewConnection())
            {
                connection.Open();

                 SqlCommand command;

                using (command = new SqlCommand($"delete from product where id = '{Id}'", connection))
                {
                    command.ExecuteNonQuery();
                }

            }
        }
    }
}
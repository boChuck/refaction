using System;
/*
 * moved from Products class
 * having seperate model class will be easier 
 * to read and simplify the code
 */
namespace refactor_me.Models
{
    using System.Data;
    using System.Data.SqlClient;

    public class ProductOption : GenericObject
    {
        public Guid ProductId { get; set; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            //use using statement to manage unmanaged resource, 
            //not have to worry about connection closed for example
            using (var connection = Helpers.NewConnection())
            {
                SqlCommand command;
                connection.Open();
                using (command = new SqlCommand($"select * from productoption where id = '{id}'",connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IsNew = false;
                        Id = Guid.Parse(reader["Id"].ToString());
                        ProductId = Guid.Parse(reader["ProductId"].ToString());
                        Name = reader["Name"].ToString();
                        Description = (DBNull.Value == reader["Description"]) ? null : reader["Description"].ToString();  
                    }
                    IsNew = true;
                }
            }
        }

        public void Save()
        {
            //use using statement to manage unmanaged resource, 
            //not have to worry about connection closed for example
            using (var connection = Helpers.NewConnection())
            {
                connection.Open();
                SqlCommand command;
                if (IsNew)
                {
                    using (command = new SqlCommand($"insert into productoption (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (command = new SqlCommand($"update productoption set name = '{Name}', description = '{Description}' where id = '{Id}'", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }  
        }

        public void Delete()
        {
             //use using statement to manage unmanaged resource, 
            //not have to worry about connection closed for example
            using (var connection = Helpers.NewConnection())
            {
                connection.Open();
                SqlCommand command;
                using (command = new SqlCommand($"delete from productoption where id = '{Id}'", connection))
                {
                   command.ExecuteNonQuery();
                }
            }  
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
/*
 * moved from Products class
 * having seperate model class will be easier 
 * to read and simplify the code
 */ 
namespace refactor_me.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }
        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts($"where lower(name) like '%{name.ToLower()}%'");
        }

        private void LoadProducts(string where)
        {
            Items = new List<Product>();
             using (var connection = Helpers.NewConnection())
            {
                SqlCommand command;
                connection.Open();
                using (command = new SqlCommand($"select id from product {where}",connection))
                {
                    var rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        var id = Guid.Parse(rdr["id"].ToString());
                        Items.Add(new Product(id));
                    }
                }
             } 
        }
    }
}
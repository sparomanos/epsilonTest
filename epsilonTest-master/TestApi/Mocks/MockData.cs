using Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApi.Mocks
{
    public static class MockData
    {
        public static List<Customer> GetCustomerMockData()
        {
            List<Customer> mockCustomerData = new List<Customer>();
            
            for(int i=0; i<12; i++)
            {
                mockCustomerData.Add( new Customer()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CompanyName = $"Company Test {i}",
                    City = "Thessaloniki",
                    ContactName = $"Person {i}",
                    Country = "Greece",
                    PostalCode = "55452",
                    Address = $"Address {i}",
                    Phone = "+302310222444",
                    Region = $"Region {i}"
                });
            }
            return mockCustomerData;
        }

        public static Customer GetOneCustomerMockData()
        {
            Customer mockCustomer = new Customer()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CompanyName = "Company Test",
                City = "Thessaloniki",
                ContactName = "Person",
                Country = "Greece",
                PostalCode = "55452",
                Address = "Address 1",
                Phone = "+302310222333",
                Region = "Region"
            };

            return mockCustomer;
        }
    }
}

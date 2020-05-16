using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.DbSettings;

namespace TestApi.Mocks
{
    public static class DatabaseSettingsMock
    {
        public static DatabaseSettings GetDatabaseSettings()
        {
            return new DatabaseSettings() { DatabaseName = "Tests", ConnectionString = "mongodb://localhost:27017" };
        }
    }
}

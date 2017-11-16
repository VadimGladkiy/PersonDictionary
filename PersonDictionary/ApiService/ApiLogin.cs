using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonDictionary.Models;

namespace PersonDictionary.ApiService
{
    public class ApiLogin
    {
        public static String Login(string userName, string password)
        {
            using (DataContext _DBEntities = new DataContext())
            {
                return  _DBEntities.Persons.Single(x => x.Name
                .Equals(userName, StringComparison.OrdinalIgnoreCase)
                && x.password == password).Id;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface ICustomer:IDocument
    {
       string CompanyName { get; set; }
       string ContactName { get; set; }
       string Address { get; set; }
       string City { get; set; }
       string Region { get; set; }
       string PostalCode { get; set; }
       string Country { get; set; }
       string Phone { get; set; }
    }
}

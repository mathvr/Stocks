using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STOCKS.Models
{
    public class AddCompanyApiModel
    {
        public string UserEmail {get; set;}
        public string CompanySymbol {get; set;}
    }
}
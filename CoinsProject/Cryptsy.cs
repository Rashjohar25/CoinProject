using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CoinsProject
{
    public class Currency
    {
        public int id { get; set; }
        public String name { get; set; }
        public String code { get; set; }
        public String maintenance { get; set; }
    }
   
}

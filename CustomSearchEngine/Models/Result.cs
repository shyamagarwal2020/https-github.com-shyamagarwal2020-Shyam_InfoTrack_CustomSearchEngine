using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomSearchEngine.Models
{
    public class Result
    {
        public string Results { get; set; }

        public string Keywords { get; set; }

        public string Url { get; set; }

        public int NumberTimes { get; set; }
      
    }
}
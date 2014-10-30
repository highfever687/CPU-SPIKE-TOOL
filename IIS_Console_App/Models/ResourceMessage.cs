using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIS_Console_App.Models
{
    class ResourceMessage
    {
        public string Message { get; set; }
        public bool NeedsAttention
        //time stamp for the email handling the interval between email alerts
        { get; set; }
        public string elapsedTime{ get; set; }       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAConsoleApp.DTO
{
    public class ResponseDTO
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
    }
}

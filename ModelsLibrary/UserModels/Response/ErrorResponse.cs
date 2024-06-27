using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.UserModels.Response
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}

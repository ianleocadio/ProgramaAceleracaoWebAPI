using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ViewModels.Common
{
    public class ErrorViewModel
    {
        public int Status { get; set; }
        public string Error { get; set; }

        public ErrorViewModel(int statusCode, string serverMessage)
        {
            this.Status = statusCode;
            this.Error = serverMessage;
        }
    }
}

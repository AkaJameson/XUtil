using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.HttpResponseHelper
{
    public class ApiResponse
    {
        public int Code { get; set; }

        public dynamic Data { get; set; }

        public string? Message { get; set; }

        public string Status { get; set; }

        public int Total { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Core.HttpHelper
{
    public static class HttpCreateFactory
    {
        private static object _lock = new object();
        private static IRequest singletonHttpRequest;
        public static IRequest CreateRequest()
        {
            lock (_lock)
            {
                if (singletonHttpRequest == null)
                {
                    singletonHttpRequest = Request.Instance;
                }
                return singletonHttpRequest;
            }
        }

        public static void DeleteRequest()
        {
            lock (_lock)
            {
                if(singletonHttpRequest is not null)
                    singletonHttpRequest = null;
            }

        } 

    }
}

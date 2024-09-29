namespace Xin.DotnetUtil.HttpHelper
{
    public static class HttpCreateFactory
    {
        private static object _lock = new object();
        private static IRequest singletonHttpRequest;
        /// <summary>
        /// 创建全局只持有一个的Http请求对象
        /// </summary>
        /// <param name="RequestTimeout"></param>
        /// <returns></returns>
        public static IRequest CreateInstanceRequest(double? RequestTimeout = null)
        {
            lock (_lock)
            {
                if (singletonHttpRequest == null)
                {
                    singletonHttpRequest = new Request(RequestTimeout);
                }
                return singletonHttpRequest;
            }
        }
        public static IRequest CreateRequest(double? RequestTimeout = null)
        {
            IRequest request = new Request(RequestTimeout);
            return request;
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

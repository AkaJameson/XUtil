namespace Xin.DotnetUtil.HttpResponseHelper
{
    public class ApiResponseHelper
    {
        public static ApiResponse Success(dynamic data)
        {
            return new ApiResponse
            {
                Code = 200,
                Message = "操作成功",
                Status = "success",
                Data = data,
                Total = 0
            };
        }
        //分页使用
        public static ApiResponse Success(dynamic data, int total)
        {
            return new ApiResponse
            {
                Code = 200,
                Data = data,
                Status = "success",
                Message = "操作成功",
                Total = total
            };
        }

        public static ApiResponse Error(string msg)
        {
            return new ApiResponse
            {
                Message = msg,
                Code = 500,
                Status = "Error",
                Data = null,
                Total = 0
            };
        }
        /// <summary>
        /// Debug模式，上线程序请勿使用
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponse Debug(string msg, dynamic data = null)
        {
            return new ApiResponse
            {
                Message = msg,
                Code = 300,
                Status = "Debug",
                Data = data,
                Total = 0
            };
        }
    }
}


namespace XUtil.Captcha.Service
{
    public class CaptchaService : ICaptchaService
    {
        private static CaptchaCache captchaCache = new CaptchaCache();
        private static CaptchaGenerater generater = new CaptchaGenerater();
        /// <summary>
        /// 返回id和验证码图片byte[]数组
        /// </summary>
        /// <param name="expireinMinite"></param>
        /// <returns></returns>
        public (Guid, byte[]) GetCaptcha(int expireinMinite)
        {
            var model = new CaptchaModel();
            var id = Guid.NewGuid();
            var info = generater.GetCaptcha();
            model.Tag = info.CaptchaText;
            model.CreatedAt = DateTime.UtcNow;
            model.ExpirationTime = TimeSpan.FromMinutes(expireinMinite);
            captchaCache.AddCaptchaModel(id, model);
            return (id,info.CaptchaImage);
        }

        public (bool, string) Verify(Guid id, string tag)
        {
           return captchaCache.VerifyModel(id, tag);
        }
    }
}

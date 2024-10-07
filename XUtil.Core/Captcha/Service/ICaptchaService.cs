namespace XUtil.Captcha.Service
{
    public interface ICaptchaService
    {
        (Guid, byte[]) GetCaptcha(int expireinMinite);

        (bool, string) Verify(Guid id, string tag);
    }
}

namespace XUtil.Captcha
{
    internal class CaptchaModel
    {
        public string Tag { get; set; }
        public DateTime CreatedAt { get; set; }
        public TimeSpan ExpirationTime { get; set; } // 过期时间
        public bool IsExpired()
        {
            return DateTime.UtcNow - CreatedAt > ExpirationTime;
        }
    }
}

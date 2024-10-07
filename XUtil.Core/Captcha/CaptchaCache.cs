using System.Collections.Concurrent;

namespace XUtil.Captcha
{
    internal class CaptchaCache
    {
        public ConcurrentDictionary<Guid, CaptchaModel> bitmapCache = new ConcurrentDictionary<Guid, CaptchaModel>();
        private readonly TimeSpan cleanupInterval = TimeSpan.FromMinutes(5);
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public CaptchaCache()
        {
            Task.Run(() => CacheSustainer(cancellationToken:cancellationTokenSource.Token));
        }

        private async Task CacheSustainer(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(cleanupInterval, cancellationToken); // 等待清理间隔

                // 清理过期的验证码
                var expiredKeys = bitmapCache
                    .Where(kvp => kvp.Value.IsExpired())
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in expiredKeys)
                {
                    bitmapCache.TryRemove(key, out _); // 移除过期的验证码
                }
            }
        }
        public void Restart()
        {
            if (!cancellationTokenSource.IsCancellationRequested)
                return;
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => CacheSustainer(cancellationToken: cancellationTokenSource.Token));
        }
        public void Stop()
        {
            cancellationTokenSource.Cancel(); // 停止缓存维护线程
            bitmapCache.Clear();
        }
        public void AddCaptchaModel(Guid id, CaptchaModel model) => bitmapCache[id] = model;
        private void RemoveCaptchaModel(Guid id)
        {
            if(bitmapCache.ContainsKey(id))
                bitmapCache.TryRemove(id, out _);
        }

        public (bool,string) VerifyModel(Guid id,string tag)
        {
            if (!bitmapCache.TryGetValue(id, out var captchaModel))
                return (false, "验证码已过期");
            return captchaModel.Tag.ToLower().Equals(tag.ToLower())
                ? (true, "验证码正确")
                : (false, "验证码错误");
        }
    }
}

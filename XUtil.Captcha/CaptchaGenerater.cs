using SkiaSharp;
using System.Reflection;

namespace XUtil.Captcha
{
    public class CaptchaGenerater
    {
        private Assembly Assembly;
        private SKBitmap TextureImage;
        private Random RandomGenerator;

        public CaptchaGenerater()
        {
            Assembly = Assembly.GetExecutingAssembly();
            RandomGenerator = new Random();

            // 加载嵌入的资源图像
            using (Stream stream = Assembly.GetManifestResourceStream("XUtil.Captcha.Resources.background.png"))
            {
                if (stream != null)
                {
                    TextureImage = SKBitmap.Decode(stream);
                }
            }
        }

        // 生成验证码图片并返回相关参数
        public (string CaptchaText, byte[] CaptchaImage) GetCaptcha()
        {
            int width = 300;
            int height = 100;
            var captchaText = GenerateRandomText(4);
            // 创建一个空的Bitmap图像
            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                // 画背景图
                canvas.Clear(SKColors.White);

                // 绘制纹理背景
                if (TextureImage != null)
                {
                    using (SKShader shader = SKShader.CreateBitmap(TextureImage, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat))
                    using (SKPaint texturePaint = new SKPaint { Shader = shader })
                    {
                        canvas.DrawRect(new SKRect(0, 0, width, height), texturePaint);
                    }
                }

                // 添加底噪 - 随机点和线条
                AddNoise(canvas, width, height);

                // 设置字体和刷子
                using (SKPaint textPaint = new SKPaint())
                {
                    textPaint.TextSize = 40;
                    textPaint.IsAntialias = true;
                    textPaint.Typeface = SKTypeface.FromFamilyName("Georgia", SKFontStyle.BoldItalic);
                    textPaint.Shader = SKShader.CreateLinearGradient(
                        new SKPoint(0, 0),
                        new SKPoint(width, height),
                        new SKColor[] { SKColors.Blue, SKColors.DarkRed },
                        null,
                        SKShaderTileMode.Clamp);

                    // 计算每个字符的宽度和居中位置
                    float totalWidth = 0;
                    float charSpacing = 20; // 字符间距
                    foreach (char c in captchaText)
                    {
                        totalWidth += textPaint.MeasureText(c.ToString()) + charSpacing;
                    }

                    // 计算起始X坐标使得文本居中
                    float startX = (width - totalWidth) / 2;
                    float baselineY = height / 2; // Y轴基线位置

                    Random random = new Random();

                    // 绘制每个字符
                    for (int i = 0; i < captchaText.Length; i++)
                    {
                        char c = captchaText[i];

                        // 随机旋转角度
                        float rotation = (float)(random.NextDouble() * 20 - 10); // 在-10到10度之间

                        // 随机上下偏移
                        float offsetY = (float)(random.NextDouble() * 10 - 5); // 在-5到5像素之间

                        // 计算当前字符的X坐标
                        float x = startX + i * (textPaint.MeasureText(c.ToString()) + charSpacing);

                        // 保存当前画布状态
                        canvas.Save();

                        // 移动到字符的绘制位置并进行旋转
                        canvas.Translate(x, baselineY + offsetY);
                        canvas.RotateDegrees(rotation);

                        // 绘制字符
                        canvas.DrawText(c.ToString(), 0, 0, textPaint);

                        // 恢复画布状态
                        canvas.Restore();
                    }
                }

                // 绘制干扰线
                DrawRandomLines(canvas, width, height);

                // 将生成的图像保存到内存流中
                using (MemoryStream ms = new MemoryStream())
                {
                    using (SKData data = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100))
                    {
                        data.SaveTo(ms);
                    }
                    return (captchaText, ms.ToArray());
                }
            }
        }

        // 生成随机验证码字符串
        private string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] captchaChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                captchaChars[i] = chars[RandomGenerator.Next(chars.Length)];
            }
            return new string(captchaChars);
        }

        // 添加底噪 - 随机点和线条
        private void AddNoise(SKCanvas canvas, int width, int height)
        {
            using (SKPaint paint = new SKPaint { Color = SKColors.Gray })
            {
                // 画随机点
                for (int i = 0; i < 100; i++)
                {
                    int x = RandomGenerator.Next(width);
                    int y = RandomGenerator.Next(height);
                    canvas.DrawRect(new SKRect(x, y, x + 2, y + 2), paint);  // 2x2 小方块噪点
                }
            }
        }

        // 画随机干扰线
        private void DrawRandomLines(SKCanvas canvas, int width, int height)
        {
            using (SKPaint paint = new SKPaint { Color = SKColors.LightGray, StrokeWidth = 2 })
            {
                for (int i = 0; i < 5; i++)
                {
                    int x1 = RandomGenerator.Next(width);
                    int y1 = RandomGenerator.Next(height);
                    int x2 = RandomGenerator.Next(width);
                    int y2 = RandomGenerator.Next(height);
                    canvas.DrawLine(x1, y1, x2, y2, paint);
                }
            }
        }
    }
}

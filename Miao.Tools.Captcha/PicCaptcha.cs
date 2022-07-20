using System;
using System.Drawing;

namespace Miao.Tools.Captcha
{
    /// <summary>
    /// 图形验证码类
    /// </summary>
    public class PicCaptcha
    {
        #region Constructed Methods

        /// <summary>
        /// 默认构造方法
        /// </summary>
        public PicCaptcha()
        {
        }

        /// <summary>
        ///构造方法 
        /// </summary>
        /// <param name="cpatchaType">验证码类型</param>
        public PicCaptcha(CaptchaType cpatchaType)
        {
            this.CaptchaType = cpatchaType;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="captchaType">验证码类型</param>
        /// <param name="width">图形宽度</param>
        /// <param name="height">图形高度</param>
        public PicCaptcha(CaptchaType captchaType, int width, int height)
            : this(captchaType)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="captchaType">验证码类型</param>
        /// <param name="width">图形宽度</param>
        /// <param name="height">图形高度</param>
        /// <param name="fontSize">字体大小</param>
        public PicCaptcha(CaptchaType captchaType, int width, int height, int fontSize)
            : this(captchaType, width, height)
        {
            this.FontSize = fontSize;
        }

        #endregion

        #region Fields

        private int _width = 80;                                    //default width
        private int _height = 25;                                   //default height
        private int _fontSize = 16;                                 //default fontSize
        private CaptchaType _captchaType = CaptchaType.RandomType;  //default captchaType

        #endregion

        #region Properties
        /// <summary>
        /// 验证码图形宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        /// <summary>
        /// 验证码图形高度
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        /// <summary>
        /// 验证码字体大小
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        /// <summary>
        /// 验证码类型
        /// </summary>
        public CaptchaType CaptchaType
        {
            get { return _captchaType; }
            set { _captchaType = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取生成的验证码图片
        /// </summary>
        /// <param name="matchCode">与验证码图片相匹配的内容,一般将该内容保存至session中,用于校验</param>
        /// <returns>验证码图片</returns>
        public Bitmap GetCaptchaPic(out string matchCode)
        {
            matchCode = null;
            string captchaContent;
            switch (_captchaType)  //需要生成的验证码类型
            {
                case CaptchaType.RandomString:
                    {
                        string randomStr = CreateCheckCodeString(4);
                        captchaContent = randomStr;
                        matchCode = randomStr;
                        break;
                    }
                case CaptchaType.RandomOperationExpression:
                    {
                        string[] codes = CreateOperateCheckCode();
                        captchaContent = codes[0];
                        matchCode = codes[1];
                        break;
                    }
                case CaptchaType.RandomType:
                    {
                        RandomSelectCreateMethod(out captchaContent, out matchCode);
                        break;
                    }
                default:
                    throw new Exception("unknow CaptchaType!");
            }
            return GenerateCaptchaPic(captchaContent);   //生成验证码图片
        }

        #endregion

        #region  Private Methods

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="captchaContent">验证码内容</param>
        /// <returns>图片</returns>
        private Bitmap GenerateCaptchaPic(string captchaContent)
        {
            var font = new Font("Arial", _fontSize, FontStyle.Bold);//验证码字体
            var brush = new SolidBrush(Color.Black);//用于写验证码的画笔
            var crosswise = new Pen(Color.Green, 0);//画横向干扰线的钢笔
            var vertical = new Pen(Color.FromArgb(255, 100, 100, 100), 0);//画纵向干扰线的钢笔
            var image = new Bitmap(_width, _height);//生成图像
            using (Graphics g = Graphics.FromImage(image))  //生成一个绘画面板（画布）
            {
                g.Clear(ColorTranslator.FromHtml("#f0f0f0"));//用指定颜色填充画布
                RectangleF rect = new RectangleF(5, 2, _width, _height);//定义文字的绘制矩形
                Random rand = new Random((int)DateTime.Now.Ticks);//生成干扰线的随机对象
                for (int i = 1; i <= 4; i++)   //干扰线
                {
                    Point start = new Point(0, rand.Next(_height));
                    Point end = new Point(_width, rand.Next(_height));
                    g.DrawLine(crosswise, start, end);
                }
                for (int i = 1; i <= 5; i++)  //干扰线
                {
                    Point start = new Point(rand.Next(_width), 0);
                    Point end = new Point(rand.Next(_width), _height);
                    g.DrawLine(vertical, start, end);
                }
                g.DrawString(captchaContent, font, brush, rect);//将验证码写到画布上
            }
            return image;
        }

        /// <summary>
        /// 随机选择生成验证码的方法
        /// </summary>
        /// <param name="captchaContent">验证码图片显示的内容</param>
        /// <param name="matchCode">与验证码图片匹配的内容</param>
        private void RandomSelectCreateMethod(out string captchaContent, out string matchCode)
        {
            int currSeconds = DateTime.Now.Second;
            if (currSeconds % 2 == 0)
            {
                captchaContent = matchCode = CreateCheckCodeString(4);
            }
            else
            {
                string[] codes = CreateOperateCheckCode();
                captchaContent = codes[0];
                matchCode = codes[1];
            }
            return;
        }

        /// <summary>
        /// 生成len位随机字符串
        /// </summary>
        /// <param name="len">随机字符串长度</param>
        /// <returns>len位随机字符串</returns>
        private string CreateCheckCodeString(int len)
        {
            //定义用于验证码的字符数组
            char[] checkCodeArray ={ '1','2','3','4','5','6','7','8','9','A','B','C',
        'D','E','F','G','H','I','J','K','L','M','N','P','Q','R','S','T','U','V','W',
        'X','Y','Z'};
            //定义验证码字符串
            string randomcode = "";
            Random rd = new Random();
            //生成4位验证码字符串
            for (int i = 1; i <= len; i++)
                randomcode += checkCodeArray[rd.Next(checkCodeArray.Length)];
            return randomcode;
        }

        /// <summary>
        /// 生成随机的运算表达式  1+1=?,2
        /// </summary>
        /// <returns>codes[0]: 表达式; codes[1]: 运算结果</returns>
        private string[] CreateOperateCheckCode()
        {
            string[] codes = new string[2] { "", "" };
            string[] operators = new string[] { "+", "-" };
            var random = new Random();
            int num1 = random.Next(10); // [0,19)
            int num2 = random.Next(10);
            string oper = operators[random.Next(operators.Length)];
            int result;
            switch (oper)
            {
                case "+":
                    {
                        result = num1 + num2;
                        break;
                    }
                case "-":
                    {
                        result = num1 - num2;
                        break;
                    }
                default:
                    throw new Exception("operators error");
            }
            codes[0] = string.Format("{0}{1}{2}=?", num1, oper, num2);
            codes[1] = result.ToString();
            return codes;
        }

        #endregion
    }
}

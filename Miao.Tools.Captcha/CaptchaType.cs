namespace Miao.Tools.Captcha
{
    /// <summary>
    /// 生成的验证码类型
    /// </summary>
    public enum CaptchaType
    {
        /// <summary>
        /// 随机类型, 在所有支持的验证码类型中,随机选择一种
        /// </summary>
        RandomType,

        /// <summary>
        /// 普通的随机字符串,如: jH9Y, 12H8
        /// </summary>
        RandomString,

        /// <summary>
        /// 随机运算表达式, 如: 1+2=?, 5加2=?, 4-2=?
        /// </summary>
        RandomOperationExpression,
    }
}

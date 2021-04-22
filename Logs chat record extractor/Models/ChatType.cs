namespace Logs_chat_record_extractor.Models
{
    public enum ChatType
    {
        /// <summary>
        /// 小队
        /// </summary>
        Party,

        /// <summary>
        /// 团队
        /// </summary>
        Alliance,

        /// <summary>
        /// 部队
        /// </summary>
        FreeCompany,

        /// <summary>
        /// 说话
        /// </summary>
        Speak,

        /// <summary>
        /// 呼喊
        /// </summary>
        Yell,

        /// <summary>
        /// 喊话
        /// </summary>
        Shout,

        /// <summary>
        /// 悄悄话（别人发过来的）
        /// </summary>
        Tell,

        /// <summary>
        /// 情感动作
        /// </summary>
        Motion,
        
        /// <summary>
        /// 新频
        /// </summary>
        Beginner,

        /// <summary>
        /// 通讯贝
        /// </summary>
        LinkShell1,
        LinkShell2,
        LinkShell3,
        LinkShell4,
        LinkShell5,
        LinkShell6,
        LinkShell7,
        LinkShell8,

        /// <summary>
        /// 跨服通讯贝
        /// </summary>
        CwLinkShell1,
        CwLinkShell2,
        CwLinkShell3,
        CwLinkShell4,
        CwLinkShell5,
        CwLinkShell6,
        CwLinkShell7,
        CwLinkShell8,

        /// <summary>
        /// 用于计数的END枚举
        /// </summary>
        End,
        
        // 下面这俩必须得放在End后面，这俩不能计数
        
        /// <summary>
        /// 自定义情感动作
        /// </summary>
        MotionCustom,
        
        /// <summary>
        /// 给别人发送的悄悄话
        /// </summary>
        TellToOther
        
    }

}
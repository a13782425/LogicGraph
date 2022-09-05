using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 事件委托
    /// </summary>
    /// <param name="args"></param>
    /// <returns>返回false可中断当前消息</returns>
    public delegate bool MessageEventHandler(object args);
    /// <summary>
    /// 逻辑图内部消息通知
    /// </summary>
    public static class LogicMessage
    {
        private static Dictionary<int, MessageDto> _allMsg = new Dictionary<int, MessageDto>();

        public static void AddListener(int messageId, MessageEventHandler callback)
        {
            if (!_allMsg.ContainsKey(messageId))
            {
                _allMsg[messageId] = new MessageDto(messageId);
            }
            _allMsg[messageId].Add(callback);
        }


        private class MessageDto
        {
            public readonly int Id;
            public HashSet<MessageEventHandler> MessageEvent;
            public MessageDto(int id)
            {
                this.Id = id;
            }

            public void Add(MessageEventHandler messageEvent)
            {

            }
            public void OnEvent(object args)
            {
                //MessageEvent?.Invoke(args);
            }
        }
    }
}

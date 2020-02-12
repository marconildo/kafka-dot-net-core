using System;

namespace Poc.Core.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TopicsAttribute : Attribute
    {
        public string[] Topics { get; }

        public TopicsAttribute(params string[] topics)
        {
            Topics = topics;
        }
    }
}

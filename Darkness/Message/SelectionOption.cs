using System;

namespace Darkness
{
    public class SelectionOption
    {
        private Func<SelectionNode> nextNodeFactory;

        public string Text { get; private set; }
        public string Description { get; private set; }
        public bool Enabled { get; private set; }
        public SelectionNode NextNode { get; private set; }
        public object Value { get; private set; }

        public SelectionOption(
            string text,
            string description,
            bool enabled,
            SelectionNode nextNode)
            : this(text, description, enabled, nextNode, null)
        {
        }

        public SelectionOption(
            string text,
            string description,
            bool enabled,
            SelectionNode nextNode,
            object value)
        {
            Text = text;
            Description = description;
            Enabled = enabled;
            NextNode = nextNode;
            Value = value;
        }

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        public SelectionNode GetNextNode()
        {
            return nextNodeFactory == null
                ? NextNode
                : nextNodeFactory();
        }

        public static SelectionOption DynamicNode(
            string text,
            string description,
            bool enabled,
            Func<SelectionNode> nextNodeFactory)
        {
            if (nextNodeFactory == null)
            {
                throw new ArgumentNullException("nextNodeFactory");
            }

            SelectionOption option = new SelectionOption(
                text,
                description,
                enabled,
                null);
            option.nextNodeFactory = nextNodeFactory;
            return option;
        }
    }
}

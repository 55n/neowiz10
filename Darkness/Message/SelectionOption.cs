using System;

namespace Darkness
{
    public class SelectionOption
    {
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
    }
}

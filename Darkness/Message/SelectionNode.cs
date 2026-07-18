using System;
using System.Collections.Generic;

namespace Darkness
{
    public class SelectionNode
    {
        public string Id { get; private set; }
        public string Description { get; private set; }
        public List<SelectionOption> Options { get; private set; }
        public SelectionNode Parent { get; private set; }

        public SelectionNode(
            string id,
            string description,
            List<SelectionOption> options,
            SelectionNode parent)
        {
            Id = id;
            Description = description;
            Options = options;
            Parent = parent;
        }
    }
}

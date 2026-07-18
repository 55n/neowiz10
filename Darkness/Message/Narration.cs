using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Narration
    {
        private static Narration _instance = null;

        public static Narration Instance
        {
            get
            {
                if (_instance == null) _instance = new Narration();
                return _instance;
            }
        }

        private List<string> _lines;
        public IReadOnlyList<string> Lines { get { return _lines; } }

        public Narration()
        {
            _lines = new List<string>();
        }

        public void SetLine(string text)
        {
            _lines.Clear();
            _lines.Add(text);
        }

        public void SetLines(string[] lines)
        {
            _lines.Clear();
            _lines.AddRange(lines);
        }

        public void AppendLines(string[] lines)
        {
            _lines.AddRange(lines);
        }

        public void Clear()
        {
            _lines.Clear();
        }
    }
}

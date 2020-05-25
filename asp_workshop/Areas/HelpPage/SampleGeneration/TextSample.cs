using global::System;

namespace asp_workshop.Areas.HelpPage
{
    /// <summary>
    /// This represents a preformatted text sample on the help page. There's a display template named TextSample associated with this class.
    /// </summary>
    public class TextSample
    {
        private string _text;

        public TextSample(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException("text");
            }

            Text = text;
        }

        public string Text
        {
            get
            {
                return _text;
            }

            private set
            {
                _text = value;
            }
        }

        public override bool Equals(object obj)
        {
            bool EqualsRet = default;
            EqualsRet = false;
            TextSample other = obj as TextSample;
            if (!(other is null))
            {
                EqualsRet = (Text ?? "") == (other.Text ?? "");
            }

            return EqualsRet;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
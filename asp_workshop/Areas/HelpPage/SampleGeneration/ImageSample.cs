using global::System;

namespace asp_workshop.Areas.HelpPage
{
    /// <summary>
    /// This represents an image sample on the help page. There's a display template named ImageSample associated with this class.
    /// </summary>
    public class ImageSample
    {
        private string _src;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSample"/> class.
        /// </summary>
        /// <param name="src">The URL of an image.</param>
        public ImageSample(string src)
        {
            if (src is null)
            {
                throw new ArgumentNullException("src");
            }

            Src = src;
        }

        public string Src
        {
            get
            {
                return _src;
            }

            private set
            {
                _src = value;
            }
        }

        public override bool Equals(object obj)
        {
            ImageSample other = obj as ImageSample;
            return other is object && (Src ?? "") == (other.Src ?? "");
        }

        public override int GetHashCode()
        {
            return Src.GetHashCode();
        }

        public override string ToString()
        {
            return Src;
        }
    }
}
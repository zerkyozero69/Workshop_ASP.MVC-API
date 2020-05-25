using global::System;

namespace asp_workshop.Areas.HelpPage
{
    /// <summary>
    /// This represents an invalid sample on the help page. There's a display template named InvalidSample associated with this class.
    /// </summary>
    public class InvalidSample
    {
        private string _errorMessage;

        public InvalidSample(string errorMessage)
        {
            if (errorMessage is null)
            {
                throw new ArgumentNullException("errorMessage");
            }

            ErrorMessage = errorMessage;
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            private set
            {
                _errorMessage = value;
            }
        }

        public override bool Equals(object obj)
        {
            InvalidSample other = obj as InvalidSample;
            return other is object && (ErrorMessage ?? "") == (other.ErrorMessage ?? "");
        }

        public override int GetHashCode()
        {
            return ErrorMessage.GetHashCode();
        }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
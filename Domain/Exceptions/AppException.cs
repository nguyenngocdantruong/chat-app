namespace ChatApp.Domain.Exceptions
{
    public abstract class AppException : Exception
    {
        public virtual int StatusCode { get; }
        public virtual string ErrorCode { get; } = string.Empty;
        private List<string> Errors { get; set; } = new List<string>();

        protected AppException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public void AddError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                Errors.Add(error);
            }
        }

        public IReadOnlyList<string> GetErrors()
        {
            return Errors.AsReadOnly();
        }

        public void ClearErrors()
        {
            Errors.Clear();
        }
    }
}

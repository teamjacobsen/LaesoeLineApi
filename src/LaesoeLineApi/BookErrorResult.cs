namespace LaesoeLineApi
{
    public class BookErrorResult
    {
        private readonly BookStatus _status;

        public int StatusCode => (int)_status;
        public string Status => _status.ToString();

        public BookErrorResult(BookStatus status)
        {
            _status = status;
        }
    }
}

namespace LaesoeLineApi.Features
{
    public class BookErrorResult
    {
        private readonly ApiStatus _status;

        public int StatusCode => (int)_status;
        public string Status => _status.ToString();

        public BookErrorResult(ApiStatus status)
        {
            _status = status;
        }
    }
}

namespace Domain.Models
{
    public class Result<T>
    {
        public T Data { get; set; }
        public IEnumerable<Error> Errors { get; set; } = Enumerable.Empty<Error>();

        public Result() { }

        public Result(T data)
        {
            Data = data;
        }

        public Result(IEnumerable<Error> errors)
        {
            Errors = errors;
        }
    }
}

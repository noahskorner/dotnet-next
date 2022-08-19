namespace Domain.Models
{
    public class Result<T>
    {
        public T Data { get; set; }
        public IEnumerable<Error> Errors { get; set; }

        public Result()
        {
            Errors = new List<Error>();
        }

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

namespace Api.Models
{
    public class Result<T>
    {
        public T? Data { get; set; }
        public List<Error> Errors { get; set; }

        public Result()
        {
            Errors = new List<Error>();
        }

        public Result(T data)
        {
            Data = data;
        }

        public Result(List<Error> errors)
        {
            Errors = errors;
        }
    }
}

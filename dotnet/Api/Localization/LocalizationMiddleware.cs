using System.Globalization;

namespace Api.Localization
{
    public class LocalizationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cultureCodes = context.Request
                .GetTypedHeaders().AcceptLanguage?
                .OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToList() ?? new List<string>();
            if (cultureCodes.Any())
            {
                var culture = GetCulture(cultureCodes.First());
                if (culture != null)
                {
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }

            await next(context);
        }

        public static CultureInfo? GetCulture(string cultureCode)
        {
            try
            {
                var culture = new CultureInfo(cultureCode);

                return culture;
            }
            catch
            {
                return null;
            }
        }
    }
}

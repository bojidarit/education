using System.Diagnostics;

namespace LanguageFeatures.Models;

public class MyAsyncMethods
{
    public static async IAsyncEnumerable<long?> GetPageLengths(
        List<string> output,
        params string[] urls)
    {
        HttpClient client = new();

        foreach (var url in urls)
        {
            output.Add($"Started request for '{url}'");
            Stopwatch stopwatch = new();
            stopwatch.Start();

            var httpTask = await client.GetAsync($"http://{url}");

            stopwatch.Stop();
            output.Add($"Completed request for '{url}' in {GetElapsedTime(stopwatch.Elapsed)}");

            yield return httpTask.Content.Headers.ContentLength;
        }
    }

    public static async Task<IEnumerable<long?>> GetPageLengthsAtOnce(
        List<string> output,
        params string[] urls)
    {
        var result = new List<long?>();
        HttpClient client = new();

        if (output == null)
        {
            return result;
        }

        foreach (var url in urls)
        {
            output.Add($"Started request for '{url}'");
            Stopwatch stopwatch = new();
            stopwatch.Start();

            var httpTask = await client.GetAsync($"http://{url}");
            result.Add(httpTask.Content.Headers.ContentLength);

            stopwatch.Stop();
            output.Add($"Completed request for '{url}' in {GetElapsedTime(stopwatch.Elapsed)}");
        }

        return result;
    }

    public async static Task<long?> GetPageLength()
    {
        HttpClient client = new();

        var httpTask = await client.GetAsync("http://apress.com");

        // Thread.Sleep(3000);
        // await Task.Delay(3000);

        return httpTask.Content.Headers.ContentLength;
    }

    public static Task<long?> GetPageLengthDirectTask()
    {
        HttpClient client = new();

        var httpTask = client.GetAsync("http://apress.com");

        return httpTask.ContinueWith((Task<HttpResponseMessage> antecedent) =>
        {
            return antecedent.Result.Content.Headers.ContentLength;
        });
    }

    private static string GetElapsedTime(TimeSpan ts) =>
        String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
}
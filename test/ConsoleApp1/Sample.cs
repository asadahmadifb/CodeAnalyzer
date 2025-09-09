using System.Threading.Tasks;

namespace CodeAnalyzer.Test;

internal static class Sample
{
    public static string NormalMethod() // این باید خطا بدهد
    {
        return "Normal";
    }

    public static string ZCorrectMethod() // این باید OK باشد
    {
        return "Correct";
    }

    public static async Task<string> ZAsyncMethodAsync() // این باید خطا بدهد
    {
        return await Task.FromResult("Async").ConfigureAwait(false);
    }

    public static async void ZMethodAsync()
    {
        await Task.Delay(1).ConfigureAwait(false);
    }

    public static async Task<string> GetDataAsync()
    {
        try
        {
            await Task.Delay(1).ConfigureAwait(false);

        }
        catch (Exception)
        {

            throw;
        }
        return "Hello World";
    }

    public static async Task ZProcessData()
    {
        // Missing await - Task<string> converted to string
        string result =await GetDataAsync().ConfigureAwait(false); // ❌ EPC18 - This will be "System.Threading.Tasks.Task`1[System.String]"

        // In string interpolation
        Console.WriteLine($"Result: {GetDataAsync()}"); // ❌ EPC18 - Prints task type, not result

        // In concatenation
        string message = "Data: " + GetDataAsync(); // ❌ EPC18 - Concatenates with task type
    }


}

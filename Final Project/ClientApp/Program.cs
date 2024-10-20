using SharedLib.Models;
using System.Text.Json;

class Program
{
    static HttpClient httpClient = new HttpClient();
    static async Task Main(string[] args)
    {
        await RunAsync();
    }
    static async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine("Card Management");
            Console.WriteLine("1. Get card by id");
            Console.WriteLine("2. Delete card by id");
            Console.WriteLine("3. Exit");
            Console.WriteLine("Select option from 1-3: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await GetCardByIdAsync();
                    break;
                case "2":
                    await DeleteCardByIdAsync();
                    break;
                case "3":
                    Console.WriteLine("Exiting program");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select from 1 to 3");
                    break;
            }
        }
    }

    private static async Task GetCardByIdAsync()
    {
        Console.WriteLine("Enter the card id: ");
        if (!int.TryParse(Console.ReadLine(), out var cardId))
        {
            Console.WriteLine("Invalid input");
            return;
        }

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"http://example:5252/cards/{cardId}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response received successfully.");

            var card = JsonSerializer.Deserialize<Card>(responseBody);
            if (card != null)
            {
                Console.WriteLine($"\nCard Details:\n- Owner: {card.OwnerName} {card.OwnerSurname}\n- Balance: {card.Balance:C}\n- CVV: {card.CVV}");
            }
            else
            {
                Console.WriteLine("The card could not be found");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }

    private static async Task DeleteCardByIdAsync()
    {
        Console.WriteLine("Enter card id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var cardId))
        {
            Console.WriteLine("Invalid input");
            return;
        }

        try
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://example:5252/cards/{cardId}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Card id {cardId} deleted successfully. Response: {responseBody}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }

}
using SharedLib.Models;
using System.Net.Http;
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
            Console.WriteLine("3. Add new card");
            Console.WriteLine("4. Update existing card");
            Console.WriteLine("5. Exit");
            Console.WriteLine("Select option from 1-5: ");

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
                    await AddCardAsync();
                    break;
                case "4":
                    await UpdateCardAsync();
                    break;
                case "5":
                    Console.WriteLine("Exiting program");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select from 1 to 5");
                    break;
            }
        }
    }

    private static async Task GetCardByIdAsync()
    {
        Console.Write("Enter the card id: ");
        if (!int.TryParse(Console.ReadLine(), out var cardId))
        {
            Console.WriteLine("Invalid input");
            return;
        }

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:7070/cards/{cardId}");
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
        Console.Write("Enter card id to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var cardId))
        {
            Console.WriteLine("Invalid input");
            return;
        }

        try
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"http://localhost:7070/cards/{cardId}");
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"Card id {cardId} deleted successfully.");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }
    }

    private static async Task AddCardAsync()
    {
        Console.Write("Enter card owner name: ");
        string ownerName = Console.ReadLine();

        Console.Write("Enter card owner surname: ");
        string ownerSurname = Console.ReadLine();

        Console.Write("Enter card CVV: ");
        if (!int.TryParse(Console.ReadLine(), out var cvv))
        {
            Console.WriteLine("Invalid CVV input");
            return;
        }

        Console.Write("Enter card balance: ");
        if (!decimal.TryParse(Console.ReadLine(), out var balance))
        {
            Console.WriteLine("Invalid balance input");
            return;
        }

        var newCard = new Card
        {
            OwnerName = ownerName,
            OwnerSurname = ownerSurname,
            CVV = cvv,
            Balance = balance
        };

        var jsonContent = JsonSerializer.Serialize(newCard);
        var contentString = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

try
{
    HttpResponseMessage response = await httpClient.PostAsync("http://localhost:7070/cards", contentString);
    response.EnsureSuccessStatusCode();

    Console.WriteLine("Card added successfully.");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Request error: {ex.Message}");
}
    }

    private static async Task UpdateCardAsync()
{
    Console.Write("Enter the card id to update: ");
    if (!int.TryParse(Console.ReadLine(), out var cardId))
    {
        Console.WriteLine("Invalid input");
        return;
    }

    Console.Write("Enter new card owner name: ");
    string ownerName = Console.ReadLine();

    Console.Write("Enter new card owner surname: ");
    string ownerSurname = Console.ReadLine();

    Console.Write("Enter new card CVV: ");
    if (!int.TryParse(Console.ReadLine(), out var cvv))
    {
        Console.WriteLine("Invalid CVV input");
        return;
    }

    Console.Write("Enter new card balance: ");
    if (!decimal.TryParse(Console.ReadLine(), out var balance))
    {
        Console.WriteLine("Invalid balance input");
        return;
    }

    var updatedCard = new Card
    {
        OwnerName = ownerName,
        OwnerSurname = ownerSurname,
        CVV = cvv,
        Balance = balance
    };

    var jsonContent = JsonSerializer.Serialize(updatedCard);
    var contentString = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

    try
    {
        HttpResponseMessage response = await httpClient.PutAsync($"http://localhost:7070/cards/{cardId}", contentString);
        response.EnsureSuccessStatusCode();

        Console.WriteLine($"Card id {cardId} updated successfully.");
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Request error: {ex.Message}");
    }
}
}
using SharedLib.Models;
using SharedLib.Repositories;
using System.Net;
using System.Text.Json;

const int port = 7070;
HttpListener httpListener = new HttpListener();

httpListener.Prefixes.Add($"http://*:{port}/");

httpListener.Start();

Console.WriteLine($"HTTP Server started on 'http://localhost:{port}/'");

while (true)
{
    var repository = new CardsEntityFrameworkRepository();
    var context = await httpListener.GetContextAsync();
    
    var reader = new StreamReader(context.Request.InputStream);
    var writer = new StreamWriter(context.Response.OutputStream);

    var requestBodyStr = await reader.ReadToEndAsync();

    System.Console.WriteLine(context.Request.RawUrl);
    context.Response.ContentType = "application/json";

    var normalizedRawUrl = context.Request.RawUrl?.Trim().ToLower() ?? "/";
    var rawUrlItems = normalizedRawUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);
    var methodType = context.Request.HttpMethod;

    if (rawUrlItems.Length == 0)
    {
        await writer.WriteLineAsync("Welcome");
        await writer.FlushAsync();

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
    else if (rawUrlItems[0] == "cards")
    {
        switch (methodType)
        {
            case "GET":
                if (rawUrlItems.Length == 2)
                {
                    if (int.TryParse(rawUrlItems[1], out int cardId))
                    {
                        await writer.WriteLineAsync(await repository.ShowCardAsync(cardId));
                        await writer.FlushAsync();
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    await writer.WriteLineAsync(await repository.ShowAllCardsAsync());
                    await writer.FlushAsync();
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                break;
            case "POST":
                var sentCard = JsonSerializer.Deserialize<Card>(requestBodyStr);
                if (sentCard == null) break;
                await repository.InsertCardAsync(sentCard);
                await writer.WriteLineAsync("Card added successfully!");
                await writer.FlushAsync();
                break;
            case "PUT":
                if (rawUrlItems.Length == 2)
                {
                    if (int.TryParse(rawUrlItems[1], out int cardId))
                    {
                        var updatedCard = JsonSerializer.Deserialize<Card> (requestBodyStr);
                        if(updatedCard == null) break;
                        await repository.UpdateCardAsync(updatedCard, cardId);
                        await writer.WriteLineAsync("Card added successfully!");
                        await writer.FlushAsync();
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                break;
            case "DELETE":
                if (rawUrlItems.Length == 2)
                {
                    if (int.TryParse(rawUrlItems[1], out int cardId))
                    {
                        await repository.RemoveCardAsync(cardId);
                        await writer.FlushAsync();
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                break;
        }
        if (context.Request.HttpMethod == HttpMethod.Get.Method)
        {

            if (rawUrlItems.Length == 2)
            {
                if (int.TryParse(rawUrlItems[1], out int cardId))
                {

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
    }

    context.Response.Close();
}
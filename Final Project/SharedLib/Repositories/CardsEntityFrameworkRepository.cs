using SharedLib.Data;
using SharedLib.Models;

namespace SharedLib.Repositories;

public class CardsEntityFrameworkRepository
{
    public async Task<string> ShowAllCardsAsync()
    {
        return await Task.Run(() =>
        {
            var dbcontext = new MyDbContext();
            new MyDbContext().Database.EnsureCreated();

            var allCards = dbcontext.Cards.ToList();
            var str = "";
            foreach (var card in allCards)
            {
                str += card;
            }

            return $"All Cards: {str}";
        });
    }

    public async Task<string> ShowCardAsync(int id)
    {
        return await Task.Run(() =>
        {
            var dbcontext = new MyDbContext();
            new MyDbContext().Database.EnsureCreated();

            var card = dbcontext.Cards
            .ToList()
            .Where(x => x.Id == id);

            return $"{card}";
        });
    }

    public async Task RemoveCardAsync(int id)
    {
        await Task.Run(() =>
        {
            var dbcontext = new MyDbContext();
            new MyDbContext().Database.EnsureCreated();

            var cardToDelete = new Card() { Id = id };
            dbcontext.Cards.Remove(cardToDelete);
            dbcontext.SaveChanges();
        });
    }

    public async Task InsertCardAsync(Card card)
    {
        await Task.Run(() =>
        {
            var dbContext = new MyDbContext();
            new MyDbContext().Database.EnsureCreated();

            dbContext.Cards.Add(card);
            dbContext.SaveChanges();
        });
    }

    public async Task UpdateCardAsync(Card card, int id)
    {
        await Task.Run(() =>
        {
            var dbcontext = new MyDbContext();
            new MyDbContext().Database.EnsureCreated();

            var foundCard = dbcontext.Cards.FirstOrDefault(prod => prod.Id == id);
            if (foundCard == null) return false;

            foundCard.OwnerName = card.OwnerName;
            foundCard.OwnerSurname = card.OwnerSurname;
            foundCard.CVV = card.CVV;
            foundCard.Balance = card.Balance;
            dbcontext.Cards.Update(foundCard);
            dbcontext.SaveChanges();
            return true;
        });
    }
}

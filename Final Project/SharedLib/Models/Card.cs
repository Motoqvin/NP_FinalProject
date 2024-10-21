using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SharedLib.Models;

public class Card
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? OwnerName { get; set; }
    public string? OwnerSurname { get; set; }
    public int CVV { get; set; }
    public decimal Balance { get; set; }

    public override string ToString()
    {
        return $"{Id}: {OwnerName} {OwnerSurname}: \nCard CVV : {CVV},\nCard Balance: {Balance}\n\n";
    }
}

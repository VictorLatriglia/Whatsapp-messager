using System.Text;
using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.Utils;
public class PlatanizatorService
{
    public static string PlatanizeOutgoings(List<UserOutgoing> outgoings)
    {
        StringBuilder builder = new StringBuilder("Hola!, este es el resumen de tus gastos hasta el momento:\n");
        var categories = outgoings
            .Select(x => x.Tag).Select(x => x.OutgoingsCategory)
            .Distinct().ToList();
        foreach (var category in categories)
        {
            var relevantOutgoings = outgoings.Where(x => x.Tag.OutgoingsCategoryId.Equals(category.Id)).ToList();
            var tags = relevantOutgoings
                .Select(x => x.Tag)
                .Distinct().ToList();
            builder.Append($"Categoria {category.Name}: {relevantOutgoings.Sum(x => x.Ammount).ToString("C")}\n");
            foreach (var tag in tags)
            {
                var summaryData = relevantOutgoings.Where(x => x.TagId.Equals(tag.Id)).Sum(x => x.Ammount);
                builder.Append($"* {tag.Name}: {summaryData.ToString("C")}\n");
            }
        }
        builder.Append($"En total has gastado {outgoings.Sum(x => x.Ammount).ToString("C")}");
        return builder.ToString();
    }
}
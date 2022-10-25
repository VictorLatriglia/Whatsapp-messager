using System.Diagnostics.CodeAnalysis;
using System.Text;
using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.Utils;
[ExcludeFromCodeCoverage]
public class PlatanizatorService
{
    protected PlatanizatorService()
    {

    }
    public static string PlatanizeOutgoings(List<MoneyMovement> outgoings)
    {
        StringBuilder builder = new StringBuilder("Hola! este es el resumen de tus gastos hasta el momento:\n");
        var categories = outgoings
            .Select(x => x.Category)
            .Distinct().ToList();
        foreach (var category in categories)
        {
            var relevantOutgoings = outgoings.Where(x => x.CategoryId.Equals(category.Id)).ToList();
            var tags = relevantOutgoings
                .Select(x => x.Tag)
                .Distinct().ToList();
            builder.Append($"Categoria {category.Name}: {relevantOutgoings.Sum(x => x.Ammount).ToString("C")}\n");
            foreach (var tag in tags)
            {
                var summaryData = relevantOutgoings.Where(x => x.Tag.Equals(tag)).Sum(x => x.Ammount);
                builder.Append($"* {tag}: {summaryData.ToString("C")}\n");
            }
        }
        builder.Append($"En total has gastado {outgoings.Sum(x => x.Ammount).ToString("C")}");
        return builder.ToString();
    }
}
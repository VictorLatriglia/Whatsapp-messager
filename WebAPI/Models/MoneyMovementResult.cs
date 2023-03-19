using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.Models
{
    public class MoneyMovementResult
    {
        public MoneyMovementResult(List<MoneyMovement> movements, IList<OutgoingsCategory> categories)
        {
            Data = BuildData(movements);
            Categories = categories;
        }

        public IList<OutgoingsCategory> Categories { get; set; }
        public List<ResultCategoryInformation> Data { get; set; }

        private List<ResultCategoryInformation> BuildData(List<MoneyMovement> _movements)
        {
            var categories = _movements.Select(x => x.Category).Distinct().ToList();
            var output = new List<ResultCategoryInformation>();
            foreach (var category in categories)
            {
                var relevantOutgoings = _movements.Where(x => x.CategoryId.Equals(category.Id)).ToList();
                output.Add(new ResultCategoryInformation(category.Name, category.Id, relevantOutgoings));
            }
            return output;
        }

        public class ResultCategoryInformation
        {
            public string Category { get; set; }
            public Guid CategoryId { get; set; }
            public List<ResultMovementInformation> Movements { get; set; }
            public ResultCategoryInformation(string category, Guid categoryId, List<MoneyMovement> movements)
            {
                Category = category;
                CategoryId = categoryId;
                Movements = BuildMovements(movements);
            }
            private List<ResultMovementInformation> BuildMovements(List<MoneyMovement> movements)
            {
                List<ResultMovementInformation> output = new List<ResultMovementInformation>();
                output.AddRange(movements.Select(x => new ResultMovementInformation(x.Ammount, x.Tag, x.CreatedOn, x.Id)));
                return output;
            }
        }
        public class ResultMovementInformation
        {
            public double Ammount { get; set; }
            public string Tag { get; set; }
            public DateTime CreatedOn { get; set; }
            public Guid Id { get; set; }

            public ResultMovementInformation(double ammount, string tag, DateTime createdOn, Guid id)
            {
                Ammount = ammount;
                Tag = tag;
                CreatedOn = createdOn;
                Id = id;
            }
        }
    }

}

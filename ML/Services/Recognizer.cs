namespace ML.Services
{
    public interface IRecognizer
    {
        string[] GetCategories(byte[] file);
    }

    public class Recognizer : IRecognizer
    {
        private static readonly string[] _categories = new[] {
            "Bottle", "Glass", "Cocktail", "Main course", "Starter", "Side dish", "Salad"
        };

        public string[] GetCategories(byte[] file)
        {
            TestHelper.Delay();

            return new[] {
                _categories.GetRandomOne(),
                _categories.GetRandomOne()
            };
        }
    }
}

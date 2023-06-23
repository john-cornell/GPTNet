using GPTNet.Models;

namespace GPTNet.Tests
{
    [TestFixture]
    internal class WhenFactoryCalled
    {
        [Test]
        public void ForOpenAPI_ByGeneric_ShouldReturnCorrect()
        {
            GPTApiFactory factory = new GPTApiFactory();
            var api = factory.GetApi<GPTAPIOpenAI>("", "");
            Assert.IsTrue(api.ApiType == GPTApiType.OpenAI);
        }

        [Test]
        public void ForOpenAPI_ByType_ShouldReturnCorrect()
        {
            GPTApiFactory factory = new GPTApiFactory();
            var api = factory.GetApi(GPTApiType.OpenAI, "", "");
            Assert.IsTrue(api.ApiType == GPTApiType.OpenAI);
        }

        [Test]
        public void ForForHuggingface_ShouldReturnCorrect()
        {
            GPTApiFactory factory = new GPTApiFactory();
            var api = factory.GetApi<GPTApiHuggingface>("", "");
            Assert.IsTrue(api.ApiType == GPTApiType.Huggingface);
        }

        [Test]
        public void ForForHuggingface_ByType_ShouldReturnCorrect()
        {
            GPTApiFactory factory = new GPTApiFactory();
            var api = factory.GetApi(GPTApiType.Huggingface, "", "");
            Assert.IsTrue(api.ApiType == GPTApiType.Huggingface);
        }
    }
}

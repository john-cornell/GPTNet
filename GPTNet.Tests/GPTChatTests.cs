using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Tests
{
    [TestFixture]
    public class GPTChatTests
    {
        //Not guaranteed to work all the time, as OpenAI may throw error, just a test harness, hence commented out attribute
        [Test]
        public async Task ChatEntryTestHarness()
        {
            bool error = false;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            GPTChat chat = new GPTChat(configuration["OpenApiKey"], configuration["Model"]); ;

            chat.OnError += (sender, e) =>
            {
                error = true;
                Console.WriteLine(e.ErrorMessage);
            };

            var response = await chat.Chat("Hello!");
            await Task.Delay(1000);
            response = await chat.Chat("Don't mind me, this is just a test ...");
            await Task.Delay(1000);
            response = await chat.Chat("Testing Testing 123 ...");
            await Task.Delay(1000);
            response = await chat.Chat("Test completed. Please carry on.");

            Assert.IsFalse(error);
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPTNet.Models;
using GPTNet.Conversations;
using NUnit.Framework.Internal;

namespace GPTNet.Tests
{
    [TestFixture]
    public class GPTChatTests
    {
        //Not guaranteed to work all the time, as chat may throw error, just a test harness, hence commented out attribute
        //Also, if you don't have keys for these all, they won't work
        [Test]
        public async Task HuggingfaceChatEntryTestHarness()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            GPTChat chat = new GPTChat(configuration["HFApiKey"], configuration["Model"], GPTApiType.Huggingface);

            await RunTest(chat);
        }

        [Test]
        public async Task OpenAIChatEntryTestHarness()
        {
            bool error = false;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            GPTChat chat = new GPTChat(configuration["ApiKey"], configuration["35Model"]);

            await RunTest(chat);
        }

        [Test]
        public async Task AnthropicChatEntryTestHarness()
        {
            bool error = false;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            GPTApiProperties properties = GPTApiProperties.Create<GPTApiAnthropic>(
                configuration["AApiKey"], configuration["AModel"], configuration["AModelVersion"]);

            GPTChat chat = new GPTChat(properties);

            await RunTest(chat);
        }

        private static async Task RunTest(GPTChat chat)
        {
            bool error = false;

            chat.OnError += (sender, e) =>
            {
                error = true;
                Console.WriteLine(e.ErrorMessage);
            };

            var response = await chat.Chat("Hello!");
            Console.WriteLine(response.Response);
            await Task.Delay(100);
            response = await chat.Chat("Don't mind me, this is just a test ...");
            Console.WriteLine(response.Response);
            await Task.Delay(100);
            response = await chat.Chat("Testing Testing 123 ...");
            Console.WriteLine(response.Response);
            await Task.Delay(100);
            response = await chat.Chat("Test completed. Please carry on.");
            await Task.Delay(100);
            Console.WriteLine(response.Response);

            Assert.IsFalse(error);
        }
    }
}

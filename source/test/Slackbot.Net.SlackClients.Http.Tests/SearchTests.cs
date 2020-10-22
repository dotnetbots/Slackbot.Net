using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CronBackgroundServices.Tests
{
    public class SearchTests : Setup
    {
        public SearchTests(ITestOutputHelper helper) : base(helper)
        {
        }

        [Fact]
        public async Task SearchForLinksWorks()
        {
            var response = await SearchClient.SearchMessagesAsync("http");
            Assert.True(response.Ok);
            Assert.NotEmpty(response.Messages.Matches);
        }
    }
}
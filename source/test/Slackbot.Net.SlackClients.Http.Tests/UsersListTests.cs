using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Slackbot.Net.Tests
{
    public class UsersListTests : Setup
    {
        public UsersListTests(ITestOutputHelper helper) : base(helper)
        {
        }
        
        [Fact]
        public async Task UsersListWorks()
        {
            var response = await SlackClient.UsersList();
            Assert.True(response.Ok);
        }
        
        [Fact]
        public async Task FindsAdmins()
        {
            var response = await SlackClient.UsersList();
            var adminFound = response.Members.Any(u => u.Is_Admin);
            Assert.True(adminFound);
        }
        
        [Fact]
        public async Task FindsBots()
        {
            var response = await SlackClient.UsersList();
            var botFound = response.Members.Where(u => u.Is_Bot);
            var bots = botFound.Select(c => c.Name);
            var names = string.Join(" ", bots);
            Assert.Contains("oldbot", bots);
        }
    }
}
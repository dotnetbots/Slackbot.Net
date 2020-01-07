using System;
using System.Threading;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Monitoring;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.Connections.Monitoring
{
    public class DateTimeKeeperTests
    {
        [Theory, AutoMoqData]
        private void should_throw_exception_if_get_time_is_called_before_being_set(DateTimeKeeper dateTimeKeeper)
        {
            Assert.Throws<DateTimeKeeper.DateTimeNotSetException>(() => dateTimeKeeper.TimeSinceDateTime());
            dateTimeKeeper.HasDateTime().ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        private void should_return_time_since_if_datetime_has_been_set(DateTimeKeeper dateTimeKeeper)
        {
            // given

            // when
            dateTimeKeeper.SetDateTimeToNow();

            // then
            dateTimeKeeper.HasDateTime().ShouldBeTrue();

            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            dateTimeKeeper.TimeSinceDateTime().ShouldBeGreaterThan(TimeSpan.FromMilliseconds(100));
            dateTimeKeeper.TimeSinceDateTime().ShouldBeLessThan(TimeSpan.FromMilliseconds(600));
        }
    }
}
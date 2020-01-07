using System;
using System.Linq.Expressions;
using ExpectedObjects;
using Moq;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.TestExtensions
{
    public static class Looks
    {
        public static T Like<T>(T obj)
        {
            var expected = obj.ToExpectedObject();
            return It.Is<T>(t => expected.Equals(t));
        }

        public static T Like<T>(Expression<Func<T>> initializer) where T : class
        {
            return It.Is<T>(t => ShouldMatch(initializer, t));
        }


        private static bool ShouldMatch<T>(Expression<Func<T>> initializer, T o) where T : class
        {
            try
            {
                o.ShouldLookLike(initializer);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
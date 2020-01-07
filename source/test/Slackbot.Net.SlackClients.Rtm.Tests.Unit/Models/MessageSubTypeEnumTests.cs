﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Models;
using Xunit;
using MessageSubType = Slackbot.Net.SlackClients.Rtm.Models.MessageSubType;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.Models
{
    public class MessageSubTypeEnumTests
    {
        [Fact]
        public void should_have_same_number_of_enum_values_as_internal_enum_type()
        {
            // given 
            int numberOfInternalEnumNames = Enum.GetNames(typeof(Rtm.Connections.Sockets.Messages.Inbound.MessageSubType)).Length;
            int numberOfPublicEnumNames = Enum.GetNames(typeof(MessageSubType)).Length;

            // then
            numberOfPublicEnumNames.ShouldBe(numberOfInternalEnumNames);
        }

        [Fact]
        public void should_have_same_message_types_as_internal_models()
        {
            // given 
            var internalEnumNames = Enum.GetNames(typeof(Rtm.Connections.Sockets.Messages.Inbound.MessageSubType))
                .Select(x => x.Replace("_", string.Empty))
                .Select(x => x.ToLower());
            var publicEnumNames = Enum.GetNames(typeof(MessageSubType))
                .Select(x => x.ToLower());

            // then
            publicEnumNames.All(x => internalEnumNames.Contains(x)).ShouldBeTrue();
        }

        [Fact]
        public void should_have_same_message_type_values_as_internal_models()
        {
            // given 
            var internalEnumValues = GetEnumValues<Rtm.Connections.Sockets.Messages.Inbound.MessageSubType>();
            var publicEnumValues = GetEnumValues<MessageSubType>();

            // then
            foreach (var publicEnumName in publicEnumValues.Keys)
            {
                publicEnumValues[publicEnumName].ShouldBe(internalEnumValues[publicEnumName]);
            }
        }

        private static Dictionary<string, int> GetEnumValues<T>() where T : struct, IConvertible
        {
            var internalEnumNames = Enum.GetNames(typeof(T));

            var vals = new Dictionary<string, int>();
            foreach (var enumName in internalEnumNames)
            {
                T thingy;
                Enum.TryParse(enumName, out thingy);

                var name = enumName
                    .Replace("_", string.Empty)
                    .ToLower();

                vals.Add(name, thingy.ToInt32(new NumberFormatInfo()));
            }
            return vals;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Models;
using File = Slackbot.Net.SlackClients.Rtm.Models.File;

namespace Slackbot.Net.SlackClients.Rtm.Extensions
{
    internal static class FileExtensions
    {
        public static IEnumerable<File> ToSlackFiles(this IEnumerable<Connections.Sockets.Messages.Inbound.File> file)
        {
            if (file == null)
            {
                return Enumerable.Empty<File>();
            }

            return file.Select(ToSlackFile);
        }

        private static File ToSlackFile(this Connections.Sockets.Messages.Inbound.File file)
        {
            if (file == null)
                return null;

            return new File(
                file.Id,
                file.Created,
                file.Timestamp,
                file.Name,
                file.Title,
                file.Mimetype,
                file.FileType,
                file.PrettyType,
                file.User,
                file.Editable,
                file.Size,
                file.Mode,
                file.IsExternal,
                file.ExternalType,
                file.IsPublic,
                file.PublicUrlShared,
                file.DisplayAsBot,
                file.Username,
                CreateUri(file.UrlPrivate),
                CreateUri(file.UrlPrivateDownload),
                file.ImageExifRotation,
                file.OriginalWidth,
                file.OriginalHeight,
                CreateUri(file.DeanimateGif),
                CreateUri(file.Permalink),
                CreateUri(file.PermalinkPublic),
                new Thumbnail(
                    CreateUri(file.Thumb64),
                    CreateUri(file.Thumb80),
                    CreateUri(file.Thumb360),
                    file.Thumb360Width,
                    file.Thumb360Height,
                    CreateUri(file.Thumb160),
                    CreateUri(file.Thumb360Gif)
                )
            );
        }

        private static Uri CreateUri(string url)
        {
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)
                ? uri
                : null;
        }
    }
}
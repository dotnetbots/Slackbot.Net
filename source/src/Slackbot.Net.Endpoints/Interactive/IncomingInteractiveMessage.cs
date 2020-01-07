namespace Slackbot.Net.Endpoints.Interactive
{
    public class IncomingInteractiveMessage
    {
        /*
        {
             "type": "block_actions",
             "team": { },
             "user": { },
             "api_app_id": "",
             "token": "<some-token>",
             "container": { },
             "trigger_id": "713398624851.14411458112.99f8cc4028ed75e7735f53ab29819474",
             "channel": { },
             "message": { },
             "response_url": "https://hooks.slack.com/actions/T0EC3DG3A/724838343024/jv9EZCm4TfOXFxzVPDJ5xiRL",
             "actions": [
                 {
                     "action_id": "storsdag-rsvp-yes",
                     "block_id": "JMyh6",
                     "text": {
                         "type": "plain_text",
                         "text": "Deltar! :beer:",
                         "emoji": true
                     },
                     "value": "deltar",
                     "style": "primary",
                     "type": "button",
                     "action_ts": "1565699506.599969"
                 }
             ]
        }

        */
        public string Response_Url
        {
            get;
            set;
        }

        public ValueBlock[] Actions
        {
            get;
            set;
        }

        internal bool HasValues() => !string.IsNullOrEmpty(Response_Url) && Actions != null;
    }

    public class ValueBlock
    {
        public string value { get; set; }
        public string block_id { get; set; }
    }
}
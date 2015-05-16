using System;

namespace DetectionServer.Server.Udp
{
    public class TextEventArgs
    {
        public String Text { get; private set; }

        public static Builder Create()
        {
            return new Builder();
        }

        public class Builder
        {
            private readonly TextEventArgs eventArgs;

            public Builder()
            {
                eventArgs = new TextEventArgs();
            }

            public Builder WithText(String text)
            {
                eventArgs.Text = text;
                return this;
            }

            public TextEventArgs Build()
            {
                return eventArgs;
            }
        }
    }
}
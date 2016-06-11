using Lemon.Transform;
using MongoDB.Bson;
using System;
using System.Linq;

namespace Demo001
{
    public class UserActiviesTransformAction : TransformAction
    {
        private class UserActivity
        {
            public string Id { get { return ComputeHash(); } }

            public string UserId { get; set; }

            public string Action { get; set; }

            public DateTime Time {get; set;}

            public string EffectOn { get; set; }

            private string ComputeHash()
            {
                var signature = string.Format("{0}-{1}-{2:MM/dd/yy H:mm:ss}-{3}", UserId, Action.ToLower(), Time, EffectOn);

                var md5Hash = Utils.ComputeStringMD5Hash(signature);

                return md5Hash;
            }
        }

        private static BsonDataRow ConvertToDataRow(UserActivity activity)
        {
            var document = new BsonDocument
            {
                {"_id", activity.Id },
                {"userId", activity.UserId },
                {"action", activity.Action },
                {"effectOn", activity.EffectOn},
                {"time", activity.Time}
            };

            return new BsonDataRow(document);
        }

        private BsonDataRow GetAskQuestionActivity(BsonDataRow row)
        {
            var threadId = row.GetValue("_id").AsString;

            var authorId = row.GetValue("authorId").AsString;

            var createdOn = row.GetValue("createdOn").ToUniversalTime();

            var activity = new UserActivity
            {
                UserId = authorId,
                Action = "Ask",
                Time = createdOn,
                EffectOn = threadId
            };

            return ConvertToDataRow(activity);
        }

        public override void Input(BsonDataRow row)
        {
            var aksActivity = GetAskQuestionActivity(row);

            Output(aksActivity);

            var messages = row.GetValue("messages").AsBsonArray;

            foreach (BsonDocument message in messages.Skip(1))
            {
                var messageId = message.GetElement("id").Value.AsString;

                var authorId = message.GetElement("authorId").Value.AsString;

                var repliedOn = DateTime.Parse(message.GetElement("createdOn").Value.AsString);

                var replyActivity = new UserActivity
                {
                    UserId = authorId,
                    Action = "Reply",
                    EffectOn = row.GetValue("_id").AsString,
                    Time = repliedOn
                };

                Output(ConvertToDataRow(replyActivity));
            }
        }
    }
}

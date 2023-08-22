namespace Web_BusinessLayer.Enum
{
    public static class Tables
    {
        public enum yesNo
        {
            No = 0,
            Yes = 1
        }

        public enum alertTarget
        {
            Post = 1,
            Friend = 2,
            Like = 3,
            Group = 4,
            Member = 5,
            Comment = 6,
            Follower = 7
        }

        public enum gender
        {
            Undefined = 0,
            Male = 1,
            Female = 2,
            Other = 3
        }

        public enum entityType
        {
            Person = 1,
            Company = 2,
            Group = 3
        }

        public enum eventType
        {
            Post = 1,
            Comment = 2,
            Message = 3,
            Album = 4,
            Photo = 5,
            Location = 6,
            Profile = 7,
            Cover = 8,
            Video = 9
        }

        public enum externalType
        {
            None = 0,
            Link = 1,
            Video = 2,
            Url = 3
        }

        public enum likeType
        {
            Like = 1,
            Love = 2,
            Angry = 3
        }

        public enum blobContainer
        {
            Album = 1,
            Profile = 2,
            Cover = 3
        }

        public enum visibility
        {
            Public = 1,
            Friends = 2,
            Private = 3,
            Anonymous = 7
        }

        public enum language
        {
            Spanish = 1,
            English = 2,
        }

        public enum country
        {
            Argentina = 10
        }
    }
}

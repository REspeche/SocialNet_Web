namespace Web_BusinessLayer.Enum
{
    public class Rules
    {
        public enum pageUser
        {
            Wall = 1,
            Photo = 2,
            Video = 3,
            Friend = 4,
            Profile = 5,
            Group = 6,
            Member = 7,
            Follower = 8,
            Company = 9
        }
        public enum actionMessage
        {
            None = 0,
            RedirectToLogin = 1,
            RedirectToPublicWall = 2,
            RedirectToWall = 3
        }

        public enum imageSize
        {
            Photo = 5,
            PhotoFull = 50,
            Profile = 7,
            Cover = 8
        }

        public enum typePicture
        {
            Profile = 1,
            Company = 2,
            Group = 3,
            Cover = 4
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;

namespace Web_BusinessLayer.Classes
{
    public class ResponsePost : ResponseMessage
    {
        public IEnumerable<PostItem> items { get; set; }
    }

    public class ResponseOnePost : ResponseMessage
    {
        public PostItem item { get; set; }
    }

    public class PostItem
    {
        public long postId { get; set; }
        public string postText { get; set; }
        public string postGuid { get; set; }
        public long? userId { get; set; }
        public string userName { get; set; }
        public string userNameDes { get; set; }
        public string profileGuid { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public int? postCreated { get; set; }
        public int typeEvent { get; set; }
        public string userProfileCode { get; set; }
        public string userProfileCodeDes { get; set; }
        public int? postExtType { get; set; }
        public string postExtLink { get; set; }
        public int? countComments { get; set; }
        public int? countLikes { get; set; }
        public bool? isLike { get; set; }
        public CommentItem[] lastComments { get; set; }
        public PhotoItem[] lastPhotos { get; set; }
        public bool? isSystem { get; set; }

        public PostItem()
        {
            this.lastComments = new CommentItem[] { };
            this.lastPhotos = new PhotoItem[] { };
        }

        public PostItem(long postId)
        {
            this.postId = postId;
            this.lastComments = new CommentItem[] { };
            this.lastPhotos = new PhotoItem[] { };
        }
    }

    public class CommentItem
    {
        public long postId { get; set; }
        public string postText { get; set; }
        public long? userId { get; set; }
        public string userName { get; set; }
        public int? postCreated { get; set; }
        public string userProfileCode { get; set; }
        public int? countLikes { get; set; }
        public bool? isLike { get; set; }
        public int? visibility { get; set; }
        public int? typeEntity { get; set; }
        public int? containerProfile { get; set; }

        private string _profileGuid = null;
        public string profileGuid
        {
            get
            {
                return (_profileGuid == null || _profileGuid.Equals("")) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), typeEntity) : _profileGuid;
            }
            set
            {
                _profileGuid = value;
            }
        }
    }

    public class CommentItems
    {
        public CommentItem[] comments { get; set; }

        public CommentItems()
        {
            this.comments = new CommentItem[] { };
        }
    }

    public class PhotoItem
    {
        public string postGuid { get; set; }
    }

    public class PhotoItems
    {
        public PhotoItem[] photos { get; set; }

        public PhotoItems()
        {
            this.photos = new PhotoItem[] { };
        }
    }
}

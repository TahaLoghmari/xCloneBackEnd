namespace TwitterCloneBackEnd.Models
{
    public enum NotificationType
    {
        Like = 1, // for {ReceiverUserId} : {UserId} Liked (Your {CommentId} On {UserId}/ Your {PostId} )
        Reply = 2, // for {ReceiverUserId} : {UserId} Replied to Your {CommentId}
        Follow = 3, // for {ReceiverUserId} : {UserId} Followed You 
        Retweet = 4, // for {ReceiverUserId} : {UserId} Retweeted Your {PostId}
        Mention = 5, // for {ReceiverUserId} : {UserId} Mentioned You In (PostId/CommentId)
    }
}
namespace TwitterCloneBackEnd.Models
{
    public class Like 
    {
        public int Id { get ; set ; } 
        public int UserId { get ; set ; } 
        public int? PostId { get ; set ; } 
        public int? CommentId { get ; set ; } 

        public User Creator { get ; set ; } = null! ;
        public Post? RelatedPost { get ; set ; } 
        public Comment? RelatedComment { get ; set ; } 
    }
}
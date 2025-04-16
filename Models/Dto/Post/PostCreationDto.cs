namespace TwitterCloneBackEnd.Models.Dto
{
    public class PostCreationDto 
    {
        public string Content { get ; set ; } = string.Empty ; 
        public string? MediaUploadPath { get ; set ; }
        public string? MediaUploadType { get ; set ; }
    }
}
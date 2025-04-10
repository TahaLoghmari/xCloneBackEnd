using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TwitterCloneBackEnd.Models.Data
{
    public class TwitterDbContext : DbContext
    {
        public TwitterDbContext( DbContextOptions<TwitterDbContext> options ) : base(options){}
        public DbSet<User> Users { get ; set ; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.PasswordHash)
                    .IsRequired();

                entity.Property(u => u.DisplayName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(u => u.CreatedAt)
                    .IsRequired();

                entity.Property(u => u.FollowerCount)
                    .HasDefaultValue(0);

                entity.Property(u => u.FollowingCount)
                    .HasDefaultValue(0);

                entity.Property(u => u.ExternalProvider)
                    .HasMaxLength(256);
                entity.Property(u => u.ExternalId)
                    .HasMaxLength(256);
            });
            modelBuilder.Entity<Follow>(entity =>
            {      
                entity.HasKey(f => f.Id);

                entity.HasOne(f => f.Follower)
                    .WithMany(u => u.Following)
                    .HasForeignKey(f => f.FollowerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Following)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(f => f.FollowingId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Post> ( entity => {
                entity.HasKey( p => p.Id ) ;
                entity.Property( p => p.Content )
                .IsRequired()
                .HasMaxLength(300);

                entity.HasOne( p => p.Creator )
                .WithMany( u => u.Posts )
                .HasForeignKey( p => p.UserId )
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.OriginalPost)
                .WithMany(p => p.Retweets)    
                .HasForeignKey(p => p.OriginalPostId)
                .IsRequired(false)                 
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Comment> ( entity => {
                entity.HasKey( c => c.Id );

                entity.Property( c => c.Content )
                .IsRequired()
                .HasMaxLength(300);

                entity.HasOne( c => c.Creator )
                .WithMany( u => u.Comments )
                .HasForeignKey( c => c.UserId )
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne( c => c.RelatedPost )
                .WithMany( p => p.Comments )
                .HasForeignKey( c => c.PostId )
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne( c => c.ParentComment )
                .WithMany( c => c.Replies )
                .HasForeignKey( c => c.ParentCommentId )
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Like> ( entity => {
                entity.HasKey( l => l.Id );

                entity.HasOne( l => l.Creator )
                .WithMany( u => u.Likes )
                .HasForeignKey( l => l.UserId )
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne( l => l.RelatedPost)
                .WithMany( p => p.Likes )
                .HasForeignKey( l => l.PostId )
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne( l => l.RelatedComment )
                .WithMany( c => c.Likes)
                .HasForeignKey( l => l.CommentId )
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Notification> ( entity => {
                entity.HasKey( n => n.Id );

                entity.HasOne(n => n.Creator)
                .WithMany(u => u.SentNotifications) 
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(n => n.Receiver)
                .WithMany(u => u.ReceivedNotifications) 
                .HasForeignKey(n => n.ReceiverUserId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne( n => n.RelatedPost )
                .WithMany( p => p.Notifications)
                .HasForeignKey( n => n.PostId )
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne( n => n.RelatedComment )
                .WithMany( c => c.Notifications )
                .HasForeignKey( n => n.CommentId )
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne( n => n.RelatedFollow )
                .WithMany ( f => f.Notifications )
                .HasForeignKey( n => n.FollowId )
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
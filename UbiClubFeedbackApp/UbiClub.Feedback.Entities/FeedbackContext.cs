﻿using Microsoft.EntityFrameworkCore;

namespace UbiClub.Feedback.Entities
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext(DbContextOptions<FeedbackContext> options)
        : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("UbiClub");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<SessionFeedback> SessionFeedbacks { get; set; }
    }
}
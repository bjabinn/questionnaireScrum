using System.Data.Entity;

namespace QuestionScrumVS2015
{
    class ScrumDbContext : DbContext
    {
        public ScrumDbContext() : base("ScrumContext")
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //one-to-many 
            modelBuilder.Entity<Answer>()
                        .HasRequired<Question>(s => s.Question)
                        .WithMany(s => s.AllAnswers)
                        .HasForeignKey(s => s.IdQuestion);

        }


    }
}
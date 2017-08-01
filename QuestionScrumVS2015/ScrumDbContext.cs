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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=scrumQuestions.db");
        //}


    }
}
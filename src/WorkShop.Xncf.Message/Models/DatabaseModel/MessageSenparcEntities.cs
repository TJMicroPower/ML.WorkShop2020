using Microsoft.EntityFrameworkCore;
using Senparc.Ncf.Database;
using Senparc.Ncf.Core.Models;
using Senparc.Ncf.XncfBase.Database;

namespace WorkShop.Xncf.Message.Models.DatabaseModel
{
    [MultipleMigrationDbContext(MultipleDatabaseType.SQLite, typeof(Register))]
    public class MessageSenparcEntities : XncfDatabaseDbContext
    {
        public MessageSenparcEntities(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        
        //DOT REMOVE OR MODIFY THIS LINE 请勿移除或修改本行 - Entities Point
        //ex. public DbSet<Color> Colors { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<MessageDetail> MessageDetails { get; set; }

        //如无特殊需需要，OnModelCreating 方法可以不用写，已经在 Register 中要求注册
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //}
    }
}

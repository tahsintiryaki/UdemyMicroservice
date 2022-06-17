using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FreeCourse.Services.Order.Infrastructure
{
    public  class OrderDbContext:DbContext
    {
        public const string DEFAULT_SCHEME = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext>options) : base(options)
        {

        }

        public DbSet<Domain.OrderAggregate.Order> Order { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Order", DEFAULT_SCHEME);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItem", DEFAULT_SCHEME);

            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(t => t.Price).HasColumnType("decimal(18,2)");

            //Address class'ı owned type olarak taanımlandı. Address diye ayrı bir tablo olmayacak. Address class'ı içerisinde bulunan fieldlar order tablosu içersinde yer alacak!
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(t => t.Address).WithOwner();

            base.OnModelCreating(modelBuilder);
        }
    }
}

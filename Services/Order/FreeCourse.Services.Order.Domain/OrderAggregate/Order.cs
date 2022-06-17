using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    //DDD uygularken EF Core 'un bize sağladığı kolaylıklar
    //EF Core Features
    // 1- OwnedType
    // 2- Shadow Property
    // 3- Backing Field


    public class Order:Entity,IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private  set; }

        //Backing Field?
        private readonly List<OrderItem> _orderItem; //EF core bu field'ı otomatik olarak dolduruyor.

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItem;

        public Order()
        {

        }
        public Order( string buyerId, Address address)
        {
            _orderItem = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address= address;
        }

        public void AddOrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            var existProduct = _orderItem.Any(t => t.ProductId == productId);
            if(!existProduct)
            {
                //OrderItem'ın set'i private olarak işaretlendi ve  dışarıdan erişim olamayacağı için orderItem ekleme işlemi için bir metod yazıldı ve metotta Ctor üzerinden model dolduruldu. 
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);

                _orderItem.Add(newOrderItem);
            }
        }
        public decimal GetTotalPrice => _orderItem.Sum(t => t.Price);          
    
    }
}

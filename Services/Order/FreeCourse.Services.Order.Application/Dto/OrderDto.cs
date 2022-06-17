
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get;  set; }
        public AddressDto Address { get;  set; }
        public string BuyerId { get;  set; }

        public List<OrderItemDto> OrdetItems { get; set; }

    }
}

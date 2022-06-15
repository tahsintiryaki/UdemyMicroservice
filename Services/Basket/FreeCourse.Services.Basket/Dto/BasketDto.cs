using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Services.Basket.Dto
{
    public class BasketDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }

        public List<BasketItemDto> BasketItems { get; set; }

        public decimal TotalPrice
        {
            get => BasketItems.Sum(t => t.Price * t.Quantity);
        }
    }
}

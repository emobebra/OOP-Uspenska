using System;

namespace lab7v3
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }

        public override string ToString()
        {
            return $"замовлення №{Id}: дата {OrderDate:dd.MM.yyyy}, сума {TotalAmount:F2}, клієнт {CustomerId}";
        }
    }
}
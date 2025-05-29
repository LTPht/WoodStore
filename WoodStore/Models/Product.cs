namespace WoodStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WoodType { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int PurchaseCount { get; set; }  
        public bool IsDiscounted { get; set; }  
        public decimal DiscountPercent { get; set; }
        public decimal Weight { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal DiscountedPrice
        {
            get
            {
                if (IsDiscounted && DiscountPercent > 0)
                    return Price * (1 - DiscountPercent / 100m);
                else
                    return Price;
            }
        }
    }
}

namespace WoodStore.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}

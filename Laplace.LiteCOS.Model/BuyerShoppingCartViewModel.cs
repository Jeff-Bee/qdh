namespace Laplace.LiteCOS.Model
{
    public class BuyerShoppingCartViewModel : BuyerShoppingCart
    {
        public string SellerName { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }

        public string ProductUnit { get; set; }
    }
}
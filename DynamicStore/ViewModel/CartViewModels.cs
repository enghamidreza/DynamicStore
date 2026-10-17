namespace DynamicStore.ViewModel
{
    public class CartViewModels
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(x => x.SubTotal);
        public int TotalItemsCount => Items.Sum(x => x.Quantity);
    }
}

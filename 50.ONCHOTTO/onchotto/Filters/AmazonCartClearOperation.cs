using OnChotto.Models.Amazon;

namespace OnChotto.Filters
{
    public class AmazonCartClearOperation : AmazonOperationBase
    {
        public AmazonCartClearOperation()
        {
            base.ParameterDictionary.Add("Operation", "CartClear");
        }

        public void ClearCart(Cart cart)
        {
            base.ParameterDictionary.Add($"CartId", cart.CartId);
            base.ParameterDictionary.Add($"HMAC", cart.HMAC);
        }
    }
}
using OnChotto.Models.Amazon;

namespace  OnChotto.Filters
{
    public class  AmazonCartGetOperation : AmazonOperationBase
    {
        public AmazonCartGetOperation()
        {
            base.ParameterDictionary.Add("Operation", "CartGet");
        }

        public void GetCart(Cart cart)
        {
            base.ParameterDictionary.Add("CartId", cart.CartId);
            base.ParameterDictionary.Add("HMAC", cart.HMAC);
        }
    }
}
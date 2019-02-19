using GeekBurger.Products.Contract;

namespace GeekBurger_HTML.Models
{
    public class ProductToGetFormat : ProductToGet
    {
        public string PriceFormat => string.Format("{0:C}", Price);
    }
}
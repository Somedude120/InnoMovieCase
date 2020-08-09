using System.Collections.Generic;

namespace Models
{
    public class OnlineUserModel
    {
        public int id;
        public string name;
        public ProductModel viewingProduct;
        public List<ProductModel> suggestedList;
    }
}
using System.Collections.Generic;

namespace Models
{
    public class UserModel
    {
        public int id;
        public string name;
        public List<ProductModel> productListViewed = new List<ProductModel>();
        public List<ProductModel> productListBought = new List<ProductModel>();
    }
}

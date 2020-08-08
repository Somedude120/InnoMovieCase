using System.Linq;
using System.Collections.Generic;
using Models;

namespace Handlers
{
    public class MostBoughtCalculator
    {
        private MostBoughtModel mostBoughtProduct { get; set; }
        public ProductModel boughtProduct { get; set; }
        public List<ProductModel> productList { get; set; }
        public List<UserModel> userList { get; set; }
        public List<ProductModel> boughtList { get; set; }
        public List<ProductModel> fullBoughtList { get; set; }
        public List<MostBoughtModel> mostBoughtList { get; set; }
        public List<MostBoughtModel> AddingMostBoughtProducts()
        {
            //Look at all the bought products from userlist
            for (int count = 0; count < userList.Count; count++)
            {

                //Find all products with matching IDs for expanding the product list
                for (int i = 0; i < productList.Count; i++)
                {
                    //Add products to a list
                    boughtList = userList[count].productListBought.FindAll(x => x.id == i);
                    //Copy that list into another list that will increase when buying more and more of that product
                    fullBoughtList.AddRange(boughtList);
                }
            }
            // Compare the elements inside the fullBoughtList and find the most bought
            var query = fullBoughtList.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();

            foreach (var productElement in query)
            {
                mostBoughtProduct = new MostBoughtModel()
                {
                    product = productElement.Element,
                    duplicatesOfThisProduct = productElement.Counter
                };
                mostBoughtList.Add(mostBoughtProduct);
                // System.Console.WriteLine($"Bought item: {productElement.Element.id} How many times: {productElement.Counter}");
            }
            return mostBoughtList;
        }
    }
}

//Træk alle værdier ud fra boughtlist for alle bruger i en anden productlist, derefter sammenlign dem alle med hinanden
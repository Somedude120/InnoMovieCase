using System.Collections.Generic;
using Models;
//Mostly, data from users and movies come from a database set in the backend.
//This will mimic the setup of getting data from a database
//THIS IS NOT HOW YOU GET STUFF FROM A DATABASE, Do not do this if you work with database!

namespace Handlers
{
    public class FakeDatabaseLogic
    {
        public List<UserModel> userList { get; set; }
        public List<ProductModel> productList { get; set; }
        private ProductModel product;
        private UserModel user;
        public List<UserModel> getUserDataFromDatabase()
        {
            string[] userLines = System.IO.File.ReadAllLines("Movie product data/Users.txt");
            //id, name, viewed (products seperated by ;), purchased (products seperated by ;)
            foreach (var perUser in userLines)
            {
                string[] perUserSplit = perUser.Split(',');
                string[] userViewedList = perUserSplit[2].Split(';');
                string[] userBoughtList = perUserSplit[3].Split(';');

                user = new UserModel
                {
                    id = int.Parse(perUserSplit[0]),
                    name = perUserSplit[1],
                };
                //Compare id with productlist.id
                foreach (var perViewedProduct in userViewedList)
                {

                    for (int count = 0; count < productList.Count; count++)
                    {
                        //If product id is equal to the viewed product add to list
                        if (productList[count].id == int.Parse(perViewedProduct))
                        {
                            user.productListViewed.Add(productList[count]);
                        }
                    }
                }
                //Add bought product to userList
                foreach (var perBoughtProduct in userBoughtList)
                {

                    for (int count = 0; count < productList.Count; count++)
                    {
                        //If product id is equal to the bought product add to list
                        if (productList[count].id == int.Parse(perBoughtProduct))
                        {
                            user.productListBought.Add(productList[count]);
                        }
                    }
                }
                userList.Add(user);

            }
            // System.Console.WriteLine($"User id: {userList[0].id}");
            // foreach (var viewed in userList[0].productListViewed){
            //     System.Console.WriteLine($"User viewed: {viewed.id}");
            // }
            return userList;
        }
        public List<ProductModel> getMovieDataFromDatabase()
        {
            //Simple get the strings from txt file and set in a simple string array
            string[] productLines = System.IO.File.ReadAllLines("Movie product data/Products.txt");
            //Check each element in the array
            foreach (var perProduct in productLines)
            {
                //split each element into id,name,year... etc.
                string[] perProductSplit = perProduct.Split(',');

                //Add elements to the product
                product = new ProductModel
                {
                    id = int.Parse(perProductSplit[0]),
                    name = perProductSplit[1],
                    year = int.Parse(perProductSplit[2]),
                    keywordOne = perProductSplit[3],
                    keywordTwo = perProductSplit[4],
                    keywordThree = perProductSplit[5],
                    keywordFour = perProductSplit[6],
                    keywordFive = perProductSplit[7],
                    rating = double.Parse(perProductSplit[8]),
                    price = double.Parse(perProductSplit[9])
                };
                //Add product to productList List
                productList.Add(product);
            }
            //Return a productList formatted and ready to be used
            return productList;
        }
    }
}
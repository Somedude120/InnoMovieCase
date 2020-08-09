using Models;
using System.Linq;
using System.Collections.Generic;
using System;

//Assuming that this is on some session on a website and not a static file
//Then this is not how you do it. This is all static. The program will be mostly static

namespace Handlers
{
    public class SessionHandler
    {
        private OnlineUserModel onlineUser { get; set; }
        public List<OnlineUserModel> onlineUserList { get; set; }
        public UserSessionModel userSession { get; set; }
        public List<ProductModel> productList { get; set; }
        //public ProductModel viewedProduct { get; set; }
        public List<UserSessionModel> userSessionList { get; set; }
        public List<ProductModel> suggestedList { get; set; }
        public List<UserModel> userList { get; set; }
        public List<UserSessionModel> sessionHandler()
        {
            //This function creates a list of session into id and productId
            //Simply get from textfile
            string[] sessionLines = System.IO.File.ReadAllLines("Movie product data/CurrentUserSession.txt");
            foreach (var session in sessionLines)
            {
                //Split into userid and productid
                string[] perUserSessionSplit = session.Split(',');
                userSession = new UserSessionModel()
                {
                    userId = int.Parse(perUserSessionSplit[0]),
                    productId = int.Parse(perUserSessionSplit[1])
                };
                //Add to list
                userSessionList.Add(userSession);
            }
            return userSessionList;
        }
        public List<OnlineUserModel> lookUpUserOnline()
        {
            //Create own query and join the lists together with ID as the common property
            var query = from session in userSessionList
                        join user in userList on session.userId equals user.id into uid
                        join product in productList on session.productId equals product.id into pid
                        select new { userId = session.userId, name = uid, product = pid };

            foreach (var item in query)
            {
                foreach (var uid in item.name)
                {
                    foreach (var pid in item.product)
                    {

                        onlineUser = new OnlineUserModel()
                        {
                            id = item.userId,
                            name = uid.name,
                            viewingProduct = pid
                        };
                        onlineUserList.Add(onlineUser);
                        // System.Console.WriteLine($"Users online: {uid.name} \t ID: {item.userId} \t Product: {pid.id}");
                    }
                }
            }
            return onlineUserList;
        }

        //Highly coupled with onlineUser now
        public List<ProductModel> recommendAMovieByGenre(OnlineUserModel onlineUser)
        {
            ProductModel viewedProduct = onlineUser.viewingProduct;
            //Look at genre user is viewing, 
            //Make sure the recommended is not the same as the one he's viewing
            suggestedList = productList.FindAll(x => x.keywordOne ==
            viewedProduct.keywordOne && x.id != viewedProduct.id ||
            x.keywordTwo == viewedProduct.keywordTwo && x.id != viewedProduct.id ||
            x.keywordThree == viewedProduct.keywordThree && x.id != viewedProduct.id ||
            x.keywordFour == viewedProduct.keywordFour && x.id != viewedProduct.id ||
            x.keywordFive == viewedProduct.keywordFive && x.id != viewedProduct.id);

            List<ProductModel> betterSuggestList = new List<ProductModel>();
            //Max 3 suggestions
            for (int i = 0; i < 4; i++)
            {
                betterSuggestList.Add(suggestedList[i]);
            }
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            onlineUser.suggestedProduct = betterSuggestList[randomNumber];

            // foreach (var item in suggestedList)
            // {
            //     System.Console.WriteLine($"Suggested: {item.id}");
            // }

            return betterSuggestList;
        }
    }
}
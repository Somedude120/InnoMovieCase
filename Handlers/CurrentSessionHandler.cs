using Models;
using System.Linq;
using System.Collections.Generic;

//Assuming that this is on some session on a website and not a static file
//Then this is not how you do it. This is all static. The program will be mostly static

namespace Handlers
{
    public class CurrentSessionHandler
    {
        public UserSessionModel userSession { get; set; }
        public List<ProductModel> productList { get; set; }
        public ProductModel viewedProduct { get; set; }
        public List<UserSessionModel> userSessionList {get;set;}
        public List<ProductModel> suggestedList { get; set; }
        public List<UserSessionModel> sessionHandler()
        {
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
        public ProductModel lookupViewedMovie()
        {
            //Look through productlist
            for (int i = 0; i < productList.Count; i++)
            {
                //Check if id to that list exists
                if (userSession.productId == productList[i].id)
                {
                    return productList[i];
                }
            }
            return null;
        }
        public List<ProductModel> recommendAMovieByGenre()
        {
            //Look at genre he's viewing, 
            //Make sure the recommended is not the same as the one he's viewing
            suggestedList = productList.FindAll(x => x.keywordOne ==
            viewedProduct.keywordOne && x.id != viewedProduct.id ||
            x.keywordTwo == viewedProduct.keywordTwo && x.id != viewedProduct.id ||
            x.keywordThree == viewedProduct.keywordThree && x.id != viewedProduct.id ||
            x.keywordFour == viewedProduct.keywordFour && x.id != viewedProduct.id ||
            x.keywordFive == viewedProduct.keywordFive && x.id != viewedProduct.id);

            return suggestedList;
        }
    }
}
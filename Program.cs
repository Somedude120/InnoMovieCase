using System.Linq;
using System;
using System.Collections.Generic;
using Handlers;
using Models;
using System.Diagnostics;

namespace InnoMovieCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Movie website.");
            System.Console.WriteLine($"The data and the sessions are all statically created");

            //Init
            FakeDatabaseLogic db = new FakeDatabaseLogic();
            MostBoughtAndRatedHandler mBCalc = new MostBoughtAndRatedHandler();
            SessionHandler sessionHandler = new SessionHandler();

            List<UserModel> userList = new List<UserModel>();
            List<ProductModel> productList = new List<ProductModel>();
            List<ProductModel> boughtList = new List<ProductModel>();
            List<ProductModel> fullBoughtList = new List<ProductModel>();
            List<MostBoughtModel> mostBoughtList = new List<MostBoughtModel>();
            List<UserSessionModel> userSessionList = new List<UserSessionModel>();
            List<ProductModel> suggestedList = new List<ProductModel>();
            List<OnlineUserModel> onlineUserList = new List<OnlineUserModel>();



            //Set db lists
            db.userList = userList;
            db.productList = productList;

            //Get data, products first, user needs productList
            db.getMovieDataFromDatabase();
            db.getUserDataFromDatabase();

            //Calculate lists
            mBCalc.boughtList = boughtList;
            mBCalc.fullBoughtList = fullBoughtList;
            mBCalc.userList = userList;
            mBCalc.productList = productList;
            mBCalc.mostBoughtList = mostBoughtList;
            mBCalc.addingMostBoughtProducts();

            //Sessions list
            sessionHandler.userSessionList = userSessionList;
            sessionHandler.suggestedList = suggestedList;
            sessionHandler.userList = userList;
            sessionHandler.productList = productList;
            sessionHandler.onlineUserList = onlineUserList;

            //Session handling
            userSessionList = sessionHandler.sessionHandler(); //Creates the sessions
            onlineUserList = sessionHandler.lookUpUserOnline();
            //sessionHandler = onlineUserList[0].viewingProduct;
            // suggestedList = sessionHandler.recommendAMovieByGenre();

            //Output
            System.Console.WriteLine($"The three most bought films are:");
            foreach (var mostBoughtProduct in mostBoughtList)
            {
                System.Console.WriteLine($"{mostBoughtProduct.product.name}");
            }
            System.Console.WriteLine($"The top rated films are:");
            List<ProductModel> sortedRatingList = mBCalc.findingTopRatedMovies();
            for (int i = 0; i < 3; i++)
            {
                System.Console.WriteLine($"{sortedRatingList[i].name} \t rating: {sortedRatingList[i].rating}");
            }

            System.Console.WriteLine($"Users Online and browsing:");
            foreach (var item in onlineUserList)
            {
                System.Console.WriteLine($"Name: {item.name} \tID: {item.id} \tBrowsing: {item.viewingProduct.name} with categories: {item.viewingProduct.keywordOne}, {item.viewingProduct.keywordTwo}, {item.viewingProduct.keywordThree} \t");
            }
            System.Console.WriteLine($"Suggestions:");
            foreach (var item in onlineUserList)
            {
                suggestedList = sessionHandler.recommendAMovieByGenre(item);
                System.Console.WriteLine($"Name: {item.name} \tSuggested: {item.suggestedProduct.name} do to category: {item.suggestedProduct.keywordOne}");
            }

            bool exit = false;
            bool subExit = false;

            //The main system
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine($"There are 40 movies to look up and 5 users you can see.");
                Console.WriteLine($"Please enter \"movie\" without quotations to go into movie section to see data on a movie.");
                Console.WriteLine($"Please enter \"user\" without quotations to go into user section to see userdetails");
                Console.WriteLine($"Please enter exit to close the program");
                Console.WriteLine("--------------------------------------------------------------");
                
                string choice = Console.ReadLine();
                string subChoice;
                int idChoice;

                Console.WriteLine("Three Top Rated movies are:");
                for (int i = 0; i < 3; i++)
                {
                    System.Console.WriteLine($"id: {sortedRatingList[i].id} \t name: {sortedRatingList[i].name} \t rating: {sortedRatingList[i].rating}");
                }
                Console.WriteLine();
                System.Console.WriteLine($"The three most bought films are:");
                foreach (var mostBoughtProduct in mostBoughtList)
                {
                    System.Console.WriteLine($"{mostBoughtProduct.product.name} with id {mostBoughtProduct.product.id}");
                }
                Console.WriteLine("--------------------------------------------------------------");

                switch (choice.ToLower())
                {
                    //MOVIE part
                    case "movie": //Movie menu
                        Console.WriteLine($"You can show all movies full detailed by entering all or you can get a specific movie by entering ID, or go back to first menu if you type back");
                        subChoice = Console.ReadLine();
                        if (subChoice.ToLower() == "back") //Back to Front menu
                        {
                            Console.Clear();
                            Console.WriteLine("Welcome back to the front!");
                            break;
                        }
                        else if (subChoice.ToLower() == "exit") //Exit program
                        {
                            Console.Clear();
                            subExit = true;
                            exit = true;
                            Console.WriteLine("Exitting!");
                            break;
                        }
                        else if (subChoice.ToLower() == "id") //Find movie by ID
                        {
                            while (!subExit)
                            {
                                Console.WriteLine("Input ID or enter back to go back to front menu");
                                Console.WriteLine($"There are total {productList.Count} movies");

                                string input = Console.ReadLine();

                                Int32.TryParse(input, out idChoice);
                                if (input.ToLower() == "back")
                                {
                                    subExit = true;
                                }
                                else if (input.ToLower() == "exit") //Exit program
                                {
                                    Console.Clear();
                                    subExit = true;
                                    exit = true;
                                    Console.WriteLine("Exitting!");
                                    break;
                                }
                                else if (!Int32.TryParse(input, out idChoice))
                                {
                                    Console.WriteLine("Not a valid ID try again");
                                }
                                else
                                {
                                    try
                                    {
                                        idChoice = idChoice - 1;
                                        Console.WriteLine($"ID: {productList[idChoice].id}\t" +
                                            $"Name: {productList[idChoice].name}\t" +
                                            $"Year: {productList[idChoice].year}\t" +
                                            $"Genre: {productList[idChoice].keywordOne} {productList[idChoice].keywordTwo} {productList[idChoice].keywordThree} {productList[idChoice].keywordFour} {productList[idChoice].keywordFive}\t" +
                                            $"Price: {productList[idChoice].price}\t" +
                                            $"Rating: {productList[idChoice].rating}");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("No such movie id exists");
                                    }
                                }
                            }
                        }
                        else if (subChoice.ToLower() == "all") //Find all movies
                        {
                            Console.Clear();
                            foreach (var item in productList)
                            {
                                Console.WriteLine("----------------------------------------");
                                Console.WriteLine($"ID: {item.id}\tName: {item.name}\tYear: " +
                                    $"{item.year}\tGenres: {item.keywordOne} {item.keywordTwo}" +
                                    $" {item.keywordThree} {item.keywordFour} {item.keywordFive}\t" +
                                    $"Price: {item.price}\tRating: {item.rating}");
                            }
                        }
                        else
                            Console.WriteLine($"Please try again");
                        break;

                    //USER part
                    case "user":
                        while (!subExit)
                        {
                            Console.WriteLine($"You can show all users full detailed by entering all or " +
                                            $"you can get a specific user by entering ID, or " +
                                            $"enter activeusers to see who's browsing what right now, or " +
                                            $"go back to first menu if you type back");
                            subChoice = Console.ReadLine();
                            if (subChoice.ToLower() == "back") //back to front menu
                            {
                                Console.Clear();
                                subExit = true;
                                Console.WriteLine("Welcome back to the front!");
                                break;
                            }
                            else if (subChoice.ToLower() == "exit") //Exit program
                            {
                                Console.Clear();
                                subExit = true;
                                exit = true;
                                Console.WriteLine("Exitting!");
                                break;
                            }
                            else if (subChoice.ToLower() == "id") //Find by id
                            {
                                while (!subExit)
                                {
                                    Console.WriteLine("Input ID or enter back to go back to front menu");
                                    Console.WriteLine($"There are total {userList.Count} users");
                                    string input = Console.ReadLine();

                                    Int32.TryParse(input, out idChoice);
                                    if (input.ToLower() == "back")
                                    {
                                        subExit = true;
                                    }
                                    else if (input.ToLower() == "exit") //Exit program
                                    {
                                        Console.Clear();
                                        subExit = true;
                                        exit = true;
                                        Console.WriteLine("Exitting!");
                                        break;
                                    }
                                    else if (!Int32.TryParse(input, out idChoice))
                                    {
                                        Console.WriteLine("Not a valid ID try again");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            idChoice = idChoice - 1;
                                            Console.WriteLine($"ID: {userList[idChoice].id}\t" +
                                                $"Name: {userList[idChoice].name}\n" +
                                                $"Movies Bought:");
                                            foreach (var item in userList[idChoice].productListBought)
                                            {
                                                Console.WriteLine($"ID: {item.id}\t" +
                                                    $"Name: {item.name}\t" +
                                                    $"Year: {item.year}\t" +
                                                    $"Genre: {item.keywordOne} {item.keywordTwo} {item.keywordThree} {item.keywordFour} {item.keywordFive}\t" +
                                                    $"Price: {item.price}\t" +
                                                    $"Rating: {item.rating}");
                                            }
                                            Console.WriteLine($"Movies Viewed:");
                                            foreach (var item in userList[idChoice].productListViewed)
                                            {
                                                Console.WriteLine($"ID: {item.id}\t" +
                                                    $"Name: {item.name}\t" +
                                                    $"Year: {item.year}\t" +
                                                    $"Genre: {item.keywordOne} {item.keywordTwo} {item.keywordThree} {item.keywordFour} {item.keywordFive}\t" +
                                                    $"Price: {item.price}\t" +
                                                    $"Rating: {item.rating}");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("No such user id exists");
                                        }
                                    }
                                }
                            }
                            else if (subChoice.ToLower() == "all") //Find all users
                            {
                                Console.Clear();
                                foreach (var item in userList)
                                {
                                    Console.WriteLine("----------------------------------------");
                                    Console.WriteLine($"ID: {item.id}\tName: {item.name}\t");
                                    Console.WriteLine($"Movies {item.name} Browsed:");
                                    foreach (var product in item.productListViewed)
                                    {
                                        Console.WriteLine($"ID: {product.id}\tName: {product.name}\tYear: " +
                                        $"{product.year}\tGenres: {product.keywordOne} {product.keywordTwo}" +
                                        $" {product.keywordThree} {product.keywordFour} {product.keywordFive}\t" +
                                        $"Price: {product.price}\tRating: {product.rating}");
                                    }
                                    Console.WriteLine($"Movies {item.name} Bought:");
                                    foreach (var product in item.productListBought)
                                    {
                                        Console.WriteLine($"ID: {product.id}\tName: {product.name}\tYear: " +
                                        $"{product.year}\tGenres: {product.keywordOne} {product.keywordTwo}" +
                                        $" {product.keywordThree} {product.keywordFour} {product.keywordFive}\t" +
                                        $"Price: {product.price}\tRating: {product.rating}");
                                    }
                                    Console.WriteLine("----------------------------------------");
                                }
                            }
                            else if (subChoice.ToLower() == "activeuser") //find all active users in session
                            {
                                foreach (var item in onlineUserList)
                                {
                                    Console.WriteLine("-------------------------------------------");
                                    Console.WriteLine($"ID: {item.id}\tName: {item.name}\n" +
                                        $"Browsing for {item.name}:\n " +
                                        $"ID: {item.viewingProduct.id}\t" +
                                        $"Name: {item.viewingProduct.name}\t" +
                                        $"Year: {item.viewingProduct.year}\t" +
                                        $"Genre: {item.viewingProduct.keywordOne} {item.viewingProduct.keywordTwo} {item.viewingProduct.keywordThree} {item.viewingProduct.keywordFour} {item.viewingProduct.keywordFive}\t" +
                                        $"Price: {item.viewingProduct.price}\t" +
                                        $"Rating: {item.viewingProduct.rating}\n" +
                                        $"Suggested for {item.name}:\n " +
                                        $"ID: {item.suggestedProduct.id}\t" +
                                        $"Name: {item.suggestedProduct.name}\t" +
                                        $"Year: {item.suggestedProduct.year}\t" +
                                        $"Genre: {item.suggestedProduct.keywordOne} {item.suggestedProduct.keywordTwo} {item.suggestedProduct.keywordThree} {item.suggestedProduct.keywordFour} {item.suggestedProduct.keywordFive}\t" +
                                        $"Price: {item.suggestedProduct.price}\t" +
                                        $"Rating: {item.suggestedProduct.rating}");
                                    Console.WriteLine("-------------------------------------------");
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"Please try again");
                            }
                        }
                        break;

                    case "exit":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("You did not enter correct phrase, please enter movies or users");
                        break;
                }
            }
        }
    }
}

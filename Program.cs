using System;
using System.Collections.Generic;
using Handlers;
using Models;

namespace InnoMovieCase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Movie website.");

            //Init
            FakeDatabaseLogic db = new FakeDatabaseLogic();
            MostBoughtCalculator mBCalc = new MostBoughtCalculator();
            List<UserModel> userList = new List<UserModel>();
            List<ProductModel> productList = new List<ProductModel>();
            List<ProductModel> boughtList = new List<ProductModel>();
            List<ProductModel> fullBoughtList = new List<ProductModel>();
            List<MostBoughtModel> mostBoughtList = new List<MostBoughtModel>();
            
            //Set lists
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

            mBCalc.AddingMostBoughtProducts();

            //Output
            System.Console.WriteLine($"The three most bought films are:");
            foreach(var mostBoughtProduct in mostBoughtList){
                System.Console.WriteLine($"{mostBoughtProduct.product.name}");
            }
            System.Console.WriteLine("Create a new user to start");
            System.Console.WriteLine($"I am very invasive, so I can see you're looking at:");

        }
    }
}

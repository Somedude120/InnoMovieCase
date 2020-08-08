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
            System.Console.WriteLine("Create a new user to start");

            //Init
            FakeDatabaseLogic db = new FakeDatabaseLogic();
            List<UserModel> userList = new List<UserModel>();
            List<ProductModel> productList = new List<ProductModel>();
            
            //Set lists
            db.userList = userList;
            db.productList = productList;
            
            //Get data, products first, user needs productList
            db.getMovieDataFromDatabase();
            db.getUserDataFromDatabase();
        }
    }
}

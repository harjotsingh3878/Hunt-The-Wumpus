/*
 * COMMENT Program.cs
 * COMMENT Class containing methods to run Hunt The Wumpus game.
 * COMMENT
 * COMMENT Version 1.0
 * COMMENT   2015.03.06: Created
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HuntTheWumpus
{
    class Program
    {
        static bool isUserLiving = true;
        static int roomOfUser = 1;
        static int numberOfArrows = 3;
        static int adjacentRoom_1, adjacentRoom_2, adjacentRoom_3;
        static string userAction;
        static int userNextRoom;
        static string fileName = "code.txt";
        static int roomOfWumpus, firstRoomOfSpider, secondRoomOfSpider, roomOfPit;

        static string[] room = new string[10];
        static string[] adjacentRooms = new string[10];
        static string[] roomDescription = new string[10];


        //Method to show menu of the game
        static void Menu(int userRoom, int noOfArrows)
        {
            roomOfUser = userRoom;
            numberOfArrows = noOfArrows;
            string[] substrings = adjacentRooms[userRoom-1].Split(' ');
            adjacentRoom_1 = Convert.ToInt32(substrings[0]);
            adjacentRoom_2 = Convert.ToInt32(substrings[1]);
            adjacentRoom_3 = Convert.ToInt32(substrings[2]);

            Console.WriteLine("You are in room {0}.", roomOfUser);
            Console.WriteLine("You have {0} arrows left.\n{1}",numberOfArrows, roomDescription[userRoom - 1]);
            Console.WriteLine("There are tunnels to rooms {0}, {1}, and {2}.", adjacentRoom_1, adjacentRoom_2, adjacentRoom_3);


            if (adjacentRoom_1 == roomOfWumpus || adjacentRoom_2 == roomOfWumpus || adjacentRoom_3 == roomOfWumpus)
            {
                Console.WriteLine("You smell some nasty Wumpus!");
            }
            if (adjacentRoom_1 == firstRoomOfSpider || adjacentRoom_2 == firstRoomOfSpider || adjacentRoom_3 == firstRoomOfSpider || adjacentRoom_1 == secondRoomOfSpider || adjacentRoom_2 == secondRoomOfSpider || adjacentRoom_3 == secondRoomOfSpider)
            {
                Console.WriteLine("You hear a faint clicking noise.");
            }
            if (adjacentRoom_1 == roomOfPit || adjacentRoom_2 == roomOfPit || adjacentRoom_3 == roomOfPit)
            {
                Console.WriteLine("You smell a dank odor. ");
            }

            UserPlay(roomOfUser, numberOfArrows);
          
        }

        //method for starting game and initialise variables
        static public void StartGame()
        {
            roomOfUser = 1;
            numberOfArrows = 3;
            isUserLiving = true;
            adjacentRoom_1 = 6;
            adjacentRoom_2 = 6;
            adjacentRoom_3 = 10;

            //Bringing random numbers to get Wumpus, Spider and Pit in different room in every game
            Random rnd = new Random();
            roomOfWumpus = rnd.Next(2, 10);

            do
            {
                firstRoomOfSpider = rnd.Next(2, 10);
            } while (firstRoomOfSpider == roomOfWumpus);

            do
            {
                secondRoomOfSpider = rnd.Next(2, 10);
            } while (secondRoomOfSpider == roomOfWumpus || secondRoomOfSpider == firstRoomOfSpider);

            do
            {
                roomOfPit = rnd.Next(2, 10);
            } while (roomOfPit == roomOfWumpus || roomOfPit == firstRoomOfSpider || roomOfPit == secondRoomOfSpider);

            TextReader tr = new StreamReader(fileName);

            string line;

            int i = 0;

            string numberOfRooms = tr.ReadLine();
            while ((line = tr.ReadLine()) != null)
            {
                room[i] = line.Substring(0, 2).Trim();
                adjacentRooms[i] = line.Substring(2).Trim();

                roomDescription[i] = tr.ReadLine();

                i++;
            }

            string[] substrings = adjacentRooms[roomOfUser - 1].Split(' ');
            adjacentRoom_1 = Convert.ToInt32(substrings[0]);
            adjacentRoom_2 = Convert.ToInt32(substrings[1]);
            adjacentRoom_3 = Convert.ToInt32(substrings[2]);

            Console.WriteLine("Welcome To **Hunt The Wumpus!**");

            Console.WriteLine("You are in room {0}.", roomOfUser);
            Console.WriteLine("You have {0} arrows left.\n{1}", numberOfArrows, roomDescription[roomOfUser - 1]);
            
            Console.WriteLine("There are tunnels to rooms {0}, {1}, and {2}.", adjacentRoom_1, adjacentRoom_2, adjacentRoom_3);

            UserPlay(roomOfUser, numberOfArrows);
           
        }

        //method containing logic of the game
        static public void UserPlay(int userRoom, int noOfArrows)
        {
            roomOfUser = userRoom;
            numberOfArrows = noOfArrows;
            Console.WriteLine("(M)ove or (S)hoot?");

            userAction = Console.ReadLine();

            if (userAction == "m" || userAction == "M")
            {
                Console.WriteLine("Which room?");

                string l = Console.ReadLine();
                int value;
                if (int.TryParse(l, out value))
                {
                    userNextRoom = Convert.ToInt32(l);

                    if (userNextRoom == adjacentRoom_1 || userNextRoom == adjacentRoom_2 || userNextRoom == adjacentRoom_3)
                    {
                        if (userNextRoom == roomOfWumpus)
                        {
                            Console.WriteLine("You got stung by Wumpus");
                            Console.WriteLine("Game Over");
                            isUserLiving = false;
                        }
                        else if (userNextRoom == firstRoomOfSpider || userNextRoom == secondRoomOfSpider)
                        {
                            Console.WriteLine("You got stung by Spider");
                            Console.WriteLine("Game Over");
                            isUserLiving = false;
                        }
                        else if (userNextRoom == roomOfPit)
                        {
                            Console.WriteLine("You fell in pit");
                            Console.WriteLine("Game Over");
                            isUserLiving = false;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Dimwit! You can't get to there from here.");
                        UserPlay(roomOfUser, numberOfArrows);
                    }

                }
                else
                {
                    Console.WriteLine("You input only Integers from 1 to 10");
                    UserPlay(roomOfUser, numberOfArrows);
                }
            }
            else if (userAction == "s" || userAction == "S")
            {
                Console.WriteLine("Which room?");
                string l = Console.ReadLine();
                int value;
                if (int.TryParse(l, out value))
                {
                    userNextRoom = Convert.ToInt32(l);

                    if (userNextRoom == adjacentRoom_1 || userNextRoom == adjacentRoom_2 || userNextRoom == adjacentRoom_3)
                    {
                        numberOfArrows = numberOfArrows - 1;
                        if (userNextRoom == roomOfWumpus)
                        {
                            Console.WriteLine("Your arrow goes down the tunnel and finds its mark!");
                            Console.WriteLine("You shot the Wumpus! ** You Win! **");
                            Console.WriteLine("Enjoy your fame!");
                            isUserLiving = false;
                            
                        }
                        else if (userNextRoom == firstRoomOfSpider || userNextRoom == secondRoomOfSpider)
                        {
                            Console.WriteLine("Your arrow goes down the tunnel and finds its mark!");
                            Console.WriteLine("You shot the Spider");
                        }
                        else if (userNextRoom == roomOfPit)
                        {
                            Console.WriteLine("You fell in pit");
                            Console.WriteLine("Game Over");
                            isUserLiving = false;
                        }
                        else
                        {
                            Console.WriteLine("Your arrow goes down the tunnel and is lost. You missed.");                           
                        }

                    }
                    else
                    {
                        Console.WriteLine("Dimwit! You can't get to there from here.");
                        UserPlay(roomOfUser, numberOfArrows);
                    }
                }
                else
                {
                    Console.WriteLine("You input only Integers from 1 to 10");
                    UserPlay(roomOfUser, numberOfArrows);
                }
            }
            else
            {               
                Console.WriteLine("Wrong Input");
                UserPlay(roomOfUser, numberOfArrows);
            }

            if (isUserLiving)
            {
                if(numberOfArrows == 0 )
                {
                    Console.WriteLine("No more Arrows");
                    Console.WriteLine("Game Over");
                    Console.WriteLine("If you want to play again press P or press any key to exit");
                    string playAgainInput = Console.ReadLine();
                    if (playAgainInput == "p" || playAgainInput == "P")
                    {
                        StartGame();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Menu(userNextRoom, numberOfArrows);
                }
                
            }
            else
            {
                Console.WriteLine("If you want to play again press P or press any key to exit");
                string playAgainInput = Console.ReadLine();
                if (playAgainInput == "p" || playAgainInput == "P")
                {
                    StartGame();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        static void Main(string[] args)
        {
            //method call to start the game
            StartGame();
          
            Console.ReadKey();
        }

    }
}

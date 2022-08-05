using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//used for Sleep()
using System.Threading;
//used for _getch()
using System.Runtime.InteropServices;

namespace BestOfThree
{
    class Program
    {
        //initialize _getch() function.
        [DllImport("msvcrt.dll")]
        static extern char _getch();

        static void Main()
        {
            //total games that can be played. the required amount of victories gets calculated from this value.
            int total_games = 0;
            int required_victories;

            short player_score;
            short computer_score;
            char option_user;
            char option_computer;
            bool user_victory;
            bool invalid;
            string input;
            float convergenceMod_Rock;    //the multiplicators used to change the chances of the computer options depending on user input.
            float convergenceMod_Scissor;
            float convergenceMod_Paper;
            float convergenceMod_Lizard;
            float convergenceMod_Spock;

            ////////////////////////////////beginning of input process for number of rounds.//////////////////////////////////////////////////
            for (; ; )
            {
                //let user choose amount of games.
                invalid = false;

                Console.WriteLine("\nChoose an amount of rounds that you would like to play.");
                string num_string = Console.ReadLine();
                char[] characters = num_string.ToCharArray();

                //check validity of input.
                for (short i = 0; i < characters.Length; i++)
                {
                    if (characters.Length > 3)
                    {
                        Console.WriteLine("\nNumber is too big!");
                        Thread.Sleep(2000);
                        Console.Clear();
                        invalid = true;
                        break;
                    }

                    //input contains anything but a number? give error and restart input process.
                    if (!CheckValidity(characters[i]))
                    {
                        Console.WriteLine("\nYour input is invalid!");
                        Thread.Sleep(2000);
                        Console.Clear();
                        invalid = true;
                        break;
                    }
                    else
                    {
                        //input is valid? convert char array to int array.
                        int[] fullNum = new int[characters.Length];
                        fullNum[i] = Convert.ToInt32(characters[i].ToString());

                        //convert int array to int. simply multiply every number by ^10
                        total_games += fullNum[i] * Convert.ToInt32(Math.Pow(10, fullNum.Length - i - 1));
                    }
                }

                if (!invalid)
                {
                    //round number has been calculated. break loop and continue. clear console aswell.
                    Console.Clear();
                    break;
                }
            }
            //////////////////////////////////////////////end of input process for number of rounds./////////////////////////////////////////////

            //calculate required victories based on the total games that have to be played. simply divide it by 2 and add +1 to the value because logically, the player with more than 50% win rate wins.
            //if total games are less than 3 then just skip the calculation and set it to 1. because 2 divided by 2 + 1 will still equal 2.
            if (total_games >= 3)
                required_victories = (int)Math.Floor(total_games * 0.5 + 1);
            else
                required_victories = 1;

            //we can only set the amount of rounds at the start of the game so we only need to define this once. calculate increment of modificator depending on amount of rounds.
            //this calculation should be the most accurate. 3 rounds = +0.66 | 5 rounds = +0.4 | 10 rounds = +0.2
            float increaseChance = (2f / total_games);
            //Console.WriteLine(increaseChance);

            for (; ; )
            {
                //reset score at the start of the game.
                player_score = 0;
                computer_score = 0;

                //multiplicators used to manipulate choice of computer to increase chance for specific options depending on the user's choices.
                convergenceMod_Rock = 1f;
                convergenceMod_Scissor = 1f;
                convergenceMod_Paper = 1f;
                convergenceMod_Lizard = 1f;
                convergenceMod_Spock = 1f;

                for (; ; )
                {
                    //starting text.
                    Console.WriteLine("\nTry to beat the computer in rocks, scissor & paper. Best of " + total_games + " wins.\n\nYour score: " + player_score + " / " + required_victories + "\nComputer score: " + computer_score + " / " + required_victories + "\n\nPress the key for one of the following options:\nR = Rock\nS = Scissor\nP = Paper\nL = Lizard\nC = Spock");

                    //player turn.
                    option_user = _getch();
                    char.ToLower(option_user);

                    //draw user input.
                    input = DrawInput(option_user, true);
                    Console.WriteLine(input);

                    //check validity.
                    switch (option_user)
                    {
                        case 'r':
                            convergenceMod_Paper += increaseChance;
                            convergenceMod_Spock += increaseChance;
                            Thread.Sleep(3000);
                            break;
                        case 's':
                            convergenceMod_Rock += increaseChance;
                            convergenceMod_Spock += increaseChance;
                            Thread.Sleep(3000);
                            break;
                        case 'p':
                            convergenceMod_Scissor += increaseChance;
                            convergenceMod_Lizard += increaseChance;
                            Thread.Sleep(3000);
                            break;
                        case 'l':
                            convergenceMod_Scissor += increaseChance;
                            convergenceMod_Rock += increaseChance;
                            Thread.Sleep(3000);
                            break;
                        case 'c':
                            convergenceMod_Paper += increaseChance;
                            convergenceMod_Lizard += increaseChance;
                            Thread.Sleep(3000);
                            break;
                        default:
                            Thread.Sleep(2000);
                            Console.Clear();
                            continue;
                    }

                    //computer turn.
                    option_computer = RandomOption(convergenceMod_Rock, convergenceMod_Scissor, convergenceMod_Paper, convergenceMod_Lizard, convergenceMod_Spock);

                    //draw computer input.
                    input = DrawInput(option_computer, false);
                    Console.WriteLine(input);
                    Thread.Sleep(3000);

                    //is it a draw? continue.
                    if (option_computer == option_user)
                    {
                        Console.WriteLine("\nDraw! Try again...");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }

                    //code will get here if the game isn't a draw. check if the computer wins.
                    if ((option_computer == 'r' && (option_user == 's' || option_user == 'l')) || (option_computer == 's' && (option_user == 'p' || option_user == 'l')) || (option_computer == 'p' && (option_user == 'r' || option_user == 'c')) || (option_computer == 'l' && (option_user == 'p' || option_user == 'c')) || (option_computer == 'c' && (option_user == 'r' || option_user == 's')))
                        user_victory = false;
                    else
                        user_victory = true;

                    if (!user_victory)
                    {
                        Console.WriteLine("\nComputer gains a point.");
                        computer_score++;
                    }
                    else
                    {
                        Console.WriteLine("\nYou gain a point.");
                        player_score++;
                    }

                    //wait and clear console if the game isn't a draw.
                    Thread.Sleep(2000);
                    Console.Clear();

                    //player or computer has won, break free from loop.
                    if (player_score >= required_victories || computer_score >= required_victories)
                    {

                        //write main text to update score after the game is over.
                        Console.WriteLine("\nTry to beat the computer in rocks, scissor & paper. Best of " + total_games + " wins.\n\nYour score: " + player_score + " / " + required_victories + "\nComputer score: " + computer_score + " / " + required_victories);

                        if (player_score >= required_victories)
                            Console.WriteLine("\nYou won!");
                        else if (computer_score >= 2)
                            Console.WriteLine("\nYou lost!");

                        break;
                    }
                }

                //option to quit - let's just recycle the option_user variable.
                Console.WriteLine("\nPress R to restart the game or press any other key to close the application.");
                option_user = _getch();
                char.ToLower(option_user);

                if (option_user == 'r')
                {
                    //user is restarting. let it jump to the beginning.
                    Console.Clear();
                }
                else
                    return; //user is closing application. return main function.
            }
        }

        //function to calculate random option and return it.
        static char RandomOption(float convergenceMod1, float convergenceMod2, float convergenceMod3, float convergenceMod4, float convergenceMod5)
        {
            Random num = new Random();
            float option1 = num.Next(100) + 1;   //+1 so it's 1-100
            float option2 = num.Next(100) + 1;
            float option3 = num.Next(100) + 1;
            float option4 = num.Next(100) + 1;
            float option5 = num.Next(100) + 1;

            //Console.WriteLine("debug: " + option1 + " " + option2 + " " + option3 + " " + option4 + " " + option5);

            option1 *= convergenceMod1; //Rock
            option2 *= convergenceMod2; //Scissor
            option3 *= convergenceMod3; //Paper
            option4 *= convergenceMod4; //Lizard
            option5 *= convergenceMod5; //Spock

            //used for debugging.
            //Console.WriteLine("debug: " + convergenceMod1 + " " + convergenceMod2 + " " + convergenceMod3 + " " + convergenceMod4 + " " + convergenceMod5 + "\ndebug: " + option1 + " " + option2 + " " + option3 + " " + option4 + " " + option5 + "\n");
            //Thread.Sleep(5000);

            if (option1 >= option2 && option1 >= option3 && option1 >= option4 && option1 >= option5)
                return 'r';
            else if (option2 >= option3 && option2 >= option4 && option2 >= option5)
                return 's';
            else if (option3 >= option4 && option3 >= option5)
                return 'p';
            else if (option4 >= option5)
                return 'l';
            else
                return 'c';

            /*
            char option;

            if (random_num <= 33.33)
                option = 'r';
            else if (random_num <= 66.66 && random_num > 33.33)
                option = 's';
            else
                option = 'p';
            */

            /*switch(random_num)
            {
                case 0:
                    option = 'r';
                break;
                case 1:
                    option = 's';
                break;
                default:
                    option = 'p';
                break;
            }*/

            //return option;
        }

        //function to draw input.
        static string DrawInput(char option, bool isUser)
        {
            string text;

            if (isUser)
                text = "\nYou've chosen: ";
            else
                text = "Computer has chosen: ";

            switch (option)
            {
                case 'r':
                    return text + "Rock.";
                case 's':
                    return text + "Scissor.";
                case 'p':
                    return text + "Paper.";
                case 'l':
                    return text + "Lizard.";
                case 'c':
                    return text + "Spock.";
                default:
                    return "\nInvalid input! Try again...";
            }
        }

        //function to check if input of total rounds is an actual number.
        static bool CheckValidity(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                default:
                    return false;
            }
        }
    }
}


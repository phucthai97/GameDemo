using System;

namespace Tic_tac_toe_ver2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int player = 0;
            int check_win = 0;
            int choose_size = 0;
            int size_board = 1;
            int score_to_win = 5;
            int row = 0;
            int col = 0;
            int vector = 0;

            string sign_player = " "; //X or O
            string player_name = "";
            string winner_name = "";

            bool choose_player_flat = false;
            bool win = false;
            bool draw = false;
            bool choose_size_flat = false;

            Console.WriteLine("\n     #########################################");
            Console.WriteLine("     #########----- TIC TAC TOE -----#########");
            Console.WriteLine("     #########################################\n");

            do
            {
                try
                {
                    Console.WriteLine("Choose size board");
                    Console.WriteLine("1.Size 5x5");
                    Console.WriteLine("2.Size 7x7");
                    Console.WriteLine("3.Size 9x9");
                    Console.WriteLine("4.Size 11x11\n");
                    choose_size = Int32.Parse(Console.ReadLine());
                    if (choose_size == 1)
                    {
                        choose_size_flat = true;
                        size_board = 5;
                    }
                    else if (choose_size == 2)
                    {
                        choose_size_flat = true;
                        size_board = 7;
                    }
                    else if (choose_size == 3)
                    {
                        choose_size_flat = true;
                        size_board = 9;
                    }
                    else if (choose_size == 4)
                    {
                        choose_size_flat = true;
                        size_board = 11;
                    }
                    else
                    {
                        choose_size_flat = false;
                        Console.WriteLine("Size is not correct. Please choose size above");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Input is not number. Please choose size above");
                }
            } while (!choose_size_flat);

            string[,] array_game = new string[size_board, size_board];
            array_game = Make_Size_Board(size_board);

            do
            {
                Console.WriteLine("Please chosse player to play first:");
                Console.WriteLine("1.Player A");
                Console.WriteLine("2.Player B");
                try
                {
                    player = Int32.Parse(Console.ReadLine());
                    if (player == 1)
                    {
                        choose_player_flat = true;
                        sign_player = "X";
                        player_name = "Player A";
                    }
                    else if (player == 2)
                    {
                        choose_player_flat = true;
                        sign_player = "O";
                        player_name = "Player B";
                    }
                    else
                    {
                        choose_player_flat = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("It's not correct. Please choose again!");
                }
            } while (!choose_player_flat);

            do
            {
                Print_Board(array_game, player_name, sign_player, win, size_board, row, col, vector);

                Doing_Game(array_game, sign_player);

                if (player == 1)
                {
                    player = 2;
                    sign_player = "O";
                }
                else if (player == 2)
                {
                    player = 1;
                    sign_player = "X";
                }

                check_win = Check_To_Win(array_game, score_to_win);
                int player_index = (int)Math.Floor((double)check_win / 100000);
                row = (int)Math.Floor(((double)check_win % 100000) / 1000);
                col = (int)Math.Floor(((double)check_win % 1000) / 10);
                vector = check_win % 10;
                if (player_index == 1)
                {
                    win = true;
                    winner_name = "Player A";
                }
                else if (player_index == 2)
                {
                    win = true;
                    winner_name = "Player B";
                }
                else
                {
                    win = Check_Enable_Win_Or_Draw(array_game, score_to_win);
                    if (win == true)
                    {
                        draw = true;
                    }
                }
            } while (!win);
            Print_Board(array_game, player_name, sign_player, win, size_board, row, col, vector);
            if (draw == true)
            {
                Console.WriteLine("\n###########################");
                Console.WriteLine($"####### Draw Game!! #######");
                Console.WriteLine("###########################\n");
            }
            else
            {
                Console.WriteLine("\n###########################");
                Console.WriteLine($"##### {winner_name} is winner #####");
                Console.WriteLine("###########################\n");
            }
            Console.ReadKey();
        }

        #region Check Win Or Draw
        private static bool Check_Enable_Win_Or_Draw(string[,] array_game, int score_to_win)
        {
            bool draw = false;
            for (int i = 0; i < array_game.GetLength(0); i++)
            {
                for (int j = 0; j < array_game.GetLength(1); j++)
                {
                    if (array_game[i, j] != "O")
                    {
                        //Check top to bottom
                        if (i <= (array_game.GetLength(0) - score_to_win))
                        {
                            if (array_game[i + 1, j] != "O" && array_game[i + 2, j] != "O" &&
                                array_game[i + 3, j] != "O" && array_game[i + 4, j] != "O")
                            {
                                //Console.WriteLine($"This is for test X step 1 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                        //Check left to right
                        if (j <= (array_game.GetLength(1) - score_to_win))
                        {
                            if (array_game[i, j + 1] != "O" && array_game[i, j + 2] != "O" &&
                               array_game[i, j + 3] != "O" && array_game[i, j + 4] != "O")
                            {
                                //Console.WriteLine($"This is for test X step 2 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                        //Check cross lef-top to right-bottom
                        if ((i <= (array_game.GetLength(0) - score_to_win)) && (j <= (array_game.GetLength(1) - score_to_win)))
                        {
                            if (array_game[i + 1, j + 1] != "O" && array_game[i + 2, j + 2] != "O" &&
                               array_game[i + 3, j + 3] != "O" && array_game[i + 4, j + 4] != "O")
                            {
                                //Console.WriteLine($"This is for test X step 3 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                        //Check cross right_top to left-bottom
                        if ((i <= (array_game.GetLength(0) - score_to_win)) && (j >= (array_game.GetLength(1) - score_to_win)))
                        {
                            if (array_game[i + 1, j - 1] != "O" && array_game[i + 2, j - 2] != "O" &&
                               array_game[i + 3, j - 3] != "O" && array_game[i + 4, j - 4] != "O")
                            {
                                //Console.WriteLine($"This is for test X step 4 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                    }
                }
            }
            //player1_cant_win = true;
            for (int i = 0; i < array_game.GetLength(0); i++)
            {
                for (int j = 0; j < array_game.GetLength(1); j++)
                {
                    if (array_game[i, j] != "X")
                    {
                        //Check top to bottom
                        if (i <= (array_game.GetLength(0) - score_to_win))
                        {
                            if (array_game[i + 1, j] != "X" && array_game[i + 2, j] != "X" &&
                                array_game[i + 3, j] != "X" && array_game[i + 4, j] != "X")
                            {
                                //Console.WriteLine($"This is for test O step 1 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                        //Check left to right
                        if (j <= (array_game.GetLength(1) - score_to_win))
                        {
                            if (array_game[i, j + 1] != "X" && array_game[i, j + 2] != "X" &&
                               array_game[i, j + 3] != "X" && array_game[i, j + 4] != "X")
                            {
                                //Console.WriteLine($"This is for test O step 2 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                        //Check cross lef-top to right-bottom
                        if ((i <= (array_game.GetLength(0) - score_to_win)) && (j <= (array_game.GetLength(1) - score_to_win)))
                        {
                            if (array_game[i + 1, j + 1] != "X" && array_game[i + 2, j + 2] != "X" &&
                               array_game[i + 3, j + 3] != "X" && array_game[i + 4, j + 4] != "X")
                            {
                                //Console.WriteLine($"This is for test O step 3 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                        //Check cross right_top to left-bottom
                        if ((i <= (array_game.GetLength(0) - score_to_win)) && (j >= (array_game.GetLength(1) - score_to_win)))
                        {
                            if (array_game[i + 1, j - 1] != "X" && array_game[i + 2, j - 2] != "X" &&
                               array_game[i + 3, j - 3] != "X" && array_game[i + 4, j - 4] != "X")
                            {
                                //Console.WriteLine($"This is for test O step 4 at array[{i}, {j}]");
                                return draw = false;
                            }
                        }
                    }
                }
            }
            draw = true;
            return draw;
        }
        #endregion

        #region Make Size Board Game
        private static string[,] Make_Size_Board(int size_board)
        {
            int push_value_size = 1;
            string[,] array_game = new string[size_board, size_board];
            for (int i = 0; i < size_board; i++)
            {
                for (int j = 0; j < size_board; j++)
                {
                    array_game[i, j] = push_value_size.ToString();
                    push_value_size++;
                }
            }
            return array_game;
        }
        #endregion

        #region Check Win
        private static int Check_To_Win(string[,] array_game, int score_to_win)
        {
            int check_win = 0;
            string[] sign_player_arr = { "X", "O" };
            for (int h = 0; h < sign_player_arr.GetLength(0); h++)
            {
                for (int i = 0; i < array_game.GetLength(0); i++)
                {
                    for (int j = 0; j < array_game.GetLength(1); j++)
                    {
                        if (array_game[i, j] == sign_player_arr[h])
                        {
                            //Check top to bottom
                            if (i <= (array_game.GetLength(0) - score_to_win))
                            {
                                //Console.WriteLine($"array[{i}, {j}]with sign {sign_player_arr[h]} top to bottom is ok");
                                if (array_game[i, j] == array_game[i + 1, j] && array_game[i + 1, j] == array_game[i + 2, j] &&
                                    array_game[i + 2, j] == array_game[i + 3, j] && array_game[i + 3, j] == array_game[i + 4, j])
                                {
                                    //Console.WriteLine("top to bottom detect to win!!!!!!!!!!!!!");
                                    if (sign_player_arr[h] == "X")
                                    { //103082
                                        return check_win = 1 * 100000 + i * 1000 + j * 10 + 1;
                                    }
                                    else if (sign_player_arr[h] == "O")
                                    {
                                        return check_win = 2 * 100000 + i * 1000 + j * 10 + 1;
                                    }
                                }
                            }
                            //Check left to right
                            if (j <= (array_game.GetLength(1) - score_to_win))
                            {
                                //Console.WriteLine($"array[{i}, {j}] with sigh {sign_player_arr[h]} left to right is ok");
                                if (array_game[i, j] == array_game[i, j + 1] && array_game[i, j + 1] == array_game[i, j + 2]
                                    && array_game[i, j + 2] == array_game[i, j + 3] && array_game[i, j + 3] == array_game[i, j + 4])
                                {
                                    //Console.WriteLine("left to right detect to win!!!!!!!!!!!!!");
                                    if (sign_player_arr[h] == "X")
                                    {
                                        return check_win = 1 * 100000 + i * 1000 + j * 10 + 2;
                                    }
                                    else if (sign_player_arr[h] == "O")
                                    {
                                        return check_win = 2 * 100000 + i * 1000 + j * 10 + 2;
                                    }
                                }
                            }
                            //Check cross lef-top to right-bottom
                            if ((i <= (array_game.GetLength(0) - score_to_win)) && (j <= (array_game.GetLength(1) - score_to_win)))
                            {
                                if (array_game[i, j] == array_game[i + 1, j + 1] && array_game[i + 1, j + 1] == array_game[i + 2, j + 2]
                                    && array_game[i + 2, j + 2] == array_game[i + 3, j + 3] && array_game[i + 3, j + 3] == array_game[i + 4, j + 4])
                                {
                                    if (sign_player_arr[h] == "X")
                                    {
                                        return check_win = 1 * 100000 + i * 1000 + j * 10 + 3;
                                    }
                                    else if (sign_player_arr[h] == "O")
                                    {
                                        return check_win = 2 * 100000 + i * 1000 + j * 10 + 3;
                                    }
                                }
                            }
                            //Check right-top to left-bottom
                            if ((i <= (array_game.GetLength(0) - score_to_win)) && (j >= (array_game.GetLength(1) - score_to_win)))
                            {
                                if (array_game[i, j] == array_game[i + 1, j - 1] && array_game[i + 1, j - 1] == array_game[i + 2, j - 2]
                                    && array_game[i + 2, j - 2] == array_game[i + 3, j - 3] && array_game[i + 3, j - 3] == array_game[i + 4, j - 4])
                                {
                                    if (sign_player_arr[h] == "X")
                                    {
                                        return check_win = 1 * 100000 + i * 1000 + j * 10 + 4;
                                    }
                                    else if (sign_player_arr[h] == "O")
                                    {
                                        return check_win = 2 * 100000 + i * 1000 + j * 10 + 4;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return check_win = 0;
        }
        #endregion

        #region Doing Game
        public static void Doing_Game(string[,] Darray_game, string sign_player)
        {
            bool choose_number_flat = false;
            string input_player = " ";
            do
            {
                bool Is_X_or_O = false;
                try
                {
                    input_player = Console.ReadLine().ToString();
                    if (input_player == "X" || input_player == "O")
                    {
                        Is_X_or_O = true;
                        Console.WriteLine($"{input_player} is not valid! Please entern the number on matrix board.");
                    }
                    else
                    {
                        for (int i = 0; i < Darray_game.GetLength(0); i++)
                        {
                            for (int j = 0; j < Darray_game.GetLength(1); j++)
                            {
                                if (Darray_game[i, j] == input_player)
                                {
                                    Darray_game[i, j] = sign_player;
                                    choose_number_flat = true;
                                }
                            }
                        }
                    }
                    if (choose_number_flat == false && Is_X_or_O == false)
                    {
                        Console.WriteLine("Input is not exits in matrix board, please try again");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Input is not the number, please try again");
                }

            } while (!choose_number_flat);
        }
        #endregion

        #region Print Board on screen
        static void Print_Board(string[,] Barray_game, string player_name, string sign_player, bool win, int size_board, int row, int col, int vector)
        {
            //Draw top borders
            Console.Write("\n     |");
            for (int k = 0; k < size_board; k++)
            {
                Console.Write("-------|");
            }
            Console.WriteLine("");
            //End Draw top borders

            for (int i = 0; i < Barray_game.GetLength(0); i++)
            {
                //Draw collum 
                Console.Write("     |");
                for (int k = 0; k < size_board; k++)
                {
                    Console.Write("       |");
                }
                Console.WriteLine("");
                Console.Write("     |   ");
                //End Draw collum
                for (int j = 0; j < Barray_game.GetLength(1); j++)
                {
                    if (Barray_game[i, j].Length == 1)
                    {
                        if (Barray_game[i, j] == "X")
                        {
                            if (i == row && j == col && vector != 0)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{Barray_game[i, j]}");
                                if (vector == 1)
                                {
                                    row++;
                                }
                                else if (vector == 2)
                                {
                                    col++;
                                }
                                else if (vector == 3)
                                {
                                    row++;
                                    col++;
                                }
                                else if (vector == 4)
                                {
                                    row++;
                                    col--;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{Barray_game[i, j]}");
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("   |   ");
                        }
                        else if (Barray_game[i, j] == "O")
                        {
                            if (i == row && j == col && vector != 0)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{Barray_game[i, j]}");
                                if (vector == 1)
                                {
                                    row++;
                                }
                                else if (vector == 2)
                                {
                                    col++;
                                }
                                else if (vector == 3)
                                {
                                    row++;
                                    col++;
                                }
                                else if (vector == 4)
                                {
                                    row++;
                                    col--;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write($"{Barray_game[i, j]}");
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("   |   ");
                        }
                        else
                        {
                            Console.Write($"{Barray_game[i, j]}");
                            Console.Write("   |   ");
                        }
                    }
                    else if (Barray_game[i, j].Length == 2)
                    {
                        Console.Write($"{Barray_game[i, j]}");
                        Console.Write("  |   ");
                    }
                    else if (Barray_game[i, j].Length == 3)
                    {
                        Console.Write($"{Barray_game[i, j]}");
                        Console.Write(" |   ");
                    }
                }
                Console.WriteLine("");
                Console.Write("     |");
                for (int n = 0; n < size_board; n++)
                {
                    Console.Write("       |");
                }
                Console.WriteLine("");
                Console.Write("     |");
                for (int k = 0; k < size_board; k++)
                {
                    Console.Write("-------|");
                }
                Console.WriteLine("");
            }
            if (win == false)
                Console.WriteLine($"\n-> Player {player_name} (with sign {sign_player}) turn, Please enter number:");
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SudokuBot
{
    class Run
    {
        int[,][] coords = new int[9, 9][];
        bool solved = false;
        Random random = new Random();
        public Run()
        {
            Console.WriteLine("Enter coords (A) or generate a sudoku? (B)");
            if (Console.ReadLine() == "B")
            {
                GenerateSudoku();
            }
            Console.WriteLine("Enter how many numbers you are going to enter");
            int amount = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    coords[j, i] = new int[9]; //creating a new array of size 10 for each specific coordinate
                    for (int k = 1; k < 10; k++)
                    {
                        coords[j, i][k-1] = k; // fills every array with every single number up to 9
                    }
                }
            }
            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine("Enter the x coordinate of the number, the y coordinate");
                int x = Convert.ToInt32(Console.ReadLine());
                int y = Convert.ToInt32(Console.ReadLine());
                int nom = Convert.ToInt32(Console.ReadLine());
                coords[x, y] = new int[] { nom, 10,10, 10, 10, 10, 10, 10, 10 };
            }
            Solve();
        }

        public void Solve()
        {
            while (solved == false)
            {
                //this removes all of the same value as the presets in the same row and column
                for (int loop = 0; loop < 2; loop++)
                {
                    for (int val = 0; val < 9; val++) //index for contents of the array
                    {
                        for (int y = 0; y < 9; y++) //y coordinate
                        {
                            for (int x = 0; x < 9; x++) //x coordinate
                            {
                                if (coords[x, y][1] == 10) //if the value there was predefined    
                                {
                                    int contents = coords[x, y][0]; //stores the value of what was predefined in that cell
                                    int originalX = x;
                                    int originalY = y;
                                    int returnX = x;
                                    int returnY = y;
                                    int newX = x / 3 * 3;
                                    int newY = y / 3 * 3;
                                    int stableX = newX;
                                    int stableY = newY;

                                    for (newX = x / 3 * 3; newX < stableX + 3; newX++) // checks the 3x3 grid and removes all of the same preset number
                                    {
                                        for (newY = y / 3 * 3; newY < stableY + 3; newY++) // the algorithm in for loop gets the top left of the 3x3 box it's in
                                        {
                                            //while (r != 1)
                                            //{
                                            //    newX++; //otherwise it'll replace itself because I'm testing it at 0,0
                                            //    r++;
                                            //}
                                            if (coords[newX, newY][1] != 10) //makes sure it doesn't delete itself
                                            {
                                                if (contents != 0)
                                                {
                                                    coords[newX, newY][contents - 1] = 0;
                                                }
                                            }
                                        }
                                    }

                                    while (x != 8) //otherwise it'll go out of bounds of array
                                    {
                                        x++;
                                        if (contents != 0)
                                        {
                                            if (coords[x, y][1] != 10) // so it doesn't damage predefined values
                                            {
                                                coords[x, y][contents - 1] = 0; //where the value appears in the same row, it gets set to 0
                                            }
                                        }
                                    }
                                    while (originalX != 0)
                                    {
                                        originalX--;
                                        if (contents != 0) //otherwise contents - 1 will be -1 and then it'll crash
                                        {
                                            if (coords[originalX, y][1] != 10) // so it doesn't damage predefined values
                                            {
                                                coords[originalX, y][contents - 1] = 0; //where the value appears in the same row, it gets set to 0
                                            }
                                        }
                                    }
                                    x = returnX;
                                    while (y != 8)
                                    {
                                        y++;
                                        if (contents != 0)
                                        {
                                            if (coords[x, y][1] != 10) // so it doesn't damage predefined values
                                            {
                                                coords[x, y][contents - 1] = 0; //where the value appears in the same column, it gets set to 0
                                            }
                                        }
                                    }
                                    while (originalY != 0)
                                    {
                                        originalY--;
                                        if (contents != 0)
                                        {
                                            if (coords[x, originalY][1] != 10) // so it doesn't damage predefined values
                                            {
                                                coords[x, originalY][contents - 1] = 0; //where the value appears in the same column, it gets set to 0
                                            }
                                        }
                                    }
                                    y = returnY;
                                }
                            }
                        }
                    }
                } // Need to display all numbers left by doing for loop and if it's a 0, it doesn't display, if it is, it does
                AnyOtherSpaces();





                //checks if there are still multiple values in any of the arrays
                int valToBeChanged = 0;
                for (int checkX = 0; checkX < 9; checkX++)
                {
                    for (int checkY = 0; checkY < 9; checkY++)
                    {
                        int counter = 0;
                        for (int indexCheck = 0; indexCheck < 9; indexCheck++)
                        {
                            if (coords[checkX, checkY][indexCheck] == 0)
                            {
                                counter++; //if counter reaches 8 then there is only 1 number available to be put there
                                           //decides whether it should be changed to a predefined number if no other number can go into that spot
                            }
                            else
                            {
                                valToBeChanged = coords[checkX, checkY][indexCheck];
                            }
                            if (counter == 8)
                            {
                                if (valToBeChanged == 10)
                                {
                                    valToBeChanged = 9;
                                }
                                SetToPredefinied(valToBeChanged, checkX, checkY);
                            }
                        }
                    }
                }
                Finished();
            }
            Console.ReadLine();
        }
        public void Finished()
        {
            int counter = 0;
            for(int x = 0; x < 9; x++)
            {
                for(int y = 0; y < 9; y++)
                {
                    if(coords[x, y][1] == 10)
                    {
                        counter++;
                        if (counter == 81)
                        {
                            solved = true;
                            DisplayGrid();
                        }
                    }
                }
            }
        }

        public void AnyOtherSpaces() //checks if any number can only go in one space in each 3x3 grid
        {
            int setX = 0;
            int setY = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    int i = x / 3 * 3;
                    int j = y / 3 * 3;
                    int stableI = i;
                    int stableJ = j;
                    for (int contents = 1; contents < 9; contents++)
                    {
                        int counter = 0;
                        for (i = x / 3 * 3; i < stableI + 3; i++) //x
                        {
                            for (j = y / 3 * 3; j < stableJ + 3; j++) //y
                            {
                                for (int index = 0; index < 8; index++)
                                {
                                    if(coords[i, j][index] == contents)  //if there is another of that number
                                    {
                                        counter++;
                                        setX = i;
                                        setY = j;
                                    }
                                }
                            }
                        }
                        if(counter == 1)
                        {
                            if(coords[setX, setY][1] != 10)
                            {
                                SetToPredefinied(contents, setX, setY);
                            }
                        }
                    }
                }
            }
        }

        public void SetToPredefinied(int valtobechanged, int x, int y)
        {
            for (int i = 0; i < 9; i++) //works through the index and sets all to 10 except the contents
            {
                coords[x, y][i] = 10;
            }
            coords[x, y][0] = valtobechanged;
        }

        public void DisplayGrid()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    Console.Write(coords[j, i][0] + "|");
                }
                Console.WriteLine("");
                Console.WriteLine("- - - - - - - - - ");
            }
            Console.ReadLine();
        }

        public void GenerateSudoku()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    coords[j, i] = new int[9]; //creating a new array of size 10 for each specific coordinate
                    for (int k = 1; k < 10; k++)
                    {
                        coords[j, i][k - 1] = k; // fills every array with every single number up to 9
                    }
                }
            }
            int amountOfNumbers = random.Next(23, 30);
            for (int i = 0; i < amountOfNumbers; i++)
            {
                int x = random.Next(0, 8);
                int y = random.Next(0, 8);
                int nom = random.Next(1, 9);
                coords[x, y] = new int[] { nom, 10, 10, 10, 10, 10, 10, 10, 10 };
            }
            DisplayGrid();
            Solve();
        }
    }
}

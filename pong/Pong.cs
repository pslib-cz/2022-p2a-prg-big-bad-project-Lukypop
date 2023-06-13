using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chleba
{ 
class Pong
{
    private const int width = 65;
    private const int height = 20;
    private const int paddleSize = 5;
    private int paddleLeft = height / 2 - 3;
    private int paddleRight = height / 2 - 3;
    private int ballX = 0;
    private int ballY = 0;
    private int ballDirX = 1;
    private int ballDirY = 1;
    private int scoreLeft = 0;
    private int scoreRight = 0;
    private bool _gameOver = false;
    private bool _gamePaused = false;
    private int _lives = 5;

    public Pong()
    {
        Console.Title = "Pong";
        Console.SetWindowSize(width, height + 3);
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
        Console.CursorVisible = false;
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(
                "                      _____                  \r\n" +
                "                     |  __ \\                 \r\n" +
                "                     | |__) |__  _ __   __ _ \r\n" +
                "                     |  ___/ _ \\| '_ \\ / _` |\r\n" +
                "                     | |  | (_) | | | | (_| |\r\n" +
                "                     |_|   \\___/|_| |_|\\__, |\r\n" +
                "                                        __/ |\r\n" +
                "                                       |___/ ");

            Console.ResetColor();
            Console.SetCursorPosition(22, 9);
            Console.Write("1. Play Game");
            Console.SetCursorPosition(22, 10);
            Console.Write("2. Check Leaderboard");
            Console.SetCursorPosition(22, 11);
            Console.Write("3. Close Game");
            Console.SetCursorPosition(22, 13);
            Console.Write("Select an option (1-3): ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.NumPad1)
            {
                Console.Clear();
                Console.WriteLine("you can move by arrows");
                Console.WriteLine("you have " + _lives + " lives.");
                Thread.Sleep(1000);
                Console.Clear();
                PlayGame();
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2)
            {
                Console.Clear();
                LoadResult();
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3)
            {
                break;
            }
        }
    }
    public void PlayGame()
    {
        _gameOver = false;
        scoreLeft = 0;
        scoreRight = 0;

        int counter = 0;
        while (!_gameOver)
        {
            if (!_gamePaused)
            {
                if (counter % 3 == 0)
                {
                    Update();

                }
                UpdateBot();
                DrawPaddle();
                DrawBall();
                DrawHud();
            }

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                ProcessInput(key);
            }

            Thread.Sleep(10);
            counter++;
        }
    }


    public void SaveResult(string winner)
    {
        string result = winner + " side has won";

        string filePath = "result.txt";

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(result);
        }
    }

    public void LoadResult()
    {
        string filePath = "result.txt";

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            int leftWins = 0;
            int rightWins = 0;

            foreach (string line in lines)
            {
                if (line.Contains("left"))
                {
                    leftWins++;
                }
                else if (line.Contains("right"))
                {
                    rightWins++;
                }
            }

            Console.WriteLine("Left side has won " + leftWins + " times.");
            Console.WriteLine("Right side has won " + rightWins + " times.");
            Console.WriteLine("\nExit by Escape.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("No result file found.");
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while loading the result: " + e.Message);
        }

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
    }

    private void Update()
    {
        Console.SetCursorPosition(ballX, ballY);
        Console.Write(" ");
        if (ballX + ballDirX == 0 || ballX + ballDirX == width - 1)
        {
            if (ballY >= paddleLeft && ballY < paddleLeft + paddleSize)
            {
                ballDirX = 1;
            }
            else if (ballY >= paddleRight && ballY < paddleRight + paddleSize)
            {
                ballDirX = -1;
            }


        }



        if (ballY + ballDirY == -1 || ballY + ballDirY == height)
        {
            ballDirY *= -1;
        }

        ballX += ballDirX;
        ballY += ballDirY;

        if (ballX == width)
        {
            scoreLeft++;
            ResetBall();
        }

        if (ballX == 0)
        {
            scoreRight++;
            ResetBall();
        }


    }
    private void UpdateBot()
    {

        paddleRight = ballY;

        if (paddleRight < 0)
        {
            paddleRight = 0;
        }
        else if (paddleRight > height - paddleSize)
        {
            paddleRight = height - paddleSize;
        }

    }
    private void DrawPaddle()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(0, 0 + i);
            Console.Write(" ");
        }

        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(width - 1, 0 + i);
            Console.Write(" ");
        }

        for (int i = 0; i < paddleSize; i++)
        {
            Console.SetCursorPosition(width - 1, paddleRight + i);
            Console.Write("|");
        }


        for (int i = 0; i < paddleSize; i++)
        {
            Console.SetCursorPosition(0, paddleLeft + i);
            Console.Write("|");
        }

        Console.ResetColor();
    }

    private void DrawBall()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(ballX, ballY);
        Console.Write("O");
        Console.ResetColor();
    }


    private void DrawHud()
    {

        for (int i = 0; i < width; i++)
        {
            Console.SetCursorPosition(i, height);
            Console.Write("_");
        }

        Console.SetCursorPosition(width / 2 - 5, height + 2);
        Console.Write($"Score: {scoreLeft} - {scoreRight}");

        if (_gamePaused)
        {
            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.Write("Game Paused");
        }
    }

    private void ResetBall()
    {
        ballX = width / 2;
        ballY = height / 2;
        ballDirX = 1;
        ballDirY = 1;

        if (scoreLeft == _lives)
        {
            SaveResult("left");
            _gameOver = true;
        }
        else if (scoreRight == _lives)
        {
            SaveResult("right");
            _gameOver = true;
        }
    }

    private void ProcessInput(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                if (paddleLeft > 0)
                {
                    paddleLeft--;
                }
                break;
            case ConsoleKey.DownArrow:
                if (paddleLeft < height - paddleSize)
                {
                    paddleLeft++;
                }
                break;
            case ConsoleKey.Escape:
                _gamePaused = !_gamePaused;
                break;
        }
    }
}
}
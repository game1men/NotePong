using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
namespace Pong
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, uint windowStyle);



        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static void Main()
        {

            Program prog = new Program();
            Thread vlakno = new Thread(prog.Start);
            vlakno.SetApartmentState(ApartmentState.STA);
            vlakno.Start();

        }

        public void Start()
        {
            //starts notepad window for player one
            Process playerOne = new Process();
            playerOne.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            playerOne.StartInfo.FileName = "C:\\Windows\\notepad.exe";
            playerOne.Start();
            //starts notepad window for player two
            Process playerTwo = new Process();
            playerTwo.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            playerTwo.StartInfo.FileName = "C:\\Windows\\notepad.exe";
            playerTwo.Start();
            //starts notepad window for ball
            Process ball = new Process();
            ball.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            ball.StartInfo.FileName = "C:\\Windows\\notepad.exe";
            ball.Start();

            int playerOneScore = 0;
            int playerTwoScore = 0;
            int xDirrection = 1;
            int yDirrection = 1;


            int playerSpeed = 14;
            int ballSpeed = 8;

            int screenWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
            int screenHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString());
            int ballX = screenWidth / 2;
            int ballY = screenHeight / 2;


            int x;
            int y = 0;
            int w = 200;
            int h = 150;
            while (true)
            {
                RECT playerOneRct = new RECT();
                RECT playerTwoRct = new RECT();
                RECT pongRtc = new RECT();
                screenWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
                screenHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString());

                x = screenHeight / 2 + w;

                GetWindowRect(playerOne.MainWindowHandle, ref playerOneRct);

                if (Keyboard.IsKeyDown(Key.Up) && !(playerOneRct.Top <= 0))
                {
                    SetWindowPos(playerOne.MainWindowHandle, new IntPtr(-1), screenWidth - 100, playerOneRct.Top - playerSpeed, 100, 200, 0x0040);
                }
                if (Keyboard.IsKeyDown(Key.Down) && !(playerOneRct.Top >= screenHeight - 10))
                {
                    SetWindowPos(playerOne.MainWindowHandle, new IntPtr(-1), screenWidth - 100, playerOneRct.Top + playerSpeed, 100, 200, 0x0040);
                }


               

                GetWindowRect(playerTwo.MainWindowHandle, ref playerTwoRct);
                //ConsoleKeyInfo key = Console.Read();
                if (Keyboard.IsKeyDown(Key.Escape))
                {
                    playerOne.Kill();
                    playerTwo.Kill();
                    ball.Kill();
                    Environment.Exit(0);
                }
                if (Keyboard.IsKeyDown(Key.W) && !(playerTwoRct.Top <= 0))
                {
                    SetWindowPos(playerTwo.MainWindowHandle, new IntPtr(-1), 0, playerTwoRct.Top - playerSpeed, 100, 200, 0x0040);
                }
                if (Keyboard.IsKeyDown(Key.S) && !(playerTwoRct.Top >= screenHeight - 10))
                {
                    SetWindowPos(playerTwo.MainWindowHandle, new IntPtr(-1), 0, playerTwoRct.Top + playerSpeed, 100, 200, 0x0040);
                }

                screenWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
                screenHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString());

                GetWindowRect(ball.MainWindowHandle, ref pongRtc);

                if (pongRtc.Top > screenHeight - 50)
                {
                    //xDirrection = 1;
                    yDirrection = -1;

                }
                if (pongRtc.Top < 50)
                {
                    //xDirrection = -1;
                    yDirrection = 1;

                }

                if (playerTwoRct.Right > pongRtc.Left && playerTwoRct.Left < pongRtc.Right && playerTwoRct.Top < pongRtc.Top && playerTwoRct.Top + 200 > pongRtc.Top)
                {
                    xDirrection = 1;
                    //yDirrection = 1;

                }

                if (playerOneRct.Right > pongRtc.Left && playerOneRct.Left < pongRtc.Right && playerOneRct.Top < pongRtc.Top && playerOneRct.Top + 200 > pongRtc.Top)
                {
                    xDirrection = -1;
                    //yDirrection = 1;

                }

                if (pongRtc.Left > screenWidth)
                {

                    playerTwoScore++;
                    ballX = screenWidth / 2;
                    ballY = screenHeight / 2;
                }


                if (pongRtc.Left < -20)
                {
                    playerOneScore++;
                    ballX = screenWidth / 2;
                    ballY = screenHeight / 2;
                }

                ShowWindow(playerOne.Handle, 5);
                ShowWindow(playerTwo.Handle, 5);
                ShowWindow(ball.Handle, 5);


                if (Keyboard.IsKeyDown(Key.L))
                {
                    Thread.Sleep(50);
                    if (Keyboard.IsKeyUp(Key.L))
                    {
                        playerSpeed++;
                    }
                }
                else if (Keyboard.IsKeyDown(Key.K))
                {
                    Thread.Sleep(50);
                    if (Keyboard.IsKeyUp(Key.K))
                    {
                        playerSpeed--;
                    }
                }
                else if (Keyboard.IsKeyDown(Key.O))
                {
                    Thread.Sleep(50);
                    if (Keyboard.IsKeyUp(Key.O))
                    {
                        ballSpeed--;
                    }
                }
                if (Keyboard.IsKeyDown(Key.P))
                {
                    Thread.Sleep(50);
                    if (Keyboard.IsKeyUp(Key.P))
                    {
                        ballSpeed++;
                    }

                }

                SetWindowPos(Process.GetCurrentProcess().MainWindowHandle, new IntPtr(-1), x, y, w, h, 0x0040);

                Console.Clear();
                //  Console.WriteLine("x: " + x + "\n" + "y: " + y + "h: " + h + "\n" + "w: " + w);
                Console.WriteLine("SCORE\nplayer1: " + playerOneScore + "\n" + "player2: " + playerTwoScore + "\n" + "ball speed: " + ballSpeed + "(O- P+) \n" + "player speed: " + playerSpeed + "(K- L+) \n Exit with Escape");
                ballX += ballSpeed * xDirrection;
                ballY += ballSpeed * yDirrection;
                Thread.Sleep(5);

                SetWindowPos(ball.MainWindowHandle, new IntPtr(-1), ballX, ballY, 10, 10, 0x0040);





            }
        }
        /*
        public void Pong()
        {
            Process Pong = new Process();
            Pong.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            Pong.StartInfo.FileName = "C:\\Windows\\notepad.exe";
            Pong.Start();

            int xDirrection = 1;
            int yDirrection = 1;
            int speedY = 1;
            int speedX = 1;
            RECT rct = new RECT();

            int screenWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
            int screenHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString());
            speedX = screenWidth / 2;
            speedY = screenHeight / 2;
            while (true)
            {
                screenWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
                screenHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString());
             
                GetWindowRect(Pong.MainWindowHandle, ref rct);
               
                if (rct.Top > screenHeight -50)
                {
                    //xDirrection = 1;
                    yDirrection = -1;
                  
                }
                if ( rct.Top < 50)
                {
                    //xDirrection = -1;
                    yDirrection = 1;
                   
                }
                speedX += 1 * xDirrection;
                speedY += 1 * yDirrection;
                Thread.Sleep(20);
                Console.WriteLine("xDirrection: " + xDirrection +"\n" +"yDirrection: " +yDirrection+" \n speedX:" + speedX + " \n speedY:" + speedY);
                SetWindowPos(Pong.MainWindowHandle, IntPtr.Zero, speedX, speedY, 10, 10, SWP_NOZORDER);

            }
        }



        public void playerOne()
        {
            Process PlayerOne = new Process();
            PlayerOne.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            PlayerOne.StartInfo.FileName = "C:\\Windows\\notepad.exe";
            PlayerOne.Start();

            while (true)
            {
                int screenWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString());
                RECT rct = new RECT();
                GetWindowRect(PlayerOne.MainWindowHandle, ref rct);

                if (Keyboard.IsKeyDown(Key.W) && rct.Top != 0)
                {
                  
                    SetWindowPos(PlayerOne.MainWindowHandle, IntPtr.Zero, screenWidth -100, rct.Top - 1, 10, 10, SWP_NOZORDER);
                }
                if (Keyboard.IsKeyDown(Key.S) && rct.Top != screenHeight - 10)
                {
                  
                    SetWindowPos(PlayerOne.MainWindowHandle, IntPtr.Zero, screenWidth -100, rct.Top + 1, 10, 10, SWP_NOZORDER);
                }




            }
        }

        public void playerTwo()
        {
            Process playerTwo = new Process();
            playerTwo.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            playerTwo.StartInfo.FileName = "C:\\Windows\\notepad.exe";
            playerTwo.Start();

            while (true)
            {
                int screenWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString());
                RECT rct = new RECT();
                GetWindowRect(playerTwo.MainWindowHandle, ref rct);
                //ConsoleKeyInfo key = Console.Read();

                if (Keyboard.IsKeyDown(Key.Up) && rct.Top != 0)
                {
                   
                    SetWindowPos(playerTwo.MainWindowHandle, IntPtr.Zero, 0, rct.Top - 1, 10, 10, SWP_NOZORDER);
                }
                if (Keyboard.IsKeyDown(Key.Down) && rct.Top != screenHeight -10)
                {

                 
                    SetWindowPos(playerTwo.MainWindowHandle, IntPtr.Zero, 0, rct.Top + 1, 10, 10, SWP_NOZORDER);
                }
              



            }
        }
        */

    }
}

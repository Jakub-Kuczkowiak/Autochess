// Copyright (C) Jakub Kuczkowiak 2017

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Autochess
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        static void AddLog(string log)
        {
            System.IO.StreamWriter sw = null; 

            try
            {
                sw = new System.IO.StreamWriter("kurniklogger.txt", true);
                sw.WriteLine(log);
            }
            catch (Exception)
            {
                //MessageBox.Show("Invalid write log operation. No permission to write file at C:\\");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
            
        }

        // If you want it to be working on different servers such as ChessOK and others all that will be required is to implement this interface.
        interface IServerManager
        {
            //Position GetPawnPosition(string text, bool bAddRelative);
            void SimulateMoveInServer(String sMove);
            void PromotePawn(char sPromotionFigure);
            string GetEnemyPromotionFigure(Position PField, Color colorCondition);
            String GetLastMoveFromServer();
            //bool IsWhiteOrientedLeft();
            //bool IsWhiteMove();

            //String TranslateMoveToBlackViewport(String sMoveToTranslate);
        }

        public class CKurnikManager : IServerManager
        {
            #region CONSTANTS
            public readonly Color WHITECOLOURFIGURE = Color.FromArgb(255, 255, 255);
            public readonly Color BLACKCOLOURFIGURE = Color.FromArgb(17, 34, 17);

            readonly Color WHITECOLOURFIELD = Color.FromArgb(234, 236, 226);
            readonly Color BLACKCOLOURFIELD = Color.FromArgb(87, 170, 101);

            const int PLANSZAWŁAŚCIWAX = 357;
            const int PLANSZAWŁAŚCIWAY = 99;
            const int PLANSZAWŁAŚCIWAX2 = 1252;
            const int PLANSZAWŁAŚCIWAY2 = 994; // teraz nie wiem czy dobrze dobrałem

            const double ROZMIARPOLAX = (PLANSZAWŁAŚCIWAX2 - PLANSZAWŁAŚCIWAX) / (double)8;
            const double ROZMIARPOLAY = (PLANSZAWŁAŚCIWAY2 - PLANSZAWŁAŚCIWAY) / (double)8;

            const int MOVETRIANGLEX = 1565;
            const int MOVETRIANGLEY = 188;
            readonly Color MOVETRIANGLECOLOUR = Color.FromArgb(31, 41, 48);

            const int SIDESQUAREX = 1530;
            const int SIDESQUAREY = 128;
            #endregion

            public Position GetPawnPosition(string text, bool bAddRelative)
            {
                Position position = new Position();

                double halfField = 0;
                if (bAddRelative)
                    halfField = 0.5;

                // Decode in 2 steps (first letters, later numbers).
                double relativeX = (text.ElementAt(0) - 0x61 + halfField) * ROZMIARPOLAX; // 0x61 = 'a'
                double relativeY = (0x38 - text.ElementAt(1) + halfField) * ROZMIARPOLAY; // 0x38 = '8'

                position.x = (int)(PLANSZAWŁAŚCIWAX + relativeX);
                position.y = (int)(PLANSZAWŁAŚCIWAY + relativeY);

                return position;
            }

            public String GetLastMoveFromServer()
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                Bitmap bmp = CaptureDesktop();

                String move = "";

                uint counter = 0;
                bool bBreak = false;
                for (char letter = 'a'; letter <= 'h'; ++letter)
                {
                    for (char number = '1'; number <= '8'; ++number)
                    {
                        string field = letter.ToString() + number.ToString();
                        Position pos = GetPawnPosition(field, false);

                        Color pixelColor = bmp.GetPixel(pos.x + 55, pos.y + (number > '3' ? 1 : 2)); //(number > '2' ? 1 : 2)); // TODO: Dla niektórych + 1 (linie od mojej strony: 8,7,6,5,4) dla innych + 2 (linie od mojej strony: 1,2,3)
                                                                                                     //47 85 45 - to białe pole zaznaczone 17 72 20 - to czarne pole zaznaczone
                        if ((pixelColor.R == 17 && pixelColor.G == 72 && pixelColor.B == 20) || (pixelColor.R == 47 && pixelColor.G == 85 && pixelColor.B == 45)) // Determines 'moving' fields.
                        {
                            // Determine whether it's the field with moved to or from. If it's "from" field, then there is no pawn in the middle of this field.
                            Color determinerColor = bmp.GetPixel(pos.x + 53, pos.y + 53); // 53 as bishops do not have middle point.
                            if (determinerColor == WHITECOLOURFIELD || determinerColor == BLACKCOLOURFIELD)
                                move = field + move; // It's the "from" move.
                            else
                                move += field; // It's the "to" move.

                            // Determine whether it's finished.
                            if (++counter == 2)
                            {
                                bBreak = true;
                                break;
                            }
                        }
                    }

                    if (bBreak)
                        break;
                }

                if (!String.IsNullOrEmpty(move))
                {
                    stopwatch.Stop();
                    //MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
                }

                AddLog(move);

                // TODO: Read promotion in one function?
                return move;
            }

            public void SimulateMoveInServer(String sMove)
            {
                Position positionFrom = GetPawnPosition(sMove.Substring(0, 2), true);
                Position positionTo = GetPawnPosition(sMove.Substring(2, 2), true);

                Mouse mouse = new Mouse();
                mouse.DragItem(positionFrom.x, positionFrom.y, positionTo.x, positionTo.y);
            }

            public void PromotePawn(char sPromotionFigure)
            {
                Mouse mouse = new Mouse();
                int x = 0, y = 340;

                switch (sPromotionFigure)
                {
                    case 'q':
                        MessageBox.Show("My promotion: QUEEN"); // TODO: Remove after checking.
                        x = 630;
                        break;

                    case 'n':
                        MessageBox.Show("My promotion: KNIGHT");// TODO: Remove after checking.
                        x = 748;
                        break;

                    case 'b':
                        MessageBox.Show("My promotion: BISHOP");// TODO: Remove after checking.
                        x = 854;
                        break;

                    case 'r':
                        MessageBox.Show("My promotion: ROOK");// TODO: Remove after checking.
                        x = 968;
                        break;

                    default:
                        x = 0;
                        break;
                }

                mouse.MoveMouse(x, y);
                mouse.LeftButtonClick();
            }

            // TODO: The function must be rechecked due to kurnik.pl's implementation change!
            public string GetEnemyPromotionFigure(Position PField, Color colorCondition)
            {
                return "q";

                Color color = GetPixelColor(PField.x + 18, PField.y + 45);
                if (color == colorCondition) // Is it a queen? (most left circle)
                {
                    MessageBox.Show("Enemy's promotion: QUEEN"); // TODO: Remove after checking.
                    return "q";
                }
                else // Is it a rook? (most left top square)
                {
                    color = GetPixelColor(PField.x + 32, PField.y + 27);
                    if (color == colorCondition)
                    {
                        MessageBox.Show("Enemy's promotion: ROOK"); // TODO: Remove after checking.
                        return "r";
                    }
                    else // Is it a bishop? (highest left point, the cap)
                    {
                        color = GetPixelColor(PField.x + 53, PField.y + 18);
                        if (color == colorCondition)
                        {
                            MessageBox.Show("Enemy's promotion: BISHOP"); // TODO: Remove after checking.
                            return "b";
                        }
                        else // It's a knight.
                        {
                            MessageBox.Show("Enemy's promotion: KNIGHT"); // TODO: Remove after checking.
                            return "n";
                        }
                    }
                }
            }

            public bool IsWhiteOrientedLeft()
            {
                Color color = GetPixelColor(SIDESQUAREX, SIDESQUAREY);
                return (color.R == 255 && color.G == 255 && color.B == 255);
            }

            public bool IsWhiteMove()
            {
                Color color = GetPixelColor(MOVETRIANGLEX, MOVETRIANGLEY); // Down triangle symbolizing move.
                if (IsWhiteOrientedLeft())
                    return (color == MOVETRIANGLECOLOUR); // White are oriented left.
                else
                    return !(color == MOVETRIANGLECOLOUR); // White are oriented right.
            }

            public String TranslateMoveToBlackViewport(String sMoveToTranslate)
            {
                String sRebuiltMove = "";
                for (int i = 0; i < 4; i++)
                {
                    char symbol = sMoveToTranslate.ElementAt(i);

                    if (symbol >= 'a' && symbol <= 'h')
                    {
                        // Algo: 0x61 + (0x68 - lettervalue)
                        sRebuiltMove += (char)(0xC9 - symbol);
                    }
                    else if (symbol >= '1' && symbol <= '8')
                    {
                        // Algo: 0x31 + (0x38 - numbervalue)
                        sRebuiltMove += (char)(0x69 - symbol);
                    }
                }

                if (sMoveToTranslate.Length == 5)
                    sRebuiltMove += sMoveToTranslate.ElementAt(4);

                return sRebuiltMove;
            }

            public MyPieceColour DetermineColour()
            {
                Position whiteKing = GetPawnPosition("e1", true);
                Color color = GetPixelColor(whiteKing.x, whiteKing.y);
                MyPieceColour myColour = MyPieceColour.NotDetermined;

                if (color == WHITECOLOURFIGURE)
                {
                    myColour = MyPieceColour.White;
                }
                else if (color == BLACKCOLOURFIGURE)
                {
                    myColour = MyPieceColour.Black;
                }

                return myColour;
            }
        }

        public class CProgramManager
        {
            public List<string> EnemyPawns = null;
            public List<string> MyPawns = null;
            private MyPieceColour myColour = MyPieceColour.NotDetermined;
            char lineCondition;
            Color colorCondition;

            // 
            public CProgramManager(MyPieceColour myColour, ref CKurnikManager KurnikManager)
            {
                EnemyPawns = new List<string>();
                MyPawns = new List<string>();
                for (char c = 'a'; c <= 'h'; ++c)
                {
                    if (myColour == MyPieceColour.White)
                    {
                        MyPawns.Add(c + "2");
                        EnemyPawns.Add(c + "7");
                    }
                    else if (myColour == MyPieceColour.Black)
                    {
                        MyPawns.Add(c + "7");
                        EnemyPawns.Add(c + "2");
                    }
                }

                lineCondition = (myColour == MyPieceColour.White ? '1' : '8');
                colorCondition = (myColour == MyPieceColour.White ? KurnikManager.BLACKCOLOURFIGURE : KurnikManager.WHITECOLOURFIGURE);
            }

            public void UpdatePawnsAfterMyMove(string myMove, string previousEnemyMove)
            {
                // Check whether the move you've made belongs to your pawn and handle enemy pawns.
                for (int i = 0; i < MyPawns.Count(); i++)
                {
                    if (MyPawns.ElementAt(i) == myMove.Substring(0, 2))
                    {
                        if (myMove.Length == 5)
                        {
                            // The pawn has been promoted.
                            MyPawns.RemoveAt(i);
                        }
                        else
                        {
                            MyPawns[i] = myMove.Substring(2, 2);

                            if (myMove.Substring(0, 1) != myMove.Substring(2, 1)) // The 'from letter' is different than 'to letter' etc.. It's for sure a capture.
                            {
                                bool bCanBeEnPassent = (myMove.Substring(1, 1) == (myColour == MyPieceColour.White ? "5" : "4")) // Our pawn from number must be on 5th (black 4th) line.
                                    && (myMove.Substring(2, 1) == previousEnemyMove.Substring(2, 1)) // Our pawn 'letter to' must be equal to previous enemy move 'letter to'
                                    && (previousEnemyMove.Substring(1, 1) == (myColour == MyPieceColour.White ? "7" : "2")) // Previous enemy move 'number from' must be 7 (2 for black)
                                    && (previousEnemyMove.Substring(3, 1) == (myColour == MyPieceColour.White ? "5" : "4")); // Previous enemy move 'number to' must be 5 (4 for black)

                                if (bCanBeEnPassent)
                                {
                                    for (int j = 0; j < EnemyPawns.Count; j++)
                                    {
                                        if (EnemyPawns.ElementAt(j) == previousEnemyMove.Substring(2, 2)) // Is it "en passent"?
                                        {
                                            EnemyPawns.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                }

                // Checks for enemy's taken pawns.
                for (int i = 0; i < EnemyPawns.Count; i++)
                {
                    if (myMove.Substring(2, 2) == EnemyPawns.ElementAt(i))
                    {
                        EnemyPawns.RemoveAt(i);
                        break;
                    }
                }
            }

            // TODO: Modify and plan the interfaces better way! Reorganize.
            public void UpdatePawnsAfterEnemyMove(string SEnemyMove, string SMoveToSendToKurnik, ref CKurnikManager KurnikManager)
            {
                //TODO: Check if the promotion checking is right for all figures!
                //Check if needs promotion and handle pawns events like enpassent.
                for (int i = 0; i < EnemyPawns.Count(); i++)
                {
                    if (EnemyPawns.ElementAt(i) == SEnemyMove.Substring(0, 2)) // =movefrom
                    {
                        // CHECKED!
                        //Console.Beep(500, 1000);
                        if (SEnemyMove.ElementAt(3) == lineCondition) // =moveto
                        {
                            // TODO: UNCHECKED!
                            // Promotion has been done. Find the figure.
                            Position positionToRead = KurnikManager.GetPawnPosition(SEnemyMove.Substring(2, 2), false);
                            SEnemyMove += KurnikManager.GetEnemyPromotionFigure(positionToRead, colorCondition);

                            // Remove it from Pawns, as it is not anymore a pawn.
                            EnemyPawns.RemoveAt(i);
                        }
                        else
                        {
                            EnemyPawns[i] = SEnemyMove.Substring(2, 2); // =moveto

                            if (SEnemyMove.Substring(0, 1) != SEnemyMove.Substring(2, 1)) // It's for sure a capture as letters differ.
                            {
                                bool bCanBeEnPassent = (SEnemyMove.Substring(1, 1) == (myColour == MyPieceColour.White ? "4" : "5")) && (SMoveToSendToKurnik.Substring(2, 1) == SEnemyMove.Substring(2, 1)) && (SMoveToSendToKurnik.Substring(1, 1) == (myColour == MyPieceColour.White ? "2" : "7")) && (SMoveToSendToKurnik.Substring(3, 1) == (myColour == MyPieceColour.White ? "4" : "5"));

                                if (bCanBeEnPassent)
                                {
                                    for (int j = 0; j < MyPawns.Count; j++)
                                    {
                                        if (MyPawns.ElementAt(j) == SMoveToSendToKurnik.Substring(2, 2))
                                        {
                                            // TODO: UNCHECKED!
                                            MessageBox.Show("Enemy's En Passent.");
                                            MyPawns.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                }

                // Determines whether my pawn was taken (exclude en passent).
                for (int i = 0; i < MyPawns.Count; i++)
                {
                    if (SEnemyMove.Substring(2, 2) == MyPawns.ElementAt(i))
                    {
                        //CHECKED!
                        //Console.Beep(500, 1000);
                        MyPawns.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetAsyncKeyState(int vKey);

        String sMoves = "";

        public struct Position
        {
            public int x;
            public int y;

            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        #region COLOUR
        public struct SIZE
        {
            public int Cx;
            public int Cy;
        }

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);


        [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
        public static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", EntryPoint = "CopyIcon")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", EntryPoint = "GetIconInfo")]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("user32.dll", EntryPoint = "DestroyIcon")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool fIcon;         // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
            public Int32 xHotspot;     // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
            public Int32 yHotspot;     // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
            public IntPtr hbmMask;     // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
            public IntPtr hbmColor;    // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
            public IntPtr hCursor;          // Handle to the cursor. 
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
        }

        [DllImport("gdi32.dll", EntryPoint = "CreateDC")]
        public static extern IntPtr CreateDC(IntPtr lpszDriver, string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest,
                                         int yDest, int wDest,
                                         int hDest, IntPtr hdcSource,
                                         int xSrc, int ySrc, int rasterOp);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap
                                    (IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);

        public const int SRCCOPY = 13369376;

        static public Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);

            return color;
        }

        public static Bitmap CaptureDesktop()
        {
            Bitmap bmp = null;
            //			lock (_lock)
            {
                IntPtr hDC = IntPtr.Zero;
                try
                {
                    SIZE size;
                    hDC = GetDC(GetDesktopWindow());
                    IntPtr hMemDC = CreateCompatibleDC(hDC);

                    size.Cx = GetSystemMetrics
                              (SM_CXSCREEN);

                    size.Cy = GetSystemMetrics
                              (SM_CYSCREEN);

                    IntPtr hBitmap = CreateCompatibleBitmap(hDC, size.Cx, size.Cy);

                    if (hBitmap != IntPtr.Zero)
                    {
                        IntPtr hOld = SelectObject
                            (hMemDC, hBitmap);

                        BitBlt(hMemDC, 0, 0, size.Cx, size.Cy, hDC,
                                                       0, 0, SRCCOPY);

                        SelectObject(hMemDC, hOld);
                        DeleteDC(hMemDC);
                        bmp = Image.FromHbitmap(hBitmap);
                        DeleteObject(hBitmap);
                        GC.Collect();
                    }
                }
                finally
                {
                    if (hDC != IntPtr.Zero)
                    {
                        ReleaseDC(GetDesktopWindow(), hDC);
                    }
                }
            }
            return bmp;
        }
        #endregion

        //public String GetLastMoveFromServerOLD()
        //{
        //    Stopwatch stopwatch = Stopwatch.StartNew();

        //    String move = "";

        //    uint counter = 0;
        //    bool bBreak = false;
        //    for (char letter = 'a'; letter <= 'h'; ++letter)
        //    {
        //        for (char number = '1'; number <= '8'; ++number)
        //        {
        //            string field = letter.ToString() + number.ToString();
        //            Position pos = GetPawnPosition(field, false);

        //            Color pixelColor = GetPixelColor(pos.x + 55, pos.y + (number > '3' ? 1 : 2)); //(number > '2' ? 1 : 2)); // TODO: Dla niektórych + 1 (linie od mojej strony: 8,7,6,5,4) dla innych + 2 (linie od mojej strony: 1,2,3)
        //            //47 85 45 - to białe pole zaznaczone 17 72 20 - to czarne pole zaznaczone
        //            if ((pixelColor.R == 17 && pixelColor.G == 72 && pixelColor.B == 20) || (pixelColor.R == 47 && pixelColor.G == 85 && pixelColor.B == 45)) // Determines 'moving' fields.
        //            {
        //                // Determine whether it's the field with moved to or from. If it's "from" field, then there is no pawn in the middle of this field.
        //                Color determinerColor = GetPixelColor(pos.x + 53, pos.y + 53); // 53 as bishops do not have middle point.
        //                if (determinerColor == WHITECOLOURFIELD || determinerColor == BLACKCOLOURFIELD)
        //                    move = field + move; // It's the "from" move.
        //                else
        //                    move += field; // It's the "to" move.

        //                // Determine whether it's finished.
        //                if (++counter == 2)
        //                {
        //                    bBreak = true;
        //                    break;
        //                }
        //            }
        //        }

        //        if (bBreak)
        //            break;
        //    }

        //    if (!String.IsNullOrEmpty(move))
        //    {
        //        stopwatch.Stop();
        //        MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
        //    }

        //    AddLog(move);

        //    // TODO: Read promotion in one function?
        //    return move;
        //}

        #region Openings Book System
        public class ChessBook
        {
            private string book = "";
            private int currentMoveNumber = 1;

            public String GetBook() { return book; }

            public void ClearBook()
            {
                String SRecreatingBook = "";

                foreach (char c in book.ToLower())
                {
                    if ((c >= 'a' && c <= 'h') || (c >= '0' && c <= '9') || (c == '#') || (c == '.'))
                    {
                        SRecreatingBook += c;
                    }
                    else if (c == 'q' || c == 'r' || c == 'n') // We miss 'b' as it's included above.
                    {
                        throw new Exception("Promotion is not yet implemented inside openings.");
                    }
                }
            }

            public int RandomizeNumber(int maxRange)
            {
                Random random = new Random();
                return random.Next(maxRange);
            }

            public void ReadBookFile(string path)
            {
                try
                {
                    book = System.IO.File.ReadAllText(path);
                }
                catch (Exception)
                {
                    throw new Exception("Could not read file.");
                }
            }

            // Returns false when the book is over and no moves are to be read.
            public bool GetAvailableMoves(out List<string> AvailableMovesList)
            {
                String SMoveFormat = "#" + currentMoveNumber.ToString() + ".";
                int MovesBegin = book.IndexOf(SMoveFormat);

                // When no move found, finish playing from the book.
                if (MovesBegin == -1)
                {
                    AvailableMovesList = null;
                    return false;
                }

                // Create a new list and add the first move to the list.
                AvailableMovesList = new List<string> { book.Substring(MovesBegin + SMoveFormat.Length, 4) };

                // Add the rest of the moves to the list.
                int LastSearchIndex = MovesBegin;
                do
                {
                    int index = book.IndexOf(SMoveFormat, LastSearchIndex + 1);

                    // Check if the list of available moves is completed.
                    if (index == -1)
                        break;

                    AvailableMovesList.Add(book.Substring(index + SMoveFormat.Length, 4));
                    LastSearchIndex = index;
                } while (true);

                return true;
            }

            public void SetMoveAsPlayed(string SMove)
            {
                String SMoveFormat = "#" + currentMoveNumber.ToString() + ".";
                int selectedMoveBeginIndex = book.IndexOf(SMoveFormat + SMove) + SMoveFormat.Length + 4;
                int selectedMoveEndIndex = book.IndexOf(SMoveFormat, selectedMoveBeginIndex);

                if (selectedMoveEndIndex != -1)
                    book = book.Substring(selectedMoveBeginIndex, selectedMoveEndIndex - selectedMoveBeginIndex);
                else
                    book = book.Substring(selectedMoveBeginIndex);

                ++currentMoveNumber;
            }

            // Returns false when no enemy move found in the book.
            public bool SetEnemyMoveAsPlayed(string SEnemyMove)
            {
                String SMoveFormat = "#" + currentMoveNumber.ToString() + ".";
                int LastSearchIndex = 0;
                do
                {
                    int beginIndex = book.IndexOf(SMoveFormat, LastSearchIndex);
                    if (beginIndex == -1)
                    {
                        return false;
                    }

                    // Read followings move from the book and compare with enemy response from kurnik.pl
                    String EnemyBookMove = book.Substring(beginIndex + SMoveFormat.Length, 4);
                    if (SEnemyMove == EnemyBookMove)
                    {
                        // The response equals. Truncate the book again.
                        int endIndex = book.IndexOf(SMoveFormat, beginIndex + 1);
                        beginIndex += SMoveFormat.Length + 4;

                        // Cut the book correctly, depending if we cut in the middle or at the end.
                        if (endIndex != -1)
                            book = book.Substring(beginIndex, endIndex - beginIndex);
                        else
                            book = book.Substring(beginIndex);

                        currentMoveNumber++;

                        return true;
                    }

                    LastSearchIndex = beginIndex + 1;
                } while (true);
            }
        }
        #endregion


        string data = "";
        string lastLine = "";

        public enum MyPieceColour
        {
            White, Black, NotDetermined
        };

        MyPieceColour myColour = MyPieceColour.NotDetermined;
        private void btnStartEngine_Click(object sender, EventArgs e)
        {
            // Here we will verify whether the graphics are correctly defined.
            // This program works in 1920 x 1080 resolution only now!
            if (Screen.PrimaryScreen.Bounds.Width != 1920 || Screen.PrimaryScreen.Bounds.Height != 1080)
            {
                MessageBox.Show("Change the resolution to NATIVE Full HD (1920 x 1080). Please, notice that scaling might not work when using native 4K!", "Screen resolution", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.WindowState = FormWindowState.Minimized;

            CKurnikManager KurnikManager = new CKurnikManager();

            // Scan what colour we are playing.
            if ((myColour = KurnikManager.DetermineColour()) == MyPieceColour.NotDetermined)
            {
                MessageBox.Show("Could not determine the colour of yours pieces!", "Colour of pieces not determined!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.WindowState = FormWindowState.Normal;
                return;
            }

            // Disable mesh.
            tmrHotkeys.Stop();

            // Initialize variables.
            CProgramManager ProgramManager = new CProgramManager(myColour, ref KurnikManager);

            //char lineCondition = (myColour == MyPieceColour.White ? '1' : '8');
            //Color colorCondition = (myColour == MyPieceColour.White ? KurnikManager.BLACKCOLOURFIGURE : KurnikManager.WHITECOLOURFIGURE);

            string strEnginePath = null;

            // Determine the engine.
            if (rdoKomodo.Checked)
            {
                strEnginePath = System.IO.Path.Combine(Application.StartupPath, "komodo-8-64bit.exe");
            }

            // This is part for both white and black.
            ProcessStartInfo psi = new ProcessStartInfo(strEnginePath);
            psi.RedirectStandardInput = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            Process process = Process.Start(psi);

            bool bCancelLastPonderedMove = false;

            process.OutputDataReceived += new DataReceivedEventHandler((s, eventHandler) =>
             {
                 if (eventHandler.Data == null)
                 {
                     MessageBox.Show("EventHandler.data is null! Check for error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     AddLog("Error: EventHandler.data is null!");
                     return;
                 }

                 data += eventHandler.Data;

                 if (bCancelLastPonderedMove && eventHandler.Data.StartsWith("bestmove")) // TODO: Inspect fact that eventHandler can become null in the game!
                 {
                     bCancelLastPonderedMove = false;
                     lastLine = ""; // TODO: Is needed?

                     // Console.Beep(5000, 10);
                 }
                 else
                 {
                     lastLine = eventHandler.Data;
                 }
             });

            process.BeginOutputReadLine();

            // Set the options.
            string sTime = myColour == MyPieceColour.White ? "wtime " : "btime "; // set the time paramter.

            int EngineTimeLeft = Convert.ToInt32(txtTime.Text);
            
            // Pondering (engine must implement it)
            if (chkPondering.Checked)
                process.StandardInput.WriteLine("setoption name Ponder value true");

            // Set the threads
            process.StandardInput.WriteLine("setoption name Threads value 8");

            // These might be used to reduce strength of the engine.
            //process.StandardInput.WriteLine("setoption name UCI_LimitStrength value true");
            //process.StandardInput.WriteLine("setoption name UCI_Elo value 50");
            
            uint uContemptValue = 1;
            if (rdoContempt_0.Checked)
                uContemptValue = 0;
            else if (rdoContempt_2.Checked)
                uContemptValue = 2;

            process.StandardInput.WriteLine("setoption name Contempt value " + uContemptValue.ToString());

            // Read the book into memory and truncate it after every move.
            bool bIsBookAvailable = false;
            ChessBook ChessBookManager = new ChessBook();

            if (chkOpeningsBook_UseOpeningsBook.Checked)
            {
                try
                {
                    ChessBookManager.ReadBookFile(myColour == MyPieceColour.White ? "white.txt" : "black.txt");
                    ChessBookManager.ClearBook();
                    bIsBookAvailable = true;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("Could not read Opening Book file! Reason: " + ex.Message + "\n" + "Continue without book?", "Read file error!", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        process.Kill();
                        return; // TODO: Clear the process.
                    }
                }
            }

            String previousEnemyMove = "";

            // If you are black, then you must first read the move from server.
            bool bContinueBook = true;
            if (myColour == MyPieceColour.Black)
            {
                String lastMove = "";
                while (lastMove == "")
                {
                    if (!KurnikManager.IsWhiteMove())
                    {
                        lastMove = KurnikManager.GetLastMoveFromServer();
                        lastMove = KurnikManager.TranslateMoveToBlackViewport(lastMove);
                        sMoves += lastMove + " ";
                        txtMoves.Text = sMoves;
                    }

                    if (lastMove != "")
                    {
                        ProgramManager.UpdatePawnsAfterEnemyMove(lastMove, "xxxx", ref KurnikManager); // We call xxxx in order to make sure en passent don't get executed as this is the first move.
                        break;
                    }
                }

                previousEnemyMove = lastMove;

                // Compare enemy response with variants in the book.
                bContinueBook = ChessBookManager.SetEnemyMoveAsPlayed(lastMove);
            }

            // Openings Read System (WHITE ONLY NOW)
            if (bIsBookAvailable && bContinueBook)
            {
                // The variable keeps information if we want to select each move or leave it for choosing by PC. It's disabled by default.
                // TODO: Ask user before round if he wants to randomize!
                bool bRandomize = rdoOpeningsBook_RandomizeMoves.Checked;

                // Read our moves list and let the user choose.
                do
                {
                    List<string> AvailableMovesList = null;
                    if (ChessBookManager.GetAvailableMoves(out AvailableMovesList) == false)
                    {
                        break;
                    }

                    // If the list contains more than 1 item, ask the user what move to choose.
                    String SMoveToSendToKurnik = "";
                    if (AvailableMovesList.Count() > 1)
                    {
                        if (!bRandomize)
                        {
                            // TODO: Make it top most... To appear high high on top.
                            dlgAskMove dlgChooseMove = new dlgAskMove(AvailableMovesList);
                            switch (dlgChooseMove.ShowDialog())
                            {
                                case DialogResult.Abort: // This is returned when the move is NOT chosen and "Never ask again" was checked.
                                    bRandomize = true;
                                    SMoveToSendToKurnik = AvailableMovesList.ElementAt(ChessBookManager.RandomizeNumber(AvailableMovesList.Count));
                                    break;
                                case DialogResult.Cancel: // This is returned when the move is NOT chosen and "Never ask again" was unchecked.
                                    SMoveToSendToKurnik = AvailableMovesList.ElementAt(ChessBookManager.RandomizeNumber(AvailableMovesList.Count));
                                    break;
                                case DialogResult.Yes: // This is returned when the move is chosen and "Never ask again" was checked.
                                    bRandomize = true;
                                    SMoveToSendToKurnik = dlgChooseMove.GetSelectedMove();
                                    break;
                                case DialogResult.OK: // This is returned when the move is chosen and "Never ask again" was unchecked.
                                    SMoveToSendToKurnik = dlgChooseMove.GetSelectedMove();
                                    break;

                                default:
                                    throw new Exception("The dialog response was different. Cannot reach that code here!"); // TODO: Should delete or warn reader though?
                            }
                        }
                        else
                        {
                            SMoveToSendToKurnik = AvailableMovesList.ElementAt(ChessBookManager.RandomizeNumber(AvailableMovesList.Count));
                        }
                    }
                    else // Only 1 move.
                    {
                        SMoveToSendToKurnik = AvailableMovesList.ElementAt(0);
                    }

                    sMoves += SMoveToSendToKurnik + " ";
                    string SOriginalMoveToSendToKurnik = SMoveToSendToKurnik;
                    if (myColour == MyPieceColour.Black)
                    {
                        SMoveToSendToKurnik = KurnikManager.TranslateMoveToBlackViewport(SMoveToSendToKurnik);
                    }

                    // Make move in kurnik.pl here.
                    // TODO: Handle promoting inside book!
                    do
                    {
                        KurnikManager.SimulateMoveInServer(SMoveToSendToKurnik);

                        if (SMoveToSendToKurnik.Length == 5) // Handles promoting.
                            KurnikManager.PromotePawn(SMoveToSendToKurnik.ElementAt(4));

                        bool bWhiteMove = KurnikManager.IsWhiteMove();

                        if ((myColour == MyPieceColour.White && !bWhiteMove) || (myColour == MyPieceColour.Black && bWhiteMove))
                            break;

                    } while (true);

                    // Updates pawns
                    ProgramManager.UpdatePawnsAfterMyMove(SOriginalMoveToSendToKurnik, previousEnemyMove);

                    // Truncate the "book" with a selected move.
                    ChessBookManager.SetMoveAsPlayed(SOriginalMoveToSendToKurnik);

                    // Get enemy response from kurnik.pl here.
                    String SEnemyMove = "";

                    do
                    {
                        bool bIsWhiteMove = KurnikManager.IsWhiteMove();

                        if (myColour == MyPieceColour.White && bIsWhiteMove)
                        {
                            SEnemyMove = KurnikManager.GetLastMoveFromServer();
                        }
                        else if (myColour == MyPieceColour.Black && !bIsWhiteMove)
                        {
                            SEnemyMove = KurnikManager.GetLastMoveFromServer();
                            SEnemyMove = KurnikManager.TranslateMoveToBlackViewport(SEnemyMove);
                        }

                        if (SEnemyMove != "")
                        {
                            ProgramManager.UpdatePawnsAfterEnemyMove(SEnemyMove, SMoveToSendToKurnik, ref KurnikManager);
                        }
                    } while (SEnemyMove == "");
                    sMoves += SEnemyMove + " ";

                    previousEnemyMove = SEnemyMove;

                    // Compare enemy response with variants in the book.
                    if (ChessBookManager.SetEnemyMoveAsPlayed(SEnemyMove) == false)
                    {
                        break;
                    }
                } while (true);
            }

            // Start the game.
            process.StandardInput.WriteLine("position startpos moves " + sMoves); // set the position to startup.
            process.StandardInput.WriteLine("go " + sTime + EngineTimeLeft);

            int EngineThinkingTimeStarted = Environment.TickCount;

            while (true)
            {
                String lastEnemyMove = "";
                if (lastLine.StartsWith("bestmove"))
                {
                    EngineTimeLeft -= Environment.TickCount - EngineThinkingTimeStarted;

                    // Read the best move.
                    int bestmoveIndex = lastLine.IndexOf("bestmove");
                    string MoveToSendToServer = lastLine.Substring(9, 5);

                    MoveToSendToServer = MoveToSendToServer.Replace(" ", "");
                    sMoves += MoveToSendToServer + " ";

                    txtMoves.Text = sMoves;

                    // Translate the move for the right side.
                    String MoveToSendToServerTranslated = MoveToSendToServer;
                    if (myColour == MyPieceColour.Black)
                    {
                        MoveToSendToServerTranslated = KurnikManager.TranslateMoveToBlackViewport(MoveToSendToServerTranslated);
                    }

                    // TODO: Check if we won here.

                    // TODO: You need to make sure, that the opponent has not done the move before we got in code here.
                    do
                    {
                        KurnikManager.SimulateMoveInServer(MoveToSendToServerTranslated);

                        if (MoveToSendToServerTranslated.Length == 5) // Handles promoting.
                            KurnikManager.PromotePawn(MoveToSendToServerTranslated.ElementAt(4));

                        bool bWhiteMove = KurnikManager.IsWhiteMove();

                        if ((myColour == MyPieceColour.White && !bWhiteMove) || (myColour == MyPieceColour.Black && bWhiteMove))
                            break;

                    } while (true);

                    // Start pondering (if selected).
                    string ponderedMove = "";
                    if (chkPondering.Checked)
                    {
                        // TODO: Cover Q K N P here too...
                        int ponderingIndex = lastLine.IndexOf("ponder");
                        ponderedMove = lastLine.Substring(ponderingIndex + 7);
                        AddLog("PONDER: " + ponderedMove);

                        // Send 'pondered' position in the engine.
                        process.StandardInput.WriteLine("position startpos moves " + sMoves + ponderedMove);

                        //process.StandardInput.WriteLine("setoption name Threads value 4");
                        process.StandardInput.WriteLine("go ponder " + sTime + EngineTimeLeft.ToString());
                    }

                    ProgramManager.UpdatePawnsAfterMyMove(MoveToSendToServer, previousEnemyMove);

                    // TODO: Should we still continue? Check if we won here. :-)
                    // Get the reponse.
                    do
                    {
                        bool bIsWhiteMove = KurnikManager.IsWhiteMove();

                        if (myColour == MyPieceColour.White && bIsWhiteMove)
                        {
                            lastEnemyMove = KurnikManager.GetLastMoveFromServer();
                        }
                        else if (myColour == MyPieceColour.Black && !bIsWhiteMove)
                        {
                            lastEnemyMove = KurnikManager.GetLastMoveFromServer();
                            lastEnemyMove = KurnikManager.TranslateMoveToBlackViewport(lastEnemyMove);
                        }

                        if (lastEnemyMove != "")
                        {
                            // TODO: Check if the promotion checking is right for all figures!
                            // Check if needs promotion and handle pawns events like enpassent.
                            for (int i = 0; i < ProgramManager.EnemyPawns.Count(); i++)
                            {
                                if (ProgramManager.EnemyPawns.ElementAt(i) == lastEnemyMove.Substring(0, 2)) // =movefrom
                                {
                                    char lineCondition = (myColour == MyPieceColour.White ? '1' : '8');
                                    Color colorCondition = (myColour == MyPieceColour.White ? KurnikManager.BLACKCOLOURFIGURE : KurnikManager.WHITECOLOURFIGURE);

                                    if (lastEnemyMove.ElementAt(3) == lineCondition) // =moveto
                                    {
// TODO: UNCHECKED!
                                        // Promotion has been done. Find the figure.
                                        Position positionToRead = KurnikManager.GetPawnPosition(lastEnemyMove.Substring(2, 2), false);
                                        lastEnemyMove += KurnikManager.GetEnemyPromotionFigure(positionToRead, colorCondition);

                                        // Remove it from Pawns, as it is not anymore a pawn.
                                        ProgramManager.EnemyPawns.RemoveAt(i);
                                    }
                                    else
                                    {
                                        ProgramManager.EnemyPawns[i] = lastEnemyMove.Substring(2, 2); // =moveto

                                        if (lastEnemyMove.Substring(0, 1) != lastEnemyMove.Substring(2, 1)) // It's for sure a capture as letters differ.
                                        {
                                            bool bCanBeEnPassent = (lastEnemyMove.Substring(1, 1) == (myColour == MyPieceColour.White ? "4" : "5")) && (MoveToSendToServer.Substring(2, 1) == lastEnemyMove.Substring(2, 1)) && (MoveToSendToServer.Substring(1, 1) == (myColour == MyPieceColour.White ? "2" : "7")) && (MoveToSendToServer.Substring(3, 1) == (myColour == MyPieceColour.White ? "4" : "5"));

                                            if (bCanBeEnPassent)
                                            {
                                                for (int j = 0; j < ProgramManager.MyPawns.Count; j++)
                                                {
                                                    if (ProgramManager.MyPawns.ElementAt(j) == MoveToSendToServer.Substring(2, 2))
                                                    {
// TODO: UNCHECKED!
                                                        MessageBox.Show("Enemy's En Passent.");
                                                        ProgramManager.MyPawns.RemoveAt(j);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    break;
                                }
                            }

                            // Determines whether my pawn was directly taken (exclude en passent).
                            for (int i = 0; i < ProgramManager.MyPawns.Count; i++)
                            {
                                if (lastEnemyMove.Substring(2, 2) == ProgramManager.MyPawns.ElementAt(i))
                                {
                                    ProgramManager.MyPawns.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    } while (lastEnemyMove == "");

                    previousEnemyMove = lastEnemyMove;

                    sMoves += lastEnemyMove + " ";
                    txtMoves.Text = sMoves;

                    lastLine = "";
                    EngineThinkingTimeStarted = Environment.TickCount;

                    // Handles last step of pondering.
                    if (chkPondering.Checked)
                    {
                        if (lastEnemyMove == ponderedMove)
                        {
                            process.StandardInput.WriteLine("ponderhit");
                        }
                        else
                        {
                            process.StandardInput.WriteLine("stop");

                            bCancelLastPonderedMove = true;

                            process.StandardInput.WriteLine("position startpos moves " + sMoves);
                            process.StandardInput.WriteLine("go " + sTime + EngineTimeLeft.ToString());
                        }
                    }
                    else
                    {
                        process.StandardInput.WriteLine("position startpos moves " + sMoves);
                        process.StandardInput.WriteLine("go " + sTime + EngineTimeLeft.ToString());
                    }
                }
            }
        } 

        const int VK_B = 0x42;
        const int VK_W = 0x57;
        const int VK_F2 = 0x71;
        const int VK_ESCAPE = 0x1B;
        private void tmrHotkeys_Tick(object sender, EventArgs e)
        {
            if (GetAsyncKeyState(VK_F2) != 0)
            {
                Console.Beep(1000, 100);
                btnStartEngine_Click(sender, e);
            }
        }

        private void tmrPause_Tick(object sender, EventArgs e)
        {
            if (GetAsyncKeyState(VK_ESCAPE) != 0)
            {
                //process.Close();
                // TODO: "quit" command MUST be sent.
                Application.Restart();
            }
        }

        private void chkOpeningsBook_UseOpeningsBook_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOpeningsBook_UseOpeningsBook.Checked)
            {
                rdoOpeningsBook_AskAboutMoves.Enabled = true;
                rdoOpeningsBook_RandomizeMoves.Enabled = true;
            }
            else
            {
                rdoOpeningsBook_AskAboutMoves.Enabled = false;
                rdoOpeningsBook_RandomizeMoves.Enabled = false;
            }
        }
    }
}

using System;
namespace ChessNumbers
   {
   using Game;

   class Program
      {
      static void Main (string[] args)
         {
         Board gameboard = new Board (7);

         foreach (int piece in Enum.GetValues (typeof (Board.PlayerType)))
            {
            gameboard.AddPiece (new Piece (piece));
            }

         int[] results = gameboard.Play ();

         for (int i = 0; i < results.Length; i++)
            {
            Console.WriteLine (String.Format ("{0}: {1} possible phone numbers.", Enum.GetName (typeof (Board.PlayerType), i), results[i]));
            }

         Console.WriteLine ("Press any key to close.");
         Console.ReadKey ();
         }
      }
   }

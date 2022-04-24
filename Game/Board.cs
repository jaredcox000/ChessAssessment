using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessNumbers.Game
   {
   class Board
      {
      public enum PlayerType
         {
         PAWN = 0,
         ROOK = 1,
         KNIGHT = 2,
         BISHOP = 3,
         QUEEN = 4,
         KING = 5
         }

      char[,] spaces;
      int boardWidth = 3;
      int boardHeight = 4;
      int startX;
      int startY;
      int initialStartX;
      int initialStartY;
      int phoneNumberLength;

      List<Piece> players;
      int[] numbers;
      string phoneNumber;

      public Board ()
         {
         phoneNumberLength = 7;

         InitializeBoard ();
         }

      public Board (int pnLength)
         {
         phoneNumberLength = pnLength;

         InitializeBoard ();
         }

      #region Rules

      // These get concatenated together when checking for a valid start space.

      char[] invalid = { '*', '#' };
      char[] invalidStart = { '0', '1' };

      #endregion

      public void AddPiece (Piece player)
         {
         player.SetPosition (startX, startY);
         players.Add (player);
         }

      private void InitializeBoard ()
         {
         bool breakOut = false;

         // Board construction is hard-coded due to time-constraints.

         spaces = new char[,] { { '1', '2', '3' }, { '4', '5', '6' }, { '7', '8', '9' }, { '*', '0', '#' } };

         players = new List<Piece> ();
         numbers = new int[Enum.GetValues (typeof (PlayerType)).Length];

         // Find the first valid starting point.

         for (int i = 0; i < boardHeight; i++)
            {
            for (int j = 0; j < boardWidth; j++)
               {
               if (!(invalid.Concat (invalidStart).Contains (spaces[i, j])))
                  {
                  startX = j;
                  startY = i;

                  breakOut = true;
                  break;
                  }
               }
            if (breakOut)
               break;
            }

         // Save where we first started so we know when to stop. We'll iterate through the board until
         // we end up where we started.

         initialStartX = startX;
         initialStartY = startY;
         phoneNumber = spaces[startY, startX].ToString ();

         // Finally, initialize each count of phone numbers.

         for (int i = 0; i < numbers.Length; i++)
            numbers[i] = 0;
         }

      /// <summary>
      /// Determines if the space we're trying to move to can be contained in a phone number.
      /// 
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      private bool IsSpaceInvalid (int x, int y)
         {
         return (invalid.Contains (spaces[y, x]));
         }

      /// <summary>
      /// Validates whether or not the space we're trying to move to is legal.
      /// 
      /// </summary>
      /// <param name="player"></param>
      /// <param name="currentNumber"></param>
      /// <param name="newX"></param>
      /// <param name="newY"></param>
      private void ValidateMove (Piece player, string currentNumber, int newX, int newY)
         {
         if (IsSpaceInvalid (newX, newY))
            return;

         player.SetPosition (newX, newY);
         currentNumber += spaces[player.BoardY, player.BoardX];
         PhoneBook (player, currentNumber);
         }

      /// <summary>
      /// Handles moving the chess piece to another space on the board. Will validate the legality
      /// of the space to move to, and recursively checks all possible moves from the current space
      /// in order to call every possible phone number known to mankind that a chess piece can create
      /// using its unique movement rules.
      /// 
      /// </summary>
      /// <param name="player"></param>
      /// <param name="currentNumber"></param>
      private void PhoneBook (Piece player, string currentNumber)
         {
         // Phone number is complete?

         if (currentNumber.Length == phoneNumberLength)
            {
            numbers[player.Type]++;
            return;
            }

         switch (player.Type)
            {
            case (int) PlayerType.PAWN:
               // A pawn can only move forward vertically.

               int pawnX = player.BoardX, pawnY = player.BoardY;

               // Move up one space.

               if (pawnY - 1 >= 0)
                  ValidateMove (player, currentNumber, pawnX, pawnY - 1);
               break;
            case (int) PlayerType.ROOK:
               // A rook can move horizontally or diagonally, as far as it wants.

               int rookX = player.BoardX, rookY = player.BoardY;

               // Iterate over all horizontal movements to the right.

               for (int i = rookX + 1; i < boardWidth; i++)
                  {
                  ValidateMove (player, currentNumber, i, rookY);
                  }

               // Iterate over all horizontal movements to the left.

               for (int i = rookX - 1; i >= 0; i--)
                  {
                  ValidateMove (player, currentNumber, i, rookY);
                  }

               // Iterate over all vertical movements down.

               for (int i = rookY + 1; i < boardHeight; i++)
                  ValidateMove (player, currentNumber, rookX, i);

               // Iterate over all vertical movements down.

               for (int i = rookY - 1; i >= 0; i--)
                  ValidateMove (player, currentNumber, rookX, i);

               break;

            case (int) PlayerType.KNIGHT:
               // Knights have a funky movement pattern. They move up or down 1 or 2 squares,
               // then left or right the opposite number of squares. (e.g. up 1, right 2. down 2, right 1.)

               int knightX = player.BoardX, knightY = player.BoardY;

               // Can we move up 2?

               if (knightY - 2 >= 0)
                  {
                  // And right 1?

                  if (knightX + 1 < boardWidth)
                     ValidateMove (player, currentNumber, knightX + 1, knightY - 2);

                  // And left 1?

                  if (knightX - 1 > 0)
                     ValidateMove (player, currentNumber, knightX - 1, knightY - 2);
                  }

               // Can we move down 2?

               if (knightY + 2 < boardHeight)
                  {
                  // And right 1?

                  if (knightX + 1 < boardWidth)
                     ValidateMove (player, currentNumber, knightX + 1, knightY + 2);

                  // And left 1?

                  if (knightX - 1 >= 0)
                     ValidateMove (player, currentNumber, knightX - 1, knightY + 2);
                  }

               // Can we move up 1?

               if (knightY - 1 >= 0)
                  {
                  // And right 2?

                  if (knightX + 2 < boardWidth)
                     ValidateMove (player, currentNumber, knightX + 2, knightY - 1);

                  // And left 2?

                  if (knightX - 2 > 0)
                     ValidateMove (player, currentNumber, knightX - 2, knightY - 1);
                  }

               // Can we move down 1?

               if (knightY + 1 < boardHeight)
                  {
                  // And right 2?

                  if (knightX + 2 < boardWidth)
                     ValidateMove (player, currentNumber, knightX + 2, knightY + 1);

                  // And left 2?

                  if (knightX - 2 >= 0)
                     ValidateMove (player, currentNumber, knightX - 2, knightY + 1);
                  }

               break;
            case (int) PlayerType.BISHOP:
               // Bishops move just like rooks, only diagonally instead of cardinally.

               int bishopX = player.BoardX, bishopY = player.BoardY;

               // Iterate over all possible movements to the "northwest"

               for (int i = bishopX - 1, j = bishopY - 1; i >= 0 && j >= 0; i--, j--)
                  ValidateMove (player, currentNumber, i, j);

               // Iterate over all possible movements to the "southwest"

               for (int i = bishopX - 1, j = bishopY + 1; i >= 0 && j < boardWidth; i--, j++)
                  ValidateMove (player, currentNumber, i, j);

               // Iterate over all possible movements to the "northeast"

               for (int i = bishopX + 1, j = bishopY - 1; i < boardWidth && j >= 0; i++, j--)
                  ValidateMove (player, currentNumber, i, j);

               // Iterate over all possible movements to the "southeast"

               for (int i = bishopX + 1, j = bishopY + 1; i < boardWidth && j < boardHeight; i++, j++)
                  ValidateMove (player, currentNumber, i, j);

               break;
            case (int) PlayerType.QUEEN:
               // The Queen can move as far as it wants in any direction, as long as it's a straight line.

               int queenX = player.BoardX, queenY = player.BoardY;

               // Iterate over all vertical movements up.

               for (int i = queenY - 1; i >= 0; i--)
                  ValidateMove (player, currentNumber, queenX, i);

               // Iterate over all vertical movements down.

               for (int i = queenY + 1; i < boardHeight; i++)
                  ValidateMove (player, currentNumber, queenX, i);

               // Iterate over all horizontal movements right.

               for (int i = queenX + 1; i < boardWidth; i++)
                  ValidateMove (player, currentNumber, i, queenY);

               // Iterate over all horizontal movements left.

               for (int i = queenX - 1; i >= 0; i--)
                  ValidateMove (player, currentNumber, i, queenY);

               // Iterate over all diagonal movements northwest

               for (int i = queenX - 1, j = queenX - 1; i >= 0 && j >= 0; i--, j--)
                  ValidateMove (player, currentNumber, i, j);

               // Iterate over all possible movements to the "southwest"

               for (int i = queenX - 1, j = queenX + 1; i >= 0 && j < boardWidth; i--, j++)
                  ValidateMove (player, currentNumber, i, j);

               // Iterate over all possible movements to the "northeast"

               for (int i = queenX + 1, j = queenX - 1; i < boardWidth && j >= 0; i++, j--)
                  ValidateMove (player, currentNumber, i, j);

               // Iterate over all possible movements to the "southeast"

               for (int i = queenX + 1, j = queenX + 1; i < boardWidth && j < boardHeight; i++, j++)
                  ValidateMove (player, currentNumber, i, j);

               break;
            case (int) PlayerType.KING:
               // The King can move one space in any direction.

               int kingX = player.BoardX, kingY = player.BoardY;

               // Move up.

               if (kingY - 1 >= 0)
                  ValidateMove (player, currentNumber, kingX, kingY - 1);

               // Move left.

               if (kingX - 1 >= 0)
                  ValidateMove (player, currentNumber, kingX - 1, kingY);

               // Move down

               if (kingY + 1 < boardHeight)
                  ValidateMove (player, currentNumber, kingX, kingY + 1);

               // Move right

               if (kingX + 1 < boardWidth)
                  ValidateMove (player, currentNumber, kingX + 1, kingY);
               break;

            default:
               break;
            }
         }

      /// <summary>
      /// Iterates over all chess piece types and moves them through the board.
      /// 
      /// </summary>
      /// <returns>A list of integers representing the total number of phone numbers a given chess piece can create.</returns>
      public int[] Play ()
         {
         // Run through the pieces and build phone numbers for each.

         foreach (var p in players)
            {
            p.SetPosition (startX, startY);

            PhoneBook (p, phoneNumber);
            }

         // Reset and play again with a new starting point.

         if (Reset ())
            Play ();

         return (numbers);
         }

      /// <summary>
      /// Resets the board to start each chess piece from a new starting space.
      /// 
      /// </summary>
      /// <returns>False if we've looped back to our starting point. Otherwise true.</returns>
      public bool Reset ()
         {
         Random r = new Random ();

         // Determines the next legal spot on the board we can start from.

         do
            {
            startX++;

            do
               {
               if (startX >= boardWidth)
                  {
                  startY++;
                  startX = 0;
                  }

               if (startY >= boardHeight)
                  {
                  startX++;
                  startY = 0;
                  }
               } while (startX >= boardWidth || startY >= boardHeight);
            } while (invalid.Concat (invalidStart).Contains (spaces[startY, startX]));

         phoneNumber = spaces[startY, startX].ToString ();

         // Have we wrapped back to our first starting point? Game over!

         if (initialStartX == startX && initialStartY == startY)
            return false;

         return true;
         }
      }
   }

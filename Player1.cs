using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class Player1 : Player
    {
        public void getPlayers              // players ids
        (
            ref string player1_1,
            ref string player1_2
        )
        {
            player1_1 = "307863423";        // id1
            player1_2 = "305327686";        // id2
        }
        public Tuple<int, int> playYourTurn
        (
            Board board,
            TimeSpan timesup,
            char playerChar          // 1 or 2
        )
        {
            Tuple<int, int> toReturn = null;
            //Random Algorithm - Start
            int randomMove;
            List<Tuple<int, int>> legalMoves = board.getLegalMoves(playerChar);
            Random random = new Random();
            randomMove = random.Next(0, legalMoves.Count());
            toReturn = legalMoves[randomMove];
            //Random Algorithm - End
            int best = int.MinValue;

            int depth = getDepthSize(board, timesup);

            if (depth == 0) return board.getLegalMoves(playerChar)[0];

            foreach (Tuple<int, int> child in legalMoves)
            {
                Board c = new Board(board);
                c.fillPlayerMove('1', child.Item1, child.Item2);
                int v = alphabeta(c, depth, int.MinValue, int.MaxValue, '2');
                if (v > best)
                {
                    best = v;
                    toReturn = child;
                }
            }
            return toReturn;
        }

        private int getDepthSize(Board board, TimeSpan timesup)
        {
            int boardSize = board._n;

            if (timesup.CompareTo(new TimeSpan(80)) == 0)
            {
                switch (boardSize)
                {
                    case 4:
                        return 4;
                    case 6:
                        return 4;
                    case 8:
                        return 3;
                    case 10:
                        return 3;
                    case 12:
                        return 2;
                    default:
                        return 0;
                }
            }
            else if (timesup.CompareTo(new TimeSpan(100)) == 0)
            {
                switch (boardSize)
                {
                    case 4:
                        return 4;
                    case 6:
                        return 4;
                    case 8:
                        return 4;
                    case 10:
                        return 3;
                    case 12:
                        return 2;
                    default:
                        return 0;
                }
            }
            else if (timesup.CompareTo(new TimeSpan(150)) == 0)
            {
                switch (boardSize)
                {
                    case 4:
                        return 4;
                    case 6:
                        return 4;
                    case 8:
                        return 4;
                    case 10:
                        return 3;
                    case 12:
                        return 2;
                    case 14:
                        return 2;
                    case 16:
                        return 1;
                    default:
                        return 0;
                }
            }
            else if (timesup.CompareTo(new TimeSpan(200)) == 0)
            {
                switch (boardSize)
                {
                    case 4:
                        return 4;
                    case 6:
                        return 4;
                    case 8:
                        return 4;
                    case 10:
                        return 3;
                    case 12:
                        return 2;
                    case 14:
                        return 2;
                    case 16:
                        return 1;
                    default:
                        return 0;
                }
            }
            else
            {
                switch (boardSize)
                {
                    case 4:
                        return 3;
                    case 6:
                        return 3;
                    case 8:
                        return 2;
                    case 10:
                        return 1;
                    default:
                        return 0;
                }
            }

        }

               
        private char getNextPlayer(char player)
        {
            char nextPlayer;
            if (player == '1')
                nextPlayer = '2';
            else
                nextPlayer = '1';
            return nextPlayer;
        }

        private int alphabeta(Board board,int depth, int a, int b, char player)
        {
            if (depth == 0 || board.isTheGameEnded())
            {
                
                Tuple<int, int> score = board.gameScore();                
                return score.Item1 - score.Item2;
            }
            List<Tuple<int, int>> legalMoves = board.getLegalMoves(player);
            List<Tuple<int, int>> t = getBest3Moves(legalMoves, board, player);
            if (player == '1') {
                int v = int.MinValue;
                
                foreach (Tuple<int, int> move in t) {
                    Board nextBoard = new Board(board);
                    nextBoard.fillPlayerMove(player, move.Item1, move.Item2);
                    v = Math.Max(v, alphabeta(nextBoard, depth - 1, a, b, getNextPlayer(player)));
                    a = Math.Max(a, v);
                    if (b <= a)
                        break;// (*β cut - off *)
                }
                return v;
            }
            else {
                int v = int.MaxValue;
                //List<Tuple<int, int>> legalMoves = board.getLegalMoves(player);
                foreach (Tuple<int, int> move in t)
                {
                    Board nextBoard = new Board(board);
                    nextBoard.fillPlayerMove(player, move.Item1, move.Item2);
                    v = Math.Min(v, alphabeta(nextBoard, depth - 1, a, b, getNextPlayer(player)));
                    b = Math.Min(b, v);
                    if (b <= a)
                        break;// (*α cut - off *)
                }
                return v;
            }
        }

        private List<Tuple<int, int>> getBest3Moves(List<Tuple<int, int>> legalMoves, Board board, char player)
        {
            List<Tuple<int, int>> bestMoves = new List<Tuple<int, int>>(3);
            bestMoves.Add(null);
            bestMoves.Add(null);
            bestMoves.Add(null);
            int best1 = int.MinValue;
            int best2 = int.MinValue;
            int best3 = int.MinValue;
            foreach (Tuple<int, int> move in legalMoves)
            {
                Board nextBoard = new Board(board);
                nextBoard.fillPlayerMove(player, move.Item1, move.Item2);
                Tuple<int, int> score = board.gameScore();
                int s = score.Item1 - score.Item2;
                if(s > best1)
                {
                    best3 = best2;
                    bestMoves[2] = bestMoves[1];
                    best2 = best1;
                    bestMoves[1] = bestMoves[0];
                    best1 = s;
                    bestMoves[0] = move;
                }
                else if(s > best2){
                    best3 = best2;
                    bestMoves[2] = bestMoves[1];
                    best2 = s;
                    bestMoves[1] = move;
                }
                else if(s > best3)
                {
                    best3 = s;
                    bestMoves[2] = move;
                }
            }
            if (bestMoves[2] == null)
                bestMoves.RemoveAt(2);
            if (bestMoves[1] == null)
                bestMoves.RemoveAt(1);
            if (bestMoves[0] == null)
                bestMoves.RemoveAt(0);
            return bestMoves;
        }

        private int calculateCornerHeuristics(Board board, char player)
        {
            int playerCorners = 0;
            int otherPlayerCorners = 0;
            int n = board._n;
            char[,] b = board._boardGame;
            if (b[0, 0] == player)
                playerCorners++;
            else if (b[0, 0] != '0')
                otherPlayerCorners++;
            if(b[n-1,n-1] == player)
                playerCorners++;
            else if (b[n - 1, n - 1] != '0')
                otherPlayerCorners++;
            if (b[0, n - 1] == player)
                playerCorners++;
            else if (b[0, n - 1] != '0')
                otherPlayerCorners++;
            if (b[n - 1, 0] == player)
                playerCorners++;
            else if (b[n - 1, 0] != '0')
                otherPlayerCorners++;
            
            double value;
            if (playerCorners > otherPlayerCorners)
                return 1;
            else
                return 0;
        }

        private double calculateMobilityHeuristics(Board board, char player)
        {
            List<Tuple<int, int>> playerMoves = board.getLegalMoves(player);
            List<Tuple<int, int>> nextPlayerMoves = board.getLegalMoves(getNextPlayer(player));
            double value;
            if (playerMoves.Count + nextPlayerMoves.Count != 0) {
                return value = (playerMoves.Count - nextPlayerMoves.Count) / (playerMoves.Count + nextPlayerMoves.Count);
                }
            else
                return value = 0;
        }
    }
}

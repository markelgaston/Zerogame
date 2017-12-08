using System;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    private int activePlayer;
    public int MAX_DEPTH = 6;
    public const int MINUS_INFINITE = -99999;
    public const int INFINITE = 99999;

    private int previousScore = MINUS_INFINITE;
    private int windowRange = 5;

    private int globalGuess = INFINITE;
    private int MAX_ITERATIONS = 10;
    private int maximumExploredDepth = 0;


    private System.Random rng = new System.Random();

    public void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void Play(Board _board, int actPlayer)
    {
        activePlayer = actPlayer;
        ScoringSquare move;
        
        //move = Minimax(_board, 0);

        //move = Negamax(_board, 0);

        //move = NegamaxAB(_board, 0, MINUS_INFINITE, INFINITE);

        move = AspirationSearch(_board);

        Move(move);
    }

    public void Move(ScoringSquare move) //Realiza su movimiento
    {
        GameController.Instance.AIEnded(move);
    }

    ScoringSquare Minimax(Board board, int depth)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int bestMove = 0;
        int bestScore = 0;
        ScoringSquare scoringSquare; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            scoringSquare = new ScoringSquare(board.Evaluate(activePlayer), 0);
        }
        else
        {
            if (board.activePlayer == activePlayer) bestScore = MINUS_INFINITE;
            else bestScore = INFINITE;

            List<Square> possibleMoves = new List<Square>();
            possibleMoves = board.PossibleMoves();
            Shuffle(possibleMoves);

            foreach (Square move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringSquare = Minimax(newBoard, (depth + 1));
                
                // Actualizar mejor score
                if (board.activePlayer == activePlayer)
                {
                    if (scoringSquare.Score > bestScore)
                    {
                        bestScore = scoringSquare.Score;
                        bestMove = move.Index;
                    }
                }
                else
                {
                    if (scoringSquare.Score < bestScore)
                    {
                        bestScore = scoringSquare.Score;
                        bestMove = move.Index;
                    }
                }
            }
            scoringSquare = new ScoringSquare(bestScore, bestMove);
        }
        return scoringSquare;
       
    }

    ScoringSquare Negamax(Board board, byte depth)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int bestMove = 0;
        int bestScore = 0;
        ScoringSquare scoringSquare; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {
                scoringSquare = new ScoringSquare(board.Evaluate(activePlayer), 0);
            }
            else
            {
                scoringSquare = new ScoringSquare(-board.Evaluate(activePlayer), 0);
            }
        }
        else
        {
            bestScore = MINUS_INFINITE;

            List<Square> possibleMoves = new List<Square>();
            possibleMoves = board.PossibleMoves();

            foreach (Square move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringSquare = Negamax(newBoard, (byte)(depth + 1));

                int invertedScore = -scoringSquare.Score;

                // Actualizar mejor score
                if (invertedScore > bestScore)
                {
                    bestScore = invertedScore;
                    bestMove = move.Index;
                }
            }
            scoringSquare = new ScoringSquare(bestScore, bestMove);
        }
        return scoringSquare;
    }

    ScoringSquare NegamaxAB(Board board, byte depth, int alfa, int beta)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int bestMove = 0;
        int bestScore = 0;
        ScoringSquare scoringSquare; // score, movimiento
        Board newBoard;
        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {
                scoringSquare = new ScoringSquare(board.Evaluate(activePlayer), 0);
            }
            else
            {
                scoringSquare = new ScoringSquare(-board.Evaluate(activePlayer), 0);
            }
        }
        else
        {
            bestScore = MINUS_INFINITE;

            List<Square> possibleMoves = new List<Square>();
            possibleMoves = board.PossibleMoves();
            Shuffle(possibleMoves);

            foreach (Square move in possibleMoves)
            {
                newBoard = board.GenerateNewBoardFromMove(move);

                // Recursividad
                scoringSquare = NegamaxAB(newBoard, (byte)(depth + 1), -beta, -Math.Max(alfa, bestScore));

                int invertedScore = -scoringSquare.Score;

                // Actualizar mejor score
                if (invertedScore > bestScore)
                {
                    bestScore = invertedScore;
                    bestMove = move.Index;
                }
                if (bestScore >= beta)
                {
                    scoringSquare = new ScoringSquare(bestScore, bestMove);
                    return scoringSquare;
                }
            }
            scoringSquare = new ScoringSquare(bestScore, bestMove);
        }
        return scoringSquare;
    }

    ScoringSquare AspirationSearch(Board board)
    {
        int alfa, beta;
        ScoringSquare move;
        string aspirationPath = "Aspiration Path: ";

        if (previousScore != MINUS_INFINITE)
        {
            alfa = previousScore - windowRange;
            beta = previousScore + windowRange;
            while (true)
            {
                move = NegamaxAB(board, 0, alfa, beta);
                if (move.Score <= alfa)
                {
                    aspirationPath += "Fail soft alfa.";
                    alfa = MINUS_INFINITE;
                }
                else if (move.Score >= beta)
                {
                    aspirationPath += "Fail soft beta.";
                    beta = INFINITE;
                }
                else
                {
                    aspirationPath += "Success";
                    break;
                }
            }
        }
        else
        {
            aspirationPath += "Normal Negamax";
            move = NegamaxAB(board, 0, MINUS_INFINITE, INFINITE);
        }
        previousScore = move.Score;
        //aspirationPathText.text = aspirationPath;
        return move;
    }

    /*
    ScoringSquare MTD(Board board)
    {
        int gamma, guess = globalGuess;
        ScoringSquare scoringMove = null;
        maximumExploredDepth = 0;

        //string output = "MTD Path: ";


        for (byte i = 0; i < MAX_ITERATIONS; i++)
        {
            gamma = guess;
            scoringMove = Test(board, 0, gamma - 1);
            guess = scoringMove.Score;
            if (guess == gamma)
            {
                globalGuess = guess;
                //output += "guess econtrado en iteracion " + i;
                //MTDPathText.text = output;
                return scoringMove;
            }
        }
        //output += "guess no encontrado";
        //MTDPathText.text = output;
        globalGuess = guess;
        return scoringMove;
    }

    ScoringSquare Test(Board board, byte depth, int gamma)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        int bestMove = 0;
        int bestScore = 0;

        ScoringSquare scoringMove; // score, movimiento
        Board newBoard;
        Record record;

        if (depth > maximumExploredDepth)
        {
            maximumExploredDepth = depth;
        }

        record = transpositionTable.GetRecord(board.hashValue);

        if (record != null)
        {
            if (record.depth > MAX_DEPTH - depth)
            {
                if (record.minScore > gamma)
                {
                    scoringMove = new ScoringSquare(record.minScore, record.bestMove);
                    return scoringMove;
                }
                if (record.maxScore < gamma)
                {
                    scoringMove = new ScoringSquare(record.maxScore, record.bestMove);
                    return scoringMove;
                }
            }
        }
        else
        {
            record = new Record();
            record.hashValue = board.hashValue;
            record.depth = MAX_DEPTH - depth;
            record.minScore = MINUS_INFINITE;
            record.maxScore = INFINITE;
        }


        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {
                record.maxScore = board.Evaluate(activePlayer);
            }
            else
            {
                record.maxScore = -board.Evaluate(activePlayer);
            }
            record.minScore = record.maxScore;
            transpositionTable.SaveRecord(record);
            scoringMove = new ScoringSquare(record.maxScore, 0);
        }
        else
        {
            bestScore = MINUS_INFINITE;

            int[] possibleMoves;
            possibleMoves = board.PossibleMoves();

            foreach (int move in possibleMoves)
            {
                // newBoard = board.GenerateNewBoardFromMove(move);
                newBoard = board.HashGenerateNewBoardFromMove(move);

                // Recursividad
                scoringMove = Test(newBoard, (byte)(depth + 1), -gamma);

                int invertedScore = -scoringMove.Score;

                // Actualizar mejor score
                if (invertedScore > bestScore)
                {
                    bestScore = invertedScore;
                    bestMove = move;
                    record.bestMove = move;
                }
                if (bestScore < gamma)
                {
                    record.maxScore = bestScore;
                }
                else
                {
                    record.minScore = bestScore;
                }
            }
            transpositionTable.SaveRecord(record);
            scoringMove = new ScoringSquare(bestScore, bestMove);
        }
        return scoringMove;
    }*/
}
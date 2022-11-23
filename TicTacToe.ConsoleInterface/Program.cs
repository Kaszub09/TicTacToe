using System;
using System.Security.Cryptography.X509Certificates;
using TicTacToe.AI;
using TicTacToe.Game;

namespace TicTacToe.ConsoleInterface {
    internal class Program {
        static void Main(string[] args) {
            while (true) {
                Console.WriteLine(
                    @"
Welcome to TicTacToe game! Please enter command:
1. Classic TIcTacToe vs AI
2. 3D TicTacToe vs AI (Space is 4x4x4, and 4 in line wins the game)
3. Custom TicTacToe vs AI (with any number of dimensions)
4. Custom TicTacToe with any number of players
5. Exit
");

                switch (Console.ReadLine().Trim()) {
                    case "1":
                        ClassicGame();
                        break;
                    case "2":
                        MultiGame(true, true);
                        break;
                    case "3":
                        MultiGame(false, true);
                        break;
                    case "4":
                        MultiGame(false, false);
                        break;
                    case "5":
                        return;
                        break;
                    default:
                        Console.WriteLine("Option not supported");
                        break;

                }
            }



            static void ClassicGame() {
                var isPlayerMove = true;
                var bp = Factory.CreateBoardPrinter();
                var game = Factory.CreateNewGame();
                var ai = AIFactory.CreateAI();

                while (!game.IsGameFinished) {
                    Console.WriteLine(bp.PrintBoardAsString(game.Board));
                    Console.WriteLine("NEXT PLAYER: " + (isPlayerMove ? "Human" : "AI"));

                    if (isPlayerMove) {
                        var position = GetPosition();
                        if (game.IsMoveLegal(position[0], position[1])) {
                            game.MakeMove(position[0], position[1]);
                            isPlayerMove = !isPlayerMove;
                        } else {
                            Console.WriteLine("Given position is illegal");
                        }
                    } else {
                        var bestMove = ai.GetBestMove(game);
                        Console.WriteLine($"AI outcome={bestMove.PlayerOutcome}");
                        game.MakeMove(bestMove.Move.Cell.Index);
                        isPlayerMove = !isPlayerMove;
                    }
                }

                Console.WriteLine(bp.PrintBoardAsString(game.Board));

                if (game.Winner == 0) {
                    Console.WriteLine("Game ended in draw.");
                } else {
                    game.GetWinningCombination(out List<ClassicMove> combination);
                    Console.WriteLine("Winner: " + GetWinner(game.Winner));
                    Console.WriteLine("Winnig line: " + combination.Select(cell => $"({cell.Cell.Row},{cell.Cell.Col})").Aggregate((x, y) => x + ", " + y));
                }
            }

            static void MultiGame(bool standard3D, bool vsAI) {
                var isPlayerMove = true;
                var bp = Factory.CreateBoardPrinter();
                var game = standard3D? Factory.CreateNewGame(2,4,4,4,4): GetCustomGame(vsAI);
                var ai = AIFactory.CreateMultiAI();

                if (vsAI) {
                    Console.WriteLine("Ai starts? (y/n)");
                    isPlayerMove = Console.ReadLine().Trim().ToLower()!="y";
                }

                while (!game.IsGameFinished) {
                    Console.WriteLine(bp.PrintBoardAsString(game.Board));
                    Console.WriteLine("NEXT PLAYER: " + (isPlayerMove ? "Human" : "AI"));

                    if (isPlayerMove) {
                        var position = GetPosition(game.Board.Space.Dimensions);
                        if (game.IsMoveLegalByPosition(position)) {
                            game.MakeMoveByPosition(position);
                            isPlayerMove = vsAI ? !isPlayerMove : isPlayerMove;
                        } else {
                            Console.WriteLine("Given position is illegal");
                        }
                    } else {
                        var bestMove = ai.GetBestMove(game);
                        Console.WriteLine($"AI outcome={bestMove.PlayerOutcome}");
                        game.MakeMoveByIndex(bestMove.Move.Cell.Index);
                        isPlayerMove = !isPlayerMove;
                    }
                }
                Console.WriteLine(bp.PrintBoardAsString(game.Board));
                if (game.Winner == 0) {
                    Console.WriteLine("Game ended in draw.");
                } else {
                    game.GetWinningCombination(out List<MultiMove> combination);

                    Console.WriteLine("Winner: " + GetWinner(game.Winner));
                    Console.WriteLine("Winnig line: " + combination
                        .Select(cell => cell.Cell.Position.Select(x=>x.ToString()).Aggregate((x,y)=>x+", "+y))
                        .Select(x=> "("+x+")").Aggregate((cellPos,res) => cellPos+", " + res ));
                }
            }

            
            static IMultiGame GetCustomGame(bool vsAI) {
                var numberOfPlayers = 2;
                if (!vsAI) {
                    Console.WriteLine("Enter number of players:");
                    numberOfPlayers = int.Parse(Console.ReadLine());
                } 
                Console.WriteLine("Enter number of cells in line required to win:");
                var cellsInLine = int.Parse(Console.ReadLine());

                Console.WriteLine($"Enterboard size - numbers delimited by comma ',':");
                var space=  Console.ReadLine().Split(",").Select(x => int.Parse(x)).ToArray();

                return Factory.CreateNewGame(numberOfPlayers, cellsInLine, space);
            }

            static int[] GetPosition(int dimensions = 2) {
                Console.WriteLine($"Enter position ({dimensions} numbers comma ',' delimited, indexed from 0):");
                return Console.ReadLine().Split(",").Select(x => int.Parse(x)).ToArray();
            }

            static string GetWinner(int player) {
                if (player == 1) {
                    return "HUMAN";
                } else if (player == 2) {
                    return "AI";
                } else {
                    return player.ToString();
                }
            }
        }
    }
}
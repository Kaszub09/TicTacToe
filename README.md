# TicTacToe
[Usage](#usage)

[Algorithms](#algorithms)



### Usage 
For detailed usage see sample interface in https://github.com/Kaszub09/TicTacToe/blob/master/TicTacToe.ConsoleInterface/Program.cs

Everything can be created from static Factory class in Game package, or static AIFactory class in AI package.

* ``var game = Factory.CreateNewGame();`` 
Creates new game object.
* ``var game = AIFactory.CreateAI();`` 
Creates new object, which allows for evaluating moves in game.
* ``var bp = Factory.CreateBoardPrinter();`` 
Creates board to string converter, which helps to print the game in console.

### Algorithms
Comparision in time between diffrent algorithms (Rec means recurion function, a NoRec means the same funciton rewritten as a loop). Results are for calculating starting moves in classic 3x3 game.
![obraz](https://user-images.githubusercontent.com/34368953/203647370-ca341559-2af1-4a6d-8c31-1e80d624f712.png)
It doens't look like there is much to gain form hashing solutions, and in case of bigger board it consumes too much memory.

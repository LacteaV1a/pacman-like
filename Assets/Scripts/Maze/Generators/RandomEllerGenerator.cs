using Nox7atra.Mazes;
using Nox7atra.Mazes.Generators;

public class RandomEllerGenerator : EllerGenerator
{
    private readonly float _wallsValue;
    private readonly int _seed;
    private System.Random _rand;
    public RandomEllerGenerator(int seed, float wallsvalue)
    {
        _seed = seed;
        _wallsValue = wallsvalue;
        _rand = new System.Random(_seed);
    }

    public override W4Maze Generate(int width, int height)
    {
        var eulerMaze = base.Generate(width, height);
        RandomRemoveCells(eulerMaze);
        return eulerMaze;
    }

    private void RandomRemoveCells(W4Maze maze)
    {
        for (int j = 0; j < maze.RowCount; j++)
        {
            for (int i = 0; i < maze.ColumnCount; i++)
            {
                if (_rand.NextDouble() > _wallsValue)
                {
                    var cells = maze.GetCell(j, i);

                    cells.TopWall = false;
                    cells.BotWall = false;
                    cells.LeftWall = false;
                    cells.RightWall = false;

                    SetCellsNeighbors(j, i, maze.RowCount, maze.ColumnCount, maze);


                    if (i == 0) cells.BotWall = true;
                    if (i == maze.ColumnCount - 1) cells.TopWall = true;

                    if (j == 0) cells.LeftWall = true;
                    if (j == maze.RowCount - 1) cells.RightWall = true;

                }
            }
        }
    }

    private void SetCellsNeighbors(int x, int y, int width, int height, W4Maze maze)
    {

        if (x > 0)
            maze.GetCell(x - 1, y).RightWall = false;

        if (x < width - 1)
            maze.GetCell(x + 1, y).LeftWall = false;

        if (y > 0)
            maze.GetCell(x, y - 1).TopWall = false;

        if (y < height - 1)
            maze.GetCell(x, y + 1).BotWall = false;

    }
}

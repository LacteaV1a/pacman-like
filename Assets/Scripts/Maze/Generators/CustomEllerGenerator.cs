using Nox7atra.Mazes;
using Nox7atra.Mazes.Generators;

public class CustomEllerGenerator : EllerGenerator
{
    private readonly float _wallOccupancy;
    private System.Random _rand;
    public CustomEllerGenerator(float wallOccupancy)
    {
        _wallOccupancy = wallOccupancy;
    }

    public override W4Maze Generate(int width, int height, int cellSize, int seed)
    {
        _rand = seed == 0 ? new System.Random() : new System.Random(seed);
        var eulerMaze = base.Generate(width, height, cellSize, seed);
        RandomRemoveCells(eulerMaze);
        return eulerMaze;
    }

    private void RandomRemoveCells(W4Maze maze)
    {
        for (int j = 0; j < maze.ColumnCount; j++)
        {
            for (int i = 0; i < maze.RowCount; i++)
            {
                if (_rand.NextDouble() > _wallOccupancy)
                {
                    var cells = maze.GetCell(j, i);

                    cells.TopWall = false;
                    cells.BotWall = false;
                    cells.LeftWall = false;
                    cells.RightWall = false;

                    SetCellsNeighbors(j, i, maze.ColumnCount, maze.RowCount, maze);

                    if (j == 0) cells.LeftWall = true;
                    if (j == maze.ColumnCount - 1) cells.RightWall = true;

                    if (i == 0) cells.BotWall = true;
                    if (i == maze.RowCount - 1) cells.TopWall = true;
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

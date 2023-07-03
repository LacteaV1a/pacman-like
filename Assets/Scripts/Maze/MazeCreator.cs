using Nox7atra.Mazes;
using Nox7atra.Mazes.Generators;

public class MazeCreator
{
    private MazeConfig _config;
    private MazeGenerator _generator;

    public MazeCreator(MazeConfig config, MazeGenerator generator)
    {
        _config = config;
        _generator = generator;
    }

    public W4Maze Create()
    {
        var maze = _generator.Generate(_config.CellsX, _config.CellsY, _config.CellsSize, _config.Seed);
        return maze;
    }
}
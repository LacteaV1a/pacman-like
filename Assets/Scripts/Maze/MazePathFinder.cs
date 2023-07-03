using Nox7atra.Mazes;
using System.Collections.Generic;

public class MazePathFinder
{
    public List<MazeGraphCell> FindShortestPath(MazeGraphCell start, MazeGraphCell end)
    {
        Queue<List<MazeGraphCell>> queue = new Queue<List<MazeGraphCell>>();
        HashSet<MazeGraphCell> visited = new HashSet<MazeGraphCell>();

        queue.Enqueue(new List<MazeGraphCell> { start });
        visited.Add(start);

        while (queue.Count > 0)
        {
            List<MazeGraphCell> path = queue.Dequeue();
            MazeGraphCell current = path[path.Count - 1];

            if (current == end)
                return path;

            foreach (MazeGraphCell neighbor in current.Neighbours)
            {
                if (visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                List<MazeGraphCell> newPath = new List<MazeGraphCell>(path) { neighbor };
                queue.Enqueue(newPath);
            }
        }

        return null; // Path not found
    }
}

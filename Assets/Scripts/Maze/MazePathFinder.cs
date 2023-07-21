using Nox7atra.Mazes;
using System.Collections.Generic;
using UnityEngine;

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


    public List<Vector2Int> FindShortestPathDirection(MazeGraphCell start, MazeGraphCell end)
    {
        var path = FindShortestPath(start, end);
        List<Vector2Int> directions = new();

        for (int i = 0; i < path.Count; i++)
        {
            MazeGraphCell current = path[i];
            if(i+1 == path.Count)
            {
                break;
            }
            else
            {
                MazeGraphCell neighbor = path[i+1];
                Vector2Int direction = new Vector2Int(Mathf.RoundToInt(neighbor.Position.x - current.Position.x), Mathf.RoundToInt(neighbor.Position.y - current.Position.y));
                directions.Add(direction);
            }


        }



        return directions; // Path not found
    }

    private MazeGraphCell GetCellAtPosition(Vector2Int position, MazeGraphCell start)
    {
        foreach (MazeGraphCell cell in start.Neighbours)
        {
            if (cell.Position == position)
                return cell;
        }
        return start;
    }
}

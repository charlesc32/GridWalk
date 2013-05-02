using System;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Program
{
    static void Main(string[] args)
    {
        var gridWalker = new GridWalker();

        double points = gridWalker.GetNumberOfAccessiblePoints();
        Console.WriteLine(Math.Round(points, 0));
    }
}

public class GridWalker
{
    //Keep a dictionary for quick checking of existance of a point but an ordered list for iterating to the next coordinate
    //I tried using a simple struct to hold the x and y and contanenating the x and y into a string to make a key but that led me to some nasty
    //key collisions. I started to make a real class that implemented equality and a hashcode but the tuple ended up working and is simpler
    private Dictionary<Tuple<int, int>, bool> accessiblePoints;
    private List<Tuple<int,int>> orderedPoints;
    private int pointSumLimit = 19;

    public GridWalker()
    {
        accessiblePoints = new Dictionary<Tuple<int,int>, bool>() { { Tuple.Create(0,0), true } };
        orderedPoints = new List<Tuple<int,int>>() { Tuple.Create(0,0) };
    }

    public GridWalker(int argPointSumLimit) : this()
    {
        pointSumLimit = argPointSumLimit;
    }
    
    public double GetNumberOfAccessiblePoints()
    {
        int index = 0;
        do
        {
            var pointToNavigateFrom = orderedPoints[index];
            
            AddPointIfValid(pointToNavigateFrom.Item1 + 1, pointToNavigateFrom.Item2);
            AddPointIfValid(pointToNavigateFrom.Item1 - 1, pointToNavigateFrom.Item2);
            AddPointIfValid(pointToNavigateFrom.Item1, pointToNavigateFrom.Item2 + 1);
            AddPointIfValid(pointToNavigateFrom.Item1, pointToNavigateFrom.Item2 - 1);

            //If we reached the end of the visited points and none were added in the previous checks then we're done.
            if (orderedPoints.Count - 1 == index) break;
            index++;
        } while (true);

        return orderedPoints.Count;
    }

    private void AddPointIfValid(int x, int y)
    {
        if (LimitCheck(x, y, pointSumLimit))
        {
            var newPoint = Tuple.Create(x, y);
            if (!accessiblePoints.ContainsKey(newPoint))
            {   
                accessiblePoints.Add(newPoint, true);
                orderedPoints.Add(newPoint);
            }
        }
    }

    private bool LimitCheck(int x, int y, int limit)
    {
        var pointString = Math.Abs(x).ToString() + Math.Abs(y).ToString();
        double total = 0;
        for (int i = 0; i < pointString.Length; i++)
        {
            total += char.GetNumericValue(pointString[i]);
            if (total > limit) return false;
        }

        return (total <= limit);
    }
}
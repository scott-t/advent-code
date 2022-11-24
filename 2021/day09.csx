#!/usr/bin/env dotnet-script

var rawLines = System.IO.File.ReadAllLines("day09.txt");
var width = rawLines[0].Count();
var height = rawLines.Count();

var map = rawLines.Aggregate((a, b)=>a+b);

Dictionary<int, char> lowPoints = new Dictionary<int, char>();

for (int x = 0; x < width; ++x) {
    for (int y = 0; y < height; ++y) {
        var me = map.ElementAt(y*width+x);
        if (((x == 0 || me < map.ElementAt(y*width+x-1))
            && (x == width-1 || me < map.ElementAt(y*width+x+1))
            && (y == 0 || me < map.ElementAt((y-1)*width+x))
            && (y == height-1 || me < map.ElementAt((y+1)*width+x))))
            lowPoints.Add(y*width+x, me);
    }
}

Console.WriteLine("Sum of low points {0}", lowPoints.Select(x => x.Value-'0'+1).Sum());

// Is there a smart way to map this out?
Dictionary<int,List<int>> sizes = new Dictionary<int, List<int>>();

void testPosition(int p, char thisVal, ref List<int> b, ref Queue<int> c) {
    if (!b.Contains(p) && map[p] > thisVal && map[p] != '9') {
        b.Add(p);
            if (!c.Contains(p))
                c.Enqueue(p);
    }
}

foreach (var lowpoint in lowPoints) {
    List<int> basin = new List<int>();
    Queue<int> candidates = new Queue<int>();
    candidates.Enqueue(lowpoint.Key);
    basin.Add(lowpoint.Key);

    while (candidates.Count > 0)
    {
        var pos = candidates.Dequeue();
        var me = map[pos];

        if (pos % width != 0)
            testPosition(pos-1, me, ref basin, ref candidates);

        if (pos % width != width-1)
            testPosition(pos+1, me, ref basin, ref candidates);

        if (pos > width)
            testPosition(pos-width, me, ref basin, ref candidates);

        if (pos+width < map.Length)
            testPosition(pos+width, me, ref basin, ref candidates);
    }

    sizes.Add(lowpoint.Key, basin);
}


Console.WriteLine("Mult of 3 basins {0}", sizes.Select(k => k.Value.Count()).OrderBy( v => v ).TakeLast(3).Aggregate((tot,b) => tot * b));


/**
--- Day 9: Smoke Basin ---
These caves seem to be lava tubes. Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.

If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).

Smoke flows to the lowest point of the area it's in. For example, consider the following heightmap:

2199943210
3987894921
9856789892
8767896789
9899965678
Each number corresponds to the height of a particular location, where 9 is the highest and 0 is the lowest a location can be.

Your first goal is to find the low points - the locations that are lower than any of its adjacent locations. Most locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)

In the above example, there are four low points, all highlighted: two are in the first row (a 1 and a 0), one is in the third row (a 5), and one is in the bottom row (also a 5). All other locations on the heightmap have some lower adjacent location, and so are not low points.

The risk level of a low point is 1 plus its height. In the above example, the risk levels of the low points are 2, 1, 6, and 6. The sum of the risk levels of all low points in the heightmap is therefore 15.

Find all of the low points on your heightmap. What is the sum of the risk levels of all low points on your heightmap?

--- Part Two ---
Next, you need to find the largest basins so you know what areas are most important to avoid.

A basin is all locations that eventually flow downward to a single low point. Therefore, every low point has a basin, although some basins are very small. Locations of height 9 do not count as being in any basin, and all other locations will always be part of exactly one basin.

The size of a basin is the number of locations within the basin, including the low point. The example above has four basins.

The top-left basin, size 3:

2199943210
3987894921
9856789892
8767896789
9899965678
The top-right basin, size 9:

2199943210
3987894921
9856789892
8767896789
9899965678
The middle basin, size 14:

2199943210
3987894921
9856789892
8767896789
9899965678
The bottom-right basin, size 9:

2199943210
3987894921
9856789892
8767896789
9899965678
Find the three largest basins and multiply their sizes together. In the above example, this is 9 * 14 * 9 = 1134.

What do you get if you multiply together the sizes of the three largest basins?
*/
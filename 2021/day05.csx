#!/usr/bin/env dotnet-script

Console.WriteLine("Iterating over these coordinates tripped me up more than I expected");

/**
--- Day 5: Hydrothermal Venture ---
You come across a field of hydrothermal vents on the ocean floor! These vents constantly produce large, opaque clouds, so it would be best to avoid them if possible.

They tend to form in lines; the submarine helpfully produces a list of nearby lines of vents (your puzzle input) for you to review. For example:

0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2
Each line of vents is given as a line segment in the format x1,y1 -> x2,y2 where x1,y1 are the coordinates of one end the line segment and x2,y2 are the coordinates of the other end. These line segments include the points at both ends. In other words:

An entry like 1,1 -> 1,3 covers points 1,1, 1,2, and 1,3.
An entry like 9,7 -> 7,7 covers points 9,7, 8,7, and 7,7.
For now, only consider horizontal and vertical lines: lines where either x1 = x2 or y1 = y2.

So, the horizontal and vertical lines from the above list would produce the following diagram:

.......1..
..1....1..
..1....1..
.......1..
.112111211
..........
..........
..........
..........
222111....
In this diagram, the top left corner is 0,0 and the bottom right corner is 9,9. Each position is shown as the number of lines which cover that point or . if no line covers that point. The top-left pair of 1s, for example, comes from 2,2 -> 2,1; the very bottom row is formed by the overlapping lines 0,9 -> 5,9 and 0,9 -> 2,9.

To avoid the most dangerous areas, you need to determine the number of points where at least two lines overlap. In the above example, this is anywhere in the diagram with a 2 or larger - a total of 5 points.

Consider only horizontal and vertical lines. At how many points do at least two lines overlap?
*/

readonly var data = System.IO.File.ReadAllLines("day05.txt").Select(line => {
    var p1 = line.Split(new char[]{',','-','>'}, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray();
    return System.Drawing.Rectangle.FromLTRB(p1[0], p1[1], p1[2], p1[3]);
});

int maxX = data.Max(r => Math.Max(r.Right, r.Left)) + 1;
int maxY = data.Max(r => Math.Max(r.Top, r.Bottom)) + 1;

int[] grid = new int[maxX * maxY];
Array.Clear(grid, 0, grid.Length);

// Simplemode
var dataMap = data.Where(r => r.Width == 0 || r.Height == 0).Aggregate(grid, (prev, rec) => {
    for (int w = 0; Math.Abs(w) <= Math.Abs(rec.Width); w = rec.Width < 0 ? w - 1 : w + 1)
        for (int h = 0; Math.Abs(h) <= Math.Abs(rec.Height); h = rec.Height < 0 ? h - 1 : h + 1)
            prev[rec.X + w + (rec.Y+h)*maxX] += 1;

    return prev;
});

Console.WriteLine("Overlap point count: {0}", dataMap.Count(i => i > 1));

/**
--- Part Two ---
Unfortunately, considering only horizontal and vertical lines doesn't give you the full picture; you need to also consider diagonal lines.

Because of the limits of the hydrothermal vent mapping system, the lines in your list will only ever be horizontal, vertical, or a diagonal line at exactly 45 degrees. In other words:

An entry like 1,1 -> 3,3 covers points 1,1, 2,2, and 3,3.
An entry like 9,7 -> 7,9 covers points 9,7, 8,8, and 7,9.
Considering all lines from the above example would now produce the following diagram:

1.1....11.
.111...2..
..2.1.111.
...1.2.2..
.112313211
...1.2....
..1...1...
.1.....1..
1.......1.
222111....
You still need to determine the number of points where at least two lines overlap. In the above example, this is still anywhere in the diagram with a 2 or larger - now a total of 12 points.
*/
Array.Clear(grid, 0, grid.Length);

// Hardmode
dataMap = data.Aggregate(grid, (prev, rec) => {
    if (rec.Width == 0 || rec.Height == 0)
        for (int w = 0; Math.Abs(w) <= Math.Abs(rec.Width); w = rec.Width < 0 ? w - 1 : w + 1)
            for (int h = 0; Math.Abs(h) <= Math.Abs(rec.Height); h = rec.Height < 0 ? h - 1 : h + 1)
                prev[rec.X + w + (rec.Y+h)*maxX] += 1;
    else
        for (int w = 0, h = 0; Math.Abs(w) <= Math.Abs(rec.Width) && Math.Abs(h) <= Math.Abs(rec.Height); w = (rec.Width < 0 ? w - 1 : w + 1), h = (rec.Height < 0 ? h - 1 : h + 1))
            prev[rec.X + w + (rec.Y+h)*maxX] += 1;

    return prev;
});

Console.WriteLine("Hardmode overlap point count: {0}", dataMap.Count(i => i > 1));


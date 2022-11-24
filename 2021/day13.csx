#!/usr/bin/env dotnet-script

using Point = System.Drawing.Point;

var raw = System.IO.File.ReadAllLines("day13.txt");
var coords = raw.Where(x => !x.StartsWith("fold") && x.Length > 0).Select(x => { var t = x.Split(","); return new Point(int.Parse(t[0]), int.Parse(t[1]));}).ToArray();
var folds = raw.Where(x => x.StartsWith("fold") && x.Length > 0).ToArray();

int width = coords.Max(x => x.X)+1;
int height = coords.Max(x => x.Y)+1;

int[,] paper = new int[width,height];

foreach (var c in coords) {
    paper[c.X, c.Y] = 1;
}

foreach (var f in folds)
{
    var fold = f.Split('=');

    var axis = fold[0].Last(); // -1 due to zero index array

    var offset = int.Parse(fold[1]);
    if (axis == 'y') {
        for (int y = 1; y <= offset; ++y) {
            for (int x = 0; x < width; ++x) {
                paper[x, offset-y] += paper[x, offset+y];
            }
        }

        height = (height-1) / 2;
    } else {
        for (int x = 1; x <= offset; ++x) {
            for (int y = 0; y < height; ++y) {
                paper[offset-x, y] += paper[offset+x, y];
            }
        }
        width = (width - 1) / 2;
    }

    var dots = 0;
    for (var x = 0; x < width; ++x)
        for (var y = 0; y < height; ++y)
            if (paper[x,y] > 0) dots++;

    Console.WriteLine("Fold {0} dot count {1}", f, dots);
}

for (var y = 0; y < height; ++y) {
    string l = "";
    for (var x = 0; x < width; ++x) {
        l += paper[x,y] > 0 ? '#' : ' ';
    }
    Console.WriteLine(l);
}


/*
--- Day 13: Transparent Origami ---
You reach another volcanically active part of the cave. It would be nice if you could do some kind of thermal imaging so you could tell ahead of time which caves are too hot to safely enter.

Fortunately, the submarine seems to be equipped with a thermal camera! When you activate it, you are greeted with:

Congratulations on your purchase! To activate this infrared thermal imaging
camera system, please enter the code found on page 1 of the manual.
Apparently, the Elves have never used this feature. To your surprise, you manage to find the manual; as you go to open it, page 1 falls out. It's a large sheet of transparent paper! The transparent paper is marked with random dots and includes instructions on how to fold it up (your puzzle input). For example:

6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5
The first section is a list of dots on the transparent paper. 0,0 represents the top-left coordinate. The first value, x, increases to the right. The second value, y, increases downward. So, the coordinate 3,0 is to the right of 0,0, and the coordinate 0,7 is below 0,0. The coordinates in this example form the following pattern, where # is a dot on the paper and . is an empty, unmarked position:

...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........
Then, there is a list of fold instructions. Each instruction indicates a line on the transparent paper and wants you to fold the paper up (for horizontal y=... lines) or left (for vertical x=... lines). In this example, the first fold instruction is fold along y=7, which designates the line formed by all of the positions where y is 7 (marked here with -):

...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
-----------
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........
Because this is a horizontal line, fold the bottom half up. Some of the dots might end up overlapping after the fold is complete, but dots will never appear exactly on a fold line. The result of doing this fold looks like this:

#.##..#..#.
#...#......
......#...#
#...#......
.#.#..#.###
...........
...........
Now, only 17 dots are visible.

Notice, for example, the two dots in the bottom left corner before the transparent paper is folded; after the fold is complete, those dots appear in the top left corner (at 0,0 and 0,1). Because the paper is transparent, the dot just below them in the result (at 0,3) remains visible, as it can be seen through the transparent paper.

Also notice that some dots can end up overlapping; in this case, the dots merge together and become a single dot.

The second fold instruction is fold along x=5, which indicates this line:

#.##.|#..#.
#...#|.....
.....|#...#
#...#|.....
.#.#.|#.###
.....|.....
.....|.....
Because this is a vertical line, fold left:

#####
#...#
#...#
#...#
#####
.....
.....
The instructions made a square!

The transparent paper is pretty big, so for now, focus on just completing the first fold. After the first fold in the example above, 17 dots are visible - dots that end up overlapping after the fold is completed count as a single dot.

How many dots are visible after completing just the first fold instruction on your transparent paper?

--- Part Two ---
Finish folding the transparent paper according to the instructions. The manual says the code is always eight capital letters.

What code do you use to activate the infrared thermal imaging camera system?

*/
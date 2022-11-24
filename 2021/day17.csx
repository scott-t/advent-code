#!/usr/bin/env dotnet-script

using Rectangle = System.Drawing.Rectangle;

/*******************************

        🛑 HOLD UP! 🛑

Yes this could be done better.
Could combine SimPoint to have
a vector (point) speed and point
value instead and dual-calc both
x and y together. But for speed
reasons left independent calcs,
especially not knowing what pt2
would ask for initially, didn't
want to rewrite too much... 🤷‍♀️


*******************************/

class SimPoint {
    public SimPoint(int timestep = 0, int offset = 0) {
        TimeStep = timestep;
        Value = offset;
    }
    public int TimeStep { get; set; }
    public int Value { get; set; }
}

enum Direction {
    Horiz,
    Vert
}

var rawInput = System.IO.File.ReadAllText("day17.txt").Split(' ');
Rectangle target = new Rectangle();

foreach (var chunk in rawInput) {
    if (chunk.StartsWith('x') || chunk.StartsWith('y')) {
        var data = chunk.Substring(2).Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (chunk.StartsWith('x')) {
            target.X = int.Parse(data[0]);
            target.Width = int.Parse(data[1].Substring(0, data[1].Length-1)) - target.X;
        } else {
            target.Y = int.Parse(data[1]);
            target.Height = int.Parse(data[0]) - target.Y;
        }
    }
}

List<SimPoint> Simulate(Direction dir, int speed, Rectangle target, out List<SimPoint> inTarget, int? stepCount = null) {
    SimPoint step = new SimPoint();
    SimPoint prevStep = new SimPoint();
    List<SimPoint> ret = new List<SimPoint>() { step };
    bool isHoriz = dir == Direction.Horiz;
    // These are labelled wrong but using height =
    int lbound = isHoriz ? target.X : target.Y;
    int ubound = isHoriz ? target.Right : target.Bottom;

    while (isHoriz ? (stepCount.HasValue ? ret.Count < stepCount.Value : step.Value < ubound) : step.Value > ubound) {
        prevStep = step;
        step = new SimPoint(step.TimeStep + 1, step.Value + speed);
        ret.Add(step);

        if (isHoriz) {
            if (speed > 0)
                --speed;
            else if (speed < 0)
                ++speed;
        } else {
            --speed;
        }
    }

    if (isHoriz)
        inTarget = ret.FindAll(s => s.Value >= target.Left && s.Value <= target.Right);
    else
        inTarget = ret.FindAll(s => s.Value >= target.Bottom && s.Value <= target.Top);

    return ret;
}

int maxHeightSpeed = 0;
int verSpeedLimit = -target.Bottom;   // might be off by one here
List<SimPoint> targetPoints;
List<SimPoint> heightPoints;

Dictionary<int, List<SimPoint>> ySpeeds = new Dictionary<int, List<SimPoint>>(), xSpeeds = new Dictionary<int, List<SimPoint>>();

for (var i = -verSpeedLimit; i < verSpeedLimit; ++i) {
    heightPoints = Simulate(Direction.Vert, i, target, out targetPoints);
    if (targetPoints.Count > 0) {
        maxHeightSpeed = verSpeedLimit;
        ySpeeds.Add(i, targetPoints);
    }
}

// recalc max height cuz i'm lazy and it looks interesting
heightPoints = Simulate(Direction.Vert, maxHeightSpeed, target, out targetPoints);

Console.WriteLine("Highest y = {0} with vert launch speed {1}", heightPoints.Max(p => p.Value), maxHeightSpeed);

int horizSpeedLimit = target.Right;
int maxTimesteps = ySpeeds.Max(p => p.Value.Max(x => x.TimeStep)) + 1;

for (var x = 0; x <= horizSpeedLimit; ++x) {
    Simulate(Direction.Horiz, x, target, out targetPoints, maxTimesteps);
    if (targetPoints.Count > 0) {
        xSpeeds.Add(x, targetPoints);
    }
}

List<SimPoint> launchSpeeds = new List<SimPoint>();

foreach (var xSpeed in xSpeeds) {
    foreach (var ySpeed in ySpeeds) {
        if (ySpeed.Value.Find(y => xSpeed.Value.Find(x => x.TimeStep == y.TimeStep) != null) != null) {
            launchSpeeds.Add(new SimPoint(xSpeed.Key, ySpeed.Key));
            continue;
        }
    }

}

Console.WriteLine("Combo count {0}", launchSpeeds.Count);

/*
--- Day 17: Trick Shot ---
You finally decode the Elves' message. HI, the message says. You continue searching for the sleigh keys.

Ahead of you is what appears to be a large ocean trench. Could the keys have fallen into it? You'd better send a probe to investigate.

The probe launcher on your submarine can fire the probe with any integer velocity in the x (forward) and y (upward, or downward if negative) directions. For example, an initial x,y velocity like 0,10 would fire the probe straight up, while an initial velocity like 10,-1 would fire the probe forward at a slight downward angle.

The probe's x,y position starts at 0,0. Then, it will follow some trajectory by moving in steps. On each step, these changes occur in the following order:

The probe's x position increases by its x velocity.
The probe's y position increases by its y velocity.
Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 if it is greater than 0, increases by 1 if it is less than 0, or does not change if it is already 0.
Due to gravity, the probe's y velocity decreases by 1.
For the probe to successfully make it into the trench, the probe must be on some trajectory that causes it to be within a target area after any step. The submarine computer has already calculated this target area (your puzzle input). For example:

target area: x=20..30, y=-10..-5
This target area means that you need to find initial x,y velocity values such that after any step, the probe's x position is at least 20 and at most 30, and the probe's y position is at least -10 and at most -5.

Given this target area, one initial velocity that causes the probe to be within the target area after any step is 7,2:

.............#....#............
.......#..............#........
...............................
S........................#.....
...............................
...............................
...........................#...
...............................
....................TTTTTTTTTTT
....................TTTTTTTTTTT
....................TTTTTTTT#TT
....................TTTTTTTTTTT
....................TTTTTTTTTTT
....................TTTTTTTTTTT
In this diagram, S is the probe's initial position, 0,0. The x coordinate increases to the right, and the y coordinate increases upward. In the bottom right, positions that are within the target area are shown as T. After each step (until the target area is reached), the position of the probe is marked with #. (The bottom-right # is both a position the probe reaches and a position in the target area.)

Another initial velocity that causes the probe to be within the target area after any step is 6,3:

...............#..#............
...........#........#..........
...............................
......#..............#.........
...............................
...............................
S....................#.........
...............................
...............................
...............................
.....................#.........
....................TTTTTTTTTTT
....................TTTTTTTTTTT
....................TTTTTTTTTTT
....................TTTTTTTTTTT
....................T#TTTTTTTTT
....................TTTTTTTTTTT
Another one is 9,0:

S........#.....................
.................#.............
...............................
........................#......
...............................
....................TTTTTTTTTTT
....................TTTTTTTTTT#
....................TTTTTTTTTTT
....................TTTTTTTTTTT
....................TTTTTTTTTTT
....................TTTTTTTTTTT
One initial velocity that doesn't cause the probe to be within the target area after any step is 17,-4:

S..............................................................
...............................................................
...............................................................
...............................................................
.................#.............................................
....................TTTTTTTTTTT................................
....................TTTTTTTTTTT................................
....................TTTTTTTTTTT................................
....................TTTTTTTTTTT................................
....................TTTTTTTTTTT..#.............................
....................TTTTTTTTTTT................................
...............................................................
...............................................................
...............................................................
...............................................................
................................................#..............
...............................................................
...............................................................
...............................................................
...............................................................
...............................................................
...............................................................
..............................................................#
The probe appears to pass through the target area, but is never within it after any step. Instead, it continues down and to the right - only the first few steps are shown.

If you're going to fire a highly scientific probe out of a super cool probe launcher, you might as well do it with style. How high can you make the probe go while still reaching the target area?

In the above example, using an initial velocity of 6,9 is the best you can do, causing the probe to reach a maximum y position of 45. (Any higher initial y velocity causes the probe to overshoot the target area entirely.)

Find the initial velocity that causes the probe to reach the highest y position and still eventually be within the target area after any step. What is the highest y position it reaches on this trajectory?

--- Part Two ---
Maybe a fancy trick shot isn't the best idea; after all, you only have one probe, so you had better not miss.

To get the best idea of what your options are for launching the probe, you need to find every initial velocity that causes the probe to eventually be within the target area after any step.

In the above example, there are 112 different initial velocity values that meet these criteria:

23,-10  25,-9   27,-5   29,-6   22,-6   21,-7   9,0     27,-7   24,-5
25,-7   26,-6   25,-5   6,8     11,-2   20,-5   29,-10  6,3     28,-7
8,0     30,-6   29,-8   20,-10  6,7     6,4     6,1     14,-4   21,-6
26,-10  7,-1    7,7     8,-1    21,-9   6,2     20,-7   30,-10  14,-3
20,-8   13,-2   7,3     28,-8   29,-9   15,-3   22,-5   26,-8   25,-8
25,-6   15,-4   9,-2    15,-2   12,-2   28,-9   12,-3   24,-6   23,-7
25,-10  7,8     11,-3   26,-7   7,1     23,-9   6,0     22,-10  27,-6
8,1     22,-8   13,-4   7,6     28,-6   11,-4   12,-4   26,-9   7,4
24,-10  23,-8   30,-8   7,0     9,-1    10,-1   26,-5   22,-9   6,5
7,5     23,-6   28,-10  10,-2   11,-1   20,-9   14,-2   29,-7   13,-3
23,-5   24,-8   27,-9   30,-7   28,-5   21,-10  7,9     6,6     21,-5
27,-10  7,2     30,-9   21,-8   22,-7   24,-9   20,-6   6,9     29,-5
8,-2    27,-8   30,-5   24,-7
How many distinct initial velocity values cause the probe to be within the target area after any step?
*/
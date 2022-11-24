#!/usr/bin/env dotnet-script

class Map<T> {

    public Map(int initialW, int initialH) {
        _w = initialW;
        _h = initialH;
        Value = new T[_w*_h];
    }

    private int _w, _h;
    public int Width { get { return _w; } }
    public int Height { get { return _h; } }

    public void Grow(T fill) {
        T[] newMap = new T[(_w+2)*(_h+2)];
        Array.Fill(newMap, fill);

        for (int y = 0; y < _h; ++y)
            Array.Copy(Value, y*_w, newMap, (y+1)*(_w+2)+1, _w);
        Value = newMap;
        _w += 2;
        _h += 2;
    }

    public T[] GetWindow(int x, int y, int size) {
        if ((size % 2) == 0)
            throw new Exception("Bad window size");

        if (x == 0 || x == (_w - 1) || y == 0 || y == (_h -1))
            throw new Exception("Bad window size");

        var ret = new T[size*size];
        for (int i = 0; i < size; ++i)
            Array.Copy(Value, _w*(i+y-size/2) + x-size/2, ret, i*size, size);

        return ret;
    }

    public T[] Value { get; set; }
}

var rawInput = System.IO.File.ReadAllLines("day20.txt");

var algo = rawInput[0];

var map = new Map<bool>(rawInput.Skip(2).First().Length, rawInput.Length-2);

int y = 0;

foreach (var line in rawInput.Skip(2)) {
    Array.Copy(line.Select(x => x == '#').ToArray(), 0, map.Value, map.Width * y, map.Width);
    y++;
}


bool fill = false;
for (int loop = 0; loop < 50; ++loop) {
    map.Grow(fill);
    var newMap = new Map<bool>(map.Width, map.Height);
    map.Grow(fill);
    for (int y = 0; y < newMap.Height; ++y) {
        for (int x = 0; x < newMap.Width; ++x) {
            var mapWindow = map.GetWindow(x+1, y+1, 3).Aggregate("0", (t, i) => t + (i ? "1" : "0"));
            var idx = Convert.ToUInt16(mapWindow, 2);
            newMap.Value[y*newMap.Width+x] = (algo[idx] == '#');
        }
    }
    fill = algo[fill ? algo.Length-1 : 0] == '#';
    map = newMap;

    if (loop == 1)
        Console.WriteLine("Lit elements @ iter 2 {0}", map.Value.Count(x => x));
}

Console.WriteLine("Lit elements after 50 {0}", map.Value.Count(x => x));


void dumpMap() {
    for (int y = 0; y < map.Height; ++y) {
        Console.WriteLine(map.Value.Skip(y*map.Width).Take(map.Width).Aggregate("", (t, x) => t + (x ? "#" : ".")));
    }
}
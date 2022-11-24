#!/usr/bin/env dotnet-script

var rawInput = System.IO.File.ReadAllLines("day14.txt");
var template = rawInput.First();
Dictionary<string,string> rules = new Dictionary<string, string>();
foreach (var rule in rawInput.Skip(2).Select(x => x.Split("->", StringSplitOptions.TrimEntries)))
    rules.Add(rule[0], rule[1]);

for (int i = 0; i < 10; ++i) {
    int j = 0;
    while (j < (template.Length-1)) {
        string it = template.Substring(j, 2);
        template = template.Insert(j+1, rules[it]);
        j += 2;
    }
}

var totals = template.GroupBy(x => x).Select(x => x.Count());
Console.WriteLine("Element difference after 10 steps {0}", totals.Max() - totals.Min());

var elementPairs = new Dictionary<string, Int64>();
template = rawInput.First();

void addElement(Dictionary<string, Int64> dict, string elem, Int64 count) {
    dict[elem] = dict.GetValueOrDefault(elem) + count;
}

for (int i = 0; i < template.Length-1; ++i)
    addElement(elementPairs, template.Substring(i, 2), 1);

for (int i = 0; i < 40; ++i) {
    var newPolymer = new Dictionary<string, Int64>();
    foreach (var elem in elementPairs) {
        addElement(newPolymer, elem.Key.First() + rules[elem.Key], elem.Value);
        addElement(newPolymer, rules[elem.Key] + elem.Key.Last(), elem.Value);
    }
    elementPairs = newPolymer;
}


var elemTotals = new Dictionary<char,Int64>();
foreach (var elem in elementPairs)
    elemTotals[elem.Key.First()] = elemTotals.GetValueOrDefault(elem.Key.First()) + elem.Value;
elemTotals[template.Last()] = elemTotals[template.Last()]+1;
Console.WriteLine("Element difference after 40 steps {0}", elemTotals.Max(x => x.Value) - elemTotals.Min(x => x.Value));


/*
--- Day 14: Extended Polymerization ---
The incredible pressures at this depth are starting to put a strain on your submarine. The submarine has polymerization equipment that would produce suitable materials to reinforce the submarine, and the nearby volcanically-active caves should even have the necessary input elements in sufficient quantities.

The submarine manual contains instructions for finding the optimal polymer formula; specifically, it offers a polymer template and a list of pair insertion rules (your puzzle input). You just need to work out what polymer would result after repeating the pair insertion process a few times.

For example:

NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C
The first line is the polymer template - this is the starting point of the process.

The following section defines the pair insertion rules. A rule like AB -> C means that when elements A and B are immediately adjacent, element C should be inserted between them. These insertions all happen simultaneously.

So, starting with the polymer template NNCB, the first step simultaneously considers all three pairs:

The first pair (NN) matches the rule NN -> C, so element C is inserted between the first N and the second N.
The second pair (NC) matches the rule NC -> B, so element B is inserted between the N and the C.
The third pair (CB) matches the rule CB -> H, so element H is inserted between the C and the B.
Note that these pairs overlap: the second element of one pair is the first element of the next pair. Also, because all pairs are considered simultaneously, inserted elements are not considered to be part of a pair until the next step.

After the first step of this process, the polymer becomes NCNBCHB.

Here are the results of a few steps using the above rules:

Template:     NNCB
After step 1: NCNBCHB
After step 2: NBCCNBBBCBHCB
After step 3: NBBBCNCCNBBNBNBBCHBHHBCHB
After step 4: NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB
This polymer grows quickly. After step 5, it has length 97; After step 10, it has length 3073. After step 10, B occurs 1749 times, C occurs 298 times, H occurs 161 times, and N occurs 865 times; taking the quantity of the most common element (B, 1749) and subtracting the quantity of the least common element (H, 161) produces 1749 - 161 = 1588.

Apply 10 steps of pair insertion to the polymer template and find the most and least common elements in the result. What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?

--- Part Two ---
The resulting polymer isn't nearly strong enough to reinforce the submarine. You'll need to run more steps of the pair insertion process; a total of 40 steps should do it.

In the above example, the most common element is B (occurring 2192039569602 times) and the least common element is H (occurring 3849876073 times); subtracting these produces 2188189693529.

Apply 40 steps of pair insertion to the polymer template and find the most and least common elements in the result. What do you get if you take the quantity of the most common element and subtract the quantity of the least common element?

*/

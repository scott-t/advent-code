use std::fs::File;
use std::io::{self,BufRead};
use std::path::Path;

#[derive(Debug)]
struct Move {
    num: i32,
    src: i32,
    dst: i32
}

impl Move {
    fn parse(input: &str) -> Move {
        let mut input = input.split_whitespace();

        return Move{num: input.nth(1).unwrap().parse().unwrap(), src: input.nth(1).unwrap().parse::<i32>().unwrap() - 1, dst: input.nth(1).unwrap().parse::<i32>().unwrap() - 1};
    }
}


fn main() {
    if let Ok(lines) = read_lines("input/day05.txt") {

        let mut stacks: Vec<Vec<char>> = Vec::new();
        let mut moveset: Vec<Move> = Vec::new();
        let mut seen_break = false;

        for line in lines {
            if let Ok(line) = line {
                let len = line.len();
                let line = line.chars();
                if !seen_break {
                    if len == 0 {
                        seen_break = true;
                        // Reverse the order from the readin
                        for stack in stacks.iter_mut() {
                            stack.reverse();
                        }
                    }
                    else {
                        while (stacks.len() * 4) < len {
                            stacks.push(Vec::new());
                        }

                        for (idx, val) in line.enumerate().skip(1) {
                            let idx = (idx - 1) / 4;

                            if val >= 'A' && val <= 'Z' {
                                stacks[idx].push(val);
                            }
                        }
                    }
                }
                else {
                    moveset.push(Move::parse(line.as_str()));
                }
            }
        }

        let mut stacks2 = stacks.clone();

        for mv in moveset {

            for _i in 0..mv.num {
                let item = stacks[mv.src as usize].pop().unwrap();
                stacks[mv.dst as usize].push(item);
                //println!("{:?}", stacks);
            }

            // Do pt 2
            let dest_len = stacks2[mv.dst as usize].len();
            for _i in 0..mv.num {
                let item = stacks2[mv.src as usize].pop().unwrap();
                stacks2[mv.dst as usize].insert(dest_len, item);
                //println!("{:?}", stacks2);
            }

        }

        let mut pt1: String = String::new();
        for stack in stacks {
            pt1.push(*stack.last().unwrap());
        }

        let mut pt2: String = String::new();
        for stack in stacks2 {
            pt2.push(*stack.last().unwrap());
        }

        println!("Part 1: {}", pt1);
        println!("Part 1: {}", pt2);


        //println!("Part 2: {}", overlap_count);
    }
}


// Shamelessly borrowed from rust-by-example docs
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where P: AsRef<Path>, {
    let file = std::fs::File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

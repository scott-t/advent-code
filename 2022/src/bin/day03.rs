use std::fs::File;
use std::io::{self,BufRead};
use std::path::Path;

fn main() {
    if let Ok(lines) = read_lines("input/day03.txt") {
        let mut score1 = 0;
        let mut score2 = 0;
        let mut pack_groups: Vec<String> = Vec::new();
        for line in lines {
            if let Ok(items) = line {
                let (front, back) = items.split_at(items.len() / 2);
                let front = front.split("").collect::<Vec<&str>>();
                let item = front.iter().filter(|x| !x.is_empty() && back.contains(x.chars().next().unwrap())).next().unwrap().chars().next().unwrap();
                //.next().unwrap().chars().next().unwrap();
                if item >= 'a' {
                    score1 += item as i32 - 96;
                } else {
                    score1 += item as i32 - 64 + 26;
                }

                if pack_groups.len() == 2 {
                    // Third line
                    let pack1 = pack_groups.pop().unwrap();
                    let pack2 = pack_groups.pop().unwrap();
                    let badge = items.split("").filter(|x| !x.is_empty() && pack1.contains(x) && pack2.contains(x)).next().unwrap().chars().next().unwrap();

                    if badge >= 'a' {
                        score2 += badge as i32 - 96;
                    } else {
                        score2 += badge as i32 - 64 + 26;
                    }
                }
                else {
                    pack_groups.push(items);
                }
            }
        }
        println!("Part 1 {}", score1);
        println!("Part 2 {}", score2);
    }
}


// Shamelessly borrowed from rust-by-example docs
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where P: AsRef<Path>, {
    let file = std::fs::File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

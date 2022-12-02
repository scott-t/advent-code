use std::fs::File;
use std::io::{self,BufRead};
use std::path::Path;

struct Moves {
    me: i8,
    them: i8,
}

impl std::str::FromStr for Moves {
    type Err = std::num::ParseIntError;

    fn from_str(pair: &str) -> Result<Self, Self::Err> {
        let p = pair.as_bytes();
        Ok(Moves { me: ((p[2] as i8)  - ('X' as i8)) as i8, them: ((p[0] as i8)  - ('A' as i8) ) as i8 })
    }
}


fn main() {
    if let Ok(lines) = read_lines("input/day02.txt") {
        let mut score: u64 = 0;
        for line in lines {
            if let Ok(mv) = line {
                let game = mv.parse::<Moves>().unwrap();

                // Probably a neater way. +1 due to rock = 0 in input
                score += ((game.me - game.them + 4) % 3 * 3 + game.me + 1) as u64;
            }
        }
        println!("Part 1 {}", score);
    }
}


// Shamelessly borrowed from rust-by-example docs
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where P: AsRef<Path>, {
    let file = std::fs::File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

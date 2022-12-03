use std::fs::File;
use std::io::{self,BufRead};
use std::path::Path;

struct Moves {
    me: i32,
    them: i32,
}

impl std::str::FromStr for Moves {
    type Err = std::num::ParseIntError;

    fn from_str(pair: &str) -> Result<Self, Self::Err> {
        let p = pair.as_bytes();
        Ok(Moves { me: ((p[2] as i32)  - ('X' as i32)), them: ((p[0] as i32)  - ('A' as i32))})
    }
}


fn main() {
    if let Ok(lines) = read_lines("input/day02.txt") {
        let mut score1 = 0;
        let mut score2 = 0;
        for line in lines {
            if let Ok(mv) = line {
                let game = mv.parse::<Moves>().unwrap();

                // Probably a neater way. +1 due to rock = 0 in input
                score1 += (game.me - game.them + 4) % 3 * 3 + game.me + 1;
                //println!("{} {}", (game.me * 3), (game.them + game.me + 2) % 3 + 1);
                score2 += game.me * 3;
                score2 += (game.them + game.me + 2) % 3 + 1;
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

use std::fs::File;
use std::io::{self,BufRead};
use std::path::Path;

struct Span {
    start: i32,
    len: i32,
}

impl Span {
    fn contains(&self, other: &Span) -> bool {
        if self.len >= other.len {
            //println!("{} {} v {} {}", self.start, self.len, other.start, other.len);
            return self.start <= other.start && (self.start + self.len) >= (other.start + other.len);
        }
        return other.contains(self);
    }

    fn overlap(&self, other: &Span) -> bool {
        if self.start <= other.start {
            return (self.start + self.len) > other.start;
        }
        return other.overlap(self);
    }

    fn parse(pair: &str) -> Span {
        let offset = pair.find('-').unwrap();
        let start = pair[0..offset].parse::<i32>().unwrap();
        let end = pair[offset+1..pair.len()].parse::<i32>().unwrap();
        return Span { start: start, len: end - start + 1 };
    }
}



fn main() {
    if let Ok(lines) = read_lines("input/day04.txt") {
        let mut contains_count = 0;
        let mut overlap_count = 0;
        for line in lines {
            if let Ok(sections) = line {
                let sections: Vec<&str> = sections.split(",").collect();
                let first = Span::parse(sections[0]);
                let second = Span::parse(sections[1]);
                if first.contains(&second) {
                    contains_count += 1;
                }
                if first.overlap(&second) {
                    overlap_count += 1;
                }

            }
        }
        println!("Part 1: {}", contains_count);
        println!("Part 2: {}", overlap_count);
    }
}


// Shamelessly borrowed from rust-by-example docs
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where P: AsRef<Path>, {
    let file = std::fs::File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

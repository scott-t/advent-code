use std::fs::File;
use std::io::{self,BufRead};
use std::path::Path;


fn main() {
    if let Ok(lines) = read_lines("input/day06.txt") {

        for line in lines {
            let line = line.unwrap();
            let line_len = line.len();

            for i in 0..line_len {
                let mut slice = vec![];
                slice.reserve(4);
                slice.extend_from_slice(&line[i..(i+4)].as_bytes());
                slice.sort();
                slice.dedup();
                if slice.len() == 4 {
                    println!("Part 1: {}", i+4);
                    break;
                }
            }

            for i in 0..line_len {
                let mut slice = vec![];
                slice.reserve(14);
                slice.extend_from_slice(&line[i..(i+14)].as_bytes());
                slice.sort();
                slice.dedup();
                if slice.len() == 14 {
                    println!("Part 1: {}", i+14);
                    break;
                }
            }


        }
    }
}


// Shamelessly borrowed from rust-by-example docs
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where P: AsRef<Path>, {
    let file = std::fs::File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

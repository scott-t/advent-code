use std::fs::File;
use std::io::{self,BufRead};
use std::path::Path;


fn main() {
    if let Ok(lines) = read_lines("input/day01.txt") {
        let mut cal_count = 0;
        let mut elf = vec![];

        for line in lines {
            if let Ok(cal) = line {
                if cal.is_empty() {
                    if cal_count != 0 {
                        elf.push(cal_count);
                    }
                    cal_count = 0;
                }
                else {
                    cal_count += cal.parse::<i32>().unwrap();
                }
            }
        }

        if cal_count != 0 {
            elf.push(cal_count);
        }

        println!("Part 1: {}", elf.iter().max().unwrap());
        elf.sort_unstable();
        elf.reverse();
        println!("Part 2: {:?}", elf.iter().take(3).sum::<i32>());
    }
}

// Shamelessly borrowed from rust-by-example docs
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where P: AsRef<Path>, {
    let file = std::fs::File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}

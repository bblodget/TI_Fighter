#!/usr/bin/env python3

import sys
import os

source_file = "ti_fighter.fs"
block_file = "FIGHTER"
block1_start = 128
block_size = 1024
block_inc = 64
pos = 0;

block_file_size = os.path.getsize(block_file)
print("block_file_size: {}".format(block_file_size))

# Open the source file for read
with open(source_file, 'r') as sf:

    # Open block_file as binary for read
    with open(block_file, 'r+b') as bf:
        pos = block1_start;
        bf.seek(pos)

        block_line = 0;
        for line in sf:
            # replace tabs with space.
            # remove line endings
            line = line.replace("\t"," ")
            line = line.replace("\n","")
            line = line.replace("\r","")
            # Pad line to be 64 chars in length
            line = "{:<64}".format(line)
            # encode byte string
            bline = line.encode('utf-8')
            # Add continuation -->> to next block on line 15
            if block_line == 15:
                block_line = 0
                bf.write(b"-->>")
                pos = pos + block_inc
                bf.seek(pos)
            bf.write(bline)
            pos = pos + block_inc
            bf.seek(pos)
            block_line = block_line + 1

        # Clear the rest of the file
        blank_line = " " * 64
        blank_line = blank_line.encode('utf-8')
        lines_left = (block_file_size - pos) / 64;
        print("lines_left: {}".format(lines_left))
        for x in range(int(lines_left)):
            bf.write(blank_line)








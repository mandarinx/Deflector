#!/bin/sh

palette="./palette.png"

filters="fps=30,scale=506:-1:flags=lanczos"

ffmpeg -v warning -i "$1.mov" -vf "$filters,palettegen" -y $palette
ffmpeg -v warning -i "$1.mov" -i $palette -lavfi "$filters [x]; [x][1:v] paletteuse" -y "$1.gif"

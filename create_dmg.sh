#! /bin/sh

name="Deflector $1"
filename="Deflector_$1"

dmgbuild -s dmgsettings.py \
-D app="./Builds/$1/macOS/$filename.app" \
"$name" \
"./Builds/$1/macOS/$filename.dmg"

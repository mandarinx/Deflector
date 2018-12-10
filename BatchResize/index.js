'use strict';

let root = '../GIFs';

const sharp = require('sharp');
const path = require('path');
const fs = require('fs');
const { lstatSync, readdirSync } = require('fs');
const { join } = require('path');

const isDirectory = source => lstatSync(source).isDirectory();
const getDirectories = source => readdirSync(source).map(name => join(source, name)).filter(isDirectory);
const getPath = filename => path.join(__dirname, filename);
const resize = (file, dest) => sharp(file).resize(720, 576, {
      kernel: sharp.kernel.nearest
    })
    .toFile(dest);

getDirectories(root)
  .forEach(function(dir) {
    let twox = path.join(dir, '2x');

    if (!fs.existsSync(twox)){
      fs.mkdirSync(twox);
    }

    console.log(dir);

    let pngs = readdirSync(dir).filter(name => name.indexOf('.png') >= 0);

    readdirSync(dir)
      .filter(name => name.indexOf('.png') >= 0)
      .forEach(function(file) {
        let source = path.join(dir, file);
        let target = path.join(twox, file);
        console.log(source, '=>', target);
        resize(source, target);
      });
  });


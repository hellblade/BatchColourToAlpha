# Colour to Alpha

This is a very small application that is used to convert a colour (chroma key) inside of images to transparency and saves it as a PNG.

Overwrites the existing image unless you choose a new output directory.

```
> ColourToAlpha --help
ColourToAlpha:
  Converts a specfic colour (chroma key) in images and makes them transparent

Usage:
  ColourToAlpha [options] [<files>...]

Arguments:
  <files>

Options:
  -c, --color, --colour <color>    The named colour to be changed
  --outdir <outdir>                The custom directory to output the changed image.
  --version                        Display version information
```

## Future improvements
- Merge together all the DLL files into one exe and strip unused code
- Allow a threshold to be given to change colours similar to the chroma key
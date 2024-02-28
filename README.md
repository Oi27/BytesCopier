Copies range of bytes from an address in one file, to an address in many other files.
Helper tool for managing bitmap palettes in TexTile tilesets.

Usage: bytescopier.exe "fileA.bmp" #### @@@@ *.bmp
Where #### is the starting address
      @@@@ is the number of bytes
      *.bmp specifies copy to every .bmp in the same folder as "fileA.bmp"

Using it also creates a backup of the original files. To revert the changes:
    bytescopier.exe undo

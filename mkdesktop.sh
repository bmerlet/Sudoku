#! /bin/sh

# goto directory where this script lives
cd "$(dirname "$0")"

# program name
here=`pwd`
programdir=$here/FormsUI/bin/Debug
program=$programdir/FormsUI.exe
icon=$here/FormsUI/Properties/Sudoku.ico

# desktop file name
desktop=~/.local/share/applications/Sudoku.desktop

# create desktop file
mkdir -p ~/.local/share/applications

rm -f $desktop
cat - > $desktop <<EOF
[Desktop Entry]
Version=1.0
Name=Sudoku
Comment=Sudoku puzzle generator
Exec=/usr/bin/mono $program
Path=$programdir
Icon=$icon
Terminal=false
Type=Application
Categories=Game;
EOF

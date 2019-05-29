#! /bin/sh

# Builder path
# PATH=$PATH:"/c/Program Files (x86)/MSBuild/12.0/Bin/"
PATH=$PATH:"/c/Program Files (x86)/Microsoft Visual Studio/2019/Community/MSBuild/Current/Bin"

# Build Spider
msbuild.exe Sudoku.sln /property:Configuration=Debug /property:Platform="Any CPU" || exit 1



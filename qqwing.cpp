/*
 * qqwing - Sudoku solver and generator
 * Copyright (C) 2006-2014 Stephen Ostermiller http://ostermiller.org/
 * Copyright (C) 2007 Jacques Bensimon (jacques@ipm.com)
 * Copyright (C) 2011 Jean Guillerez (j.guillerez - orange.fr)
 * Copyright (C) 2014 Michael Catanzaro (mcatanzaro@gnome.org)
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 */
#include "config.h"

#include <cstdlib>
#include <iostream>

#include "qqwing.hpp"

namespace qqwing {

	string getVersion(){
		return VERSION;
	}


	void shuffleArray(int* array, int size);
	SudokuBoard::Symmetry getRandomSymmetry();
	int getLogCount(vector<LogItem*>* v, LogItem::LogType type);

	/**
	 * Create a new Sudoku board
	 */
	SudokuBoard::SudokuBoard() :
		puzzle ( new int[BOARD_SIZE] ),
		solution ( new int[BOARD_SIZE] ),
		solutionRound ( new int[BOARD_SIZE] ),
		possibilities ( new int[POSSIBILITY_SIZE] ),
		randomBoardArray ( new int[BOARD_SIZE] ),
		randomPossibilityArray ( new int[ROW_COL_SEC_SIZE] ),
		recordHistory ( false ),
		logHistory( false ),
		solveHistory ( new vector<LogItem*>() ),
		solveInstructions ( new vector<LogItem*>() ),
		printStyle ( READABLE ),
		lastSolveRound (0)
	{
		{for (int i=0; i<BOARD_SIZE; i++){
			randomBoardArray[i] = i;
		}}
		{for (int i=0; i<ROW_COL_SEC_SIZE; i++){
			randomPossibilityArray[i] = i;
		}}
	}

	/**
	 * Get the number of cells that are
	 * set in the puzzle (as opposed to
	 * figured out in the solution
	 */
	int SudokuBoard::getGivenCount(){
		int count = 0;
		{for (int i=0; i<BOARD_SIZE; i++){
			if (puzzle[i] != 0) count++;
		}}
		return count;
	}

	/**
	 * Set the board to the given puzzle.
	 * The given puzzle must be an array of 81 integers.
	 */
	bool SudokuBoard::setPuzzle(int* initPuzzle){
		{for (int i=0; i<BOARD_SIZE; i++){
			puzzle[i] = (initPuzzle==NULL)?0:initPuzzle[i];
		}}
		return reset();
	}

	/**
	 * Retrieves the puzzle as an unmodifiable array of 81 integers.
	 */
	const int* SudokuBoard::getPuzzle(){
		return puzzle;
	}

	/**
	 * Retrieves the puzzle's solution as an unmodifiable array of 81 integers.
	 */
	const int* SudokuBoard::getSolution(){
		return solution;
	}

	/**
	 * Reset the board to its initial state with
	 * only the givens.
	 * This method clears any solution, resets statistics,
	 * and clears any history messages.
	 */
	bool SudokuBoard::reset(){
		{for (int i=0; i<BOARD_SIZE; i++){
			solution[i] = 0;
		}}
		{for (int i=0; i<BOARD_SIZE; i++){
			solutionRound[i] = 0;
		}}
		{for (int i=0; i<POSSIBILITY_SIZE; i++){
			possibilities[i] = 0;
		}}

		{for (unsigned int i=0; i<solveHistory->size(); i++){
			delete solveHistory->at(i);
		}}
		solveHistory->clear();
		solveInstructions->clear();

		int round = 1;
		for (int position=0; position<BOARD_SIZE; position++){
			if (puzzle[position] > 0){
				int valIndex = puzzle[position]-1;
				int valPos = getPossibilityIndex(valIndex,position);
				int value = puzzle[position];
				if (possibilities[valPos] != 0) return false;
				mark(position,round,value);
				if (logHistory || recordHistory) addHistoryItem(new LogItem(round, LogItem::GIVEN, value, position));
			}
		}
		return true;
	}


	void SudokuBoard::shuffleRandomArrays(){
		shuffleArray(randomBoardArray, BOARD_SIZE);
		shuffleArray(randomPossibilityArray, ROW_COL_SEC_SIZE);
	}

	void SudokuBoard::clearPuzzle(){
		// Clear any existing puzzle
		{for (int i=0; i<BOARD_SIZE; i++){
			puzzle[i] = 0;
		}}
		reset();
	}

	bool SudokuBoard::generatePuzzle(){
		return generatePuzzleSymmetry(SudokuBoard::NONE);
	}

	bool SudokuBoard::generatePuzzleSymmetry(SudokuBoard::Symmetry symmetry){

		if (symmetry == SudokuBoard::RANDOM) symmetry = getRandomSymmetry();

		// Don't record history while generating.
		bool recHistory = recordHistory;
		setRecordHistory(false);
		bool lHistory = logHistory;
		setLogHistory(false);

		clearPuzzle();

		// Start by getting the randomness in order so that
		// each puzzle will be different from the last.
		shuffleRandomArrays();

		// Now solve the puzzle the whole way.  The solve
		// uses random algorithms, so we should have a
		// really randomly totally filled sudoku
		// Even when starting from an empty grid
		solve();

		if (symmetry == SudokuBoard::NONE){
			// Rollback any square for which it is obvious that
			// the square doesn't contribute to a unique solution
			// (ie, squares that were filled by logic rather
			// than by guess)
			rollbackNonGuesses();
		}

		// Record all marked squares as the puzzle so
		// that we can call countSolutions without losing it.
		{for (int i=0; i<BOARD_SIZE; i++){
			puzzle[i] = solution[i];
		}}

		// Rerandomize everything so that we test squares
		// in a different order than they were added.
		shuffleRandomArrays();

		// Remove one value at a time and see if
		// the puzzle still has only one solution.
		// If it does, leave it out the point because
		// it is not needed.
		{for (int i=0; i<BOARD_SIZE; i++){
			// check all the positions, but in shuffled order
			int position = randomBoardArray[i];
			if (puzzle[position] > 0){
				int positionsym1 = -1;
				int positionsym2 = -1;
				int positionsym3 = -1;
				switch (symmetry){
					case ROTATE90:
						positionsym2 = rowColumnToCell(ROW_COL_SEC_SIZE-1-cellToColumn(position),cellToRow(position));
						positionsym3 = rowColumnToCell(cellToColumn(position),ROW_COL_SEC_SIZE-1-cellToRow(position));
					case ROTATE180:
						positionsym1 = rowColumnToCell(ROW_COL_SEC_SIZE-1-cellToRow(position),ROW_COL_SEC_SIZE-1-cellToColumn(position));
					break;
					case MIRROR:
						positionsym1 = rowColumnToCell(cellToRow(position),ROW_COL_SEC_SIZE-1-cellToColumn(position));
					break;
					case FLIP:
						positionsym1 = rowColumnToCell(ROW_COL_SEC_SIZE-1-cellToRow(position),cellToColumn(position));
					break;
					case RANDOM: // NOTE: Should never happen
					break;
					case NONE: // NOTE: No need to do anything
					break;
				}
				// try backing out the value and
				// counting solutions to the puzzle
				int savedValue = puzzle[position];
				puzzle[position] = 0;
				int savedSym1 = 0;
				if (positionsym1 >= 0){
					savedSym1 = puzzle[positionsym1];
					puzzle[positionsym1] = 0;
				}
				int savedSym2 = 0;
				if (positionsym2 >= 0){
					savedSym2 = puzzle[positionsym2];
					puzzle[positionsym2] = 0;
				}
				int savedSym3 = 0;
				if (positionsym3 >= 0){
					savedSym3 = puzzle[positionsym3];
					puzzle[positionsym3] = 0;
				}
				reset();
				if (countSolutions(2, true) > 1){
					// Put it back in, it is needed
					puzzle[position] = savedValue;
					if (positionsym1 >= 0 && savedSym1 != 0) puzzle[positionsym1] = savedSym1;
					if (positionsym2 >= 0 && savedSym2 != 0) puzzle[positionsym2] = savedSym2;
					if (positionsym3 >= 0 && savedSym3 != 0) puzzle[positionsym3] = savedSym3;
				}
			}
		}}

		// Clear all solution info, leaving just the puzzle.
		reset();

		// Restore recording history.
		setRecordHistory(recHistory);
		setLogHistory(lHistory);

		return true;

	}

	void SudokuBoard::rollbackNonGuesses(){
		// Guesses are odd rounds
		// Non-guesses are even rounds
		{for (int i=2; i<=lastSolveRound; i+=2){
			rollbackRound(i);
		}}
	}

	void SudokuBoard::setPrintStyle(PrintStyle ps){
		printStyle = ps;
	}

	void SudokuBoard::setRecordHistory(bool recHistory){
		recordHistory = recHistory;
	}

	void SudokuBoard::setLogHistory(bool logHist){
		logHistory = logHist;
	}

	void SudokuBoard::addHistoryItem(LogItem* l){
		if (logHistory){
			l->print();
			cout << endl;
		}
		if (recordHistory){
			solveHistory->push_back(l);
			solveInstructions->push_back(l);
		} else {
			delete l;
		}
	}

	void SudokuBoard::printHistory(vector<LogItem*>* v){
		if (!recordHistory){
			cout << "History was not recorded.";
			if (printStyle == CSV){
				cout << " -- ";
			} else {
				cout << endl;
			}
		}
		{for (unsigned int i=0;i<v->size();i++){
			cout << i+1 << ". ";
			v->at(i)->print();
			if (printStyle == CSV){
				cout << " -- ";
			} else {
				cout << endl;
			}
		}}
		if (printStyle == CSV){
			cout << ",";
		} else {
			cout << endl;
		}
	}

	void SudokuBoard::printSolveInstructions(){
		if (isSolved()){
			printHistory(solveInstructions);
		} else {
			cout << "No solve instructions - Puzzle is not possible to solve." << endl;
		}
	}

	void SudokuBoard::printSolveHistory(){
		printHistory(solveHistory);
	}


	bool SudokuBoard::hasUniqueSolution(){
		return countSolutionsLimited() == 1;
	}

	int SudokuBoard::countSolutions(){
		return countSolutions(false);
	}

	int SudokuBoard::countSolutionsLimited(){
		return countSolutions(true);
	}

	int SudokuBoard::countSolutions(bool limitToTwo){
		// Don't record history while generating.
		bool recHistory = recordHistory;
		setRecordHistory(false);
		bool lHistory = logHistory;
		setLogHistory(false);

		reset();
		int solutionCount = countSolutions(2, limitToTwo);

		// Restore recording history.
		setRecordHistory(recHistory);
		setLogHistory(lHistory);

		return solutionCount;
	}

	int SudokuBoard::countSolutions(int round, bool limitToTwo){
		while (singleSolveMove(round)){
			if (isSolved()){
				rollbackRound(round);
				return 1;
			}
			if (isImpossible()){
				rollbackRound(round);
				return 0;
			}
		}

		int solutions = 0;
		int nextRound = round+1;
		for (int guessNumber=0; guess(nextRound, guessNumber); guessNumber++){
			solutions += countSolutions(nextRound, limitToTwo);
			if (limitToTwo && solutions >=2){
				rollbackRound(round);
				return solutions;
			}
		}
		rollbackRound(round);
		return solutions;
	}





	/**
	 * Print the sudoku puzzle.
	 */
	void SudokuBoard::printPuzzle(){
		print(puzzle);
	}

	/**
	 * Print the sudoku solution.
	 */
	void SudokuBoard::printSolution(){
		print(solution);
	}

	SudokuBoard::~SudokuBoard(){
		clearPuzzle();
		delete[] puzzle;
		delete[] solution;
		delete[] possibilities;
		delete[] solutionRound;
		delete[] randomBoardArray;
		delete[] randomPossibilityArray;
		delete solveHistory;
		delete solveInstructions;
	}

	/**
	 * Shuffle the values in an array of integers.
	 */
	void shuffleArray(int* array, int size){
		{for (int i=0; i<size; i++){
			int tailSize = size-i;
			int randTailPos = rand()%tailSize+i;
			int temp = array[i];
			array[i] = array[randTailPos];
			array[randTailPos] = temp;
		}}
	}

}

using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    internal class PositionRandomizer
    {
        private Position[] positions = new Position[Position.BOARD_SIZE];

        public Position this[Position pos] => positions[pos.Cell];

        public PositionRandomizer(Random random)
        {

            // Initialize array in order
            for (uint i = 0; i < positions.Length; i++)
            {
                positions[i] = Position.GetCellFromCell(i);
            }

            // Randomize it
            for (int i = positions.Length - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                var value = positions[rnd];
                positions[rnd] = positions[i];
                positions[i] = value;
            }
        }

    }
}

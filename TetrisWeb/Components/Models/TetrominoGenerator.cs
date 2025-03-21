﻿using TetrisWeb.Components.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TetrisWeb.Components.Models.Tetrominos;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TetrisWeb.Components.Models
{
    public class TetrominoGenerator
    {
        //gets a style that isn't one of the last ones spawned (given from board)
        public TetrominoStyle Next(params TetrominoStyle[] unusableStyles)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            var style = (TetrominoStyle)rand.Next(1, 8);

            while (unusableStyles.Contains(style))
                style = (TetrominoStyle)rand.Next(1, 8);

            return style;
        }

        //given a style it returns a tetromino for use in the board
        public Tetromino CreateFromStyle(TetrominoStyle style, Grid grid)
        {
            return style switch
            {
                TetrominoStyle.Block => new Block(grid),
                TetrominoStyle.Straight => new Straight(grid),
                TetrominoStyle.TShaped => new TShaped(grid),
                TetrominoStyle.LeftZigZag => new LeftZigZag(grid),
                TetrominoStyle.RightZigZag => new RightZigZag(grid),
                TetrominoStyle.LShaped => new LShaped(grid),
                TetrominoStyle.ReverseLShaped => new ReverseLShaped(grid),
                _ => new Block(grid),
            };
        }
        public Tetromino MakeGarbage(Grid grid)
        {
            return new Garbage(grid);
        }
    }
}

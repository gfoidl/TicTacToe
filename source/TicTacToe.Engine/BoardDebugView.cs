﻿using System;
using System.Diagnostics;

namespace TicTacToe.Engine
{
    [DebuggerNonUserCode]
    internal class BoardDebugView
    {
        private readonly Board _board;
        //---------------------------------------------------------------------
        public BoardDebugView(Board board) => _board = board;
        //---------------------------------------------------------------------
        public string Fields => GetString(_board);
        //---------------------------------------------------------------------
        public static string GetString(Board board) =>
            $" {GetFieldSign(board, 0)} | {GetFieldSign(board, 1)} | {GetFieldSign(board, 2)}\n" +
            "---+---+---\n" +
            $" {GetFieldSign(board, 3)} | {GetFieldSign(board, 4)} | {GetFieldSign(board, 5)}\n" +
            "---+---+---\n" +
            $" {GetFieldSign(board, 6)} | {GetFieldSign(board, 7)} | {GetFieldSign(board, 8)}";
        //---------------------------------------------------------------------
        private static string GetFieldSign(Board board, int index)
            => (board[index]) switch
            {
                FieldState.Empty   => " ",
                FieldState.Machine => "O",
                FieldState.User    => "X",
                _                  => throw new NotSupportedException(),
            };
    }
}

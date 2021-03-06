﻿using NUnit.Framework;

namespace TicTacToe.Engine.Tests.GameTests
{
    [TestFixture]
    public class MakeMove
    {
        [Test]
        public void Legal_move___OK()
        {
            var sut = new Game();

            MoveResult actual = sut.MakeMove(3);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual.MoveIsLegal);
                Assert.IsFalse(actual.GameEnded);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Illegal_move___OK()
        {
            var sut = new Game();
            sut.MakeMove(3);

            MoveResult actual = sut.MakeMove(3);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(actual.MoveIsLegal);
                Assert.IsFalse(actual.GameEnded);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Legal_move_game_done___OK()
        {
            var sut   = new Game();

            for (int i = 0; i < 9; ++i)
            {
                if (i == 3) continue;
                sut.MakeMove(i);
            }

            MoveResult actual = sut.MakeMove(3);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual.MoveIsLegal);
                Assert.IsTrue(actual.GameEnded);
            });
        }
    }
}

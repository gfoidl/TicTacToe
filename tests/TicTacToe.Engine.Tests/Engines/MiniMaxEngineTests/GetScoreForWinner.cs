using NUnit.Framework;
using TicTacToe.Engine.Engines;

namespace TicTacToe.Engine.Tests.Engines.MiniMaxEngineTests
{
    [TestFixture]
    public class GetScoreForWinner
    {
        [Test]
        public void Machine_turn_machine_is_winner_depth_0___10()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.Machine, true, 0);

            Assert.AreEqual(10, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Machine_turn_machine_is_winner_depth_2___8()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.Machine, true, 2);

            Assert.AreEqual(8, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Machine_turn_user_is_winner_depth_0___m10()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.User, true, 0);

            Assert.AreEqual(-10, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Machine_turn_user_is_winner_depth_2___m8()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.User, true, 2);

            Assert.AreEqual(-8, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void User_turn_machine_is_winner_depth_0___m10()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.Machine, false, 0);

            Assert.AreEqual(-10, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void User_turn_machine_is_winner_depth_2___m8()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.Machine, false, 2);

            Assert.AreEqual(-8, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void User_turn_user_is_winner_depth_0___10()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.User, false, 0);

            Assert.AreEqual(10, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void User_turn_user_is_winner_depth_2___8()
        {
            var sut = new AlphaBetaPruningEngine();

            int actual = sut.GetScoreForWinner(Winner.User, false, 2);

            Assert.AreEqual(8, actual);
        }
    }
}

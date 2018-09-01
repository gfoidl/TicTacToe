using System.Diagnostics;
using NUnit.Framework;
using TicTacToe.Engine.Engines;

namespace TicTacToe.Engine.Tests.Engines.MiniMaxEngineTests
{
    [TestFixture]
    public class FindBestMove
    {
        [Test]
        public void User_starts___center_picked()
        {
            //  X |   |
            // ---+---+---
            //    |   |
            // ---+---+---
            //    |   |

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[0]  = FieldState.User;

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(4, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Machine_can_win_start___correct_picked()
        {
            //  O |   |
            // ---+---+---
            //  X | O | X
            // ---+---+---
            //  X |   |

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[0]  = FieldState.Machine;
            board[3]  = FieldState.User;
            board[4]  = FieldState.Machine;
            board[5]  = FieldState.User;
            board[6]  = FieldState.User;

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(8, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Machine_can_win_final___correct_picked()
        {
            //  O | X | O
            // ---+---+---
            //  X | O | X
            // ---+---+---
            //  X |   |

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[0]  = FieldState.Machine;
            board[1]  = FieldState.User;
            board[2]  = FieldState.Machine;
            board[3]  = FieldState.User;
            board[4]  = FieldState.Machine;
            board[5]  = FieldState.User;
            board[6]  = FieldState.User;

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(8, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void User_can_win_start___correct_defense()
        {
#if NET471
            Debug.Listeners.Add(new ConsoleTraceListener());
#endif
            //  O | O | X
            // ---+---+---
            //    | X |
            // ---+---+---
            //    |   |

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[0]  = FieldState.Machine;
            board[4]  = FieldState.User;
            board[1]  = FieldState.Machine;
            board[2]  = FieldState.User;

            TestContext.WriteLine(board);

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(6, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void User_can_win_start_horizontal_mirror___correct_defense()
        {
#if NET471
            Debug.Listeners.Add(new ConsoleTraceListener());
#endif
            //  X | O | O
            // ---+---+---
            //    | X |
            // ---+---+---
            //    |   |

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[0]  = FieldState.User;
            board[1]  = FieldState.Machine;
            board[2]  = FieldState.Machine;
            board[4]  = FieldState.User;

            TestContext.WriteLine(board);

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(8, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void User_can_win_final___correct_defense()
        {
            //  O | O | X
            // ---+---+---
            //    | X | O
            // ---+---+---
            //    | X | X

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[0]  = FieldState.Machine;
            board[1]  = FieldState.Machine;
            board[2]  = FieldState.User;
            board[4]  = FieldState.User;
            board[5]  = FieldState.Machine;
            board[7]  = FieldState.User;
            board[8]  = FieldState.User;

            TestContext.WriteLine(board);

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(6, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Only_two_choices___correct_picked()
        {
            //  O | X | O
            // ---+---+---
            //  X | X | O
            // ---+---+---
            //  X |   |

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[0]  = FieldState.Machine;
            board[1]  = FieldState.User;
            board[2]  = FieldState.Machine;
            board[3]  = FieldState.User;
            board[4]  = FieldState.User;
            board[5]  = FieldState.Machine;
            board[6]  = FieldState.User;

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(8, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Machine_can_win_01___correct_picked()
        {
            //    | X | O
            // ---+---+---
            //  X | O | O
            // ---+---+---
            //  X |   |

            var sut   = new MiniMaxEngine();
            var board = new Board();
            board[1]  = FieldState.User;
            board[2]  = FieldState.Machine;
            board[3]  = FieldState.User;
            board[4]  = FieldState.Machine;
            board[5]  = FieldState.Machine;
            board[6]  = FieldState.User;

            int actual = sut.FindBestMove(board);

            Assert.AreEqual(8, actual);
        }
    }
}

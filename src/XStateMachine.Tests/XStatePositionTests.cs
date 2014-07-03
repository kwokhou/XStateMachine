using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XStateMachine.Core;

namespace XStateMachine.Tests
{
    [TestFixture]
    public class XStatePositionTests
    {
        [Test]
        public void Can_Create_StatePosition_With_StringInt_Combinations()
        {
            var statePosition = new XStatePosition<string, int, string>("new", 10, "submit");
            Assert.IsNotNull(statePosition);
        }

        private static bool IsCorrectStatePosition(string state, int actor, string command, XStatePosition<string, int, string> statePosition)
        {
            if (statePosition.State == state && statePosition.Actor == actor && statePosition.Command == command)
            {
                return true;
            }
            return false;
        }

        [Test]
        public void Can_Read_State_Properties_Initialized_During_Creation()
        {
            var statePosition = new XStatePosition<string, int, string>("new", 10, "submit");
            Assert.IsTrue(IsCorrectStatePosition("new", 10, "submit", statePosition));
        }

        [Test]
        public void Can_Create_StatePosition_With_Decimal_String_Combinations()
        {
            var statePosition = new XStatePosition<decimal, string, decimal>(0, "poweruser", new decimal(200.00));
            Assert.IsNotNull(statePosition);
        }

        [Test]
        public void Can_Compare_Similar_StatePosition()
        {
            var sm1 = new XStatePosition<int, int, int>(1, 2, 3);
            var sm2 = new XStatePosition<int, int, int>(1, 2, 3);

            Assert.AreEqual(sm1, sm2);
        }

        [Test]
        public void Can_Compare_Similar_StatePosition_With_StringIntDecimal()
        {
            var sm1 = new XStatePosition<string, decimal, int>("aaa", 2, 50);
            var sm2 = new XStatePosition<string, decimal, int>("aaa", 2, 50);

            Assert.AreEqual(sm1, sm2);
        }

        [Test]
        public void Can_Differenciate_Not_Equal_StatePosition()
        {
            var sm1 = new XStatePosition<int, int, int>(1, 2, 3);
            var sm2 = new XStatePosition<int, int, int>(1, 2, 4);

            Assert.AreNotEqual(sm1, sm2);
        }

        [Test]
        public void Can_Differenciate_Not_Equal_StatePosition_With_StringIntDecimal()
        {
            var sm1 = new XStatePosition<string, decimal, int>("aaa", 2, 50);
            var sm2 = new XStatePosition<string, decimal, int>("aab", 2, 50);

            Assert.AreNotEqual(sm1, sm2);
        }
    }
}

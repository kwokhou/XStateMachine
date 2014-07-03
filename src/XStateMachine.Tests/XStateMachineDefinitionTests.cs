using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XStateMachine.Core;

namespace XStateMachine.Tests
{
    [TestFixture]
    public class XStateMachineDefinitionTests
    {
        [Test]
        public void Can_Create_StateDefinitions_with_Integers()
        {
            var definition = new XStateMachineDefinition<int, int, int>();
            definition.Add(new XStatePosition<int, int, int>(0, 0, 0), new XStateAction<int>(1, nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));

            Assert.IsFalse(definition.IsEmpty());
        }

        [Test]
        public void StateDefinitions_with_Empty_Workflow()
        {
            var definition = new XStateMachineDefinition<string, int, int>();

            Assert.IsTrue(definition.IsEmpty());
        }

        [Test]
        public void Can_Create_StateDefinitions_with_Strings()
        {
            var definition = new XStateMachineDefinition<string, int, int>();
            definition.Add(new XStatePosition<string, int, int>(string.Empty, 0, 0), new XStateAction<string>("", nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));

            Assert.IsFalse(definition.IsEmpty());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_StateDefinitions_with_null_State_will_throws_exception()
        {
            var definition = new XStateMachineDefinition<string, int, int>();
            definition.Add(new XStatePosition<string, int, int>(null, 0, 0), new XStateAction<string>("", nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));

            Assert.IsFalse(definition.IsEmpty());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_StateDefinitions_with_null_Actor_will_throws_exception()
        {
            var definition = new XStateMachineDefinition<string, string, string>();
            definition.Add(new XStatePosition<string, string, string>("", null, ""), new XStateAction<string>("", nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));
            Assert.IsFalse(definition.IsEmpty());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_StateDefinitions_with_null_Command_will_throws_exception()
        {
            var definition = new XStateMachineDefinition<string, string, string>();
            definition.Add(new XStatePosition<string, string, string>("", "", null), new XStateAction<string>("", nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));
            Assert.IsFalse(definition.IsEmpty());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_StateDefinitions_with_null_NextState_will_throws_exception()
        {
            var definition = new XStateMachineDefinition<string, string, string>();
            definition.Add(new XStatePosition<string, string, string>("", "", ""), new XStateAction<string>(null, nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));
            Assert.IsFalse(definition.IsEmpty());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Cannot_Create_StateDefinitions_with_similar_StatePosition()
        {
            var definition = new XStateMachineDefinition<string, int, int>();
            definition.Add(new XStatePosition<string, int, int>(string.Empty, 1, 0), new XStateAction<string>("", nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));
            definition.Add(new XStatePosition<string, int, int>(string.Empty, 1, 0), new XStateAction<string>("", nextState =>
            {
                System.Diagnostics.Debug.WriteLine("I am now at state:" + nextState);
            }));
            Assert.IsFalse(definition.IsEmpty());
        }

    }
}

using NUnit.Framework;
using System;
using XStateMachine.Core;

namespace XStateMachine.Tests
{
    [TestFixture()]
    public class XStateMachineTests
    {
        [Test()]
        public void TestCase()
        {
        }

        [Test]
        public void CanSayHello()
        {
            MyClass myClass = new MyClass();
            var actual = myClass.SayHello();
            var expected = "Hello World";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_create_StateMachine_without_Workflow_Definitions()
        {
            XStatePosition<int, int, int> position = new XStatePosition<int, int, int>(1, 2, 3);
            var sm = new XStateMachine<int, int, int>(null, position);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_create_StateMachine_with_Empty_Workflow_Definitions()
        {
            XStateMachineDefinition<int, int, int> definitions = new XStateMachineDefinition<int, int, int>();
            XStatePosition<int, int, int> position = new XStatePosition<int, int, int>(1, 2, 3);
            var sm = new XStateMachine<int, int, int>(definitions, position);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_create_StateMachine_with_No_InitialState()
        {
            XStateMachineDefinition<int, int, int> definitions = MakeSampleStateMachineDefinitions();
            XStatePosition<int, int, int> position = new XStatePosition<int, int, int>(1, 2, 3);
            var sm = new XStateMachine<int, int, int>(definitions, null);
        }

        [Test]
        public void Create_StateMachine_with_Workflow_Definitions()
        {
            XStateMachineDefinition<int, int, int> definitions = MakeSampleStateMachineDefinitions();
            var initialPosition = new XStatePosition<int, int, int>(1, 2, 3);
            var sm = new XStateMachine<int, int, int>(definitions, initialPosition);
            Assert.IsNotNull(sm);
        }

        [Test]
        public void Create_StateMachine_with_InitialPosition()
        {
            XStateMachineDefinition<int, int, int> definitions = MakeSampleStateMachineDefinitions();
            var initialPosition = new XStatePosition<int, int, int>(1, 2, 3);
            var sm = new XStateMachine<int, int, int>(definitions, initialPosition);
            Assert.IsNotNull(sm.CurrentStatePosition);
        }

        [Test]
        public void Can_MoveNext_in_StateMachine()
        {
            XStateMachineDefinition<int, int, int> definitions = MakeSampleStateMachineDefinitions();
            var initialPosition = new XStatePosition<int, int, int>(2, 2, 3);
            var sm = new XStateMachine<int, int, int>(definitions, initialPosition);
            sm.MoveNext(2, 3);
            Assert.AreEqual(20, sm.CurrentStatePosition.State); 
        }

        [Test]
        public void MoveNext_in_StateMachine_Execute_Action()
        {
            XStateMachineDefinition<int, int, int> definitions = MakeSampleStateMachineDefinitions();
            var initialPosition = new XStatePosition<int, int, int>(2, 2, 3);
            var sm = new XStateMachine<int, int, int>(definitions, initialPosition);
            sm.MoveNext(2, 3);
            Assert.AreEqual(20, sm.CurrentStatePosition.State);
        }


        [Test]
        public void StateMachine_Can_Run_Approval_WorkFlow()
        {
            var workflow = MakeSampleApprovalStateMachineDefinitions();
            var position = new XStatePosition<string, string, string>("draft", "requester", "submit-for-approval");

            var sm = new XStateMachine<string, string, string>(workflow, position);
            sm.MoveNext("requester", "submit-for-approval");

            Assert.AreEqual("pending-for-approval", sm.CurrentStatePosition.State);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void StateMachine_Cannot_Run_Undefined_WorkFlow()
        {
            var workflow = MakeSampleApprovalStateMachineDefinitions();
            var position = new XStatePosition<string, string, string>("draft", "requester", "submit-for-approval");

            var sm = new XStateMachine<string, string, string>(workflow, position);
            sm.MoveNext("approver", "submit-for-approval");
        }

        private XStateMachineDefinition<int, int, int> MakeSampleStateMachineDefinitions()
        {
            XStateMachineDefinition<int, int, int> definitions = new XStateMachineDefinition<int, int, int>();

            definitions.Add(new XStatePosition<int, int, int>(1, 2, 3), new XStateAction<int>(10, x => { }));

            definitions.Add(new XStatePosition<int, int, int>(2, 2, 3), new XStateAction<int>(20, x =>
            {
                var isExecuted = true;
                Assert.IsTrue(isExecuted);
            }));

            return definitions;
        }

        private XStateMachineDefinition<string, string, string> MakeSampleApprovalStateMachineDefinitions()
        {
            XStateMachineDefinition<string, string, string> definitions = new XStateMachineDefinition<string, string, string>();

            definitions.Add(new XStatePosition<string, string, string>("pending-for-approval", "approver", "reject-request"), new XStateAction<string>("rejected", x => { }));

            definitions.Add(new XStatePosition<string, string, string>("pending-for-approval", "requester", "cancel-request"), new XStateAction<string>("draft", x => { }));

            definitions.Add(new XStatePosition<string, string, string>("draft", "requester", "submit-for-approval"), new XStateAction<string>("pending-for-approval", x =>
            {
                var isExecuted = true;
                Assert.IsTrue(isExecuted);
            }));

            return definitions;
        }

    }
}

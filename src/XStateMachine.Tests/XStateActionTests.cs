using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XStateMachine.Core;

namespace XStateMachine.Tests
{
    [TestFixture]
    public class XStateActionTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cannot_create_StateAction_without_NextState()
        {
            var sa = new XStateAction<string>(null, x => { });
            Assert.AreEqual(sa, null);
        }

        [Test]
        public void Can_create_StateAction_without_Action()
        {
            var sa = new XStateAction<string>("", null);
            Assert.IsNotNull(sa);
        }

        [Test]
        public void Can_create_StateAction_with_State()
        {
            var sa = new XStateAction<string>("flare", null);
            Assert.AreEqual("flare", sa.State);
        }

        [Test]
        public void Can_Execute_Action_Within_StateAction()
        {
            Action<string> act = x =>
            {
                Assert.Pass("state is " + x);
            };

            var stateAction = new XStateAction<string>("fly", act);
            stateAction.Execute();
        }
    }
}

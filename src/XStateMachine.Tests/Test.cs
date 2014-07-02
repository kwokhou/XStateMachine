using NUnit.Framework;
using System;
using XStateMachine.Core;

namespace XStateMachine.Tests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestCase ()
		{
		}

		[Test]
		public void CanSayHello(){
			MyClass myClass = new MyClass ();
			var actual = myClass.SayHello ();
			var expected = "Hello World";

			Assert.AreEqual (expected, actual);
		}

		[Test]
		public void Can_Create_StateDefinitions_with_Integers(){
			var definition = new XStateMachineDefinition<int, int, int> ();
			definition.Add (new XStatePosition<int, int, int> (0, 0, 0), new XStateAction<int> (1, nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));

			Assert.IsFalse (definition.IsEmpty ());
		}

		[Test]
		public void StateDefinitions_with_Empty_Workflow(){
			var definition = new XStateMachineDefinition<string, int, int> ();

			Assert.IsTrue (definition.IsEmpty ());
		}

		[Test]
		public void Can_Create_StateDefinitions_with_Strings(){
			var definition = new XStateMachineDefinition<string, int, int> ();
			definition.Add (new XStatePosition<string, int, int> (string.Empty, 0, 0), new XStateAction<string> ("", nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));

			Assert.IsFalse (definition.IsEmpty ());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_StateDefinitions_with_null_State_will_throws_exception(){
			var definition = new XStateMachineDefinition<string, int, int> ();
			definition.Add (new XStatePosition<string, int, int> (null, 0, 0), new XStateAction<string> ("", nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));

			Assert.IsFalse (definition.IsEmpty ());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_StateDefinitions_with_null_Actor_will_throws_exception(){
			var definition = new XStateMachineDefinition<string, string, string> ();
			definition.Add (new XStatePosition<string, string, string> ("", null, ""), new XStateAction<string> ("", nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));
			Assert.IsFalse (definition.IsEmpty ());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_StateDefinitions_with_null_Command_will_throws_exception(){
			var definition = new XStateMachineDefinition<string, string, string> ();
			definition.Add (new XStatePosition<string, string, string> ("", "", null), new XStateAction<string> ("", nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));
			Assert.IsFalse (definition.IsEmpty ());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Create_StateDefinitions_with_null_NextState_will_throws_exception(){
			var definition = new XStateMachineDefinition<string, string, string> ();
			definition.Add (new XStatePosition<string, string, string> ("", "", ""), new XStateAction<string> (null, nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));
			Assert.IsFalse (definition.IsEmpty ());
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Cannot_Create_StateDefinitions_with_similar_StatePosition(){
			var definition = new XStateMachineDefinition<string, int, int> ();
			definition.Add (new XStatePosition<string, int, int> (string.Empty, 1, 0), new XStateAction<string> ("", nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));
			definition.Add (new XStatePosition<string, int, int> (string.Empty, 1, 0), new XStateAction<string> ("", nextState => {
				System.Diagnostics.Debug.WriteLine ("I am now at state:" + nextState);
			}));
			Assert.IsFalse (definition.IsEmpty ());
		}

		[Test]
		public void Compare_Similar_StatePosition_Will_Equal(){
			var sm1 = new XStatePosition<int, int, int> (1, 2, 3);
			var sm2 = new XStatePosition<int, int, int> (1, 2, 3);

			Assert.AreEqual (sm1, sm2);
		}

		[Test]
		public void Compare_Diff_StatePosition_WillNot_Equal(){
			var sm1 = new XStatePosition<int, int, int> (1, 2, 3);
			var sm2 = new XStatePosition<int, int, int> (1, 2, 4);

			Assert.AreNotEqual (sm1, sm2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Cannot_create_StateAction_without_NextState(){
			var sa = new XStateAction<string> (null, x => {});
			Assert.AreEqual (sa, null);
		}


		[Test]
		public void Can_create_StateAction_without_Action(){
			var sa = new XStateAction<string> ("", null);
			Assert.IsNotNull (sa);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Cannot_create_StateMachine_without_Workflow_Definitions()
		{
			XStatePosition<int,int,int> position = new XStatePosition<int, int, int> (1, 2, 3);
			var sm = new XStateMachine<int,int,int> (null, position);
			Assert.IsNotNull (sm);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Cannot_create_StateMachine_with_Empty_Workflow_Definitions()
		{
			XStateMachineDefinition<int,int,int> definitions = new XStateMachineDefinition<int, int, int> ();
			XStatePosition<int,int,int> position = new XStatePosition<int, int, int> (1, 2, 3);
			//			XStateAction<int> stateAction = new XStateAction<int> (2, x => {
			//
			//			});
			var sm = new XStateMachine<int,int,int> (definitions, position);
			Assert.IsNotNull (sm);
		}

		[Test]
		public void Create_StateMachine_with_Workflow_Definitions()
		{
			XStateMachineDefinition<int,int,int> definitions = MakeSampleStateMachineDefinitions ();
			var initialPosition = new XStatePosition<int, int, int> (1, 2, 3);
			var sm = new XStateMachine<int,int,int> (definitions, initialPosition);
			Assert.IsNotNull (sm);
		}

		[Test]
		public void Can_MoveNext_in_StateMachine()
		{
			XStateMachineDefinition<int,int,int> definitions = MakeSampleStateMachineDefinitions ();
			var initialPosition = new XStatePosition<int, int, int> (1, 2, 3);
			var sm = new XStateMachine<int,int,int> (definitions, initialPosition);

			Assert.IsNotNull (sm);

			sm.MoveNext (2, 3);

		}

		private XStateMachineDefinition<int,int,int> MakeSampleStateMachineDefinitions()
		{
			XStateMachineDefinition<int,int,int> definitions = new XStateMachineDefinition<int, int, int> ();

			definitions.Add (new XStatePosition<int, int, int> (1, 2, 3), new XStateAction<int> (10, x => {
				System.Diagnostics.Debug.WriteLine("this is state 10 action");
			}));

			definitions.Add (new XStatePosition<int, int, int> (2, 2, 3), new XStateAction<int> (20, x => {
				System.Diagnostics.Debug.WriteLine("this is state 10 action");
			}));

			return definitions;
		}
	}
}

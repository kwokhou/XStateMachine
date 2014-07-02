using System;
using System.Collections.Generic;
using System.Linq;

namespace XStateMachine.Core
{
	public class XStateAction<TState> {
		private readonly TState _state;
		private Action<TState> _fn;

		//public TState State { get { return _state; } }

		public XStateAction (TState nextState, Action<TState> fn) {
			if (nextState == null)
				throw new ArgumentNullException ("nextState");

			_state = nextState;
			_fn = fn;
		}

		public void Execute() {
			_fn (_state);
		}
	}

	public class XStatePosition<TState, TActor, TCommand> {
		private readonly TState _state;
		private readonly TActor _actor;
		private readonly TCommand _command;

		public TState State { get { return _state; } }
		public TActor Actor { get { return _actor; } }
		public TCommand Command { get { return _command; } }

		public XStatePosition (TState state, TActor actor, TCommand command)
		{
			if (state == null)
				throw new ArgumentNullException ("state");
			if (actor == null)
				throw new ArgumentNullException ("actor");
			if (command == null)
				throw new ArgumentNullException ("command");

			_state = state;
			_actor = actor;
			_command = command;
		}

		public override int GetHashCode(){
			unchecked {
				int hash = 17;
				hash = hash * 31 + _state.GetHashCode ();
				hash = hash * 31 + _actor.GetHashCode ();
				hash = hash * 31 + _command.GetHashCode ();
				return hash;
			}
		}

		public override bool Equals(object obj) {
			var other = obj as XStatePosition<TState, TActor, TCommand>;
			return
				other != null &&
			EqualityComparer<TState>.Default.Equals (_state, other._state) &&
			EqualityComparer<TActor>.Default.Equals (_actor, other._actor) &&
			EqualityComparer<TCommand>.Default.Equals (_command, other._command);
		}
	}

	public class XStateMachineDefinition<TState, TActor, TCommand>
	{
		public Dictionary<XStatePosition<TState, TActor, TCommand>, XStateAction<TState>> Workflows {get; private set;}

		public XStateMachineDefinition ()
		{
			this.Workflows = new Dictionary<XStatePosition<TState, TActor, TCommand>, XStateAction<TState>> ();
		}

		public bool IsEmpty() {
			return this.Workflows == null || !this.Workflows.Any ();
		}

		public void Add(XStatePosition<TState, TActor, TCommand> position, XStateAction<TState> action) {
			if (this.Workflows.ContainsKey (position))
				throw new ArgumentException("exDuplicateStatePositionDefinition", "position");
			this.Workflows.Add (position, action);
		}
	}

	public class XStateMachine<TState, TActor, TCommand>
	{
		private XStateMachineDefinition<TState, TActor, TCommand> _definitions;
		public XStatePosition<TState, TActor, TCommand> CurrentStatePosition { get; private set; }

		public XStateMachine (XStateMachineDefinition<TState, TActor, TCommand> definitions, XStatePosition<TState, TActor, TCommand> currentStatePosition)
		{
			if (definitions == null || definitions.IsEmpty()) {
				throw new ArgumentNullException("workflows", "exMissingWorkflowDefinitions");
			}

			if (currentStatePosition == null) {
				throw new ArgumentNullException ("currentStatePosition", "exMissingCurrentStatePosition");
			}

			_definitions = definitions;
		}

		private XStateAction<TState> FindNext(TState state, TActor actor, TCommand command) {
			var position = new XStatePosition<TState, TActor, TCommand>(state, actor, command);
			XStateAction<TState> stateAction;
			if (!_definitions.Workflows.TryGetValue(position, out stateAction))
				throw new ArgumentException(string.Format("exInvalidStateActorCommand:{0}>>{1}>>{2}", state, actor, command));
			return stateAction;
		}

		public void MoveNext(TActor actor, TCommand command) {
			var nextStateAction = FindNext (CurrentStatePosition.State, actor, command);
			nextStateAction.Execute ();
		}
	}
}

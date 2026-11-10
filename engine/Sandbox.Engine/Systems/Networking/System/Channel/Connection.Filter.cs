using Sandbox.Network;

namespace Sandbox;

public abstract partial class Connection
{
	public struct Filter
	{
		public enum FilterType
		{
			/// <summary>
			/// Only include the connections in the filter when sending a message.
			/// </summary>
			Include,

			/// <summary>
			/// Exclude the connections in the filter when sending a message.
			/// </summary>
			Exclude
		}

		private IEnumerable<Connection> Connections { get; set; }
		private Predicate<Connection> Predicate { get; set; }
		private FilterType Type { get; set; }

		/// <summary>
		/// The one connection an include filter names, when it names only one: its recipients are found
		/// without testing every connection against it.
		/// </summary>
		internal Connection Single { get; private set; }

		/// <summary>
		/// Only this connection.
		/// </summary>
		internal Filter( Connection single )
		{
			Single = single;
			Predicate = c => c == single;
			Type = FilterType.Include;
		}

		public Filter( FilterType type, Predicate<Connection> predicate )
		{
			Predicate = predicate;
			Type = type;
		}

		public Filter( FilterType type, IEnumerable<Connection> connections )
		{
			Connections = connections;
			Type = type;
		}

		/// <summary>
		/// Is the specified <see cref="Connection"/> a valid recipient?
		/// </summary>
		public bool IsRecipient( Connection connection )
		{
			if ( Type == FilterType.Exclude )
				return !Predicate?.Invoke( connection ) ?? !Connections.Contains( connection );

			return Predicate?.Invoke( connection ) ?? Connections.Contains( connection );
		}
	}
}

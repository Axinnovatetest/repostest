using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Hubs
{
	public class ConnectionMapping<T>
	{
		private readonly Dictionary<T, HashSet<string>> _connections =
			new Dictionary<T, HashSet<string>>();

		public int Count
		{
			get
			{
				return _connections.Count;
			}
		}

		public void Add(T key, string connectionId)
		{
			lock(_connections)
			{
				HashSet<string> connections;
				if(!_connections.TryGetValue(key, out connections))
				{
					connections = new HashSet<string>();
					_connections.Add(key, connections);
				}

				lock(connections)
				{
					connections.Add(connectionId);
				}
			}
		}

		public IEnumerable<string> GetConnections(T key)
		{
			HashSet<string> connections;
			if(_connections.TryGetValue(key, out connections))
			{
				return connections;
			}

			return Enumerable.Empty<string>();
		}

		public void Remove(T key, string connectionId)
		{
			lock(_connections)
			{
				HashSet<string> connections;
				if(!_connections.TryGetValue(key, out connections))
				{
					return;
				}

				lock(connections)
				{
					connections.Remove(connectionId);

					if(connections.Count == 0)
					{
						_connections.Remove(key);
					}
				}
			}
		}

		public IEnumerable<T> GetKeys()
		{
			return _connections.Select(e => e.Key).ToList();
		}

		public T GetKey(string connectionId)
		{
			var result = _connections.FirstOrDefault(e => e.Value.Contains(connectionId)).Key;

			return result;
		}
	}
}

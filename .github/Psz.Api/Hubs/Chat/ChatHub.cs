using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api.Hubs
{
	public class ChatHub: Hub
	{
		//private readonly static ConnectionMapping<ConnectedUser> _usersConnections = new ConnectionMapping<ConnectedUser>();

		//private readonly static ConnectionMapping<Visitor> _visitorsConnections = new ConnectionMapping<Visitor>();

		public override Task OnConnectedAsync()
		{
			try
			{
				//string userId = getUserId(Context);
				//var remoteIpAddress = Context.GetHttpContext()?.Connection.RemoteIpAddress;

				//if (!string.IsNullOrEmpty(userId)) // Is User
				//{
				//    string id = getId(Context);
				//    string userRole = getRole(Context);
				//    string name = getName(Context);

				//    _usersConnections.Add(new ConnectedUser()
				//    {
				//        Id = id,
				//        UserId = userId,
				//        Type = (userRole == "AppUser"
				//            ? Core.Enums._MessageSenderTypes.CoreUser
				//            : Core.Enums._MessageSenderTypes.ErpUser),
				//        Name = name
				//    }, Context.ConnectionId);
				//}
				//else // Is Visitor
				//{
				//    var newVisitor = new Core.Models.Visitor()
				//    {
				//        Ip = remoteIpAddress != null ? remoteIpAddress.ToString() : "",
				//        CreationTime = DateTime.Now,
				//        Name = "",
				//        Number = -1,
				//        Email = "",
				//        Id = -1,
				//        DeviceId = "",
				//        DeviceType = null
				//    };

				//    var response = Core.Handlers.Visitor.Add(newVisitor);

				//    newVisitor.Id = response.Item1;
				//    newVisitor.Number = response.Item1;

				//    _visitorsConnections.Add(new Visitor()
				//    {
				//        Ip = newVisitor.Ip,
				//        Id = response.Item1,
				//        Number = response.Item2
				//    }, Context.ConnectionId);

				//    Clients.All.SendAsync("VisitorConnected",
				//        newVisitor.Id,
				//        newVisitor.Number,
				//        newVisitor.Name,
				//        newVisitor.Email);
				//}

				return base.OnConnectedAsync();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return base.OnConnectedAsync();
			}
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			try
			{
				//string userId = getUserId(Context);

				//if (!string.IsNullOrEmpty(userId)) // Is User
				//{
				//    var userConnection = _usersConnections.GetKeys().FirstOrDefault(e => e.UserId == userId);
				//    if (userConnection != null)
				//    {
				//        _usersConnections.Remove(userConnection, Context.ConnectionId);
				//    }
				//}
				//else // Is Visitor
				//{
				//    var visitorConnection = _visitorsConnections.GetKey(Context.ConnectionId);
				//    if (visitorConnection != null)
				//    {
				//        _visitorsConnections.Remove(visitorConnection, Context.ConnectionId);

				//        Clients.All.SendAsync("VisitorDisconnected", visitorConnection.Id);
				//    }
				//}

				return base.OnDisconnectedAsync(exception);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return base.OnDisconnectedAsync(exception);
			}
		}

		public async Task Send(string message)
		{
			try
			{
				//var visitor = _visitorsConnections.GetKey(Context.ConnectionId);
				//if (visitor == null)
				//{
				//    throw new Exception("visitor not found");
				//}

				//Core.Handlers.Message.Message.NewToAllErpUsers(new Core.Models.Message.Message()
				//{
				//    Text = message,

				//    FromUserType = Core.Enums._MessageSenderTypes.Visitor,
				//    FromUserId = visitor.Id.ToString(),

				//    ToUserType = Core.Enums._MessageSenderTypes.ErpUser,
				//});

				//await Clients.Caller.SendAsync("ReceiveMessage", "visitor", visitor.Id, message);

				//foreach (var connectedErpUser in _usersConnections.GetKeys()
				//    .Where(e => e.Type == Core.Enums._MessageSenderTypes.ErpUser))
				//{
				//    foreach (var connectionId in _usersConnections.GetConnections(connectedErpUser))
				//    {
				//        var client = Clients.Client(connectionId);
				//        if (client != null)
				//        {
				//            await client.SendAsync("ReceiveMessage", "visitor", visitor.Id, message);
				//        }
				//    }
				//}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public async Task SetVisitorData(string name, string email)
		{
			try
			{
				//var visitor = _visitorsConnections.GetKey(Context.ConnectionId);
				//if (visitor == null)
				//{
				//    throw new Exception("visitor not found");
				//}

				//Core.Handlers.Visitor.SetData(visitor.Id, name, email);

				//visitor.Name = !string.IsNullOrEmpty(name) ? name.Trim() : "";
				//visitor.Email = !string.IsNullOrEmpty(email) ? email.Trim() : "";

				//foreach (var connectedErpUser in _usersConnections.GetKeys()
				//    .Where(e => e.Type == Core.Enums._MessageSenderTypes.ErpUser))
				//{
				//    foreach (var connectionId in _usersConnections.GetConnections(connectedErpUser))
				//    {
				//        var client = Clients.Client(connectionId);
				//        if (client != null)
				//        {
				//            await client.SendAsync("visitorDataUpdate", visitor.Id, visitor.Name, visitor.Email);
				//        }
				//    }
				//}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//[Authorize]
		public async Task SendAdminMessage(string message, string receiverId, string receiverType)
		{
			try
			{
				//if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
				//{
				//    return;
				//}

				//string senderType = getRole(Context);
				//if (string.IsNullOrEmpty(senderType) || senderType != "ErpUser")
				//{
				//    throw new Exception("access denied");
				//}

				//if (string.IsNullOrEmpty(receiverId) || string.IsNullOrEmpty(receiverType))
				//{
				//    throw new Exception("receiver not found");
				//}

				//string senderId = getId(Context);

				//if (string.IsNullOrEmpty(senderId))
				//{
				//    throw new Exception("sender not found");
				//}

				//var receiverCoreType = (receiverType.ToLower() == "visitor")
				//    ? Core.Enums._MessageSenderTypes.Visitor
				//    : Core.Enums._MessageSenderTypes.CoreUser;

				//// > get Receiver Connections Ids
				//var receiverConnectionsIds = new List<string>();
				//if (receiverCoreType == Core.Enums._MessageSenderTypes.CoreUser)
				//{
				//    var connection = _usersConnections.GetKeys().FirstOrDefault(e => e.Type == Core.Enums._MessageSenderTypes.CoreUser
				//        && e.Id == receiverId);
				//    if (connection != null)
				//    {
				//        receiverConnectionsIds.AddRange(_usersConnections.GetConnections(connection));
				//    }
				//}
				//else if (receiverCoreType == Core.Enums._MessageSenderTypes.Visitor)
				//{
				//    var connection = _visitorsConnections.GetKeys().FirstOrDefault(e => e.Id.ToString() == receiverId);
				//    if (connection != null)
				//    {
				//        receiverConnectionsIds.AddRange(_visitorsConnections.GetConnections(connection));
				//    }
				//}

				//// > Save
				//Core.Handlers.Message.Message.New(new Core.Models.Message.Message()
				//{
				//    Text = message,

				//    FromUserType = Core.Enums._MessageSenderTypes.ErpUser,
				//    FromUserId = senderId,

				//    ToUserType = receiverCoreType,
				//    ToUserId = receiverId
				//});

				//// > Call back to Sender
				//await Clients.Caller.SendAsync("ReceiveMessage", "admin", senderId, message);

				//// > Send to Receiver
				//await Clients.Clients(receiverConnectionsIds).SendAsync("ReceiveMessage", "admin", senderId, message);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//[Authorize]
		public async Task RequestConversationMessages(int visitorId)
		{
			try
			{
				//var messages = new List<Core.Models.Message.Message>();

				//var visitorConnection = _visitorsConnections.GetKeys().FirstOrDefault(e => e.Id == visitorId);
				//if (visitorConnection != null)
				//{
				//    messages = Core.Handlers.Message.Message.Get(Core.Enums._MessageSenderTypes.Visitor, visitorId.ToString());
				//}

				//foreach (var message in messages)
				//{
				//    var sender = ((message.FromUserType == Core.Enums._MessageSenderTypes.ErpUser) ? "admin" : "visitor");

				//    await Clients.Caller.SendAsync("ReceiveMessage", sender, message.FromUserId, message.Text);
				//}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//[Authorize]
		public async Task RequestVisitors()
		{
			try
			{
				//foreach (var visitor in _visitorsConnections.GetKeys())
				//{
				//    await Clients.Caller.SendAsync("VisitorConnected",
				//        visitor.Id,
				//        visitor.Number,
				//        visitor.Name,
				//        visitor.Email);
				//}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		#region > Classes
		//public class ConnectedUser
		//{
		//    public string UserId { get; set; }
		//    public Core.Enums._MessageSenderTypes Type { get; set; }
		//    public string Id { get; set; }
		//    public string Name { get; set; }
		//}

		//public class Visitor
		//{
		//    public int Id { get; set; }
		//    public int Number { get; set; }
		//    public string Name { get; set; }
		//    public string Email { get; set; }
		//    public string Ip { get; set; }
		//}
		#endregion

		#region > Helpers
		private string getUserId(HubCallerContext context)
		{
			var userId = context.User?.Claims.FirstOrDefault(e => e.Type == "MembershipId")?.Value;
			return userId;
		}

		private string getId(HubCallerContext context)
		{
			var id = context.User?.Claims.FirstOrDefault(e => e.Type == "Id")?.Value;
			return id;
		}

		private string getName(HubCallerContext context)
		{
			var name = context.User?.Claims.FirstOrDefault(e => e.Type == "Name")?.Value;
			return name;
		}

		private string getRole(HubCallerContext context)
		{
			var role = context.User?.Claims.FirstOrDefault(e => e.Type == "Role")?.Value;
			return role;
		}
		#endregion
	}
}

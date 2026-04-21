using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Psz.Api.Hubs
{
	public class OrderHub: Hub
	{
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
	}
}

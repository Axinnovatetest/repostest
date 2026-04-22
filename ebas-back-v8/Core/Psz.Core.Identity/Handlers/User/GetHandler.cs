using Infrastructure.Services.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Psz.Core.Identity.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Psz.Core.Identity.Handlers.User
{
	public class GetHandler: SharedKernel.Interfaces.IHandle<string, Models.UserModel>
	{
		private int _id { get; set; }
		private IHeaderDictionary _headerDictionary { get; set; }
		private bool _includeAccess = false;
		private UseModes _userMode;

		private enum UseModes { byId, byHttpHeaderDictionary }

		#region > Security Key
		private static SymmetricSecurityKey _securityKey = null;
		public static void SetSecret(string secret)
		{
			if(string.IsNullOrEmpty(secret))
			{
				throw new Exception("secret must have a value");
			}

			var base64String = Convert.FromBase64String(secret);

			_securityKey = new SymmetricSecurityKey(base64String);
		}
		#endregion

		public GetHandler(int id, bool includeAccess)
		{
			this._id = id;
			this._includeAccess = includeAccess;
			this._userMode = UseModes.byId;
		}
		public GetHandler(IHeaderDictionary headerDictionary, bool includeAccess)
		{
			this._headerDictionary = headerDictionary;
			this._includeAccess = includeAccess;
			this._userMode = UseModes.byHttpHeaderDictionary;
		}

		public Models.UserModel Handle()
		{
			try
			{
				switch(this._userMode)
				{
					default:
					case UseModes.byId:
						return getUserById(this._id);
					case UseModes.byHttpHeaderDictionary:
						return getUserByHttpHeaders(this._headerDictionary);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public UserModel Validate()
		{
			throw new NotImplementedException();
		}

		public Models.UserModel getUserById(int id)
		{
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(id);
			if(userEntity == null)
			{
				return null;
			}

			var accessProfile = this._includeAccess
				? AccessProfile.GetUserProfiles(id)
				: null;

			return new Models.UserModel(userEntity, accessProfile);
		}
		private Models.UserModel getUserByHttpHeaders(IHeaderDictionary headers)
		{
			var token = getTokenFromRequesHeader(headers);
			var user = getUserByToken(token);

			return user;
		}
		private Models.UserModel getUserByToken(string token)
		{
			try
			{
				if(string.IsNullOrWhiteSpace(token))
				{
					Logger.Log("jwtToken == null");
					return new UserModel
					{

					};
				}

				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
				if(jwtToken == null)
				{
					Logger.Log("jwtToken == null");
					return null;
				}

				var parameters = new TokenValidationParameters()
				{
					RequireExpirationTime = false,
					ValidateIssuer = false,
					ValidateAudience = false,
					IssuerSigningKey = _securityKey,
					ValidateLifetime = false,
				};

				SecurityToken securityToken;
				var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

				if(principal == null || principal.Claims == null)
				{
					Logger.Log("principal == null || principal.Claims == null");
					return null;
				}

				var idStr = principal.Claims.FirstOrDefault(e => e.Type == "Id");
				if(idStr == null || string.IsNullOrEmpty(idStr.Value))
				{
					Logger.Log("idStr == null || string.IsNullOrEmpty(idStr.Value)");
					return null;
				}

				var id = -1;
				if(!int.TryParse(idStr.Value, out id))
				{
					Logger.Log("!int.TryParse(idStr.Value, out id)");
					return null;
				}

				return getUserById(id);
			} catch(Exception e)
			{
				Logger.Log(e);
				throw;
			}
		}
		private string getTokenFromRequesHeader(IHeaderDictionary headers)
		{
			StringValues value;
			var got = headers.TryGetValue("Authorization", out value);
			if(!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value) && value.ToString().Length > 7)
				value = value.ToString().Substring(7); //remove Bearer
			return value;
		}
	}
}

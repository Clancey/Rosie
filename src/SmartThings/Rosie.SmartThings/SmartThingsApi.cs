using System;
using SimpleAuth;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;

namespace Rosie.SmartThings
{

	public class SmartThingsAuthenticator : BasicAuthAuthenticator
	{
		public SmartThingsAuthenticator (HttpClient client, string loginUrl) : base (client, loginUrl)
		{

		}
		public override async Task<bool> CheckCredentails (string username, string password)
		{
			try {
				if (string.IsNullOrWhiteSpace (username))
					throw new Exception ("Invalid Username");
				if (string.IsNullOrWhiteSpace (password))
					throw new Exception ("Invalid Password");

				Response = await SmartThingsApi.GetTokenFromCredentials (client, loginUrl, username, password);
				FoundAuthCode (Response.AccessToken);
				Username = username;
				Password = password;
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}
			return false;
		}
		public OauthResponse Response { get; private set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class SmartThingsAccount : BasicAuthAccount
	{
		public string Token { get; set; }

		public string TokenType { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public long ExpiresIn { get; set; }
		//UTC Datetime created
		public DateTime Created { get; set; }
		public override bool IsValid ()
		{
			if (string.IsNullOrWhiteSpace (Token))
				return false;
			// This allows you to specify -1 for never expires
			if (ExpiresIn < 0)
				return true;
			if (string.IsNullOrWhiteSpace (Key))
				return false;
			var expireTime = Created.AddSeconds (ExpiresIn);
			return expireTime > DateTime.UtcNow;
		}

	}

	public partial class SmartThingsApi : BasicAuthApi
	{
		public SmartThingsApi (string identifier) : base (identifier, "https://auth-global.api.smartthings.com/oauth/token")
		{
			Client.DefaultRequestHeaders.Add ("X-ST-Client-DeviceModel", "iPhone");
			Client.DefaultRequestHeaders.Add ("X-ST-Api-Version", "2.6");
			Client.DefaultRequestHeaders.Add ("X-ST-Client-AppVersion", "2.1.4");
			Client.DefaultRequestHeaders.Add ("Accept", "application / json");
			Client.DefaultRequestHeaders.Add ("User-Agent", "SmartThings / 2.1.4 (iPhone; iOS 9.3.3; Scale / 2.00)");
			Client.DefaultRequestHeaders.Add ("X-ST-Client-OS", "iOS 9.3.3");
		}

		#region Api Configuration
		public SmartThingsAccount CurrentSmartThingsAccount => CurrentAccount as SmartThingsAccount;
		protected override SimpleAuth.BasicAuthAuthenticator CreateAuthenticator ()
		{
			return new SmartThingsAuthenticator (Client, LoginUrl);
		}
		public override Task PrepareClient (System.Net.Http.HttpClient client)
		{
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", CurrentBasicAccount.Key);
			return Task.FromResult (true);
		}

		protected override async Task<Account> PerformAuthenticate ()
		{
			var account = CurrentSmartThingsAccount ?? GetAccount<SmartThingsAccount> (Identifier);
			if (account?.IsValid () == true) {
				return CurrentAccount = account;
			}

			authenticator = CreateAuthenticator ();
			var smartAuthenticator = authenticator as SmartThingsAuthenticator;
			if (CurrentShowAuthenticator != null)
				CurrentShowAuthenticator (authenticator);
			else
				ShowAuthenticator (authenticator);

			var token = await authenticator.GetAuthCode ();
			if (string.IsNullOrEmpty (token)) {
				throw new Exception ("Null token");
			}
			account = new SmartThingsAccount {
				Key = token,
				ExpiresIn = smartAuthenticator.Response.ExpiresIn,
				Created = DateTime.UtcNow,
				TokenType = smartAuthenticator.Response.TokenType,
				Token = smartAuthenticator.Response.AccessToken,
				UserName = smartAuthenticator.Username,
				Password = smartAuthenticator.Password
			};
			account.Identifier = Identifier;
			SaveAccount (account);
			CurrentAccount = account;
			return account;
		}

		protected override async Task<bool> RefreshAccount (Account account)
		{
			try {
				var smartAccount = account as SmartThingsAccount;
				//It's sad that we need to use credentials to refresh tokens. the api doesnt send refresh tokens :(
				var result = await GetTokenFromCredentials (new HttpClient (), LoginUrl, smartAccount.UserName, smartAccount.Password);
				smartAccount.TokenType = result.TokenType;
				smartAccount.Token = result.AccessToken;
				smartAccount.ExpiresIn = result.ExpiresIn;
				smartAccount.Created = DateTime.UtcNow;
				if (smartAccount == CurrentAccount)
					await OnAccountUpdated (smartAccount);
				CurrentAccount = smartAccount;
				SaveAccount (smartAccount);
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}
		public static async Task<OauthResponse> GetTokenFromCredentials (HttpClient client, string loginUrl, string username, string password)
		{
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Basic", "ZTRhYmEwZTQtYmQ5OC0xMWUyLTljMTAtMjc0NmU4YjllNTUzOjVkMGE4MGFlLTk1NWUtMTFlMi04M2NmLWRhZmU2NzA5ZjhjMA==");
			var response = await client.PostAsync (loginUrl, new FormUrlEncodedContent (new Dictionary<string, string> {
				{"client_id","e4aba0e4-bd98-11e2-9c10-2746e8b9e553"},
				{"client_secret","5d0a80ae-955e-11e2-83cf-dafe6709f8c0"},
				{"grant_type","password"},
				{"password",password},
				{"scope","mobile"},
				{"username",username},
			}));
			var respString = await response.Content.ReadAsStringAsync ();
			response.EnsureSuccessStatusCode ();
			return await respString.ToObjectAsync<OauthResponse> ();
		}
		#endregion

		#region Api Calls
		public Task<UserInfo> GetUserInfo ()
		{
			return Get<UserInfo> ("https://auth-global.api.smartthings.com/users/me");
		}
		public UserLocation [] CurrentLocations { get; set; }

		public async Task<UserLocation []> GetLocations ()
		{
			CurrentLocations = await Get<UserLocation []> ("https://account-global.api.smartthings.com/locations");
			return CurrentLocations;
		}


		public async Task<UserLocation> GetDefaultLocation ()
		{
			return CurrentLocations?.FirstOrDefault () ?? (await GetLocations())?.FirstOrDefault();
		}
		public async Task<UserLocation> GetDefaultLocation (Device device)
		{
			return CurrentLocations?.FirstOrDefault () ?? (await GetLocations ())?.FirstOrDefault (x=> x.Id == device.LocationId);
		}

		public async Task<LocationResponse> GetLocationDetails (UserLocation location = null)
		{
			if (location == null)
				location = await GetDefaultLocation ();

			return await Get<LocationResponse> (location.CreateApiUrl ( $"api/locations/{location.Id}"));
		}


		public async Task<Device[]> GetDevices (UserLocation location = null)
		{
			if (location == null)
				location = await GetDefaultLocation ();

			return await Get<Device[]> (location.CreateApiUrl ( $"api/locations/{location.Id}/devices"));
		}

		public async Task<string> GetSocketClientID (UserLocation location = null)
		{
			if (location == null)
				location = await GetDefaultLocation ();

			var data = await Post (new StringContent ("", Encoding.UTF8, "application/json"),location.CreateApiUrl($"api/clients"));
			var dict = await data.ToObjectAsync<Dictionary<string, string>> ();
			return dict ["clientId"];
		}

		public async Task<DeviceInfo> GetDeviceInfo (Device device)
		{
			var location = await GetDefaultLocation (device);
			return await Get<DeviceInfo> (location.CreateApiUrl ($"api/devices/{device.Id}"));
		}

		public async Task<bool> TurnOnDevice (Device device)
		{
			try {
				var location = await GetDefaultLocation (device);
				var url = location.CreateApiUrl ($"api/devices/{device.Id}/commands/action/switch.on");
				await Post (new CommandModel (), url);
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}


		public async Task<bool> TurnOffDevice (Device device)
		{
			try {
				var location = await GetDefaultLocation (device);
				await Post (new CommandModel (), location.CreateApiUrl ($"api/devices/{device.Id}/commands/action/switch.off"));
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}

		public async Task<bool> SetDeviceLevel (Device device, int level)
		{
			try {
				var location = await GetDefaultLocation (device);
				await Post (new CommandModel { Arguments = new List<object> { level } }, location.CreateApiUrl ($"api/devices/{device.Id}/commands/action/switch%20level.setLevel"));
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}

		public async Task<bool> RefreshDevice (Device device)
		{
			try {
				var location = await GetDefaultLocation (device);
				await Post (new CommandModel (), location.CreateApiUrl ($"api/devices/{device.Id}/commands/action/refresh.refresh"));
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}

		public async Task<bool> SetDeviceColor (Device device, ColorArgument args)
		{
			try {
				var location = await GetDefaultLocation (device);
				await Post (new CommandModel {
					Arguments = new List<object> {
						args
					}
				}, location.CreateApiUrl ($"api/devices/{device.Id}/commands/action/setAdustedColor"));
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}

		public async Task<bool> SetDevicePresence (Device device, bool present)
		{
			try {
				var location = await GetDefaultLocation (device);
				await Post (new EventsModel {
					Data = $"presence:{(present ? 1 : 0)}"
				}, location.CreateApiUrl ($"api/devices/{device.Id}/events"));
				return true;
			} catch (Exception ex) {
				Console.WriteLine (ex);
				return false;
			}
		}
		#endregion
	}
}


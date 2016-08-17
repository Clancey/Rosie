using System;
using SimpleAuth;

namespace Rosie.SmartThings
{
	public partial class SmartThingsApi
	{
		static SmartThingsApi ()
		{
			SimpleAuth.Resolver.Register<SimpleAuth.IAuthStorage, AuthStorage> ();
			BasicAuthApi.ShowAuthenticator = async (auth) => {
				var authenticator = new BasicAuthController (auth);
				await authenticator.GetCredentials ("SmartThings Login");
			};
		}
	}
}


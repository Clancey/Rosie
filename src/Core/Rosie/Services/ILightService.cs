using System;
using System.Threading.Tasks;

namespace Rosie.Services
{
	public interface ILightService 
	{
		Task TurnOn(Data data);
		Task TurnOff(Data data);
	}
}

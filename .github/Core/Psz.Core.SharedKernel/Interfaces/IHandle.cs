using System.Threading.Tasks;

namespace Psz.Core.SharedKernel.Interfaces
{
	public interface IHandle<TData, TResponse>
	{
		TResponse Handle();
		TResponse Validate();
	}
	public interface IHandleAsync<TData, TResponse>
	{
		Task<TResponse> HandleAsync();
		Task<TResponse> ValidateAsync();
	}
}

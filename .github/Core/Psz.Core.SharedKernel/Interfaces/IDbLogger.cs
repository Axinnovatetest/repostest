using System;

namespace Psz.Core.SharedKernel.Interfaces
{
	public interface IDbLogger
	{
		void Info(string message);
		void Warning(string message);
		void Error(string message, Exception ex = null);
	}
}

namespace Psz.Core.SharedKernel.Interfaces
{
	public interface ISpecification<T>
	{
		bool IsSatisfiedBy(T entity);
		void Apply(T entity);
	}
}

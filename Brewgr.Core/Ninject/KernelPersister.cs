using System;
using Ninject;

namespace ctorx.Core.Ninject
{
	public static class KernelPersister
	{
		static readonly object Locker = new object();
		static IKernel Kernel;

		/// <summary>
		/// Gets the Kernel
		/// </summary>
		public static IKernel Get()
		{
			return Kernel;
		}

		/// <summary>
		/// Sets the Kernel
		/// </summary>
		public static void Set(IKernel kernel)
		{
			lock (Locker)
			{
				Kernel = kernel;
			}
		}
	}
}
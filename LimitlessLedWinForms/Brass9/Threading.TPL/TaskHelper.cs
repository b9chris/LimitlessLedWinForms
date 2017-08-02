using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitlessLedWinForms.Brass9.Threading.TPL
{
	public static class TaskHelper
	{
		public static void RunBg(Func<Task> fn)
		{
			Task.Run(fn).ConfigureAwait(false);
		}
	}
}

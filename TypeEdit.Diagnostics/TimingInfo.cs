using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TypeEdit.Interfaces.Diagnostics;

namespace TypeEdit.Diagnostics
{
	public class TimingInfo : ITimingInfo
	{
		private Stopwatch mStopwatch;

		private int mCallDepth;

		IEnumerable<ITimingInfo> ITimingInfo.Children => Children;

		public List<TimingInfo> Children { get; private set; }

		ITimingInfo ITimingInfo.Parent => Parent;

		public TimingInfo Parent { get; private set; }

		public string Name { get; private set; }

		public long AccumulatedTime { get; private set; }

		public int Iterations { get; private set; }

		public long MaximumTime { get; private set; }

		public int UID { get; private set; }

		public int Start()
		{
			mCallDepth++;
			if (mCallDepth == 1)
			{
				mStopwatch.Restart();
			}
			return mCallDepth;
		}

		public int Stop()
		{
			if (mCallDepth == 0)
			{
				throw new InvalidOperationException("TimingInfo " + Name + " was already stopped!");
			}
			foreach (TimingInfo child in Children)
			{
				if (child.mCallDepth > 0)
				{
					while (child.Stop() > 0)
					{
					}
				}
			}
			mCallDepth--;
			Iterations++;
			if (mCallDepth == 0)
			{
				mStopwatch.Stop();
				AccumulatedTime += mStopwatch.ElapsedMilliseconds;
				MaximumTime = Math.Max(MaximumTime, AccumulatedTime);
			}
			return mCallDepth;
		}

		public void FrameReset()
		{
			Iterations = 0;
			AccumulatedTime = 0L;
			foreach (TimingInfo child in Children)
			{
				child.FrameReset();
			}
		}

		public TimingInfo(string name, TimingInfo parent, int uid)
		{
			mStopwatch = new Stopwatch();
			Children = new List<TimingInfo>();
			Parent = parent;
			Name = name;
			UID = uid;
		}

		public override string ToString()
		{
			if (Iterations <= 0 && mCallDepth != 0)
			{
				return Name + ": Running...";
			}
			return Name + ": " + Iterations + "x " + (double)AccumulatedTime / 1000.0 + "s [Max: " + (double)AccumulatedTime / 1000.0 + "s]";
		}

		public void FullReset()
		{
			MaximumTime = 0L;
			foreach (TimingInfo child in Children)
			{
				child.FullReset();
			}
			Children.RemoveAll((TimingInfo ti) => ti.mCallDepth == 0);
		}

		internal void ToStringIndent(int indent, StringBuilder sb)
		{
			sb.Append(string.Empty.PadLeft(indent, ' '));
			sb.AppendLine(ToString());
			foreach (TimingInfo child in Children)
			{
				child.ToStringIndent(indent + 4, sb);
			}
		}
	}
}

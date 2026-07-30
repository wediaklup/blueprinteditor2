using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TypeEdit.Interfaces.Diagnostics;

namespace TypeEdit.Diagnostics
{
	public class TimingManager : ITimingManager
	{
		private class TimingSectionHandle : ITimingSection, IDisposable
		{
			public TimingManager Manager;

			public ITimingInfo TimingInfo;

			public void Dispose()
			{
				EndTiming();
			}

			public ITimingInfo EndTiming()
			{
				if (Manager != null)
				{
					Manager.EndTiming(this);
					Manager = null;
				}
				return TimingInfo;
			}
		}

		private Dictionary<Thread, TimingInfo> mCurrentThreadedTimingInfos;

		private List<Thread> mActiveThreads;

		private TimingDialog mTimingDialog;

		[DebuggerHidden]
		public ITimingSection StartDebugTiming(string name)
		{
			return null;
		}

		[DebuggerHidden]
		internal ITimingInfo EndDebugTiming(ITimingSection section)
		{
			return null;
		}

		private ITimingSection startTiming(string name)
		{
			TimingInfo value;
			lock (mCurrentThreadedTimingInfos)
			{
				if (!mCurrentThreadedTimingInfos.TryGetValue(Thread.CurrentThread, out value))
				{
					value = new TimingInfo("Thread " + Thread.CurrentThread.ManagedThreadId, null, 0);
					value.Start();
					mCurrentThreadedTimingInfos.Add(Thread.CurrentThread, value);
					mActiveThreads.Add(Thread.CurrentThread);
				}
			}
			if (name == null || name.Length == 0)
			{
				throw new InvalidOperationException("name cannot be null or empty!");
			}
			StackFrame stackFrame = new StackFrame(2, needFileInfo: false);
			int uid = stackFrame.GetNativeOffset();
			TimingInfo timingInfo = value;
			if (timingInfo.UID != uid)
			{
				timingInfo = value.Children.Find((TimingInfo ti) => ti.UID == uid);
				if (timingInfo == null)
				{
					timingInfo = new TimingInfo(name, value, uid);
					value.Children.Add(timingInfo);
				}
				lock (mCurrentThreadedTimingInfos)
				{
					mCurrentThreadedTimingInfos[Thread.CurrentThread] = timingInfo;
				}
			}
			timingInfo.Start();
			TimingSectionHandle timingSectionHandle = new TimingSectionHandle();
			timingSectionHandle.Manager = this;
			timingSectionHandle.TimingInfo = timingInfo;
			return timingSectionHandle;
		}

		public ITimingSection StartTiming(string name)
		{
			return startTiming(name);
		}

		internal ITimingInfo EndTiming(ITimingSection section)
		{
			if (!(section is TimingSectionHandle timingSectionHandle))
			{
				throw new InvalidOperationException("Section handle not recognised!");
			}
			TimingInfo value;
			lock (mCurrentThreadedTimingInfos)
			{
				if (!mCurrentThreadedTimingInfos.TryGetValue(Thread.CurrentThread, out value))
				{
					throw new InvalidOperationException("Timer not found on current thread!");
				}
			}
			if (timingSectionHandle.TimingInfo == value)
			{
				if (value.Stop() == 0)
				{
					lock (mCurrentThreadedTimingInfos)
					{
						mCurrentThreadedTimingInfos[Thread.CurrentThread] = value.Parent;
					}
				}
				return value;
			}
			throw new InvalidOperationException("Wrong timer ended. Expected " + value.Name + " but got " + timingSectionHandle.TimingInfo.Name);
		}

		private void showTimingDialog()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (mTimingDialog == null)
			{
				mTimingDialog = new TimingDialog(this);
			}
			((Form)mTimingDialog).ShowDialog();
			((Component)(object)mTimingDialog).Disposed += mTimingDialog_Disposed;
		}

		private void mTimingDialog_Disposed(object sender, EventArgs e)
		{
			mTimingDialog = null;
		}

		public TimingManager()
		{
			Thread thread = new Thread(threadCheck);
			thread.IsBackground = true;
			thread.Start();
			mCurrentThreadedTimingInfos = new Dictionary<Thread, TimingInfo>();
			mActiveThreads = new List<Thread>();
		}

		private void threadCheck()
		{
			while (true)
			{
				Thread.Sleep(100);
				foreach (Thread mActiveThread in mActiveThreads)
				{
					TimingInfo timingInfo = mCurrentThreadedTimingInfos[mActiveThread];
					if (!mActiveThread.IsAlive && timingInfo != null)
					{
						while (timingInfo.Parent != null)
						{
							timingInfo = timingInfo.Parent;
						}
						while (timingInfo.Stop() > 0)
						{
						}
					}
				}
				mActiveThreads.RemoveAll((Thread t) => !t.IsAlive);
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			lock (mCurrentThreadedTimingInfos)
			{
				foreach (KeyValuePair<Thread, TimingInfo> mCurrentThreadedTimingInfo in mCurrentThreadedTimingInfos)
				{
					mCurrentThreadedTimingInfo.Value.ToStringIndent(0, stringBuilder);
				}
			}
			return stringBuilder.ToString();
		}

		public void ClearDeadThreads()
		{
			lock (mCurrentThreadedTimingInfos)
			{
				List<Thread> list = new List<Thread>();
				foreach (KeyValuePair<Thread, TimingInfo> mCurrentThreadedTimingInfo in mCurrentThreadedTimingInfos)
				{
					if (!mCurrentThreadedTimingInfo.Key.IsAlive)
					{
						list.Add(mCurrentThreadedTimingInfo.Key);
					}
				}
				foreach (Thread item in list)
				{
					mCurrentThreadedTimingInfos.Remove(item);
				}
			}
		}

		public void ResetIterations()
		{
			lock (mCurrentThreadedTimingInfos)
			{
				foreach (KeyValuePair<Thread, TimingInfo> mCurrentThreadedTimingInfo in mCurrentThreadedTimingInfos)
				{
					mCurrentThreadedTimingInfo.Value.FrameReset();
				}
			}
		}

		public void ResetAll()
		{
			lock (mCurrentThreadedTimingInfos)
			{
				foreach (KeyValuePair<Thread, TimingInfo> mCurrentThreadedTimingInfo in mCurrentThreadedTimingInfos)
				{
					mCurrentThreadedTimingInfo.Value.FullReset();
				}
			}
			ClearDeadThreads();
		}
	}
}

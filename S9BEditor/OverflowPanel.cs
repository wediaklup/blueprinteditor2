using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace S9BEditor
{
	internal class OverflowPanel : Panel
	{
		private List<WeakReference> mChildOrdering;

		private List<double> mItemOffsets;

		private UIElement mDraggingElement;

		private bool mDragging;

		private Point mDragStartPoint;

		private int mCurrentDragOverIndex;

		private double mLastDragThresholdStart;

		private double mLastDragThresholdEnd;

		private bool mLastDragThresholdsValid;

		public static readonly DependencyProperty OrientationProperty;

		public static readonly DependencyProperty IsOnOverflowProperty;

		public static readonly DependencyProperty HasOverflowItemsProperty;

		public static readonly DependencyProperty OverflowAppendModeProperty;

		public static readonly DependencyProperty CanRearrangeProperty;

		public Orientation Orientation
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Orientation)((DependencyObject)this).GetValue(OrientationProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((DependencyObject)this).SetValue(OrientationProperty, (object)value);
			}
		}

		public bool HasOverflowItems
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(HasOverflowItemsProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(HasOverflowItemsProperty, (object)value);
			}
		}

		public OverflowAppendMode OverflowAppendMode
		{
			get
			{
				return (OverflowAppendMode)((DependencyObject)this).GetValue(OverflowAppendModeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(OverflowAppendModeProperty, (object)value);
			}
		}

		public bool CanRearrange
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(CanRearrangeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(CanRearrangeProperty, (object)value);
			}
		}

		public event EventHandler<OverflowStatesChangedEventArgs> OverflowStatesChanged;

		public OverflowPanel()
		{
			mItemOffsets = new List<double>();
			mChildOrdering = new List<WeakReference>();
		}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			if (CanRearrange)
			{
				UIElement val = null;
				foreach (UIElement internalChild in ((Panel)this).InternalChildren)
				{
					UIElement val2 = internalChild;
					if (val2.InputHitTest(Mouse.GetPosition((IInputElement)(object)val2)) != null)
					{
						val = val2;
						break;
					}
				}
				if (val != null)
				{
					mDraggingElement = val;
					mDragStartPoint = Mouse.GetPosition((IInputElement)(object)this);
				}
			}
			((UIElement)this).OnPreviewMouseLeftButtonDown(e);
		}

		protected override void OnPreviewMouseMove(MouseEventArgs e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Invalid comparison between Unknown and I4
			if (mDraggingElement != null)
			{
				if (!mDragging)
				{
					Point position = Mouse.GetPosition((IInputElement)(object)this);
					Vector val = position - mDragStartPoint;
					if (Math.Abs(((Vector)(val)).X) > 4.0 || Math.Abs(((Vector)(val)).Y) > 4.0)
					{
						mDragging = ((UIElement)this).CaptureMouse();
					}
					return;
				}
				int num = mChildOrdering.FindIndex((WeakReference o) => o.Target == mDraggingElement);
				if (num < 0 || num > mItemOffsets.Count)
				{
					return;
				}
				Point position2 = e.GetPosition((IInputElement)(object)this);
				double num2 = (((int)Orientation == 1) ? ((Point)(position2)).Y : ((Point)(position2)).X);
				int num3;
				for (num3 = 0; num3 < mItemOffsets.Count - 1; num3++)
				{
					double num4 = mItemOffsets[num3];
					double num5 = mItemOffsets[num3 + 1];
					if (mLastDragThresholdsValid)
					{
						if (num3 + 1 == num)
						{
							num5 = mLastDragThresholdStart;
						}
						else if (num3 == num)
						{
							if (num2 >= mItemOffsets[num3] && num2 < mItemOffsets[num3 + 1])
							{
								mLastDragThresholdsValid = false;
							}
							else
							{
								num4 = mLastDragThresholdStart;
								num5 = mLastDragThresholdEnd;
							}
						}
						else if (num3 - 1 == num)
						{
							num4 = mLastDragThresholdEnd;
						}
					}
					if (num2 < num4)
					{
						num3--;
						break;
					}
					if (num2 >= num4 && num2 < num5)
					{
						break;
					}
				}
				int num6;
				for (num6 = 0; num6 < mChildOrdering.Count; num6++)
				{
					object? target = mChildOrdering[num6].Target;
					UIElement val2 = (UIElement)((target is UIElement) ? target : null);
					if (val2 != null && GetIsOnOverflow((DependencyObject)(object)val2))
					{
						break;
					}
				}
				int num7 = Math.Max(0, Math.Min(Math.Min(num3, ((Panel)this).InternalChildren.Count - 1), num6 - 1));
				if (num7 != num)
				{
					mLastDragThresholdEnd = mItemOffsets[num7 + 1];
					mLastDragThresholdStart = mItemOffsets[num7];
					mLastDragThresholdsValid = true;
					MoveItem(mDraggingElement, num7);
				}
				mCurrentDragOverIndex = num7;
			}
			else
			{
				((UIElement)this).OnPreviewMouseMove(e);
			}
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (mDragging)
			{
				((UIElement)this).ReleaseMouseCapture();
			}
			mDragging = false;
			mDraggingElement = null;
			mLastDragThresholdsValid = false;
			((UIElement)this).OnPreviewMouseLeftButtonUp(e);
		}

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			((Panel)this).OnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			updateIndices();
			bool flag = (int)Orientation == 1;
			Size result = default(Size);
			foreach (WeakReference item in mChildOrdering)
			{
				object? target = item.Target;
				UIElement val = (UIElement)((target is UIElement) ? target : null);
				if (val != null)
				{
					val.Measure(availableSize);
					Size desiredSize = val.DesiredSize;
					if (flag)
					{
						((Size)(result)).Height = Math.Min(((Size)(result)).Height + ((Size)(desiredSize)).Height, ((Size)(availableSize)).Height);
						((Size)(result)).Width = Math.Max(((Size)(result)).Width, ((Size)(desiredSize)).Width);
					}
					else
					{
						((Size)(result)).Width = Math.Min(((Size)(result)).Width + ((Size)(desiredSize)).Width, ((Size)(availableSize)).Width);
						((Size)(result)).Height = Math.Max(((Size)(result)).Height, ((Size)(desiredSize)).Height);
					}
				}
			}
			return result;
		}

		private bool updateIndices()
		{
			bool changed = false;
			mChildOrdering.RemoveAll(delegate(WeakReference o)
			{
				object? target = o.Target;
				UIElement val2 = (UIElement)((target is UIElement) ? target : null);
				if (val2 == null || !((Panel)this).InternalChildren.Contains(val2))
				{
					changed = true;
					return true;
				}
				return false;
			});
			bool[] array = new bool[((Panel)this).InternalChildren.Count];
			for (int num = 0; num < ((Panel)this).InternalChildren.Count; num++)
			{
				UIElement val = ((Panel)this).InternalChildren[num];
				foreach (WeakReference item in mChildOrdering)
				{
					if (item.Target == val)
					{
						array[num] = true;
					}
				}
			}
			for (int num2 = 0; num2 < ((Panel)this).InternalChildren.Count; num2++)
			{
				if (!array[num2])
				{
					switch (OverflowAppendMode)
					{
						case OverflowAppendMode.AppendFront:
							mChildOrdering.Insert(0, new WeakReference(((Panel)this).InternalChildren[num2]));
							break;
						case OverflowAppendMode.AppendBack:
							mChildOrdering.Add(new WeakReference(((Panel)this).InternalChildren[num2]));
							break;
					}
					changed = true;
				}
			}
			return changed;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			bool flag = (int)Orientation == 1;
			double num = 0.0;
			Dictionary<UIElement, bool> dictionary = new Dictionary<UIElement, bool>();
			mItemOffsets.Clear();
			bool flag2 = false;
			bool flag3 = true;
			foreach (WeakReference item in mChildOrdering)
			{
				mItemOffsets.Add(num);
				object? target = item.Target;
				UIElement val = (UIElement)((target is UIElement) ? target : null);
				if (val != null)
				{
					Size desiredSize = val.DesiredSize;
					bool isOnOverflow = GetIsOnOverflow((DependencyObject)(object)val);
					bool flag4 = isOnOverflow;
					if (flag)
					{
						flag4 = !flag3 && num + ((Size)(desiredSize)).Height > ((Size)(finalSize)).Height;
						val.Arrange(new Rect(0.0, num, ((Size)(desiredSize)).Width, ((Size)(desiredSize)).Height));
						num += ((Size)(desiredSize)).Height;
					}
					else
					{
						flag4 = !flag3 && num + ((Size)(desiredSize)).Width > ((Size)(finalSize)).Width;
						val.Arrange(new Rect(num, 0.0, ((Size)(desiredSize)).Width, ((Size)(desiredSize)).Height));
						num += ((Size)(desiredSize)).Width;
					}
					if (isOnOverflow != flag4)
					{
						dictionary.Add(val, flag4);
					}
					flag2 = flag2 || flag4;
					flag3 = false;
				}
			}
			mItemOffsets.Add(num);
			foreach (KeyValuePair<UIElement, bool> item2 in dictionary)
			{
				SetIsOnOverflow((DependencyObject)(object)item2.Key, item2.Value);
			}
			if (dictionary.Count > 0)
			{
				OnOverflowStatesChanged(new OverflowStatesChangedEventArgs(dictionary.Keys));
			}
			HasOverflowItems = flag2;
			return finalSize;
		}

		public void MoveItem(UIElement item, int index)
		{
			index = Math.Min(index, mChildOrdering.Count);
			if (item != null && mChildOrdering.RemoveAll((WeakReference o) => o.Target == item) > 0)
			{
				mChildOrdering.Insert(index, new WeakReference(item));
				((UIElement)this).InvalidateArrange();
				((UIElement)this).InvalidateMeasure();
			}
		}

		public void BringItemToFront(UIElement item)
		{
			MoveItem(item, 0);
		}

		protected virtual void OnOverflowStatesChanged(OverflowStatesChangedEventArgs e)
		{
			OverflowStatesChanged?.Invoke(this, e);
		}

		public static bool GetIsOnOverflow(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsOnOverflowProperty);
		}

		public static void SetIsOnOverflow(DependencyObject obj, bool value)
		{
			obj.SetValue(IsOnOverflowProperty, (object)value);
		}

		static OverflowPanel()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
			OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(OverflowPanel), (PropertyMetadata)new FrameworkPropertyMetadata((object)(Orientation)0, (FrameworkPropertyMetadataOptions)3));
			IsOnOverflowProperty = DependencyProperty.RegisterAttached("IsOnOverflow", typeof(bool), typeof(OverflowPanel), (PropertyMetadata)new UIPropertyMetadata((object)false));
			HasOverflowItemsProperty = DependencyProperty.Register("HasOverflowItems", typeof(bool), typeof(OverflowPanel), (PropertyMetadata)new UIPropertyMetadata((object)false));
			OverflowAppendModeProperty = DependencyProperty.Register("OverflowAppendMode", typeof(OverflowAppendMode), typeof(OverflowPanel), (PropertyMetadata)new UIPropertyMetadata((object)OverflowAppendMode.AppendBack));
			CanRearrangeProperty = DependencyProperty.Register("CanRearrange", typeof(bool), typeof(OverflowPanel), (PropertyMetadata)new UIPropertyMetadata((object)false));
		}
	}
}

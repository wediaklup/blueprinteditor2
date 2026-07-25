using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace S9BEditor;

internal class CommandsCanExecuteValuesConverter : IMultiValueConverter
{
	private static CommandsCanExecuteValuesConverter mInstance;

	public static CommandsCanExecuteValuesConverter Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new CommandsCanExecuteValuesConverter();
			}
			return mInstance;
		}
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = false;
		foreach (object obj in values)
		{
			if (!(obj is ICommand command) || command.CanExecute(null))
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}

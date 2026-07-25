using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows.Markup;

namespace S9BEditor;

[ContentProperty("Commands")]
internal class AtLeastOneCommandProxy : ICommand
{
	public ObservableCollection<ICommand> Commands { get; private set; }

	public event EventHandler CanExecuteChanged
	{
		add
		{
			CommandManager.RequerySuggested += value;
		}
		remove
		{
			CommandManager.RequerySuggested -= value;
		}
	}

	public AtLeastOneCommandProxy()
	{
		Commands = new ObservableCollection<ICommand>();
		Commands.CollectionChanged += Commands_CollectionChanged;
	}

	private void Commands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.NewItems != null)
		{
			foreach (ICommand newItem in e.NewItems)
			{
				newItem.CanExecuteChanged += command_CanExecuteChanged;
			}
		}
		if (e.OldItems != null)
		{
			foreach (ICommand oldItem in e.OldItems)
			{
				oldItem.CanExecuteChanged -= command_CanExecuteChanged;
			}
		}
		OnCanExecuteChanged(EventArgs.Empty);
	}

	private void command_CanExecuteChanged(object sender, EventArgs e)
	{
		OnCanExecuteChanged(EventArgs.Empty);
	}

	protected virtual void OnCanExecuteChanged(EventArgs eventArgs)
	{
		CommandManager.InvalidateRequerySuggested();
	}

	public bool CanExecute(object parameter)
	{
		if (Commands != null)
		{
			foreach (ICommand command in Commands)
			{
				if (command.CanExecute(parameter))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Execute(object parameter)
	{
	}
}

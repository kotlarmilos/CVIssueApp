﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVIssueApp.Controls
{
    public class DelegateCommand : ICommand
    {
        bool _inFlight;
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _execute;
        readonly Func<Task> _task;

        public event EventHandler CanExecuteChanged;

        internal DelegateCommand(Action<object> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
        }

        internal DelegateCommand(Action execute)
            : this(o => execute())
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
        }

        internal DelegateCommand(Func<Task> task)
            : this(task, null)
        {
        }

        internal DelegateCommand(Func<Task> task, Func<object, bool> canExecute)
        {
            _task = task;
            _canExecute = canExecute;
        }

        async void InvokeCommandTask(Task commandTask)
        {
            _inFlight = true;
            //ChangeCanExecute(); // TODO: Bug in Xamarin Forms 1.3 causes this to crash Android when command is bound to a context action
            try
            {
                await commandTask;
            }
            finally
            {
                _inFlight = false;
                //ChangeCanExecute(); // TODO: Bug in Xamarin Forms 1.3 causes this to crash Android when command is bound to a context action
            }
        }

        internal DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
            : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
            _canExecute = canExecute;
        }

        internal DelegateCommand(Action execute, Func<bool> canExecute)
            : this(o => execute(), o => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
        }

        public void Execute(object parameter)
        {
            if (_task == null)
            {
                _execute(parameter);
                return;
            }
            InvokeCommandTask(_task());
        }

       
        public bool CanExecute(object parameter)
        {
            if (_inFlight)
            {
                return false;
            }
            return _canExecute == null || _canExecute(parameter);
        }

        public void ChangeCanExecute()
        {
            EventHandler eventHandler = CanExecuteChanged;
            if (eventHandler == null)
                return;
            eventHandler(this, EventArgs.Empty);
        }
    }

    
    public sealed class DelegateCommand<T> : DelegateCommand
    {
        internal DelegateCommand(Action<T> execute)
            : base(o => execute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
        }

        internal DelegateCommand(Func<Task<T>> task)
            : base(task)
        {
        }

        internal DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
            : base(o => execute((T)o), o => canExecute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");
        }

        internal DelegateCommand(Func<Task<T>> task, Func<T, bool> canExecute)
            : base(task, o => canExecute((T)o))
        {
        }
    }
}

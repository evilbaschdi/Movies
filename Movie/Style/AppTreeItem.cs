using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Movie.Style
{
    public interface IAppTreeDataProvider
    {
        string Name { get; }

        string Path { get; }

        Bitmap Icon { get; }

        bool CanBeDisplayed { get; }
    }

    public class AppTreeContainer : AppTreeNode
    {
        private readonly ObservableCollection<AppTreeNode> _children;
        private readonly bool _enablePreview;

        private AppTreeNode _previewChild;

        public AppTreeContainer(string selectionGroup, string header, bool enablePreview = false)
            : base(selectionGroup, header)
        {
            _children = new ObservableCollection<AppTreeNode>();
            _enablePreview = enablePreview;

            if(_enablePreview)
            {
                PropertyChanged += (sender, args) =>
                {
                    if(args.PropertyName != "IsSelected")
                    {
                        return;
                    }

                    if(!IsSelected)
                    {
                        foreach(var item in Children)
                        {
                            item.IsSelected = false;
                        }
                    }

                    UpdatePreview();
                };
            }
        }

        public IEnumerable<AppTreeContainer> ContainerChildren
        {
            get { return _children.OfType<AppTreeContainer>(); }
        }

        public IEnumerable<AppTreeItem> ItemChildren
        {
            get { return _children.OfType<AppTreeItem>(); }
        }

        public IEnumerable<AppTreeNode> Children
        {
            get { return _children; }
        }

        public void AddChild(AppTreeNode item)
        {
            item.SelectionChanged += OnSelectionChanged;
            item.Parent = this;
            _children.Add(item);

            if(_enablePreview)
            {
                UpdatePreview();
            }
        }

        public bool RemoveChild(AppTreeNode item)
        {
            item.SelectionChanged -= OnSelectionChanged;
            item.Parent = null;
            var result = _children.Remove(item);

            if(_enablePreview)
            {
                UpdatePreview();
            }
            return result;
        }

        private void UpdatePreview()
        {
            if(_previewChild != null)
            {
                _previewChild.PropertyChanged -= PreviewPropertyChanged;
            }

            if(_children.Count == 0)
            {
                Icon = null;
                OnPropertyChanged("Icon");

                return;
            }

            _previewChild = _children.First();
            _previewChild.PropertyChanged += PreviewPropertyChanged;

            if(IsSelected)
            {
                Icon = null;
                OnPropertyChanged("Icon");
            }
            else
            {
                Icon = _previewChild.Icon;
                OnPropertyChanged("Icon");
            }
        }

        private void PreviewPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName == "Icon")
            {
                UpdatePreview();
            }
        }
    }

    public class AppTreeGenerator
    {
        public AppTreeContainer Generate(IEnumerable<IAppTreeDataProvider> dataProviders, bool enablePreview = false)
        {
            var root = new AppTreeContainer("root", "root");

            foreach(var item in dataProviders.Where(el => el.CanBeDisplayed))
            {
                var path = item.Path.Split('/');
                var lastContainer = root;

                int i;
                for(i = 0; i < path.Length; i++)
                {
                    var container = lastContainer.ContainerChildren.FirstOrDefault(el => el.Header == path[i]);
                    if(container == null)
                    {
                        container = new AppTreeContainer(i.ToString("D"), path[i], enablePreview);
                        lastContainer.AddChild(container);
                    }

                    lastContainer = container;
                }

                lastContainer.AddChild(new AppTreeItem(i.ToString("D"), item.Name, item, item.Icon));
            }

            return root;
        }
    }

    public class AppTreeItem : AppTreeNode
    {
        public AppTreeItem(string selectionGroup, string header, object dataContext)
            : base(selectionGroup, header)
        {
            DataContext = dataContext;
        }

        public AppTreeItem(string selectionGroup, string header, object dataContext, Bitmap icon)
            : base(selectionGroup, header, icon)
        {
            DataContext = dataContext;
        }
    }

    public abstract class AppTreeNode : INotifyPropertyChanged
    {
        protected readonly string _group;
        private bool _isSelected;

        public AppTreeNode(string selectionGroup, string header)
        {
            _group = selectionGroup;
            Header = header;
        }

        public AppTreeNode(string selectionGroup, string header, Image icon)
        {
            _group = selectionGroup;
            Header = header;

            if(icon == null)
            {
                return;
            }
            using(var memory = new MemoryStream())
            {
                icon.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = memory;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();

                Icon = bmi;
            }
        }

        public ImageSource Icon { get; set; }

        public AppTreeContainer Parent { get; set; }

        public string Header { get; set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");

                if(value)
                {
                    OnSelectionChanged(this);
                }
            }
        }

        public object DataContext { get; set; }

        public string Group
        {
            get { return _group; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<AppTreeNode> SelectionChanged;

        public void Select()
        {
            IsSelected = true;
            OnSelectionChanged(this);

            if(Parent != null)
            {
                Parent.Select();
            }
        }

        protected void OnSelectionChanged(AppTreeNode sender)
        {
            if(SelectionChanged != null)
            {
                SelectionChanged(sender);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    internal class RelayCommand : ICommand
    {
        #region Fields

        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///     Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if(execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion ICommand Members
    }
}
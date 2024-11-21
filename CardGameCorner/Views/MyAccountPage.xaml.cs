using System.ComponentModel;

namespace CardGameCorner.Views;

public partial class MyAccountPage : ContentPage
{
    private readonly ToolbarItem _editButton;
    private readonly ToolbarItem _backButton;
    private readonly ToolbarItem _doneButton;

    public MyAccountPage(MyAccountViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        // Toolbar setup
        _editButton = new ToolbarItem { Text = "Edit", Command = viewModel.EditCommand };
        _backButton = new ToolbarItem { Text = "Back", Command = viewModel.BackCommand };
        _doneButton = new ToolbarItem { Text = "Done", Command = viewModel.DoneCommand };

        ToolbarItems.Add(_editButton);
        viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MyAccountViewModel.IsEditMode))
        {
            var viewModel = (MyAccountViewModel)BindingContext;
            ToolbarItems.Clear();
            if (viewModel.IsEditMode)
            {
                ToolbarItems.Add(_backButton);
                ToolbarItems.Add(_doneButton);
            }
            else
            {
                ToolbarItems.Add(_editButton);
            }
        }
    }
}

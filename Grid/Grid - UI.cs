using System;
using cAlgo.API;

namespace cAlgo
{
    public partial class Grid
    {
        private Button _settingsButton;
        private Border _dialogBorder;
        private TextBox _zeroLineTextBox;
        private TextBox _gridSizeTextBox;
        private bool _isDialogOpen = false;

        // Store asset info when dialog opens to prevent saving to wrong asset
        private string _dialogBroker;
        private string _dialogAsset;
        private string _dialogTimeFrame;

        /// <summary>
        /// Updates the button text to show current grid size
        /// </summary>
        private void UpdateButtonText()
        {
            if (_settingsButton != null)
            {
                _settingsButton.Text = $"Grid Settings\n({_actualGridSize}p)";
            }
        }

        /// <summary>
        /// Creates the settings button on the chart
        /// </summary>
        private void CreateSettingsButton()
        {
            try
            {
                _settingsButton = new Button
                {
                    BackgroundColor = Color.FromArgb(100, 68, 68, 68),
                    ForegroundColor = Color.White,
                    Padding = 5,
                    Margin = 2,
                    FontSize = 9,
                    BorderColor = Color.Gray,
                    BorderThickness = 1
                };

                // Set initial text with current grid size
                UpdateButtonText();

                // Calculate position and margin based on parameter
                int x = ButtonOffsetX;
                int y = ButtonOffsetY;
                HorizontalAlignment hAlign;
                VerticalAlignment vAlign;
                Thickness margin;

                switch (ButtonPosition)
                {
                    case SettingsButtonPosition.TopLeft:
                        hAlign = HorizontalAlignment.Left;
                        vAlign = VerticalAlignment.Top;
                        margin = new Thickness(x, y, 0, 0);
                        break;
                    case SettingsButtonPosition.TopRight:
                        hAlign = HorizontalAlignment.Right;
                        vAlign = VerticalAlignment.Top;
                        margin = new Thickness(0, y, x, 0);
                        break;
                    case SettingsButtonPosition.TopCenter:
                        hAlign = HorizontalAlignment.Center;
                        vAlign = VerticalAlignment.Top;
                        margin = new Thickness(0, y, 0, 0);
                        break;
                    case SettingsButtonPosition.BottomLeft:
                        hAlign = HorizontalAlignment.Left;
                        vAlign = VerticalAlignment.Bottom;
                        margin = new Thickness(x, 0, 0, y);
                        break;
                    case SettingsButtonPosition.BottomRight:
                        hAlign = HorizontalAlignment.Right;
                        vAlign = VerticalAlignment.Bottom;
                        margin = new Thickness(0, 0, x, y);
                        break;
                    case SettingsButtonPosition.BottomCenter:
                        hAlign = HorizontalAlignment.Center;
                        vAlign = VerticalAlignment.Bottom;
                        margin = new Thickness(0, 0, 0, y);
                        break;
                    case SettingsButtonPosition.CenterLeft:
                        hAlign = HorizontalAlignment.Left;
                        vAlign = VerticalAlignment.Center;
                        margin = new Thickness(x, 0, 0, 0);
                        break;
                    case SettingsButtonPosition.CenterRight:
                        hAlign = HorizontalAlignment.Right;
                        vAlign = VerticalAlignment.Center;
                        margin = new Thickness(0, 0, x, 0);
                        break;
                    default:
                        hAlign = HorizontalAlignment.Right;
                        vAlign = VerticalAlignment.Top;
                        margin = new Thickness(0, y, x, 0);
                        break;
                }

                _settingsButton.HorizontalAlignment = hAlign;
                _settingsButton.VerticalAlignment = vAlign;
                _settingsButton.Margin = margin;

                _settingsButton.Click += SettingsButton_Click;

                Chart.AddControl(_settingsButton);
            }
            catch (Exception ex)
            {
                Log($"Error creating settings button: {ex.Message}");
            }
        }

        /// <summary>
        /// Handler for settings button click
        /// </summary>
        private void SettingsButton_Click(ButtonClickEventArgs obj)
        {
            if (_isDialogOpen)
            {
                CloseSettingsDialog();
            }
            else
            {
                OpenSettingsDialog();
            }
        }

        /// <summary>
        /// Opens the settings dialog
        /// </summary>
        private void OpenSettingsDialog()
        {
            try
            {
                _isDialogOpen = true;

                // Store current asset info to prevent saving to wrong asset if user changes chart
                _dialogBroker = Account.BrokerName;
                _dialogAsset = Symbol.Name;
                _dialogTimeFrame = TimeFrame.Name;

                Log($"Opening dialog for {_dialogAsset}/{_dialogTimeFrame} - Current values: ZeroLine={_actualZeroLine}, GridSize={_actualGridSize}");

                // Create text boxes
                _zeroLineTextBox = new TextBox
                {
                    Text = _actualZeroLine.ToString("F5"),
                    Width = 150,
                    Margin = 5,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.FromArgb(255, 40, 40, 40)
                };

                _gridSizeTextBox = new TextBox
                {
                    Text = _actualGridSize.ToString(),
                    Width = 150,
                    Margin = 5,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.FromArgb(255, 40, 40, 40)
                };

                var saveButton = new Button
                {
                    Text = "Save",
                    BackgroundColor = Color.Green,
                    ForegroundColor = Color.White,
                    Margin = 5,
                    Width = 70
                };
                saveButton.Click += SaveButton_Click;

                var cancelButton = new Button
                {
                    Text = "Cancel",
                    BackgroundColor = Color.Gray,
                    ForegroundColor = Color.White,
                    Margin = 5,
                    Width = 70
                };
                cancelButton.Click += CancelButton_Click;

                // Build button panel
                var buttonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = "0,15,0,0"
                };
                buttonPanel.AddChild(saveButton);
                buttonPanel.AddChild(cancelButton);

                // Build dialog layout
                var dialogContent = new StackPanel
                {
                    BackgroundColor = Color.FromArgb(230, 30, 30, 30),
                    Orientation = Orientation.Vertical
                };

                dialogContent.AddChild(new TextBlock { Text = $"Grid Settings - {Symbol.Name} {TimeFrame.Name}", ForegroundColor = Color.White, FontWeight = FontWeight.Bold, Margin = 5 });
                dialogContent.AddChild(new TextBlock { Text = "Zero Line (0 = auto):", ForegroundColor = Color.LightGray, Margin = "5,10,5,0" });
                dialogContent.AddChild(_zeroLineTextBox);
                dialogContent.AddChild(new TextBlock { Text = "Grid Size (pips):", ForegroundColor = Color.LightGray, Margin = "5,10,5,0" });
                dialogContent.AddChild(_gridSizeTextBox);
                dialogContent.AddChild(buttonPanel);

                _dialogBorder = new Border
                {
                    BorderColor = Color.Gray,
                    BorderThickness = 2,
                    Child = dialogContent,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Chart.AddControl(_dialogBorder);
            }
            catch (Exception ex)
            {
                Log($"Error opening dialog: {ex.Message}");
                _isDialogOpen = false;
            }
        }

        /// <summary>
        /// Closes the settings dialog
        /// </summary>
        private void CloseSettingsDialog()
        {
            if (_dialogBorder != null)
            {
                Chart.RemoveControl(_dialogBorder);
                _dialogBorder = null;
            }
            _isDialogOpen = false;
        }

        /// <summary>
        /// Handler for Save button
        /// </summary>
        private void SaveButton_Click(ButtonClickEventArgs obj)
        {
            try
            {
                // Parse input values
                if (!double.TryParse(_zeroLineTextBox.Text, out double zeroLine))
                {
                    Log("Invalid Zero Line value. Please enter a valid number.");
                    return;
                }

                if (!int.TryParse(_gridSizeTextBox.Text, out int gridSize) || gridSize < 1)
                {
                    Log("Invalid Grid Size value. Please enter a positive integer.");
                    return;
                }

                Log($"Saving settings from dialog: ZeroLine={zeroLine}, GridSize={gridSize} for {_dialogAsset}/{_dialogTimeFrame}");

                // Save to storage using asset info from when dialog was opened
                SaveStoredSettings(zeroLine, gridSize, _dialogBroker, _dialogAsset, _dialogTimeFrame);

                // Update button text with new grid size
                UpdateButtonText();

                // Redraw grid with new settings
                DrawGrid();

                // Close dialog
                CloseSettingsDialog();
            }
            catch (Exception ex)
            {
                Log($"Error saving settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Handler for Cancel button
        /// </summary>
        private void CancelButton_Click(ButtonClickEventArgs obj)
        {
            CloseSettingsDialog();
        }
    }
}

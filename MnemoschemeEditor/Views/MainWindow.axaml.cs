using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using AvAp2.Models.Controls;
using MnemoschemeEditor.Tests;

namespace MnemoschemeEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PropertyGrid.SelectedObject = new CAutomaticSwitch();
        }
    }
}
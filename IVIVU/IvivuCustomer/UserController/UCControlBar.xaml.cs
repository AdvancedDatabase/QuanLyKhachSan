using Ivivu.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ivivu.UserController
{
    /// <summary>
    /// Interaction logic for UCControlBar.xaml
    /// </summary>
    public partial class UCControlBar : UserControl
    {
        public VMControlBar ViewModel { get; set; }
        public UCControlBar()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new VMControlBar();
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Bookstore.WPF.Views.Components
{
    public partial class PageHeaderControl : UserControl
    {
        public PageHeaderControl()
        {
            InitializeComponent();
        }

        #region Title
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PageHeaderControl), new PropertyMetadata("TIÊU ĐỀ"));

        public PackIconKind HeaderIcon
        {
            get { return (PackIconKind)GetValue(HeaderIconProperty); }
            set { SetValue(HeaderIconProperty, value); }
        }
        public static readonly DependencyProperty HeaderIconProperty =
            DependencyProperty.Register("HeaderIcon", typeof(PackIconKind), typeof(PageHeaderControl), new PropertyMetadata(PackIconKind.ViewDashboard));
        #endregion

        #region Add Button
        public bool ShowAddButton
        {
            get { return (bool)GetValue(ShowAddButtonProperty); }
            set { SetValue(ShowAddButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowAddButtonProperty =
            DependencyProperty.Register("ShowAddButton", typeof(bool), typeof(PageHeaderControl), new PropertyMetadata(true));

        public PackIconKind AddIcon
        {
            get { return (PackIconKind)GetValue(AddIconProperty); }
            set { SetValue(AddIconProperty, value); }
        }
        public static readonly DependencyProperty AddIconProperty =
            DependencyProperty.Register("AddIcon", typeof(PackIconKind), typeof(PageHeaderControl), new PropertyMetadata(PackIconKind.Plus));

        public string AddTitle
        {
            get { return (string)GetValue(AddTitleProperty); }
            set { SetValue(AddTitleProperty, value); }
        }
        public static readonly DependencyProperty AddTitleProperty =
            DependencyProperty.Register("AddTitle", typeof(string), typeof(PageHeaderControl), new PropertyMetadata("THÊM"));

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(PageHeaderControl), new PropertyMetadata(null));

        public object AddCommandParameter
        {
            get { return GetValue(AddCommandParameterProperty); }
            set { SetValue(AddCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty AddCommandParameterProperty =
            DependencyProperty.Register("AddCommandParameter", typeof(object), typeof(PageHeaderControl), new PropertyMetadata(null));
        #endregion

        #region Second Button
        public bool ShowSecondButton
        {
            get { return (bool)GetValue(ShowSecondButtonProperty); }
            set { SetValue(ShowSecondButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowSecondButtonProperty =
            DependencyProperty.Register("ShowSecondButton", typeof(bool), typeof(PageHeaderControl), new PropertyMetadata(false));

        public PackIconKind SecondIcon
        {
            get { return (PackIconKind)GetValue(SecondIconProperty); }
            set { SetValue(SecondIconProperty, value); }
        }
        public static readonly DependencyProperty SecondIconProperty =
            DependencyProperty.Register("SecondIcon", typeof(PackIconKind), typeof(PageHeaderControl), new PropertyMetadata(PackIconKind.Plus));

        public string SecondTitle
        {
            get { return (string)GetValue(SecondTitleProperty); }
            set { SetValue(SecondTitleProperty, value); }
        }
        public static readonly DependencyProperty SecondTitleProperty =
            DependencyProperty.Register("SecondTitle", typeof(string), typeof(PageHeaderControl), new PropertyMetadata("THÊM"));

        public ICommand SecondCommand
        {
            get { return (ICommand)GetValue(SecondCommandProperty); }
            set { SetValue(SecondCommandProperty, value); }
        }
        public static readonly DependencyProperty SecondCommandProperty =
            DependencyProperty.Register("SecondCommand", typeof(ICommand), typeof(PageHeaderControl), new PropertyMetadata(null));

        public object SecondCommandParameter
        {
            get { return GetValue(SecondCommandParameterProperty); }
            set { SetValue(SecondCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty SecondCommandParameterProperty =
            DependencyProperty.Register("SecondCommandParameter", typeof(object), typeof(PageHeaderControl), new PropertyMetadata(null));
        #endregion

        #region Excel Export
        public bool ShowExportExcel
        {
            get { return (bool)GetValue(ShowExportExcelProperty); }
            set { SetValue(ShowExportExcelProperty, value); }
        }
        public static readonly DependencyProperty ShowExportExcelProperty =
            DependencyProperty.Register("ShowExportExcel", typeof(bool), typeof(PageHeaderControl), new PropertyMetadata(false));

        public ICommand ExportExcelCommand
        {
            get { return (ICommand)GetValue(ExportExcelCommandProperty); }
            set { SetValue(ExportExcelCommandProperty, value); }
        }
        public static readonly DependencyProperty ExportExcelCommandProperty =
            DependencyProperty.Register("ExportExcelCommand", typeof(ICommand), typeof(PageHeaderControl), new PropertyMetadata(null));

        public object ExportExcelCommandParameter
        {
            get { return GetValue(ExportExcelCommandParameterProperty); }
            set { SetValue(ExportExcelCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty ExportExcelCommandParameterProperty =
            DependencyProperty.Register("ExportExcelCommandParameter", typeof(object), typeof(PageHeaderControl), new PropertyMetadata(null));
        #endregion

        #region PDF Export
        public bool ShowExportPdf
        {
            get { return (bool)GetValue(ShowExportPdfProperty); }
            set { SetValue(ShowExportPdfProperty, value); }
        }
        public static readonly DependencyProperty ShowExportPdfProperty =
            DependencyProperty.Register("ShowExportPdf", typeof(bool), typeof(PageHeaderControl), new PropertyMetadata(false));

        public ICommand ExportPdfCommand
        {
            get { return (ICommand)GetValue(ExportPdfCommandProperty); }
            set { SetValue(ExportPdfCommandProperty, value); }
        }
        public static readonly DependencyProperty ExportPdfCommandProperty =
            DependencyProperty.Register("ExportPdfCommand", typeof(ICommand), typeof(PageHeaderControl), new PropertyMetadata(null));

        public object ExportPdfCommandParameter
        {
            get { return GetValue(ExportPdfCommandParameterProperty); }
            set { SetValue(ExportPdfCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty ExportPdfCommandParameterProperty =
            DependencyProperty.Register("ExportPdfCommandParameter", typeof(object), typeof(PageHeaderControl), new PropertyMetadata(null));
        #endregion

        #region Reload
        public bool ShowReloadButton
        {
            get { return (bool)GetValue(ShowReloadButtonProperty); }
            set { SetValue(ShowReloadButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowReloadButtonProperty =
            DependencyProperty.Register("ShowReloadButton", typeof(bool), typeof(PageHeaderControl), new PropertyMetadata(false));

        public ICommand ReloadCommand
        {
            get { return (ICommand)GetValue(ReloadCommandProperty); }
            set { SetValue(ReloadCommandProperty, value); }
        }
        public static readonly DependencyProperty ReloadCommandProperty =
            DependencyProperty.Register("ReloadCommand", typeof(ICommand), typeof(PageHeaderControl), new PropertyMetadata(null));
        #endregion
    }
}
' The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

Imports Windows.Phone.UI.Input
Imports Windows.UI
Imports Windows.UI.Core
Imports Windows.UI.Popups

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public AboutInfo As String = "Keep up to date with your daily dose of CommitStrip, with this simple UWP app." & vbCrLf & vbCrLf & "Open source, and forever free (and we're talking free beer AND free speech)."
    Public AppName As String = Package.Current.DisplayName
    Public HomeURL As String = "http://www.commitstrip.com/en/?#"
    Public IsPanelOpen As Boolean = False
    Public localSettings As Windows.Storage.ApplicationDataContainer = Windows.Storage.ApplicationData.Current.LocalSettings
    Public Stored_ID As Object = localSettings.Values("COUNTRY")

    Public SideBar_E As String() = {"Home", "Change Language", "About"}
    Public SideBar_F As String() = {"Accueil", "Changer de langue", "Sur"}

    Public Sub ChangeWords(ByVal x As Integer)
        If x = 0 Then
            txtHOME.Text = SideBar_E(0)
            txtCHANGE.Text = SideBar_E(1)
            txtABOUT.Text = SideBar_E(2)
        Else
            txtHOME.Text = SideBar_F(0)
            txtCHANGE.Text = SideBar_F(1)
            txtABOUT.Text = SideBar_F(2)
        End If
    End Sub

    Async Sub BackPressed(sender As Object, e As BackPressedEventArgs)
        Dim AppName As String = Package.Current.DisplayName
        'Handles any Back button presses.
        e.Handled = True
        If wv.CanGoBack Then
            wv.GoBack()
        Else
            Await displayMessageAsync(AppName, "Are you sure you want to exit the app?", "")
        End If
    End Sub

    Public Sub CLOSESIDEBAR()
        SIDEBAR.Background = New SolidColorBrush(Color.FromArgb(255, 43, 63, 107))
        SIDEBAR.Width = 50
        IsPanelOpen = False
    End Sub

    Public Async Function displayMessageAsync(ByVal title As String, ByVal content As String, ByVal dialogType As String) As Task
        Dim messageDialog = New MessageDialog(content, title)
        If dialogType = "notification" Then
        Else
            messageDialog.Commands.Add(New UICommand("Yes", Nothing))
            messageDialog.Commands.Add(New UICommand("No", Nothing))
            messageDialog.DefaultCommandIndex = 0
        End If
        messageDialog.CancelCommandIndex = 1
        Dim cmdResult = Await messageDialog.ShowAsync()
        If cmdResult.Label = "Yes" Then
            Application.Current.Exit()
        End If
    End Function

    Private Async Sub BFS_Click(sender As Object, e As RoutedEventArgs) Handles BFS.Click
        CLOSESIDEBAR()
        If wv.CanGoBack Then
            wv.GoBack()
        Else
            Await displayMessageAsync(AppName, "Are you sure you want to exit the app?", "")
        End If
    End Sub

    Private Sub btnABOUT_Click(sender As Object, e As RoutedEventArgs) Handles btnABOUT.Click
        CLOSESIDEBAR()
        INFO.Visibility = Visibility.Visible
        ABOUTTEXT.Text = AboutInfo
    End Sub

    Private Sub btnCHANGE_Click(sender As Object, e As RoutedEventArgs) Handles btnCHANGE.Click
        CLOSESIDEBAR()
        LANG.Visibility = Visibility.Visible
    End Sub

    Private Sub btnHOME_Click(sender As Object, e As RoutedEventArgs) Handles btnHOME.Click
        CLOSESIDEBAR()
        Go_Home(HomeURL)
    End Sub

    Private Sub btnOPEN_Click(sender As Object, e As RoutedEventArgs) Handles btnOPEN.Click
        If IsPanelOpen Then
            SIDEBAR.Background = New SolidColorBrush(Color.FromArgb(255, 43, 63, 107))
            IsPanelOpen = False
            SIDEBAR.Width = 50
        Else
            SIDEBAR.Background = New SolidColorBrush(Color.FromArgb(191, 43, 63, 107))
            IsPanelOpen = True
            SIDEBAR.Width = 220
        End If
    End Sub

    Private Sub CI_Click(sender As Object, e As RoutedEventArgs) Handles CI.Click
        INFO.Visibility = Visibility.Collapsed
    End Sub

    Private Sub CL_Click(sender As Object, e As RoutedEventArgs) Handles CL.Click
        LANG.Visibility = Visibility.Collapsed
    End Sub

    Private Sub Go_Home(ByVal x As String)
        HomeURL = x
        wv.Source = New Uri(x)
    End Sub

    Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Me.InitializeComponent()
        If Stored_ID Is Nothing Then
            localSettings.Values("COUNTRY") = "0"
            RADIO_E.IsChecked = True
            RADIO_F.IsChecked = False
            HomeURL = "http://www.commitstrip.com/en/?#"
            ChangeWords(0)
        Else
            If Stored_ID = "0" Then
                RADIO_E.IsChecked = True
                RADIO_F.IsChecked = False
                HomeURL = "http://www.commitstrip.com/en/?#"
                ChangeWords(0)
            Else
                RADIO_E.IsChecked = False
                RADIO_F.IsChecked = True
                HomeURL = "http://www.commitstrip.com/fr/??#"
                ChangeWords(1)
            End If
        End If
        Go_Home(HomeURL)
        AddHandler HardwareButtons.BackPressed, AddressOf BackPressed
        AddHandler SystemNavigationManager.GetForCurrentView().BackRequested, Sub(s, a)

                                                                                  If wv.CanGoBack Then
                                                                                      wv.GoBack()
                                                                                      a.Handled = True
                                                                                  End If
                                                                              End Sub
    End Sub

    Private Sub RADIO_E_Checked(sender As Object, e As RoutedEventArgs) Handles RADIO_E.Checked
        If RADIO_E.IsChecked Then
            RADIO_F.IsChecked = False
            localSettings.Values("FULLSCREEN") = "0"
            LANG.Visibility = Visibility.Collapsed
            ChangeWords(0)
            Go_Home("http://www.commitstrip.com/en/?#")
        End If
    End Sub

    Private Sub RADIO_F_Checked(sender As Object, e As RoutedEventArgs) Handles RADIO_F.Checked
        If RADIO_F.IsChecked Then
            RADIO_E.IsChecked = False
            localSettings.Values("FULLSCREEN") = "1"
            LANG.Visibility = Visibility.Collapsed
            ChangeWords(1)
            Go_Home("http://www.commitstrip.com/fr/??#")
        End If
    End Sub

    Private Async Sub TFS_Click(sender As Object, e As RoutedEventArgs) Handles TFS.Click
        CLOSESIDEBAR()
        Dim ScrollToTopString = "var int = setInterval(function(){window.scrollBy(0, -36); if( window.pageYOffset === 0 ) clearInterval(int); }, 0.1);"
        Await wv.InvokeScriptAsync("eval", New String() {ScrollToTopString})
    End Sub

    Private Sub wv_GotFocus(sender As Object, e As RoutedEventArgs) Handles wv.GotFocus
        CLOSESIDEBAR()
    End Sub

    Private Sub wv_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles wv.LoadCompleted
        If wv.CanGoBack Then
            XFS.Visibility = Visibility.Collapsed
            BFS.Visibility = Visibility.Visible
        Else
            XFS.Visibility = Visibility.Visible
            BFS.Visibility = Visibility.Collapsed
        End If
    End Sub

    Private Async Sub XFS_Click(sender As Object, e As RoutedEventArgs) Handles XFS.Click
        CLOSESIDEBAR()
        Await displayMessageAsync(AppName, "Are you sure you want to exit the app?", "")
    End Sub

End Class
Public Class Form_ItemMaker
    Public CL As CommonLibrary
    Public SL As SpecificLibrary
    Sub New(ByRef _commonLibrary As CommonLibrary, ByRef _specificLibrary As SpecificLibrary)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CL = _commonLibrary
        SL = _specificLibrary
    End Sub
    Private Sub Form_ItemMaker_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(50, 150)
        Me.Size = New Point(1355, Me.Size.Height)

        Dim RaceList(9) As String
        RaceList(0) = "Dark Elf"
        RaceList(1) = "Troll"
        RaceList(2) = "Ogre"
        RaceList(3) = "Elf"
        RaceList(4) = "Dwarf"
        RaceList(5) = "Gnome"
        RaceList(6) = "Halfling"
        RaceList(7) = "Erudite"
        RaceList(8) = "Human"
        RaceList(9) = "Barbarian"
        For Each line In RaceList
            CheckedListBox2.Items.Add(line, False)
        Next
        Dim Classlist(14) As String
        Classlist(0) = "Warrior"
        Classlist(1) = "Paladin"
        Classlist(2) = "Shadowknight"
        Classlist(3) = "Enchanter"
        Classlist(4) = "Magician"
        Classlist(5) = "Wizard"
        Classlist(6) = "Alchemist"
        Classlist(7) = "Necromancer"
        Classlist(8) = "Monk"
        Classlist(9) = "Rogue"
        Classlist(10) = "Ranger"
        Classlist(11) = "Bard"
        Classlist(12) = "Druid"
        Classlist(13) = "Shaman"
        Classlist(14) = "Cleric"
        For Each line In Classlist
            CheckedListBox8.Items.Add(line, False)
        Next

        ListBox1.Items.Add("Livenilla")
        ListBox1.SelectedIndex = 0
        ListBox2.Items.Add("Worn")
        ListBox2.Items.Add("Wielded")
        ListBox2.Items.Add("Tools")
        ListBox2.Items.Add("Gem")
        ListBox2.Items.Add("Misc")
        ListBox2.Items.Add("NONE")
        ListBox2.SelectedIndex = 0
        ListBox1.Enabled = False
        ListBox2.SelectedIndex = ListBox2.Items.Count - 1
        ListBox2.Enabled = False

        Dim Stats(17) As String
        Stats(0) = "Strength"
        Stats(1) = "Stamina"
        Stats(2) = "Agility"
        Stats(3) = "Dexterity"
        Stats(4) = "Wisdom"
        Stats(5) = "Intelligence"
        Stats(6) = "Charisma"
        Stats(7) = "FR"
        Stats(8) = "CR"
        Stats(9) = "LR"
        Stats(10) = "AR"
        Stats(11) = "PR"
        Stats(12) = "DR"
        Stats(13) = "HoT"
        Stats(14) = "PoT"
        Stats(15) = "HPMAX"
        Stats(16) = "POWMAX"
        Stats(17) = "AC"
        For Each line In Stats
            CheckedListBox4.Items.Add(line, False)
        Next

        Dim Stats_Tradeskills(6) As String
        Stats_Tradeskills(0) = "Fishing"
        Stats_Tradeskills(1) = "Jewel Crafting"
        Stats_Tradeskills(2) = "Armor Crafting"
        Stats_Tradeskills(3) = "Weapon Crafting"
        Stats_Tradeskills(4) = "Tailoring"
        Stats_Tradeskills(5) = "Alchemy"
        Stats_Tradeskills(6) = "Carpentry"
        For Each line In Stats_Tradeskills
            CheckedListBox7.Items.Add(line, False)
        Next

        Dim Stats_Atypical(3) As String
        Stats_Atypical(0) = "Def Mod"
        Stats_Atypical(1) = "Off Mod"
        Stats_Atypical(2) = "HP Factor"
        Stats_Atypical(3) = "Movement Rate"
        For Each line In Stats_Atypical
            CheckedListBox6.Items.Add(line, False)
        Next

        GroupBox4.Enabled = False
    End Sub

    Private Sub CheckBoxes_5_1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox5.CheckedChanged, CheckBox1.CheckedChanged
        ' the (select) "ALL" buttons for Race & Class respectively.
        Dim UseControl As Object
        If sender Is CheckBox5 Then
            UseControl = CheckedListBox2
        ElseIf sender Is CheckBox1 Then
            UseControl = CheckedListBox8
        End If

        If sender.Checked = True Then
            For index As Integer = 0 To UseControl.Items.Count - 1
                UseControl.SetItemChecked(index, True)
            Next
            UseControl.Enabled = False
        Else
            UseControl.Enabled = True
        End If
    End Sub

    'valued checked list box functions
    Private Sub CheckedListBox4_7_6ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox4.ItemCheck, CheckedListBox7.ItemCheck, CheckedListBox6.ItemCheck
        'Initiates the Adding/Removal of the values in the Valued CheckedListBoxes
        If e.NewValue = CheckState.Checked Then
            If sender.items.Item(e.Index).Contains("+") = False Then
                If sender.items.Item(e.Index).Contains("-") = False Then
                    AskForStatModAmount(sender, e)
                End If
            End If
        Else
            If sender.items.Item(e.Index).Contains("+") Then
                sender.items.Item(e.Index) = Microsoft.VisualBasic.Left(sender.items.Item(e.Index), sender.items.Item(e.Index).IndexOf("+") - 1)
            ElseIf sender.items.Item(e.Index).Contains("-") Then
                sender.items.Item(e.Index) = Microsoft.VisualBasic.Left(sender.items.Item(e.Index), sender.items.Item(e.Index).IndexOf("-") - 1)
            End If
        End If
    End Sub
    Private Sub AskForStatModAmount(ByRef sender As System.Object, ByRef e As System.Windows.Forms.ItemCheckEventArgs)
        'Asks for the amount to modify the Valued CheckedListBoxes by
        Dim result As String = InputBox("Enter Stat Increase/Decrease:", "Enter Stat Modification Amount").Trim
        Do While result.Contains("+ ")
            result = result.Replace("+ ", "+")
        Loop
        Do While result.Contains(" +")
            result = result.Replace(" +", "+")
        Loop
        Do While result.Contains("- ")
            result = result.Replace("- ", "-")
        Loop
        Do While result.Contains(" -")
            result = result.Replace(" -", "-")
        Loop
        Try
            If CInt(result) Then
            End If
        Catch ex As Exception
            e.NewValue = CheckState.Unchecked
            Exit Sub
        End Try
        If AlterCheckedListBoxItemsStatMod(sender.Items.Item(e.Index), result) <> 0 Then e.NewValue = CheckState.Unchecked
    End Sub
    Private Function AlterCheckedListBoxItemsStatMod(ByRef _senderItem As System.Object, ByVal modvalue As String)
        'Adds or Removes the value from the Valued CheckedListBoxes
        Try
            Dim wasNegative As Boolean = modvalue.Contains("-")
            modvalue = modvalue.Replace("+", "")
            If modvalue.Replace("-", "").Length > 3 Then Return -2
            If _senderItem.Contains("+") Or _senderItem.Contains("-") Then Return -1
            If CInt(modvalue) >= 0 Then
                If CInt(modvalue) = 0 And wasNegative Then
                    _senderItem = _senderItem & " - " & modvalue.Replace("-", "")
                Else
                    _senderItem = _senderItem & " + " & modvalue.Replace("-", "")
                End If
            ElseIf CInt(modvalue) < 0 Then
                modvalue = modvalue.Replace("-", "").Trim
                _senderItem = _senderItem & " - " & modvalue
            End If
        Catch ex As Exception
            Return -3
        End Try
        Return 0
    End Function
End Class
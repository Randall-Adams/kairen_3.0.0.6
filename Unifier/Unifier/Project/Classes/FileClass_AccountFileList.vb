﻿Public Class FileClass_AccountFileList
    Inherits FileClass_Base
    Sub New(ByVal _fileLocation As String, ByVal _fileName As String, Optional ByVal _fileExtension As String = ".txt")
        MyBase.New(_fileLocation, _fileName, _fileExtension)
    End Sub
    Public Property AutoLoadingAccount() As String
        Get
            Return MyBase.GetTagsData("Autoload Account")
        End Get
        Set(value As String)
            MyBase.UpdateDataInTag("Autoload Account") = value
        End Set
    End Property
    Public ReadOnly Property AccountNameList() As String()
        Get
            Dim _generatedAccountList As New List(Of String)
            For Each item In MyBase.ListOfTags
                If Microsoft.VisualBasic.Left(item, 10) = "Account - " Then
                    _generatedAccountList.Add(Microsoft.VisualBasic.Right(item, item.Length - 10))
                End If
            Next
            Return _generatedAccountList.ToArray
        End Get
    End Property
    Public WriteOnly Property AddAccountToFile()
        Set(value)
            MyBase.AddDataToTag("Account - " & value) = value
            MyBase.SaveAndReloadFile()
        End Set
    End Property
End Class
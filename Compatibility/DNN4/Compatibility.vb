#If DNNVERSION = "DNN4" Then

Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Host

Module Compatibility
    Public Sub InjectModules(ByVal mr As DNNStuff.ModuleRotator.ModuleRotator, ByVal modules As ArrayList)
        ' Inject modules into the page
        Dim baseSkin As DotNetNuke.UI.Skins.Skin = DotNetNuke.UI.Skins.Skin.GetParentSkin(mr)
        Dim paneCtrl As Web.UI.Control

        If baseSkin Is Nothing Then
            DotNetNuke.UI.Skins.Skin.AddModuleMessage(mr, "Error: Could not find ParentSkin", DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
        Else
            Dim moduleNumber As Integer = 1
            For Each _moduleSettings As ModuleInfo In modules
                _moduleSettings.PaneName = "TabPage" & moduleNumber

                If mr.HideTitles Then
                    _moduleSettings.ContainerSrc = mr.ResolveUrl("no container.ascx")
                End If
                paneCtrl = mr.FindControlRecursive(mr, _moduleSettings.PaneName)

                If Not paneCtrl Is Nothing Then
                    If mr.ModuleNeeded(moduleNumber, _moduleSettings) Then
                        baseSkin.InjectModule(paneCtrl, _moduleSettings, mr.PortalSettings)
                    End If
                End If
                ' todo: remove isdeleted setting, now done in hide modules RDE 17/JUL/06
                _moduleSettings.IsDeleted = True

                moduleNumber += 1
            Next
        End If

    End Sub

End Module

#End If
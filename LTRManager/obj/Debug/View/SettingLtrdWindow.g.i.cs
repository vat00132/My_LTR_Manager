﻿#pragma checksum "..\..\..\View\SettingLtrdWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DB886A74F4763651688EDEBDCADBBF129D6F51C1E71322BB5F976F19B3C07ED1"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using LTRManager.View;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace LTRManager.View {
    
    
    /// <summary>
    /// SettingLtrdWindow
    /// </summary>
    public partial class SettingLtrdWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox TransferWithoutDelay_CheckBox;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TimeSurvey_TextBox;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TimeoutForCommandExecution_TextBox;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ConnectionEstablishmentTimeout_TextBox;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TimeRecconect_TextBox;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SizeBufferTransfer_TextBox;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SizeBufferReceive_TextBox;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OK;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\View\SettingLtrdWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Cancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LTRManager;component/view/settingltrdwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\SettingLtrdWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TransferWithoutDelay_CheckBox = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 2:
            this.TimeSurvey_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 38 "..\..\..\View\SettingLtrdWindow.xaml"
            this.TimeSurvey_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TimeSurvey_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TimeoutForCommandExecution_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 39 "..\..\..\View\SettingLtrdWindow.xaml"
            this.TimeoutForCommandExecution_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TimeoutForCommandExecution_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ConnectionEstablishmentTimeout_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 40 "..\..\..\View\SettingLtrdWindow.xaml"
            this.ConnectionEstablishmentTimeout_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ConnectionEstablishmentTimeout_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TimeRecconect_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\..\View\SettingLtrdWindow.xaml"
            this.TimeRecconect_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TimeRecconect_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SizeBufferTransfer_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 61 "..\..\..\View\SettingLtrdWindow.xaml"
            this.SizeBufferTransfer_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SizeBufferTransfer_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SizeBufferReceive_TextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 62 "..\..\..\View\SettingLtrdWindow.xaml"
            this.SizeBufferReceive_TextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.SizeBufferReceive_TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.OK = ((System.Windows.Controls.Button)(target));
            
            #line 71 "..\..\..\View\SettingLtrdWindow.xaml"
            this.OK.Click += new System.Windows.RoutedEventHandler(this.OK_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Cancel = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\..\View\SettingLtrdWindow.xaml"
            this.Cancel.Click += new System.Windows.RoutedEventHandler(this.Cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


﻿#pragma checksum "..\..\..\Join.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "435E5567C5B87886370318B8049E298639A2A571948D1A7D3E7F0AC36931A138"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using Schcduler;
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


namespace Schcduler {
    
    
    /// <summary>
    /// Join
    /// </summary>
    public partial class Join : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbName;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtName;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbPhone;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPhone;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbPassword;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPassword;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbWage;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbWage;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbAuthority;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbAuthority;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Join.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnJoin;
        
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
            System.Uri resourceLocater = new System.Uri("/Schcduler;component/join.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Join.xaml"
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
            
            #line 9 "..\..\..\Join.xaml"
            ((Schcduler.Join)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lbName = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.txtName = ((System.Windows.Controls.TextBox)(target));
            
            #line 26 "..\..\..\Join.xaml"
            this.txtName.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtName_KeyDown);
            
            #line default
            #line hidden
            
            #line 26 "..\..\..\Join.xaml"
            this.txtName.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtName_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.lbPhone = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.txtPhone = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\..\Join.xaml"
            this.txtPhone.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtPhone_KeyDown);
            
            #line default
            #line hidden
            
            #line 28 "..\..\..\Join.xaml"
            this.txtPhone.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtPhone_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 6:
            this.lbPassword = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.txtPassword = ((System.Windows.Controls.TextBox)(target));
            
            #line 30 "..\..\..\Join.xaml"
            this.txtPassword.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtPassword_KeyDown);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\Join.xaml"
            this.txtPassword.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtPassword_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 8:
            this.lbWage = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.tbWage = ((System.Windows.Controls.TextBox)(target));
            
            #line 32 "..\..\..\Join.xaml"
            this.tbWage.KeyDown += new System.Windows.Input.KeyEventHandler(this.tbWage_KeyDown);
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\Join.xaml"
            this.tbWage.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.tbWage_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 10:
            this.lbAuthority = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.cbAuthority = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 12:
            this.btnJoin = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\Join.xaml"
            this.btnJoin.Click += new System.Windows.RoutedEventHandler(this.btnJoin_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


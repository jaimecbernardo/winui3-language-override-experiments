using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Windows.ApplicationModel.Resources;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LanguageOverrideUnpackagedCSharp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            ResourceManagerRequested += (_, e) =>
            {
                IResourceManager resourceManager = new LanguageOverrideResourceManager("resources.pri", "pt-PT");
                e.CustomResourceManager = resourceManager;
            };
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
    }

    internal sealed class LanguageOverrideResourceManager : Microsoft.Windows.ApplicationModel.Resources.IResourceManager
    {
        private IResourceManager _internalResourceManager;
        private string _language;

        public LanguageOverrideResourceManager(string filename, string language)
        {
            _internalResourceManager = new ResourceManager(filename);
            _language = language;
        }

        ResourceMap IResourceManager.MainResourceMap => _internalResourceManager.MainResourceMap;

        event TypedEventHandler<ResourceManager, ResourceNotFoundEventArgs> IResourceManager.ResourceNotFound
        {
            add
            {
                _internalResourceManager.ResourceNotFound += value;
            }

            remove
            {
                _internalResourceManager.ResourceNotFound -= value;
            }
        }

        ResourceContext IResourceManager.CreateResourceContext()
        {
            var overrideResourceContext = _internalResourceManager.CreateResourceContext();
            overrideResourceContext.QualifierValues["Language"] = _language;
            return overrideResourceContext;
        }
    }
}

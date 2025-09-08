using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Interop;
using Renga;

namespace VinnyRengaLoader
{
    public class PluginLoader : IPlugin
    {
        private readonly List<Renga.ActionEventSource> m_eventSources = new List<Renga.ActionEventSource>();
        public bool Initialize(string pluginFolder)
        {
            string executingAssemblyFile = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            string executionDirectoryPath = System.IO.Path.GetDirectoryName(executingAssemblyFile);

            //Load Vinny
            string vinnyPath = new DirectoryInfo(executionDirectoryPath).Parent.Parent.Parent.FullName;
            string VinnyLibConverterCommonPath = Path.Combine(vinnyPath, "VinnyLibConverterCommon.dll");
            string VinnyLibConverterKernelPath = Path.Combine(vinnyPath, "VinnyLibConverterKernel.dll");
            string VinnyRengaAdapterPath = Path.Combine(executionDirectoryPath, "VinnyRengaAdapter.dll");

            string VinnyLibConverterUIPath = Path.Combine(vinnyPath, "ui", "net8.0-windows", "VinnyLibConverterUI.dll");

            Assembly.LoadFrom(VinnyLibConverterCommonPath);
            Assembly.LoadFrom(VinnyLibConverterKernelPath);
            Assembly.LoadFrom(VinnyLibConverterUIPath);
            Assembly.LoadFrom(VinnyRengaAdapterPath);

            Renga.Application rengaApp = new Renga.Application();
            Renga.IUI rengaUI = rengaApp.UI;
            Renga.IUIPanelExtension vinnyRengaUiPanel = rengaUI.CreateUIPanelExtension();

            Renga.IAction vinnyRengaAdapterButton = rengaUI.CreateAction();
            vinnyRengaAdapterButton.ToolTip = "Vinny Run";
            vinnyRengaAdapterButton.DisplayName = "Vinny Run";

            Renga.IImage vinnyRengaAdapterButtonIcon = rengaUI.CreateImage();
            vinnyRengaAdapterButtonIcon.LoadFromFile(Path.Combine(executionDirectoryPath, "vinnyIcon_32x32.png"));
            vinnyRengaAdapterButton.Icon = vinnyRengaAdapterButtonIcon;

            ActionEventSource vinnyRengaAdapterActionEvent = new ActionEventSource(vinnyRengaAdapterButton);
            vinnyRengaAdapterActionEvent.Triggered += (o, s) =>
            {
                VinnyRengaAdapter.VinnyRengaAdapter.CreateInstance().Start();
            };
            m_eventSources.Add(vinnyRengaAdapterActionEvent);

            vinnyRengaUiPanel.AddToolButton(vinnyRengaAdapterButton);
            rengaUI.AddExtensionToPrimaryPanel(vinnyRengaUiPanel);
            
            return true;
        }

        public void Stop()
        {
            foreach (var eventSource in m_eventSources)
            {
                eventSource.Dispose();
            }
            m_eventSources.Clear();
        }
    }
}

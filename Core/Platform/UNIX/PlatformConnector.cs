﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbiote.Core.Plugin.Connector;
using System.Diagnostics;
using Symbiote.Core.Configuration;
using Symbiote.Core.Plugin;

namespace Symbiote.Core.Platform.UNIX
{
    internal class PlatformConnector : IConnector
    {
        private ConnectorItem itemRoot;
        private PerformanceCounter cpuUsed;
        private PerformanceCounter cpuIdle;

        private double lastCPUUsed;
        private double lastCPUIdle;

        public string Name { get; private set; }
        public string FQN { get; private set; }
        public Version Version { get; private set; }
        public PluginType PluginType { get; private set; }
        public ConfigurationDefinition ConfigurationDefinition { get; private set; }
        public string InstanceName { get; private set; }
        public string Configuration { get; private set; }
        public bool IsConfigured { get { return true; } }
        public bool Browseable { get { return true; } }
        public bool Writeable { get { return false; } }

        public event EventHandler<ConnectorEventArgs> Changed;

        public PlatformConnector(string instanceName)
        {
            InstanceName = instanceName;
            Name = "UNIXConnector";
            FQN = "Symbiote.Core.Platform.UNIX.PlatformConnector";
            Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            PluginType = PluginType.Connector;

            cpuUsed = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuIdle = new PerformanceCounter("Processor", "% Idle Time", "_Total");

            InitializeItems();
        }

        public void Configure(string configuration)
        {

        }

        public OperationResult Start()
        {
            return new OperationResult();
        }

        public OperationResult Stop()
        {
            return new OperationResult();
        }

        public Item FindItem(string fqn)
        {
            return FindItem(itemRoot, fqn);
        }

        private Item FindItem(Item root, string fqn)
        {
            if (root.FQN == fqn) return root;

            Item found = default(Item);
            foreach (Item child in root.Children)
            {
                found = FindItem(child, fqn);
                if (found != default(Item)) break;
            }
            return found;
        }

        public Item Browse()
        {
            return itemRoot;
        }


        public List<Item> Browse(Item root)
        {
            return root.Children;
        }

        public object Read(string item)
        {
            string[] itemName = item.Split('.');

            switch (itemName[itemName.Length - 2] + "." + itemName[itemName.Length - 1])
            {
                case "CPU.% Processor Time":
                    lastCPUUsed = cpuUsed.NextValue();
                    return lastCPUUsed;
                case "CPU.% Idle Time":
                    lastCPUIdle = cpuIdle.NextValue();
                    return lastCPUIdle;
                case "Memory.Total":
                    return 0;
                case "Memory.Available":
                    return new PerformanceCounter("Memory", "Available Bytes").NextValue();
                case "Memory.Cached":
                    return new PerformanceCounter("Memory", "Cache Bytes").NextValue();
                case "Memory.% Used":
                    return new PerformanceCounter("Memory", "% Committed Bytes In Use").NextValue();
                default:
                    return 0;
            }
        }

        public OperationResult Write(string item, object value)
        {
            return new OperationResult().AddError("The connector is not writeable.");
        }

        private void InitializeItems()
        {
            // instantiate an item root
            itemRoot = new ConnectorItem(this, InstanceName);

            // create CPU items
            Item cpuRoot = itemRoot.AddChild(new ConnectorItem(this, "CPU"));
            cpuRoot.AddChild(new ConnectorItem(this, "CPU.% Processor Time", typeof(double), ""));
            cpuRoot.AddChild(new ConnectorItem(this, "CPU.% Idle Time", typeof(double), ""));

            // prepare variables to use for processor time.  you need two successive values to 
            // report accurately so rather than sleeping the thread we will just keep track from 
            // call to call.  the first values will always be zero and that's ok.
            lastCPUUsed = 0;
            lastCPUIdle = 0;

            // create memory items
            Item memRoot = itemRoot.AddChild(new ConnectorItem(this, "Memory"));
            memRoot.AddChild(new ConnectorItem(this, "Memory.Total", typeof(double), ""));
            memRoot.AddChild(new ConnectorItem(this, "Memory.Available", typeof(double), ""));
            memRoot.AddChild(new ConnectorItem(this, "Memory.Cached", typeof(double), ""));
            memRoot.AddChild(new ConnectorItem(this, "Memory.% Used", typeof(double), ""));

            // create drive items
            ConnectorItem dRoot = itemRoot.AddChild(new ConnectorItem(this, "Drives"));

            // system drive
            ConnectorItem sdRoot = dRoot.AddChild(new ConnectorItem(this, "Drives.System"));
            sdRoot.AddChild(new ConnectorItem(this, "Drives.System.Name", typeof(string), ""));
            sdRoot.AddChild(new ConnectorItem(this, "Path", typeof(string), ""));
            sdRoot.AddChild(new ConnectorItem(this, "Type", typeof(Platform.DriveType), ""));
            sdRoot.AddChild(new ConnectorItem(this, "Capacity", typeof(long), ""));
            sdRoot.AddChild(new ConnectorItem(this, "UsedSpace", typeof(long), ""));
            sdRoot.AddChild(new ConnectorItem(this, "FreeSpace", typeof(long), ""));
            sdRoot.AddChild(new ConnectorItem(this, "PercentUsed", typeof(double), ""));
            sdRoot.AddChild(new ConnectorItem(this, "PercentFree", typeof(double), ""));

            // data drive
            ConnectorItem ddRoot = dRoot.AddChild(new ConnectorItem(this, "Data"));
            ddRoot.AddChild(new ConnectorItem(this, "Name", typeof(string), ""));
            ddRoot.AddChild(new ConnectorItem(this, "Path", typeof(string), ""));
            ddRoot.AddChild(new ConnectorItem(this, "Type", typeof(Platform.DriveType), ""));
            ddRoot.AddChild(new ConnectorItem(this, "Capacity", typeof(long), ""));
            ddRoot.AddChild(new ConnectorItem(this, "UsedSpace", typeof(long), ""));
            ddRoot.AddChild(new ConnectorItem(this, "FreeSpace", typeof(long), ""));
            ddRoot.AddChild(new ConnectorItem(this, "PercentUsed", typeof(double), ""));
            ddRoot.AddChild(new ConnectorItem(this, "PercentFree", typeof(double), ""));
        }

        private void OnChange(string fqn, object value)
        {
            if (Changed != null)
                Changed(this, new ConnectorEventArgs(fqn, value));
        }
    }
}

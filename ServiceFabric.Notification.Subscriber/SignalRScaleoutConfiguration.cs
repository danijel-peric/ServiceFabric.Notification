﻿using System;
using System.Fabric;

namespace ServiceFabric.Notification.Subscriber
{
    public class SignalRScaleoutConfiguration
    {
        public bool UseScaleout { get; }
        public string RedisConnectionString { get; }
        public string RedisAppName { get; } = "SignalRSelfHostScaleOut";
        public string EncryptionPassword { get; } = "no password";

        public SignalRScaleoutConfiguration(StatelessServiceContext context)
        {
            var codeContext = context;
            if (codeContext == null) // sanity check
                throw new ApplicationException("CodePackageActivationContext is null");

            ConfigurationPackage configurationPackage = codeContext.CodePackageActivationContext.GetConfigurationPackageObject("Config");

            if (configurationPackage.Settings?.Sections == null || !configurationPackage.Settings.Sections.Contains("SignalRScaleout"))
                return;

            var param = configurationPackage.Settings.Sections["SignalRScaleout"].Parameters;

            if (param.Contains("UseScaleout"))
                UseScaleout = bool.Parse(param["UseScaleout"].Value);
            if (param.Contains("RedisConnectionString"))
                RedisConnectionString = param["RedisConnectionString"].Value;
            if (param.Contains("RedisAppName"))
                RedisAppName = param["RedisAppName"].Value;
            if (param.Contains("EncryptionPassword"))
                EncryptionPassword = param["EncryptionPassword"].Value;
        }
    }
}

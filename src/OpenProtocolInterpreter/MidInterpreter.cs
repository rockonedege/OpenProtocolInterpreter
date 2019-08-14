﻿using OpenProtocolInterpreter.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenProtocolInterpreter
{
    public class MidInterpreter
    {
        private readonly IList<IMessagesTemplate> _messagesTemplates;

        public MidInterpreter()
        {
            _messagesTemplates = new List<IMessagesTemplate>();
        }

        public string Pack(Mid mid) => mid.Pack();

        public byte[] PackBytes(Mid mid) => mid.PackBytes();

        public Mid Parse(string package)
        {
            int mid = int.Parse(package.Substring(4, 4));
            var instance = TryParseStandaloneMid(mid);
            if (instance != default)
                return instance;

            var template = GetMessageTemplate(mid);
            return template.ProcessPackage(mid, package);
        }

        public Mid Parse(byte[] package)
        {
            int mid = int.Parse(Encoding.ASCII.GetString(package, 4, 4));
            var instance = TryParseStandaloneMid(mid);
            if (instance != default)
                return instance;

            var template = GetMessageTemplate(mid);
            return template.ProcessPackage(mid, package);
        }

        public ExpectedMid Parse<ExpectedMid>(string package) where ExpectedMid : Mid
        {
            Mid mid = Parse(package);
            if (mid.GetType().Equals(typeof(ExpectedMid)))
                return (ExpectedMid)mid;

            throw new InvalidCastException($"Package is Mid {mid.GetType().Name}, cannot be casted to {typeof(ExpectedMid).Name}");
        }

        public ExpectedMid Parse<ExpectedMid>(byte[] package) where ExpectedMid : Mid
        {
            Mid mid = Parse(package);
            if (mid.GetType().Equals(typeof(ExpectedMid)))
                return (ExpectedMid)mid;

            throw new InvalidCastException($"Package is Mid {mid.GetType().Name}, cannot be casted to {typeof(ExpectedMid).Name}");
        }

        internal void UseTemplate(IMessagesTemplate template)
        {
            if (!_messagesTemplates.Any(x => x.GetType().Equals(template.GetType())))
            {
                _messagesTemplates.Add(template);
            }
        }

        internal void UseTemplate<T>() where T : IMessagesTemplate
        {
            var instance = (IMessagesTemplate)Activator.CreateInstance(typeof(T));
            UseTemplate(instance);
        }

        internal void UseTemplate<T>(InterpreterMode mode) where T : IMessagesTemplate
        {
            var instance = (IMessagesTemplate)Activator.CreateInstance(typeof(T), new object[] { mode });
            UseTemplate(instance);

        }

        internal void UseTemplate<T>(IEnumerable<Type> types) where T : IMessagesTemplate
        {
            if (types.Any())
            {
                var instance = (IMessagesTemplate)Activator.CreateInstance(typeof(T), new object[] { types });
                UseTemplate(instance);
            }
        }

        private IMessagesTemplate GetMessageTemplate(int mid)
        {
            var template = _messagesTemplates.FirstOrDefault(x => x.IsAssignableTo(mid));
            if (template == null)
            {
                throw new NotImplementedException($@"Could not found a message parser for mid {mid}, please register it before using");
            }

            return template;
        }

        private Mid TryParseStandaloneMid(int mid)
        {
            switch (mid)
            {
                case KeepAlive.Mid9999.MID:
                    return new KeepAlive.Mid9999();
                case ApplicationController.Mid0270.MID:
                    return new ApplicationController.Mid0270();
            }

            return default;
        }
    }
}

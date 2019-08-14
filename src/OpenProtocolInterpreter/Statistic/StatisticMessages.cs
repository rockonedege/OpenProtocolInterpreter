﻿using OpenProtocolInterpreter.Messages;
using System;
using System.Collections.Generic;

namespace OpenProtocolInterpreter.Statistic
{
    internal class StatisticMessages : MessagesTemplate
    {
        public StatisticMessages() : base()
        {
            _templates = new Dictionary<int, Type>()
            {
                { Mid0300.MID, typeof(Mid0300) },
                { Mid0301.MID, typeof(Mid0301) }
            };
        }

        public StatisticMessages(IEnumerable<Type> selectedMids) : this()
        {
            FilterSelectedMids(selectedMids);
        }

        public StatisticMessages(InterpreterMode mode) : this()
        {
            FilterSelectedMids(mode);
        }

        public override bool IsAssignableTo(int mid) => mid > 299 && mid < 302;
    }
}

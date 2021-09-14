using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Controls;
using System.Text;

namespace DebugLogger.Wpf
{
    public class ActiveLogList<T> : BindingList<T> where T : ContentPresenter
    {
        private List<LogMessage> activeLogM = new List<LogMessage>();

        private SortedList<DateTime, LogMessage> pairs = new SortedList<DateTime, LogMessage>();

        public void Initialize()
        {
            this.AllowEdit = true;
            this.AllowNew = true;
            this.AllowRemove = true;
        }

        public void Add(LogMessage logM)
        {
            if(!pairs.ContainsKey(logM.logData.occurred))
            {
                pairs.Add(logM.logData.occurred, logM);

                int i = pairs.IndexOfKey(logM.logData.occurred);

                dummy();

                this.Insert(i, (T)logM.frame);
            }
            else
            {
                logM.logData.OffsetOccurredTick();
                Add(logM);
            }
        }

        private void dummy()
        {

        }

        public void Remove(LogMessage logM)
        {
            activeLogM.Remove(logM);
            pairs.Remove(logM.logData.occurred);

            base.Remove((T)logM.frame);
        }

        public void Add(List<LogMessage> logMList)
        {
            this.RaiseListChangedEvents = false;

            foreach (LogMessage logM in logMList)
                Add(logM);

            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }

        public void Remove(List<LogMessage> logMList)
        {
            this.RaiseListChangedEvents = false;

            foreach (LogMessage logM in logMList)
                Remove(logM);

            this.RaiseListChangedEvents = true;
            this.ResetBindings();
        }

        public new void Clear()
        {
            activeLogM.Clear();
            pairs.Clear();

            base.Clear();
        }


        [Obsolete]
        public new void Add(T item)
        {
        }

        [Obsolete]
        public new void Remove(T item)
        {
        }
    }
}

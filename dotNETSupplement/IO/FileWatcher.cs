using ch3plusStudio.dotNETSupplement.Core.Collections.Generic;
using ch3plusStudio.dotNETSupplement.Core.Event;
using ch3plusStudio.dotNETSupplement.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ch3plusStudio.dotNETSupplement.IO
{
    public class FilesWatcher : PeriodicTaskExecutor
    {
        public event EventHandler<EventArgs<String>> OnFileAppeared;
        public event EventHandler<EventArgs<String>> OnFileModified;
        public event EventHandler<EventArgs<String>> OnFileDisappeared;

        private FilesWatcher(Action action, TimeSpan timeSpan) : base(action, timeSpan){ }

        public FilesWatcher(List<String> filePaths, TimeSpan timeSpan)
        {
            _FilesToWatch = filePaths;
            _TimeSpan = timeSpan;

            Initialization();
        }

        private List<String> _FilesToWatch;

        private void Initialization()
        {
            var status = _FilesToWatch.Select(path => new KeyValuePairMutable<string, FileChangesInfoSnapshot>(path, new FileChangesInfoSnapshot(path))).ToList();

            _Action = () =>
            {
                foreach (var entry in status)
                {
                    var newStatus = new FileChangesInfoSnapshot(entry.Key);
            
                    if (newStatus != entry.Value)
                    {
                        if (newStatus.Exists != entry.Value.Exists)
                        {
                            if (newStatus.Exists)
                            {
                                Task.Factory.StartNew(() =>
                                {
                                    if (OnFileAppeared != null)
                                    {
                                        OnFileAppeared.Invoke(this, new EventArgs<string>(entry.Key));
                                    }
                                });
                            }
                            else
                            {
                                Task.Factory.StartNew(() =>
                                {
                                    if (OnFileDisappeared != null)
                                    {
                                        OnFileDisappeared.Invoke(this, new EventArgs<string>(entry.Key));
                                    }
                                });
                            }
                        }
                        else
                        {
                            Task.Factory.StartNew(() => {
                                if (OnFileModified != null)
                                { 
                                    OnFileModified.Invoke(this, new EventArgs<string>(entry.Key));
                                }
                            });
                        }
            
                        entry.Value = newStatus;
                    }
                }
            };
        }
    }
}

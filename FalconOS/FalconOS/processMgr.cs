using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    public class processMgr
    {
        public List<string> processes = new List<string>();
        
        public processMgr()
        {
            log.print("Process Manager", "Started process manager.");
        }

        public void addProc(string procName) { processes.Add(procName); return; }
        public void removeProc(string procName) { processes.Remove(procName); return; }
        public void clearProc() { processes.Clear(); return; }
        public string findProc(Predicate<string> procName) { return processes.Find(procName); }
    }
}

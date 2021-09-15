using System;
using Microsoft.Samples.Debugging.MdbgEngine;
using JoymonsCode.DotNetWatcher.Model;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace JoymonsCode.DotNetWatcher
{
    public class Processor
    {
        #region "Shared"
        static Processor _instance=new Processor();
        public static Processor Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Properties
        private AsyncOperation asyncOp;
        private NotificationCallback notificationCallback;
        #endregion

        #region "Constructor

        private Processor()
        {
        }
#endregion

        #region Method
        public void Attach(string []args, NotificationCallback callback)
        {
            this.notificationCallback = callback;
            this.asyncOp = AsyncOperationManager.CreateOperation(1);
            MyAsyncDelegate del=new MyAsyncDelegate(Start);
            del.BeginInvoke(args,callback,null,null);
        }
        public void Start(string[] args, NotificationCallback callback)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine("Usage: PrintEx <filename>");
                Console.WriteLine("   Will run <filename> and print all exceptions.");
                return;
            }
            Console.WriteLine("Run '{0}' and print all exceptions.", args[0]);

            MDbgEngine debugEngine = new MDbgEngine();
            debugEngine.Options.CreateProcessWithNewConsole = false;

            // Specify which debug events we want to receive. 
            // The underlying ICorDebug API will stop on all debug events.
            // The MDbgProcess object implements all of these callbacks, but only stops on a set of them 
            // based off the Options settings.
            // See CorProcess.DispatchEvent and MDbgProcess.InitDebuggerCallbacks for more details.
            debugEngine.Options.StopOnException = true;
            //Do 'debugger.Options.StopOnExceptionEnhanced = true;' to get additional exception notifications.

            // Launch the debuggee.
            MDbgProcess proc;
            int processId;
            if (int.TryParse(args[0], out processId))
            {
                proc = debugEngine.Attach(processId);
            }
            else
            {
                proc = debugEngine.CreateProcess(args[0], "", DebugModeFlag.Debug, null);
            }
            
            while (proc.IsAlive)
            {
                // Let the debuggee run and wait until it hits a debug event.
                ManualResetEvent handle = proc.Go() as ManualResetEvent;
                handle.WaitOne();

                // Process is now stopped. proc.StopReason tells us why we stopped.
                // The process is also safe for inspection.            
                ExceptionThrownStopReason m = proc.StopReason as ExceptionThrownStopReason;
                if (m != null)
                {
                    ProcessException(m, proc, callback);
                    continue;
                }
            }

            Trace.WriteLine("Done!");
        }

        private void ProcessException(ExceptionThrownStopReason m,MDbgProcess proc, NotificationCallback callback)
        {
            try
            {
                MDbgThread t = proc.Threads.Active;
                MDbgValue ex = t.CurrentException;
                MDbgFrame f = t.CurrentFrame;
                string message = "Exception is thrown:" + ex.TypeName + "(" + m.EventType +")"+Environment.NewLine+
                    "function " + f.Function.FullName + 
                    Environment.NewLine+" source file:" + t.CurrentSourcePosition.Path + ":" + t.CurrentSourcePosition.Line;
                Console.WriteLine(message);
                //SynchronizationContext.Post(new SendOrPostCallback(ExceptionWorker), new ExceptionNotification() { Messege = message });
                this.asyncOp.Post(new SendOrPostCallback(ExceptionWorker), new ExceptionNotification() { Messege = message });
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception is thrown, but can't inspect it.Details are :"+e.Message);
            }
        }
        private void ExceptionWorker(object state)
        {
            this.notificationCallback(state as NotificationBase);
        }
        #endregion

        #region Delegates
        private delegate void MyAsyncDelegate(string[] args, NotificationCallback callback);

        #endregion
        #region Classes
       
        #endregion
    }
}

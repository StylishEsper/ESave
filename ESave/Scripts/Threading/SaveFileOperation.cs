//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: June 2024
// Description: Save file background operation.
//***************************************************************************************

using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace Esper.ESave.Threading
{
    public class SaveFileOperation
    {
        /// <summary>
        /// The current state of the operation.
        /// </summary>
        public OperationState state { get; private set; } = OperationState.None;

        private Thread thread;
        private System.Action action;

        private bool inBackground;

        /// <summary>
        /// This is invoked when the operation has completed, canceled, or failed. Events are cleared
        /// after they are invoked.
        /// </summary>
        public UnityEvent onOperationEnded { get; private set; } = new();

        private static SynchronizationContext mainThreadContext;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="inBackground">If this operation should be executed in the background..</param>
        public SaveFileOperation(System.Action action, bool inBackground)
        {
            this.action = action;
            this.inBackground = inBackground;
        }

        /// <summary>
        /// Starts the operation.
        /// </summary>
        public void Start()
        {
            System.Action operation = () =>
            {
                state = OperationState.Ongoing;

                try
                {
                    action();
                    state = OperationState.Completed;
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"Save File Operation: Failed. Error: {ex}");
                    state = OperationState.Failed;
                }
            };

            if (!inBackground)
            {
                operation();
                OnOperationEnded();
            }
            else
            {
                if (mainThreadContext == null)
                {
                    mainThreadContext = SynchronizationContext.Current;
                }

                Thread thread = new(() =>
                {
                    operation();
                    mainThreadContext.Post(_ => OnOperationEnded(), null);
                });

                thread.IsBackground = true;
                thread.Start();
            }
        }

        /// <summary>
        /// Cancels the operation.
        /// </summary>
        public void Cancel()
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
                state = OperationState.Canceled;
                OnOperationEnded();
            }
        }

        /// <summary>
        /// Executes operation ended events.
        /// </summary>
        private void OnOperationEnded()
        {
            onOperationEnded.Invoke();
            onOperationEnded.RemoveAllListeners();
        }

        /// <summary>
        /// Operation state.
        /// </summary>
        public enum OperationState
        {
            None,
            Ongoing,
            Completed,
            Canceled,
            Failed
        }
    }
}
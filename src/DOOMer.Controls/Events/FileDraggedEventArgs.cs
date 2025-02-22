namespace DOOMer.Controls;

using System;
using System.Windows.Forms;

public class FileDraggedEventArgs : EventArgs {
    private readonly DragEventArgs backingEvent;

    public IDataObject? Data => backingEvent.Data;
    public int KeyState => backingEvent.KeyState;
    public int X => backingEvent.X;
    public int Y => backingEvent.Y;
    public DragDropEffects AllowedEffect => backingEvent.AllowedEffect;
    public DragDropEffects Effect { get => backingEvent.Effect; set => backingEvent.Effect = value; }
    public DropImageType DropImageType { get => backingEvent.DropImageType; set => backingEvent.DropImageType = value; }
    public string? Message { get => backingEvent.Message; set => backingEvent.Message = value; }
    public string? MessageReplacementToken { get => backingEvent.MessageReplacementToken; set => backingEvent.MessageReplacementToken = value; }

    /// <summary>
    ///     The array of paths associated with the event.
    /// </summary>
    public string[] Paths { get; }

    /// <summary>
    ///     Whether the files are not allowed to be dragged.
    /// </summary>
    public bool Cancel { get; set; }

    public FileDraggedEventArgs(DragEventArgs backingEvent, string[] paths) {
        this.backingEvent = backingEvent;
        this.Paths = paths;
    }
}

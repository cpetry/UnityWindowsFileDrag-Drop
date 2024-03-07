using System.Collections.Generic;
using UnityEngine;
using B83.Win32;
using UnityEngine.Events;
using System;

public interface IFileDragAndDrop
{
    void AddListenerOnFilesDropped(UnityAction<Tuple<List<string>, Vector2>> onFilesDropped);
}

public class FileDragAndDrop : MonoBehaviour, IFileDragAndDrop
{
    public class FilesDroppedEvent : UnityEvent<Tuple<List<string>, Vector2>> { };

    [SerializeField]
    private FilesDroppedEvent _onFilesDroppedEvent = new FilesDroppedEvent();

    void OnEnable ()
    {
        InstallHook();
    }

    private void InstallHook()
    {
        // must be installed on the main thread to get the right thread id.
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;
    }

    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    public void AddListenerOnFilesDropped(UnityAction<Tuple<List<string>, Vector2>> onFilesDropped)
    {
        _onFilesDroppedEvent.AddListener(onFilesDropped);
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {
        var position = new Vector2(aPos.x, aPos.y);
        _onFilesDroppedEvent.Invoke(new Tuple<List<string>, Vector2>(aFiles, position));
    }
}

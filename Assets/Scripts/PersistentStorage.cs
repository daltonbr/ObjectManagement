using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;

// Theoretically, we could have multiple such storage objects, used to store different things,
// or to provide access to different storage types. But in this tutorial we use just this
// single file storage object.

public class PersistentStorage : MonoBehaviour
{
    private string _savePath;
    private const string SaveFileName = "saveFile";
    private const string SaveExtension = ".txt";

    private void Awake()
    {
        _savePath = Path.Combine(Application.persistentDataPath, $"{SaveFileName}.txt");
        Debug.Log(Application.persistentDataPath);
        SetupFileBrowser();
    }

    public void Save(PersistableObject o, int version, string savePath)
    {
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            writer.Write(version);
            o.Save(new GameDataWriter(writer));
        }
    }

    // public void Save(PersistableObject o, int version)
    // {
    //     using (var writer = new BinaryWriter(File.Open(_savePath, FileMode.Create)))
    //     {
    //         writer.Write(version);
    //         o.Save(new GameDataWriter(writer));
    //     }
    // }

    private void SetupFileBrowser()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("SaveFile", SaveExtension), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        // Set default filter that is selected when the dialog is shown (optional)
        // Returns true if the default filter is set successfully
        // In this case, set Images filter as the default filter
        FileBrowser.SetDefaultFilter(SaveExtension);

        // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
        // Note that when you use this function, .lnk and .tmp extensions will no longer be
        // excluded unless you explicitly add them as parameters to the function
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
        // It is sufficient to add a quick link just once
        // Name: Users
        // Path: C:\Users
        // Icon: default (folder icon)
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        // Show a save file dialog 
        // onSuccess event: not registered (which means this dialog is pretty useless)
        // onCancel event: not registered
        // Save file/folder: file, Allow multiple selection: false
        // Initial path: "C:\", Title: "Save As", submit button text: "Save"
        // FileBrowser.ShowSaveDialog( null, null, false, false, "C:\\", "Save As", "Save" );

        // Show a select folder dialog 
        // onSuccess event: print the selected folder's path
        // onCancel event: print "Canceled"
        // Load file/folder: folder, Allow multiple selection: false
        // Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
        // FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
        //                            () => { Debug.Log( "Canceled" ); },
        //                            true, false, null, "Select Folder", "Select" );
    }

    public void LoadWithBrowser(PersistableObject o)
    {
        StartCoroutine(ShowLoadDialogCoroutine(o));
    }
    
    public void SaveWithBrowser(PersistableObject o, int version)
    {
        StartCoroutine(ShowSaveDialogCoroutine(o, version));
    }

    private IEnumerator ShowSaveDialogCoroutine(PersistableObject o, int version)
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Allow multiple selection: true
        // Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        // yield return FileBrowser.WaitForLoadDialog(false, true, null, "Load File", "Load");
        yield return FileBrowser.WaitForSaveDialog(false, false, null, default, default);
        
        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log($"Save dialog: {FileBrowser.Success}");

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            // for (int i = 0; i < FileBrowser.Result.Length; i++)
            //     Debug.Log (FileBrowser.Result[i]);

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            //byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            
            Save(o, version, FileBrowser.Result[0]);
        }
    }
    
    private IEnumerator ShowLoadDialogCoroutine(PersistableObject o)
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Allow multiple selection: true
        // Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, true, null, "Load File", "Load");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log($"Load dialog: {FileBrowser.Success}");

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log (FileBrowser.Result[i]);

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            
            using (var reader = new BinaryReader(File.Open(FileBrowser.Result[0], FileMode.Open)))
            {
                o.Load(new GameDataReader(reader, reader.ReadInt32()));
            }
        }
    }
    
    public void Load(PersistableObject o)
    {
        using (var reader = new BinaryReader(File.Open(_savePath, FileMode.Open)))
        {
            o.Load(new GameDataReader(reader, reader.ReadInt32()));
        }
    }
    
    // Try-catch implementations
    
    // private void Save()
    // {
    //     try
    //     {
    //         FileStream stream = File.Open(_savePath, FileMode.Create);
    //         var writer = new BinaryWriter(stream, Encoding.UTF8);
    //         writer.Write(_objects.Count);
    //
    //         foreach (var o in _objects)
    //         {
    //             writer.Write(o.localPosition.x);
    //             writer.Write(o.localPosition.y);
    //             writer.Write(o.localPosition.z);
    //         }
    //
    //         Debug.Log($"Saved: {_savePath}");
    //         
    //         //TODO: is this the right way to close
    //         stream.Close();
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogWarning(e);
    //         //throw;
    //     }
    //     
    //     //TODO: Overwrite warning
    //     //TODO: save project  version
    //     //TODO: save in XML
    //     
    // }
    //
    // private void Load()
    // {
    //     //TODO: ask to save before Load (if it's not saved
    //     BeginNewGame();
    //     try
    //     {
    //         FileStream stream = File.OpenRead(_savePath);
    //         var reader = new BinaryReader(stream, Encoding.UTF8);
    //         int count = reader.ReadInt32();
    //         
    //         for (int i = 0; i < count; i++)
    //         {
    //             Vector3 p;
    //             p.x = reader.ReadSingle();
    //             p.y = reader.ReadSingle();
    //             p.z = reader.ReadSingle();
    //             
    //             Transform t = Instantiate(prefab);
    //             t.localPosition = p;
    //             _objects.Add(t);
    //         }
    //         
    //         //TODO: is this the right way to close
    //         //stream.Dispose();
    //         stream.Close();
    //         Debug.Log($"Load complete: {_savePath}");
    //
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogWarning(e);
    //         //throw;
    //     }
    // }
    
}

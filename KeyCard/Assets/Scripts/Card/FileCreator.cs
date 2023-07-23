using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class FileCreator : MonoBehaviour
{
    void Start()
    {
        test("\"\"");
    }

    void test(string msg)
    {
        string file_name = "Truth.png";
        string sourcePath = Application.dataPath + "/StreamingAssets";
        string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        string source_file = Path.Combine(sourcePath, file_name);
        string dest_file = Path.Combine(targetPath, file_name);

        File.Copy(source_file, dest_file, true);

        /*        string savePath = targetPath + @"\Truth.txt";
                string textValue = @msg;

                if (System.IO.File.Exists(savePath) == true)
                {
                    System.IO.File.AppendAllText(savePath, $"\r\n{textValue}", Encoding.Default);
                }
                else
                {
                    System.IO.File.WriteAllText(savePath, $"{textValue}", Encoding.Default);
                }*/
    }
}
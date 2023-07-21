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
        test("¿¹¾Æ");

    }

    void test(string msg)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        string savePath = path + @"\ÈùÆ®.txt";
        string textValue = @msg;

        if (System.IO.File.Exists(savePath) == true)
        {
            System.IO.File.AppendAllText(savePath, $"\r\n:{textValue}", Encoding.Default);
        }
        else
        {
            System.IO.File.WriteAllText(savePath, $"{textValue}", Encoding.Default);
        }
    }
}

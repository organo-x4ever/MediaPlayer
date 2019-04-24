using System;
using System.Collections.Generic;
using System.Text;

namespace MediaPlayer
{
    public interface IDirectoryPath
    {
        List<string> GetFiles(string[] extensions);
    }
}
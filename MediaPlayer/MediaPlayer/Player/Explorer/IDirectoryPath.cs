using MediaPlayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    public interface IDirectoryPath
    {
        Task<List<FileDetail>> GetFiles();
    }
}
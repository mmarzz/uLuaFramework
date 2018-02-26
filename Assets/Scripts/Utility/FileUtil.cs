using UnityEngine;
using System.IO;
using System;

// using SimpleFramework.Manager;

namespace SimpleFramework.Utils {
    public class FileUtil {

    	public static bool CopyFile(string srcPath, string destPath) {
    		if (!File.Exists(srcPath))
    			return false;
    		File.Copy(srcPath, destPath);
    		return true;
    	}

    	public static bool CopyDirectory(string srcDir, string destDir) {
    		if (!CopyFilesInDirectory(srcDir, destDir))
    			return false;

    		string destSubDirPath = null;
    		foreach (string srcSubDirPath in Directory.GetDirectories(srcDir)) {
    			destSubDirPath = destDir + srcSubDirPath.Substring(srcDir.Length);
    			if (!CopyDirectory(srcSubDirPath, destSubDirPath))
    			return false;
    		}
    		return true;
    	}

    	public static bool CopyFilesInDirectory(string srcDir, string destDir) {
    		if (!Directory.Exists(srcDir))
    			return false;
    		if (!ExistOrCreateDirectory(destDir))
    			return false;

    		string destFilePath = null;
    		foreach (string srcFilePath in Directory.GetFiles(srcDir)) {
    			destFilePath = destDir + srcFilePath.Substring(srcDir.Length);
    			File.Copy(srcFilePath, destFilePath, true);
    		}
    		return true;
    	}

    	public static bool ExistOrCreateDirectory(string dir) {
			if (!Directory.Exists(dir)) 
				Directory.CreateDirectory(dir);
			return true;
		}

		public static bool ExistOrClearDirectory(string dir) {
			if (Directory.Exists(dir))
				Directory.Delete(dir, true);
			Directory.CreateDirectory(dir);
			return true;
		}

		public static bool Write (string fileName, byte[] bytes, FileMode mode = FileMode.OpenOrCreate) {
			if (!ExistOrCreateDirectory(Path.GetDirectoryName(fileName)))
				return false;
			
			using (FileStream fs = new FileStream(fileName, mode)) {
				fs.Write(bytes, 0, bytes.Length);
			}
			return true;
		}
    }
}
